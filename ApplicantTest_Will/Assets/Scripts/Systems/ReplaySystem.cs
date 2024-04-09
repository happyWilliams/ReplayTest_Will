using System.Collections;
using System.Collections.Generic;
using ECSFramework;
using UnityEngine;

public class ReplaySystem : ISystem
{
    public void Initialize()
    {
    }

    public void Tick()
    {
        Debug.Log("ReplaySystem Tick");
    }
}