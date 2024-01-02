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
        [SerializeField] private Stat[] enemyHealthStats;
        
        private int _currentWaveIdx;
        private Wave _currentWave;
        
        private void Start()
        {
            onGameStarted += GameStarted;
            onWaveEnded += OnWaveEnded;
        }

        private void GameStarted(EventParams obj)
        {
            foreach (var health in enemyHealthStats)
                health.ClearModifiers();
            _currentWaveIdx = 0;
            if (_currentWave != null)
                _currentWave.ClearEnemies();
            _currentWave = null;
            SpawnNextWave();
        }

        private void OnDestroy()
        {
            onWaveEnded -= OnWaveEnded;
            onGameStarted -= GameStarted;
        }

        private async void OnWaveEnded(EventParams data)
        {
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
            _currentWave = waves[_currentWaveIdx].GetRandom();
            _currentWave.Spawn(player);
            ++_currentWaveIdx;
            if (_currentWaveIdx >= waves.Count)
                foreach (var health in enemyHealthStats)
                    health.AddModifier(1.1f, ModifierType.Multiply);
            _currentWaveIdx %= waves.Count;
        }
    }
}