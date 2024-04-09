using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Reflection;
using UnityEngine;


//A Super Simplified ECS framework, 
namespace ECSFramework
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AssociatedStructAttribute : Attribute
    {
        public Type StructType { get; }

        public AssociatedStructAttribute(Type structType)
        {
            StructType = structType;
        }
    }

    public enum EComponentType
    {
        [AssociatedStruct(typeof(Components.MoveComponent))]
        MoveComponent,

        [AssociatedStruct(typeof(Components.PositionComponent))]
        PositionComponent,

        [AssociatedStruct(typeof(Components.TeamInfoComponent))]
        TeamInfoComponent,

        [AssociatedStruct(typeof(Components.TransformComponent))]
        TransformComponent,

        [AssociatedStruct(typeof(Components.WorldComponent))]
        WorldComponent,
    }

    /// <summary>
    /// Basic Interface for all components
    /// </summary>
    public interface IComponent : IDisposable
    {
    }

    /// <summary>
    /// In order to quickly fulfill the task's purpose, This is not a tradition ECS framework
    /// Normally an entity would just contain a unique ID
    /// But in this case, I let the entity has own function of getting or checking all components it has
    /// Also with a default Destructor
    /// </summary>
    public interface IEntity
    {
        public bool AddComponent(EComponentType addType);
        public bool GetComponent<T>(EComponentType checkType, out T componentGet) where T : IComponent;
        public bool HasComponent(EComponentType checkType);
        public void Destructor();
        public void Initialize();
    }

    public interface ISystem
    {
        public void Tick(float deltaTime);
        public void Initialize();
    }
}