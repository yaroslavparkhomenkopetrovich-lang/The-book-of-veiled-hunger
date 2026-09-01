using UnityEngine;
using UnityEngine.AI;

namespace Assets.Project.Scripts.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float _pathUpdateInterval = 0.2f;
        private NavMeshAgent _agent;
        private float _nextPathUpdateTime;

        public NavMeshAgent Agent => _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        // Throttling pathfinding updates to improve performance
        public void MoveTowards (Vector3 destination)
        {
            if (!_agent.enabled || _agent.isStopped) return;

            if (Time.time >= _nextPathUpdateTime)
            {
                _nextPathUpdateTime = Time.time + _pathUpdateInterval;
                _agent.SetDestination(destination);
            }
        }

        public void Stop()
        {
            if (_agent.enabled) _agent.isStopped = true;
        }

        public void Resume()
        {
            if (_agent.enabled) _agent.isStopped = false;
        }
    }
}