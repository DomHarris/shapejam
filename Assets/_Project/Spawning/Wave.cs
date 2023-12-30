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
        [SerializeField] private SpawnableEnemy[] enemies;
        [SerializeField] private SpawnPattern pattern;
        [SerializeField] private float radius;
        [SerializeField] private float radiusThickness;
        [SerializeField] private Vector3 offset;
        [SerializeField] private EventAction onWaveSpawned;
        [SerializeField] private EventAction onEnemySpawned;
        [SerializeField] private float minSpawnTime;
        [SerializeField] private float maxSpawnTime;
        public int NumEnemies => _spawnedEnemies.Count;
        
        [NonSerialized] private readonly List<SpawnableEnemy> _spawnedEnemies = new();
        
        public void Spawn(Transform player)
        {
            var num = enemies.Length;
            var enemiesToSpawn = new List<SpawnableEnemy>(enemies);
            
            switch (pattern)
            {
                case SpawnPattern.Circle:
                    SpawnCircle(enemiesToSpawn, player, num);
                    break;
                case SpawnPattern.Line:
                    SpawnLine(enemiesToSpawn, player, num);
                    break;
                case SpawnPattern.Square:
                    SpawnSquare(enemiesToSpawn, player, num);
                    break;
            }
            onWaveSpawned.TriggerAction(new WaveParams { NumEnemies = _spawnedEnemies.Count });
        }

        private void SpawnEnemy(SpawnableEnemy prefab, Vector3 position)
        {
            var enemy = LeanPool.Spawn(prefab, position, Quaternion.identity);
            enemy.OnDeath += OnEnemyDie;
            _spawnedEnemies.Add(enemy);
            onEnemySpawned.TriggerAction(new SpawnParams { Enemy = enemy });
        }

        private void SpawnCircle(List<SpawnableEnemy> enemiesToSpawn, Transform player, int num)
        {
            for (int i = 0; i < num; ++i)
            {
                var instanceIdx = Random.Range(0, enemiesToSpawn.Count);
                var angle = i * Mathf.PI * 2f / num;
                var circlePos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                circlePos += Random.insideUnitCircle * radiusThickness;
                
                var pos = player.position + offset + (Vector3) circlePos;
                var toSpawn = enemiesToSpawn[instanceIdx];
                DOVirtual.DelayedCall(Random.Range(minSpawnTime, maxSpawnTime), 
                    () => SpawnEnemy(toSpawn, pos));
                enemiesToSpawn.RemoveAt(instanceIdx);
            }
        }
        
        private void SpawnLine(List<SpawnableEnemy> enemiesToSpawn, Transform player, int num)
        {
            for (int i = 0; i < num; ++i)
            {
                var linePos = Vector3.Lerp(Vector3.left, Vector3.right, i / (float)num) * radius;
                linePos += (Vector3) Random.insideUnitCircle * radiusThickness;
                var instanceIdx = Random.Range(0, enemiesToSpawn.Count);
                var pos = player.position + offset + linePos;
                SpawnEnemy(enemiesToSpawn[instanceIdx], pos);
                enemiesToSpawn.RemoveAt(instanceIdx);
            }
        }
        
        private void SpawnSquare(List<SpawnableEnemy> enemiesToSpawn, Transform player, int num)
        {
            int numPerSide = num / 4;

            Vector2 previousCorner = new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * radius;
            
            for (int i = 1; i < 5; ++i)
            {
                var angle = i * Mathf.PI * 2f / 4;
                if (i == 4) angle = 0;
                var cornerPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                
                for (int j = 0; j < numPerSide; ++j)
                {
                    var instanceIdx = Random.Range(0, enemiesToSpawn.Count);
                    var sidePos = Vector3.Lerp(previousCorner, cornerPos, j / (float)numPerSide);
                    sidePos += (Vector3) Random.insideUnitCircle * radiusThickness;
                    sidePos = player.position + offset + sidePos;
                    SpawnEnemy(enemiesToSpawn[instanceIdx], sidePos);
                    enemiesToSpawn.RemoveAt(instanceIdx);
                }

                previousCorner = cornerPos;
            }
        }
        
        private void OnEnemyDie (SpawnableEnemy enemy)
        {
            enemy.OnDeath -= OnEnemyDie;
            _spawnedEnemies.Remove(enemy);
            LeanPool.Despawn(enemy);
        }
    }
}
