using System.Numerics;
using ECSFramework;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    public class RenderSyncSystem : ISystem
    {
        public RenderSyncSystem()
        {
            Initialize();
        }

        public void Tick(float deltaTime)
        {
            RefreshTransform(deltaTime);
        }

        /// <summary>
        /// For current stage, we only have players and ball, so we could simply initialize the visual parts here
        /// </summary>
        public void Initialize()
        {
            foreach (var entity in GameWorld.Instance.GetAllEntities())
            {
                if (!entity.GetComponent<Components.TransformComponent, Components.TeamInfoComponent>(EComponentType.TransformComponent, EComponentType.TeamInfoComponent, out var transformComp, out var teamInfoComp)) continue;
                if (transformComp.localTransform == null) continue;

                SetColor(entity, transformComp, teamInfoComp.teamSide);

                //Special Initialize case for Ball
                if (entity.entityType != EEntityType.Ball) continue;
                transformComp.localTransform.localScale = 1.5f * Vector3.one;

                RefreshTransform(Time.deltaTime, false);
            }
        }

        public void ForceRefresh(float deltaTime, bool isForward)
        {
            RefreshTransform(deltaTime, isForward);
        }

        public void RefreshTransform(float deltaTime, bool useLerp = true)
        {
            if (!GameWorld.Instance.GetWorldEntity().GetComponent<Components.WorldComponent>(EComponentType.WorldComponent, out var worldComp)) return;

            var lerpParam = 1f;
            if (worldComp.timeGap != 0)
            {
                lerpParam = deltaTime / worldComp.timeGap;
            }

            foreach (var entity in GameWorld.Instance.GetAllEntities())
            {
                if (!entity.GetComponent<Components.TransformComponent, Components.PositionComponent>(EComponentType.TransformComponent, EComponentType.PositionComponent, out var transformComp, out var positionComp)) continue;
                if (!positionComp.isDirty) continue;
                positionComp.isDirty = false;
                if (transformComp.localTransform == null) continue;
                transformComp.localTransform.position = useLerp ? Vector3.Lerp(transformComp.localTransform.position, positionComp.position, lerpParam) : positionComp.position;

                if (entity.entityType != EEntityType.Ball) continue;
                if (!entity.GetComponent<Components.TeamInfoComponent>(EComponentType.TeamInfoComponent, out var teamInfoComponent)) continue;
                SetColor(entity, transformComp, teamInfoComponent.teamSide);
            }
        }

        private void SetColor(Entity entity, Components.TransformComponent transformComponent, int teamSide)
        {
            var color = teamSide switch
            {
                0 => entity.entityType == EEntityType.Ball ? Color.white : Color.cyan,
                1 => Color.red,
                2 => Color.blue,
                _ => Color.white
            };

            transformComponent.meshRenderer.material.color = color;
        }
    }
}