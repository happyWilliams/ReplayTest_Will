using UnityEngine;

namespace ECSFramework
{
    public class Components
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
}