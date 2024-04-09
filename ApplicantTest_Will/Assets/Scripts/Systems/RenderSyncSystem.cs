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
            foreach (var entity in GameWorld.Instance.GetAllEntities())
            {
                if (!entity.GetComponent<Components.TransformComponent, Components.TeamInfoComponent>(EComponentType.TransformComponent, EComponentType.TeamInfoComponent, out var transformComp, out var teamInfoComp)) continue;
                if (transformComp.localTransform == null) continue;
                var meshRenderer = transformComp.localTransform.GetComponent<MeshRenderer>();
                var color = teamInfoComp.teamSide == 1 ? Color.red : Color.blue;
                meshRenderer.material.color = color;
            }
        }

        public void Tick(float deltaTime)
        {
            RefreshTransform(deltaTime);
        }

        public void Initialize()
        {
        }

        public void RefreshTransform(float deltaTime)
        {
            foreach (var entity in GameWorld.Instance.GetAllEntities())
            {
                if (!entity.GetComponent<Components.TransformComponent, Components.PositionComponent>(EComponentType.TransformComponent, EComponentType.PositionComponent, out var transformComp, out var positionComp)) continue;
                if (!positionComp.isDirty) continue;
                positionComp.isDirty = false;
                if (transformComp.localTransform == null) continue;
                transformComp.localTransform.position = Vector3.Lerp(transformComp.localTransform.position, positionComp.position, deltaTime);
            }
        }
    }
}