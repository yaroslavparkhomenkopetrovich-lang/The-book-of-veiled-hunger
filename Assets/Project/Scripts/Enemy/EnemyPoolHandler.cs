using Assets.Project.Scripts.Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

namespace Assets.Project.Scripts.Enemy
{
    [RequireComponent(typeof(HealthData))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyPoolHandler : MonoBehaviour
    {
        private HealthData _healthData;
        private NavMeshAgent _agent;
        private ArmorData _armorData;
        private Collider _collider;
        private IObjectPool<EnemyPoolHandler> _originatingPool;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _collider = GetComponent<Collider>();
            _healthData = GetComponent<HealthData>();
            _armorData = GetComponent<ArmorData>();
        }

        private void OnEnable()
        {
            // 1) Subscribe to death event from Health component
            _healthData.OnDeath += HandleDeath;

            // 2) Restore physics and navigation state when pulled from pool
            if (_collider != null) _collider.enabled = true;
            if (_agent != null)
            {
                _agent.enabled = true;
                _agent.isStopped = false;
            }
        }

        private void OnDisable()
        {
            // Unsubscribe to avoid memory leaks
            _healthData.OnDeath -= HandleDeath;
        }

        /// <summary>
        /// Called by the Spawener when pulling this enemy out of the pool.
        /// </summary>
        public void Initialize(IObjectPool<EnemyPoolHandler> pool)
        {
            _originatingPool = pool;

            // Reset health and armor pools to full capacity
            if (_armorData != null)
            {
                _armorData.CurrentArmor = _armorData.MaxArmor;
                _armorData.NotifyChanged();
            }

            // Reset health
            _healthData.CurrentHealth = _healthData.MaxHealth;

        }

        private void HandleDeath(GameObject attacker)
        {
            // 1) Instantly stops AI and disable collision so bullets player can pass through
            if (_agent != null && _agent.isOnNavMesh)
            {
                _agent.isStopped = true;
                _agent.enabled = false;
            }

            if (_collider != null)
            {
                _collider.enabled = false;
            }

            // 2) Release back to poo or deactivate fallback
            if (_originatingPool != null)
            {
                _originatingPool.Release(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}