using UnityEngine;

namespace Assets.Project.Scripts.Core
{
    [RequireComponent(typeof(Collider))]
    public class EnemySpawnTriggerZone : EnemySpawnCondition
    {
        [SerializeField] private bool _triggerOnce = true;
        private bool _hasTriggered;

        private void Reset()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasTriggered && _triggerOnce) return;

            if (other.CompareTag("Player"))
            {
                _hasTriggered = true;

                TriggerSpawn(other.transform.position);
            }
        }
    }
}

