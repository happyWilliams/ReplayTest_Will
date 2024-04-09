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

        public void RefreshPosition(float deltaTime)
        {
            var offset = Vector3.zero;
            foreach (var entity in GameWorld.Instance.GetAllEntities())
            {
                if (entity.GetComponent<Components.MoveComponent, Components.PositionComponent>(EComponentType.MoveComponent, EComponentType.PositionComponent, out var moveComp, out var positionComp))
                {
                    offset = deltaTime * moveComp.speed;
                    if (offset == Vector3.zero) continue;
                    positionComp.position += offset;
                    positionComp.position.y = 0;
                    positionComp.isDirty = true;
                }
            }
        }
    }
}