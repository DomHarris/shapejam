using System;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Pool;
using Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawning
{
    public class WaveParams : EventParams
    {
        public int NumEnemies;
    }
    
    public class SpawnParams : EventParams
    {
        public SpawnableEnemy Enemy;
    }

    [CreateAssetMenu]
    public class Wave : ScriptableObject
    {
        [SerializeField] private WaveDefinition[] spawns;
        [SerializeField] private EventAction onWaveSpawned;
        [SerializeField] private EventAction onWaveEnded;
        [SerializeField] private EventAction onEnemySpawned;
        [SerializeField] private float minSpawnTime;
        [SerializeField] private float maxSpawnTime;
        public int NumEnemies => _spawnedEnemies.Count;
        
        [NonSerialized] private readonly List<SpawnableEnemy> _spawnedEnemies = new();
        
        public void Spawn(Transform player)
        {
            foreach (var spawn in spawns)
            {
                var num = spawn.enemies.Length;
                var enemiesToSpawn = new List<SpawnableEnemy>(spawn.enemies);

                switch (spawn.pattern)
                {
                    case SpawnPattern.Circle:
                        spawn.SpawnCircle(enemiesToSpawn, player, num, SpawnEnemy);
                        break;
                    case SpawnPattern.Line:
                        spawn.SpawnLine(enemiesToSpawn, player, num, SpawnEnemy);
                        break;
                    case SpawnPattern.Square:
                        spawn.SpawnSquare(enemiesToSpawn, player, num, SpawnEnemy);
                        break;
                }
            }

            onWaveSpawned.TriggerAction(new WaveParams { NumEnemies = _spawnedEnemies.Count });
        }

        private void SpawnEnemy(SpawnableEnemy prefab, Vector3 position)
        {
            DOVirtual.DelayedCall(Random.Range(minSpawnTime, maxSpawnTime), () =>
            {
                var enemy = LeanPool.Spawn(prefab, position, Quaternion.identity);
                enemy.OnDeath += OnEnemyDie;
                _spawnedEnemies.Add(enemy);
                onEnemySpawned.TriggerAction(new SpawnParams { Enemy = enemy });
            });
        }

        
        private void OnEnemyDie (SpawnableEnemy enemy)
        {
            enemy.OnDeath -= OnEnemyDie;
            _spawnedEnemies.Remove(enemy);
            LeanPool.Despawn(enemy);
            if (_spawnedEnemies.Count == 0)
            {
                onWaveEnded.TriggerAction();
            }
        }

        public void ClearEnemies()
        {
            foreach (var enemy in _spawnedEnemies)
            {
                enemy.OnDeath -= OnEnemyDie;
                LeanPool.Despawn(enemy);
            }
            _spawnedEnemies.Clear();
        }
    }
}
