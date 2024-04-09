using ECSFramework;
using UnityEngine;

namespace Systems
{
    public class MoveSystem : ISystem
    {
        public void Tick()
        {
            Debug.Log("MoveSystem Tick");
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
    }
}