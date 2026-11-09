using System;
using UnityEngine;

namespace Assets.Project.Scripts.Core
{
    public abstract class EnemySpawnCondition : MonoBehaviour
    {
        // Abstract method to determine if the spawn condition is met
        public event Action<Vector3> OnSpawnPointTriggered;

        protected void TriggerSpawn(Vector3 spawnPoint)
        {
            OnSpawnPointTriggered?.Invoke(spawnPoint);
        }
    }
}
