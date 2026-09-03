using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    [RequireComponent(typeof(DamageProcessor))]
    public class DamageReceiver : MonoBehaviour, IDamageable
    {
        private DamageProcessor _damageProcessor;
        private ArmorProcessor _armorProcessor;

        private void Awake()
        {
            _damageProcessor = GetComponent<DamageProcessor>();
            _armorProcessor = GetComponent<ArmorProcessor>();
        }

        public void TakeDamage(DamageInfo info)
        {
            if (info.Amount <= 0) return;

            int damageToHealth = info.Amount;

            // 1) Mitigate through armor if present
            if (_armorProcessor != null)
            {
                damageToHealth = _armorProcessor.MitigateDamage(info);
            }

            // 2) Apply remaining damage to health
            if (damageToHealth > 0)
            {
                _damageProcessor.ApplyDamage(damageToHealth, info.Instigator);
            }
        }
    }
}
