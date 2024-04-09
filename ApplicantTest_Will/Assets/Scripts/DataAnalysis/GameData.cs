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
        public long TimestampUTC;
        public Person[] Persons;
        public Ball Ball;
    }

    [System.Serializable]
    public class Person
    {
        public int Id;
        public long Timestamp;
        public int[] Position;
        public float Speed;
        public int TeamSide;
        public int JerseyNumber;
    }

    [System.Serializable]
    public class Ball
    {
        public int Id;
        public long Timestamp;
        public int[] Position;
        public float Speed;
        public int TeamSide;
        public int JerseyNumber;
    }
}