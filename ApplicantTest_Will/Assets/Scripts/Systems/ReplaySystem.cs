using System;
using System.Collections;
using System.Collections.Generic;
using DataAnalysis;
using ECSFramework;
using UnityEngine;
using UnityEngine.Playables;

public class ReplaySystem : ISystem
{
    public ReplaySystem()
    {
        Initialize();
    }

    /// <summary>
    /// We use ReplaySystem's Initialize function to initialize our entityList for players and ball
    /// </summary>
    public void Initialize()
    {
        var Id = -1;
        foreach (var person in PropertyManager.Instance.FrameData[0].Persons)
        {
            Id = person.TeamSide * 100 + person.Id;
            GameWorld.Instance.CreatePlayer(person, Id);
        }

        GameWorld.Instance.CreateBall(PropertyManager.Instance.FrameData[0].Ball);
    }

    public void Tick(float deltaTime)
    {
        if (!GameWorld.Instance.GetWorldEntity().GetComponent<Components.WorldComponent>(EComponentType.WorldComponent, out var worldComp))
            return;

        worldComp.gapTimeCount += deltaTime;
        if (worldComp.gapTimeCount >= worldComp.timeGap)
        {
            worldComp.currentFrame++;
            if (worldComp.currentFrame > PropertyManager.Instance.FrameData.Count - 1)
            {
                worldComp.currentFrame = 0;
            }

            RefreshData(worldComp);
            FlushComponentsData(worldComp.currentFrame);
        }
    }

    private void RefreshData(Components.WorldComponent worldComponent)
    {
        var nextFrameData = PropertyManager.Instance.FrameData[worldComponent.currentFrame];
        var timeGap = 0f;
        if (worldComponent.currentFrame < PropertyManager.Instance.FrameData.Count - 1)
        {
            timeGap = 0.001f * (PropertyManager.Instance.FrameData[worldComponent.currentFrame + 1].TimestampUTC - nextFrameData.TimestampUTC);
        }

        worldComponent.timeGap = timeGap;
        worldComponent.gapTimeCount = 0;
    }

    /// <summary>
    /// In this case, I will not consider the substitution of any players
    /// In this function, all data in positionComponents and moveComponents will be refreshed
    /// </summary>
    /// <param name="dataIndex"></param>
    private void FlushComponentsData(int dataIndex)
    {
        int checkID;
        Vector3 nextPosition;
        Vector3 newPosition;

        Entity currentEntity;

        var currentFrameData = PropertyManager.Instance.FrameData[dataIndex];
        var nextFrameData = dataIndex == PropertyManager.Instance.FrameData.Count - 1 ? currentFrameData : PropertyManager.Instance.FrameData[dataIndex + 1];

        //Persons Refresh
        foreach (var personData in currentFrameData.Persons)
        {
            checkID = personData.TeamSide * 100 + personData.Id;
            nextPosition = new Vector3(personData.Position[0], personData.Position[1], personData.Position[2]);
            foreach (var person in nextFrameData.Persons)
            {
                if (checkID != person.TeamSide * 100 + person.Id) continue;
                nextPosition = new Vector3(person.Position[0], person.Position[1], person.Position[2]);
                break;
            }

            currentEntity = GameWorld.Instance.GetEntityById(checkID);
            if (currentEntity == null)
            {
                continue;
            }

            if (currentEntity.GetComponent<Components.PositionComponent, Components.MoveComponent>(EComponentType.PositionComponent, EComponentType.MoveComponent, out var positionComponent, out var moveComponent))
            {
                newPosition = new Vector3(personData.Position[0], personData.Position[1], personData.Position[2]);
                if (positionComponent.position != newPosition)
                {
                    positionComponent.position = newPosition;
                    positionComponent.isDirty = true;
                }

                moveComponent.speed = (nextPosition - positionComponent.position).normalized * personData.Speed;
                Debug.LogFormat("frame:{3}, ID:{0},speed:{1}, position:{2}", checkID, moveComponent.speed, positionComponent.position, dataIndex);
            }
        }

        //Ball Refresh
        currentEntity = GameWorld.Instance.GetFirstEntityByType(EEntityType.Ball);
        if (currentEntity == null) return;

        nextPosition = new Vector3(nextFrameData.Ball.Position[0], nextFrameData.Ball.Position[1], nextFrameData.Ball.Position[2]);
        if (currentEntity.GetComponent<Components.PositionComponent, Components.MoveComponent>(EComponentType.PositionComponent, EComponentType.MoveComponent, out var ballPositionComp, out var ballMoveComp))
        {
            newPosition = new Vector3(currentFrameData.Ball.Position[0], currentFrameData.Ball.Position[1], currentFrameData.Ball.Position[2]);
            if (ballPositionComp.position != newPosition)
            {
                ballPositionComp.position = newPosition;
                ballPositionComp.isDirty = true;
            }

            ballMoveComp.speed = (nextPosition - ballPositionComp.position).normalized * currentFrameData.Ball.Speed;
        }

        if (currentEntity.GetComponent<Components.TeamInfoComponent>(EComponentType.TeamInfoComponent, out var teamInfoComponent))
        {
            teamInfoComponent.teamSide = currentFrameData.Ball.TrackableBallContext.Possession;
        }
    }
}