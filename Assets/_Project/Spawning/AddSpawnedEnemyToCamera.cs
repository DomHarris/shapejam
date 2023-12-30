using CameraControl;
using Stats;
using UnityEngine;

namespace Spawning
{
    public class AddSpawnedEnemyToCamera : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        [SerializeField] private EventAction enemySpawned;

        private void Start()
        {
            enemySpawned += EnemySpawned;
        }
        
        private void EnemySpawned(EventParams data)
        {
            if (data is not SpawnParams spawnData) return;
            cameraController.AddEnemy(spawnData.Enemy.transform, spawnData.Enemy.CameraWeight);
            spawnData.Enemy.OnDeath += EnemyOnDeath;
        }

        private void EnemyOnDeath(SpawnableEnemy enemy)
        {
            enemy.OnDeath -= EnemyOnDeath;
            cameraController.RemoveEnemy(enemy.transform);
        }

        private void OnDestroy()
        {
            enemySpawned -= EnemySpawned;
        }
    }
}