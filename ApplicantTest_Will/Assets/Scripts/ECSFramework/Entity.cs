using System.Collections.Generic;

namespace ECSFramework
{
    public struct Entity : IEntity
    {
        public int uniqueID;
        private Dictionary<ComponentType, IComponent> componentsList;

        public Entity(int uniqueID)
        {
            this.uniqueID = uniqueID;
            componentsList = new Dictionary<ComponentType, IComponent>();
        }

        public bool AddComponent(ComponentType addType)
        {
            return componentsList.TryAdd(addType, (IComponent)Utilities.CreateStructFromEnumValue(addType));
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
}