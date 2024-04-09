using System;
using System.Collections;
using System.Collections.Generic;
using DataAnalysis;
using UnityEngine;
using ECSFramework;
using Systems;
using UnityEngine.Playables;
using Utilities = ECSFramework.Utilities;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance => instance;

    //In this case, We want to use this worldEntity to control the pace of the game
    //this worldEntity contains the tickStatus of the whole game
    private Entity worldEntity;
    private List<Entity> entities = new List<Entity>();
    private List<ISystem> systems = new List<ISystem>();

    private void Awake()
    {
        instance = this;

        worldEntity = new Entity(Utilities.GetUniqueId());
        worldEntity.AddComponent(EComponentType.WorldComponent);
        entities.Add(worldEntity);
        InitializeSystems();
    }

    void Update()
    {
        if (worldEntity.GetComponent<Components.WorldComponent>(EComponentType.WorldComponent, out var worldComponent))
        {
            if (worldComponent.tickStatus != ETickStatus.Auto)
            {
                return;
            }
        }

        foreach (var system in systems)
        {
            system.Tick();
        }
    }

    /// <summary>
    /// There is a timing sequence for systems to update
    /// </summary>
    private void InitializeSystems()
    {
        systems.Add(new ReplaySystem());
        systems.Add(new MoveSystem());
        systems.Add(new RenderSyncSystem());
    }
}