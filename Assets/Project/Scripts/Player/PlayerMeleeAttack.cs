using Assets.Project.Scripts.Weapons;
using Assets.Project.Scripts.Combat;
using UnityEngine;

namespace Assets.Project.Scripts.Player
{
    public class PlayerMeleeAttack : MonoBehaviour
    {
        [SerializeField] private MeleeWeaponData _weapon;
        [SerializeField] private Transform _attackOrigin; // Empty child GameObject representing the origin point of the melee attack
        [SerializeField] private LayerMask _enemyLayer; // Set this to the layer(s) that your enemies are on

        private readonly Collider[] _hitBuffer = new Collider[32]; // Buffer to store hit results

        private float _nextAttackTime;

        public void TryAttack()
        {
            if (_weapon == null || _attackOrigin == null) return;

            if (Time.time < _nextAttackTime) return;

            _nextAttackTime = Time.time + _weapon.AttackInterval;

            // Sphere in front of the player to detect enemies within attack range

            Vector3 center = _attackOrigin.position + _attackOrigin.forward * (_weapon.AttackRange / 2);

            int hitCount = Physics.OverlapSphereNonAlloc
                (
                    center,
                    _weapon.HitRadius,
                    _hitBuffer,
                    _enemyLayer
                );

            for (int i = 0; i < hitCount; i++)
            {
                Collider coll = _hitBuffer[i];

                if (coll == null) continue;
                if (coll.gameObject == gameObject) continue; // Skip self
                if (!coll.TryGetComponent<IDamageable>(out var target)) continue; // Skip if the target is not damageable

                    DamageInfo info = DamageInfo.FromMeleeWeapon
                    (
                        _weapon,
                        gameObject,
                        coll.ClosestPoint(_attackOrigin.position),
                        Vector3.up
                    );

                    target.TakeDamage(info);
            }
        }
    }
}
