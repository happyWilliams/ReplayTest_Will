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
        [AssociatedStruct(typeof(BasicComponent.MoveComponent))]
        EMoveComponent,

        [AssociatedStruct(typeof(BasicComponent.PositionComponent))]
        EPositionComponent,

        [AssociatedStruct(typeof(BasicComponent.TeamInfoComponent))]
        ETeamInfoComponent,

        [AssociatedStruct(typeof(BasicComponent.TransformComponent))]
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

    public struct Entity : IEntity
    {
        public int uniqueID;
        public Dictionary<ComponentType, IComponent> componentsList;

        public Entity(int uniqueID)
        {
            this.uniqueID = uniqueID;
            componentsList = new Dictionary<ComponentType, IComponent>();
        }

        public bool AddComponent(ComponentType addType)
        {
            return componentsList.TryAdd(addType, (IComponent)ECSTools.CreateStructFromEnumValue(addType));
        }

        public bool GetComponent(ComponentType checkType, out IComponent componentGet)
        {
            if (componentsList.TryGetValue(checkType, out var component))
            {
                componentGet = component;
                return true;
            }

            componentGet = null;
            return false;
        }

        public bool HasComponent(ComponentType checkType)
        {
            return componentsList.ContainsKey(checkType);
        }

        public void Destructor()
        {
            foreach (var entityComponent in this.componentsList.Values)
            {
                entityComponent.Dispose();
            }

            componentsList.Clear();
        }

        public void Initialize()
        {
        }
    }

    public class BasicComponent
    {
        public struct MoveComponent : IComponent
        {
            public float moveSpeed;

            public void Dispose()
            {
            }
        }

        public struct PositionComponent : IComponent
        {
            public Vector3 position;

            public void Dispose()
            {
            }
        }

        public struct TransformComponent : IComponent
        {
            public Transform localTransform;

            public void Dispose()
            {
                // TODO release managed resources here
            }
        }

        public struct TeamInfoComponent : IComponent
        {
            public int teamSide;
            public int jerseyNumber;

            public void Dispose()
            {
            }
        }
    }

    public static class ECSTools
    {
        public static object CreateStructFromEnumValue(ComponentType enumValue)
        {
            FieldInfo fieldInfo = typeof(ComponentType).GetField(enumValue.ToString());
            AssociatedStructAttribute attribute = (AssociatedStructAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(AssociatedStructAttribute));
            if (attribute != null)
            {
                Type structType = attribute.StructType;
                return Activator.CreateInstance(structType);
            }
            else
            {
                throw new ArgumentException("No associated struct found for the enum value.");
            }
        }
    }
}