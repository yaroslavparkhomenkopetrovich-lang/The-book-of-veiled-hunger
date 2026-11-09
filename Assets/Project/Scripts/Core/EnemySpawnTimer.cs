using UnityEngine;

namespace Assets.Project.Scripts.Core
{
    public class EnemySpawnTimer : EnemySpawnCondition
    {
        [SerializeField] private float _spawnInterval = 2.0f;
        [SerializeField] private int _totalEnemiesToSpawn = 20;

        private float _timer;
        private int _spawnedEnemiesCount;

        private void Update()
        {
            if (_spawnedEnemiesCount >= _totalEnemiesToSpawn) return;
            
            _timer += Time.deltaTime;

            if (_timer >= _spawnInterval)
            {
                _timer = 0f;
                _spawnedEnemiesCount++;

                TriggerSpawn(transform.position); // Trigger spawn at the current position
            }
        }
    }
}
