using Assets.Project.Scripts.Status;
using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    [RequireComponent(typeof(DamageProcessor))]
    [RequireComponent(typeof(MovementAffector))]
    [RequireComponent(typeof(ImpulseProcessor))]
    [RequireComponent(typeof(StatusEffectController))]
    public class DamageController : MonoBehaviour, IDamageable
    {
        private DamageProcessor _damageProcessor;
        private ArmorProcessor _armorProcessor;
        private ImpulseProcessor _impulseProcessor;
        private MovementAffector _movementAffector;
        private StatusEffectController _statusEffectController;

        private void Awake()
        {
            _damageProcessor = GetComponent<DamageProcessor>();
            _armorProcessor = GetComponent<ArmorProcessor>();
            _impulseProcessor = GetComponent<ImpulseProcessor>();
            _movementAffector = GetComponent<MovementAffector>();
            _statusEffectController = GetComponent<StatusEffectController>();

        }

        public void TakeDamage(DamageInfo info)
        {
            if (!CanReceiveHit(info)) return;

            int damageToHealth = ResolveDamageThroughArmor(info);

            ApplyHealthDamage(damageToHealth, info.Instigator);

            TryApplyHitImpulse(info);

            TryApplyStatusEffects(info);
        }
        private static bool CanReceiveHit(DamageInfo info)
        {
            return info.Amount > 0
                || info.StunDuration > 0f
                || info.KnockbackForce > 0f
                || info.HasStatusEffect;
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
            if (_impulseProcessor == null || _movementAffector == null) return;

            ImpulseResult impulse = _impulseProcessor.CalculateImpulse
                (
                    info,
                    transform.position,
                    info.ImpulseType,
                    info.KnockbackForce,
                    info.StunDuration
                );

            if (impulse.Force <= 0f && impulse.StunDuration <= 0f) return;

            _movementAffector.ApplyHitImpulse(impulse);
        }

        private void TryApplyStatusEffects(DamageInfo info)
        {
            if (_statusEffectController == null || !info.HasStatusEffect) return;

                _statusEffectController.ApplyAllStatusEffects(info.StatusEffects);
        }
    }
}
