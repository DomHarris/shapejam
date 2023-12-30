using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stats;
using UnityEngine;

namespace Spawning
{
    public class SpawnWaves : MonoBehaviour
    {
        [SerializeField] private List<WaveCollection> waves;
        [SerializeField] private float timeBetweenWaves;
        [SerializeField] private Transform player;
        [SerializeField] private EventAction onWaveEnded;
        [SerializeField] private EventAction onGameStarted;
        
        private int _currentWave;
        
        private void Start()
        {
            onGameStarted += GameStarted;
            onWaveEnded += OnWaveEnded;
        }

        private void GameStarted(EventParams obj)
        {
            _currentWave = 0;
            SpawnNextWave();
        }

        private void OnDestroy()
        {
            onWaveEnded -= OnWaveEnded;
            onGameStarted -= GameStarted;
        }

        private async void OnWaveEnded(EventParams data)
        {
            if (data is not WaveParams) return;
            var timer = timeBetweenWaves;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                await Task.Yield();
            }
            
            SpawnNextWave();
        }

        private void SpawnNextWave()
        {
            if (_currentWave >= waves.Count) return;
            
            var wave = waves[_currentWave].GetRandom();
            wave.Spawn(player);
            ++_currentWave;
        }
    }
}