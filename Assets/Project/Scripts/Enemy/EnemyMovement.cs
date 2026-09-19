using Assets.Project.Scripts.Combat;
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

        private float _stunUntilTime;
        public bool IsStunned => Time.time < _stunUntilTime;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        // Throttling pathfinding updates to improve performance
        public void MoveTowards (Vector3 destination)
        {
            if (!_agent.enabled || _agent.isStopped || IsStunned) return;

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
            if (_agent.enabled && !IsStunned)
                _agent.isStopped = false;
        }

        ///<summary>
        /// Stage 1) stub: apply short hit stun + knockback
        /// Stage 2) will move this responsibility to MovementAffector.
        /// </summary>
        public void ApplyHitImpulse(ImpulseResult impulse)
        {
            if (impulse.StunDuration > 0f)
            {
                _stunUntilTime = Mathf.Max(_stunUntilTime, Time.time + impulse.StunDuration);
                Stop();
            }

            if (impulse.Force > 0f
                && impulse.Direction.sqrMagnitude > 0.001f
                && _agent != null
                && _agent.enabled)
            {
                _agent.Warp(transform.position + impulse.Direction.normalized * impulse.Force);
            }
        }
    }
}
