using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECSFramework;
using Systems;

public class GameController : MonoBehaviour
{
    //In this case, We want to use this worldEntity to control the pace of the game
    //this worldEntity contains the tickStatus of the whole game
    private Entity worldEntity;
    private List<Entity> entities = new List<Entity>();
    private List<ISystem> systems = new List<ISystem>();

    private void Awake()
    {
        worldEntity = new Entity(0);
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