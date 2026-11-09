using UnityEngine;

namespace Assets.Project.Scripts.Core
{
    [RequireComponent(typeof(Collider))]
    public class EnemySpawnCollider : EnemySpawnCondition
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                TriggerSpawn(other.transform.position);
            }
        }
    }
}
