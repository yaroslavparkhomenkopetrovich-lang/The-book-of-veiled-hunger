using Assets.Project.Scripts.Enemy;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Project.Scripts.Core
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyPoolHandler _enemyPrefab;
        [SerializeField] private int _defaultCapacity = 20;
        [SerializeField] private int _maxPoolSize = 50;

        private IObjectPool<EnemyPoolHandler> _enemyPool;

        private void Awake()
        {
            // Enemy pool initialization    
            _enemyPool = new ObjectPool<EnemyPoolHandler>(
                createFunc: () => Instantiate(_enemyPrefab),
                actionOnGet: enemy => enemy.gameObject.SetActive(true),
                actionOnRelease: enemy => enemy.gameObject.SetActive(false),
                actionOnDestroy: enemy => Destroy(enemy.gameObject),
                collectionCheck: false,
                defaultCapacity: _defaultCapacity,
                maxSize: _maxPoolSize
            );
        }

        public void SpawnEnemy(Vector3 spawnPosition)
        {
            EnemyPoolHandler enemy = _enemyPool.Get();
            enemy.transform.position = spawnPosition;
            enemy.Initialize(_enemyPool);
        }
    }
    
}
