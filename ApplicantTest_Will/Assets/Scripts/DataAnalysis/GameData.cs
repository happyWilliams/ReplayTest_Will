using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataAnalysis
{
    [System.Serializable]
    public class GameData
    {
        public FrameDatum[] FrameData;
    }

    [System.Serializable]
    public class FrameDatum
    {
        public int FrameCount;
        public double TimestampUTC;
        public Person[] Persons;
        public Ball Ball;
    }

    [System.Serializable]
    public class Person
    {
        public int Id;
        public float Timestamp;
        public Vector3 Position;
        public int TeamSide;
        public int JerseyNumber;
    }

    [System.Serializable]
    public class Ball
    {
        public int Id;
        public double Timestamp;
        public Vector3 Position;
        public double Speed;
        public int TeamSide;
        public int JerseyNumber;
    }
}