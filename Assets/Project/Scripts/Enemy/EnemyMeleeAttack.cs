using UnityEngine;
using Assets.Project.Scripts.Combat;
using Assets.Project.Scripts.Status;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _cooldown = 1.0f;
    [SerializeField] private float _range = 1.2f;
    [SerializeField] private DamageType _damageType = DamageType.Physical;
    [SerializeField] private StatusEffectPayload[] _onHitEffects;


    private float _lastAttackTime;

    public float AttackRange => _range;

    // Throttling attacks to prevent spamming
    public bool CanAttack(float distanceToTarget)
    {
        return distanceToTarget <= _range && Time.time >= _lastAttackTime + _cooldown;
    }

    public void Attack(IDamageable target)
    {
        _lastAttackTime = Time.time;

        DamageInfo info = DamageInfo.FromMelee(
            _damage,
            _damageType,
            gameObject,
            transform.position,
            Vector3.up,
            onHitEffects: _onHitEffects
            );

        target.TakeDamage(info);
    }
}
