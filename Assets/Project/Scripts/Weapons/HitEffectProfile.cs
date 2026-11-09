using UnityEngine;
using Assets.Project.Scripts.Status;
using Assets.Project.Scripts.Combat;

namespace Assets.Project.Scripts.Weapons
{
    [CreateAssetMenu(fileName = "HitEffect_", menuName = "Combat/Hit Effect Profile")]
    public class HitEffectProfile : ScriptableObject
    {
        [SerializeField] private int _damage = 15;
        public int Damage => _damage;

        [SerializeField] private DamageType _damageKind = DamageType.Physical;
        public DamageType DamageKind => _damageKind;

        [SerializeField] private float _stunDuration;
        public float StunDuration => _stunDuration;

        [SerializeField] private float _knockbackForce;
        public float KnockbackForce => _knockbackForce;

        [SerializeField] private ImpulseType _impulseKind = ImpulseType.PushAwayFromBullet;
        public ImpulseType ImpulseKind => _impulseKind;

        [SerializeField] private StatusEffectPayload[] _statusEffects;
        public StatusEffectPayload[] StatusEffects => _statusEffects;

        [Tooltip("If false, projectiles ignore targets with the same tag as the shooter, i.e. Enemy ignores Enemy")]
        [SerializeField] private bool _canHitAllies = false;
        public bool CanHitAllies => _canHitAllies;
    }
}
