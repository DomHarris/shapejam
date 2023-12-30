using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawning
{
    [Serializable]
    public class WaveCollection
    {
        [SerializeField] private List<Wave> waves;

        public Wave GetRandom() => waves[Random.Range(0, waves.Count)];
    }
}