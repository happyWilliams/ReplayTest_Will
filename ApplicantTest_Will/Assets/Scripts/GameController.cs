using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECSFramework;

public class GameController : MonoBehaviour
{
    private Entity worldEntity;
    private List<Entity> entities = new List<Entity>();

    private void Awake()
    {
        worldEntity = new Entity(0);
        worldEntity.AddComponent(ComponentType.EMoveComponent);
        worldEntity.AddComponent(ComponentType.EMoveComponent);
        worldEntity.AddComponent(ComponentType.ETransformComponent);
        entities.Add(worldEntity);
    }

    void Start()
    {
    }

    void Update()
    {
    }
}