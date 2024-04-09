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
        Entity currentEntity;
        var currentFrameData = PropertyManager.Instance.FrameData[dataIndex];
        var nextFrameData = dataIndex == PropertyManager.Instance.FrameData.Count - 1 ? currentFrameData : PropertyManager.Instance.FrameData[dataIndex + 1];
        int checkID;
        Vector3 nextPosition;
        Vector3 newPosition;
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

            if (currentEntity.GetComponent<Components.PositionComponent>(EComponentType.PositionComponent, out var positionComponent))
            {
                newPosition = new Vector3(personData.Position[0], personData.Position[1], personData.Position[2]);
                if (positionComponent.position != newPosition)
                {
                    positionComponent.position = newPosition;
                    positionComponent.isDirty = true;
                }

                if (currentEntity.GetComponent<Components.MoveComponent>(EComponentType.MoveComponent, out var moveComponent))
                {
                    moveComponent.speed = (nextPosition - positionComponent.position).normalized * personData.Speed;
                    Debug.LogFormat("frame:{3}, ID:{0},speed:{1}, position:{2}", checkID, moveComponent.speed, positionComponent.position, dataIndex);
                }
            }
        }
    }
}