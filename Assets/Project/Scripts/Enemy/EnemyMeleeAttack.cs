using UnityEngine;
using Assets.Project.Scripts.Combat;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _cooldown = 1.0f;
    [SerializeField] private float _range = 1.2f;
    [SerializeField] private DamageType _damageType = DamageType.Physical;

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

        DamageInfo info = new(
            _damage,
            _damageType,
            gameObject,
            transform.position,
            Vector3.up
            );

        target.TakeDamage(info);
    }
}
