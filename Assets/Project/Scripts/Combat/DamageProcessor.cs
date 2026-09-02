using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    [RequireComponent(typeof(HealthData))]
    public class DamageProcessor : MonoBehaviour
    {
        private HealthData _healthData;

        private void Awake()
        {
            _healthData = GetComponent<HealthData>();
        }

        /// <summary>
        /// Applies incomming damage directly to HealthData of entity.
        /// </summary>
        
        public void ApplyDamage (int amount, GameObject attacker =null)
        {
            // Guard: Ignore if dead, negative/zero damage, or invulnerable
            if (_healthData.IsDead || amount <= 0 || _healthData.IsInvulnerable) return;

            bool wasOverhealth = _healthData.IsOverhealth;

            // 1) Substract damage and clampt to zero
            _healthData.CurrentHealth = Mathf.Max(0, _healthData.CurrentHealth - amount);

            // 2) Notify listeners of new health state
            _healthData.NotifyChanged(wasOverhealth);

            // 3) Trigger death event if health is zero
            if (_healthData.IsDead)
            {
                _healthData.NotifyDeath(attacker);
            }

        }

        public void Defeat(GameObject attacker = null)
        {
            if (_healthData.IsDead) return;

            bool wasOverhealed = _healthData.IsOverhealth;

            _healthData.CurrentHealth = 0;
            _healthData.NotifyChanged(wasOverhealed);
            _healthData.NotifyDeath(attacker);
        }
    }
}
