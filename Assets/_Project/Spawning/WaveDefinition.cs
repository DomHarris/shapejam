using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawning
{
    [Serializable]
    public class WaveDefinition
    {
        public SpawnableEnemy[] enemies;
        public SpawnPattern pattern;
        public float radius;
        public float radiusThickness;
        public Vector3 offset;
        
        public void SpawnCircle(List<SpawnableEnemy> enemiesToSpawn, Transform player, int num, Action<SpawnableEnemy, Vector3> spawnEnemy)
        {
            for (int i = 0; i < num; ++i)
            {
                var instanceIdx = Random.Range(0, enemiesToSpawn.Count);
                var angle = i * Mathf.PI * 2f / num;
                var circlePos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                circlePos += Random.insideUnitCircle * radiusThickness;
                
                var pos = player.position + offset + (Vector3) circlePos;
                var toSpawn = enemiesToSpawn[instanceIdx];
                spawnEnemy(toSpawn, pos);
                enemiesToSpawn.RemoveAt(instanceIdx);
            }
        }
        
        public void SpawnLine(List<SpawnableEnemy> enemiesToSpawn, Transform player, int num, Action<SpawnableEnemy, Vector3> spawnEnemy)
        {
            for (int i = 0; i < num; ++i)
            {
                var linePos = Vector3.Lerp(Vector3.left, Vector3.right, i / (float)num) * radius;
                linePos += (Vector3) Random.insideUnitCircle * radiusThickness;
                var instanceIdx = Random.Range(0, enemiesToSpawn.Count);
                var pos = player.position + offset + linePos;
                spawnEnemy(enemiesToSpawn[instanceIdx], pos);
                enemiesToSpawn.RemoveAt(instanceIdx);
            }
        }
        
        public void SpawnSquare(List<SpawnableEnemy> enemiesToSpawn, Transform player, int num, Action<SpawnableEnemy, Vector3> spawnEnemy)
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
                    spawnEnemy(enemiesToSpawn[instanceIdx], sidePos);
                    enemiesToSpawn.RemoveAt(instanceIdx);
                }

                previousCorner = cornerPos;
            }
        }
    }
}