using UnityEngine;

namespace ECSFramework
{
    public class Components
    {
        public class MoveComponent : IComponent
        {
            public Vector3 speed;

            public MoveComponent()
            {
            }

            public MoveComponent(params object[] args)
            {
                speed = (Vector3)args[0];
            }


            public void Dispose()
            {
            }
        }

        public class PositionComponent : IComponent
        {
            public Vector3 position;
            public bool isDirty;

            public PositionComponent()
            {
            }

            public PositionComponent(params object[] args)
            {
                position = (Vector3)args[0];
                isDirty = true;
            }


            public void Dispose()
            {
            }
        }

        public class TransformComponent : IComponent
        {
            public Transform localTransform;
            public MeshRenderer meshRenderer;

            public TransformComponent()
            {
            }

            public TransformComponent(params object[] args)
            {
                localTransform = (Transform)args[0];
                meshRenderer = localTransform.GetComponent<MeshRenderer>();
            }

            public void Dispose()
            {
            }
        }

        public class TeamInfoComponent : IComponent
        {
            public int teamSide;
            public int jerseyNumber;

            public TeamInfoComponent()
            {
            }

            public TeamInfoComponent(params object[] args)
            {
                teamSide = (int)args[0];
                jerseyNumber = (int)args[1];
            }

            public void Dispose()
            {
            }
        }

        public class WorldComponent : IComponent
        {
            public ETickStatus tickStatus;
            public int currentFrame;
            public float timeGap;
            public float gapTimeCount;

            public bool forceRefresh = false;

            public WorldComponent()
            {
            }

            public WorldComponent(params object[] args)
            {
                tickStatus = (ETickStatus)args[0];
                currentFrame = (int)args[1];
                timeGap = 0;
                gapTimeCount = 0;
            }

            public void Dispose()
            {
            }
        }
    }
}