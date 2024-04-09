using ECSFramework;
using UnityEngine;

namespace Systems
{
    public class MoveSystem : ISystem
    {
        public void Tick(float deltaTime)
        {
            RefreshPosition(deltaTime);
        }

        public void Initialize()
        {
        }

        public void ForceRefresh(float deltaTime, bool isForward)
        {
            // RefreshPosition(deltaTime);
        }

        public void RefreshPosition(float deltaTime)
        {
            Vector3 offset;
            foreach (var entity in GameWorld.Instance.GetAllEntities())
            {
                if (!entity.GetComponent<Components.MoveComponent, Components.PositionComponent>(EComponentType.MoveComponent, EComponentType.PositionComponent, out var moveComp, out var positionComp)) continue;

                offset = deltaTime * moveComp.speed;
                if (offset == Vector3.zero) continue;
                positionComp.position += offset;
                positionComp.position.y = 0;
                positionComp.isDirty = true;
            }
        }
    }
}