using System.Collections.Generic;

namespace ECSFramework
{
    /// <summary>
    /// Normally I would use a TagComponent just to identity different entity types,
    /// In this case, I'll just use an enum to identify Entity types
    /// </summary>
    public enum EEntityType
    {
        None,
        Ball,
        Player
    }

    public class Entity : IEntity
    {
        public EEntityType entityType = EEntityType.None;
        public int uniqueID;
        private Dictionary<EComponentType, IComponent> componentsList;

        public Entity(int uniqueID)
        {
            this.uniqueID = uniqueID;
            componentsList = new Dictionary<EComponentType, IComponent>();
        }

        public bool AddComponent(EComponentType addType)
        {
            return componentsList.TryAdd(addType, (IComponent)Utilities.CreateStructFromEnumValue(addType));
        }


        public bool AddComponent(EComponentType addType, params object[] args)
        {
            return componentsList.TryAdd(addType, (IComponent)Utilities.CreateStructFromEnumValue(addType, args));
        }

        public bool GetComponent<T>(EComponentType checkType, out T componentGet) where T : IComponent
        {
            if (componentsList.TryGetValue(checkType, out var component))
            {
                componentGet = (T)component;
                return true;
            }

            componentGet = default;
            return false;
        }

        public bool GetComponent<T1, T2>(EComponentType typeA, EComponentType typeB, out T1 componentA, out T2 componentB) where T1 : IComponent where T2 : IComponent
        {
            if (componentsList.TryGetValue(typeA, out var compA) && componentsList.TryGetValue(typeB, out var compB))
            {
                componentA = (T1)compA;
                componentB = (T2)compB;
                return true;
            }

            componentA = default;
            componentB = default;
            return false;
        }

        public bool HasComponent(EComponentType checkType)
        {
            return componentsList.ContainsKey(checkType);
        }

        public void Destructor()
        {
            foreach (var entityComponent in componentsList.Values)
            {
                entityComponent.Dispose();
            }

            componentsList.Clear();
        }

        public void Initialize()
        {
        }
    }
}