using UnityEngine;
using UnityEngine.AI;

namespace Assets.Project.Scripts.Core
{
    public class EnemySpawnController : MonoBehaviour
    {
        [Header("Spawner Reference")]
        [SerializeField] private EnemySpawner _enemySpawner;

        [Header("Spawn Instruction to Send")]
        [Header("Spawn Count & Points")]
        [SerializeField] private int _enemiesToSpawn = 23;

        [Header("Optional Fixed Center Points, Leave empty to use hit position")]
        [SerializeField] private Transform[] _spawnPoints;

        [Header("Random Radius Settings")]
        [SerializeField] private bool _useRandomRadius = false;
        [SerializeField] private float _minRadius = 2.0f;
        [SerializeField] private float _maxRadius = 5.0f;

        private EnemySpawnCondition[] _conditions;

        private void Awake()
        {
            // Automatically grabs spawn conditions from the children of this GameObject
            _conditions = GetComponentsInChildren<EnemySpawnCondition>();

            if (_enemySpawner == null)
                _enemySpawner = FindAnyObjectByType<EnemySpawner>();
        }

        private void OnEnable()
        {
            for (int i = 0; i < _conditions.Length; i++)
            {
                if (_conditions[i] != null)
                    _conditions[i].OnSpawnPointTriggered += HandleSpawn;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _conditions.Length; i++)
            {
                if (_conditions[i] != null)
                    _conditions[i].OnSpawnPointTriggered -= HandleSpawn;
            }
        }

        private void HandleSpawn(Vector3 conditionPosition)
        {
            if (_enemySpawner == null || _spawnPoints == null) return;

            for (int i = 0; i < _enemiesToSpawn; i++)
            {
                Vector3 center = GetCenterPoint(conditionPosition);

                Vector3 spawnPos = _useRandomRadius
                    ? GetRandomPointInRadius(center)
                    : center;

                _enemySpawner.SpawnEnemy(spawnPos);
            }
        }

        private Vector3 GetCenterPoint(Vector3 fallbackPosition)
        {
            if (_spawnPoints == null || _spawnPoints.Length == 0)
                return fallbackPosition;

            int index = Random.Range(0, _spawnPoints.Length);

            // Return the position of the selected spawn point, or fallback to the provided position if null
            return _spawnPoints[index] != null ? _spawnPoints[index].position : fallbackPosition;
        }

        private Vector3 GetRandomPointInRadius(Vector3 center)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;

            if (dir == Vector2.zero)
                dir = Vector2.right; // Default direction if random point is zero

            float distance = Random.Range(_minRadius, _maxRadius);
            // Candidate point of spawn
            Vector3 candidate = center + new Vector3(dir.x * distance, 0, dir.y * distance);

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return center;
        }
    }
}
