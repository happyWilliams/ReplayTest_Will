using System.Collections.Generic;
using DataAnalysis;
using UnityEngine;
using ECSFramework;
using Systems;
using Utilities = ECSFramework.Utilities;

public class GameWorld : MonoBehaviour
{
    private static GameWorld instance;
    public static GameWorld Instance => instance;

    //In this case, We want to use this worldEntity to control the pace of the game
    //this worldEntity contains the tickStatus of the whole game
    private Entity worldEntity;
    private List<Entity> entities = new List<Entity>();
    private List<ISystem> overrideSystems = new List<ISystem>();
    private List<ISystem> systems = new List<ISystem>();

    public GameObject playerPrefab;
    public Transform instantiatedParent;

    private void Awake()
    {
        instance = this;

        worldEntity = CreateEntity();
        worldEntity.AddComponent(EComponentType.WorldComponent, ETickStatus.Auto, 0);

        PropertyManager.Instance.InitializeData();
        InitializeSystems();
    }

    void Update()
    {
        foreach (var system in overrideSystems)
        {
            system.Tick(Time.deltaTime);
        }

        if (worldEntity.GetComponent<Components.WorldComponent>(EComponentType.WorldComponent, out var worldComponent))
        {
            if (worldComponent.forceRefresh)
            {
                foreach (var system in systems)
                {
                    system.ForceRefresh(Time.deltaTime, worldComponent.isForward);
                }

                worldComponent.forceRefresh = false;
            }

            if (worldComponent.tickStatus != ETickStatus.Auto)
            {
                return;
            }
        }

        foreach (var system in systems)
        {
            system.Tick(Time.deltaTime);
        }
    }

    /// <summary>
    /// There is a timing sequence for systems to update
    /// </summary>
    private void InitializeSystems()
    {
        overrideSystems.Add(new InputSystem());

        systems.Add(new ReplaySystem());
        systems.Add(new MoveSystem());
        systems.Add(new RenderSyncSystem());
    }

    /// <summary>
    /// Only entrance for entity creation, at this time, I would just initialize one single entity without any components attached
    /// Components attach should be done separately
    /// </summary>
    /// <returns></returns>
    public Entity CreateEntity(int Id = -1, bool shouldAddToList = true)
    {
        var entity = new Entity(Id == -1 ? Utilities.GetUniqueId() : Id);
        entities.Add(entity);
        return entity;
    }

    public Entity GetWorldEntity()
    {
        return worldEntity;
    }

    public Entity GetEntityById(int Id)
    {
        return entities.Find(entity => entity.uniqueID == Id);
    }

    public Entity GetFirstEntityByType(EEntityType type)
    {
        return entities.Find(entity => entity.entityType == type);
    }

    public List<T> GetComponents<T>(EComponentType componentType) where T : IComponent
    {
        var componentList = new List<T>();
        foreach (var entity in entities)
        {
            if (entity.GetComponent<T>(componentType, out T componentFound))
            {
                componentList.Add(componentFound);
            }
        }

        return componentList;
    }

    public List<Entity> GetAllEntities()
    {
        return entities;
    }

    public void CreatePlayer(Person personData, int Id = -1)
    {
        var bornPosition = new Vector3(personData.Position[0], personData.Position[1], personData.Position[2]);

        var player = CreateEntity(Id);
        player.entityType = EEntityType.Player;

        player.AddComponent(EComponentType.TeamInfoComponent, personData.TeamSide, personData.JerseyNumber);
        player.AddComponent(EComponentType.MoveComponent, Vector3.zero);
        player.AddComponent(EComponentType.TransformComponent, Instantiate(playerPrefab, bornPosition, Quaternion.identity, instantiatedParent).transform);
        player.AddComponent(EComponentType.PositionComponent, bornPosition);
    }

    public void CreateBall(Ball ballData)
    {
        var bornPosition = new Vector3(ballData.Position[0], ballData.Position[1], ballData.Position[2]);

        var ball = CreateEntity();
        ball.entityType = EEntityType.Ball;
        ball.AddComponent(EComponentType.TeamInfoComponent, ballData.TeamSide, ballData.JerseyNumber);
        ball.AddComponent(EComponentType.MoveComponent, Vector3.zero);
        ball.AddComponent(EComponentType.TransformComponent, Instantiate(playerPrefab, bornPosition, Quaternion.identity, instantiatedParent).transform);
        ball.AddComponent(EComponentType.PositionComponent, bornPosition);
    }
}