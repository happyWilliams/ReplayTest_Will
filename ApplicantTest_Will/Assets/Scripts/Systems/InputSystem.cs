﻿using DataAnalysis;
using ECSFramework;
using UnityEngine;

namespace Systems
{
    public class InputSystem : ISystem
    {
        public void Tick(float deltaTime)
        {
            CheckInput();
        }

        public void Initialize()
        {
        }

        public void ForceRefresh(float deltaTime)
        {
        }

        private void CheckInput()
        {
            if (!Input.anyKey) return;
            if (!GameWorld.Instance.GetWorldEntity().GetComponent<Components.WorldComponent>(EComponentType.WorldComponent, out var worldComponent)) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                worldComponent.tickStatus = worldComponent.tickStatus switch
                {
                    ETickStatus.Auto => ETickStatus.Pause,
                    ETickStatus.Pause => ETickStatus.Auto,
                    _ => worldComponent.tickStatus
                };
            }

            if (worldComponent.tickStatus != ETickStatus.Pause) return;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                worldComponent.currentFrame = Mathf.Clamp(worldComponent.currentFrame - 1, 0, worldComponent.currentFrame);
                worldComponent.forceRefresh = true;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                worldComponent.currentFrame = Mathf.Clamp(worldComponent.currentFrame + 1, worldComponent.currentFrame, PropertyManager.Instance.FrameData.Count - 1);
                worldComponent.forceRefresh = true;
            }
        }
    }
}