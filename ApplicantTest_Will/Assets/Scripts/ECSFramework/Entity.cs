using System.Collections.Generic;
using System.ComponentModel;

namespace ECSFramework
{
    public struct Entity : IEntity
    {
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