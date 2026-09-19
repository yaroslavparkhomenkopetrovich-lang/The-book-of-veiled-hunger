using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    [RequireComponent(typeof(DamageProcessor))]
    public class DamageController : MonoBehaviour, IDamageable
    {
        private DamageProcessor _damageProcessor;
        private ArmorProcessor _armorProcessor;
        private ImpulseProcessor _impulseProcessor;

        private void Awake()
        {
            _damageProcessor = GetComponent<DamageProcessor>();
            _armorProcessor = GetComponent<ArmorProcessor>();
            _impulseProcessor = GetComponent<ImpulseProcessor>();
        }

        public void TakeDamage(DamageInfo info)
        {
            if (!CanReceiveHit(info)) return;

            int damageToHealth = ResolveDamageThroughArmor(info);

            ApplyHealthDamage(damageToHealth, info.Instigator);

            TryApplyHitImpulse(info);
        }
        private static bool CanReceiveHit(DamageInfo info)
        {
            return info.Amount > 0;
        }

        private int ResolveDamageThroughArmor(DamageInfo info)
        {
            if (_armorProcessor == null) return info.Amount;

            return _armorProcessor.MitigateDamage(info);
        }

        private void ApplyHealthDamage(int amount, GameObject instigator)
        {
            if (amount <= 0) return;

            _damageProcessor.ApplyDamage(amount, instigator);
        }

        private void TryApplyHitImpulse(DamageInfo info)
        {
            if (_impulseProcessor == null) return;

            ImpulseResult impulse = _impulseProcessor.CalculateImpulse
                (
                    info,
                    transform.position,
                    info.ImpulseType,
                    info.KnockbackForce,
                    info.StunDuration
                );

            if (impulse.Force <= 0f && impulse.StunDuration <= 0f) return;
            // Stage 1 stub: direct enemy movement.
            // Stage 2 can replace this with MovementAffector.
            if (TryGetComponent<Enemy.EnemyMovement>(out var movement))
            {
                movement.ApplyHitImpulse(impulse);
            }
        }

    }
}
