using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public enum ComponentType
    {
        [AssociatedStruct(typeof(Components.MoveComponent))]
        EMoveComponent,

        [AssociatedStruct(typeof(Components.PositionComponent))]
        EPositionComponent,

        [AssociatedStruct(typeof(Components.TeamInfoComponent))]
        ETeamInfoComponent,

        [AssociatedStruct(typeof(Components.TransformComponent))]
        ETransformComponent,
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
        public bool AddComponent(ComponentType addType);
        public bool GetComponent(ComponentType checkType, out IComponent componentGet);
        public bool HasComponent(ComponentType checkType);
        public void Destructor();
        public void Initialize();
    }

    public interface ISystem
    {
        public void Tick();
        public void Initialize();
    }
}