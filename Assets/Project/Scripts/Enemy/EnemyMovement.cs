using Assets.Project.Scripts.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Project.Scripts.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(MovementAffector))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float _pathUpdateInterval = 0.2f;

        private NavMeshAgent _agent;
        private MovementAffector _movementAffector;

        private float _nextPathUpdateTime;
        private float _baseSpeed;
        private bool _wasStunned;

        public NavMeshAgent Agent => _agent;
        public bool IsStunned => _movementAffector != null && _movementAffector.IsStunned;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _movementAffector = GetComponent<MovementAffector>();
            _baseSpeed = _agent.speed;
        }

        private void Update()
        {
            ApplyMovementModifiers();
        }

        private void ApplyMovementModifiers()
        {
            if (_movementAffector == null || !_agent.enabled) return;

            _agent.speed = _baseSpeed * _movementAffector.SpeedMultiplier;

            bool stunned = _movementAffector.IsStunned;

            if (stunned)
            {
                _agent.isStopped = true;
            }
            else if (_wasStunned)
            {
                // Stun ended
                _agent.isStopped = false;
            }

            _wasStunned = stunned;

            if (_movementAffector.HasKnockback)
            {
                _agent.Move(_movementAffector.KnockbackVelocity * Time.deltaTime);
                _movementAffector.TickKnockback(Time.deltaTime);
            }

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
    }
}
