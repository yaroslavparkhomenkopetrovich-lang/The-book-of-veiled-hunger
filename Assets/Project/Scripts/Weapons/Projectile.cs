using System.Diagnostics.Tracing;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;
using VeiledHunger.Core;

namespace VeiledHunger.Weapons
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        [Header("Projectile Base Settings")]
        [SerializeField] private DamageType _damageType = DamageType.Physical;

        private float _timer;
        private GameObject _instigator;
        private IObjectPool<Projectile> _pool;
        private WeaponData _weaponData;

        // Called when spawned from object pool or instantiated
        public void Initialize(WeaponData data, GameObject instigator, IObjectPool<Projectile> originatingPool)
        {
            _weaponData = data;
            _pool = originatingPool;
            _instigator = instigator;
        }

        private void OnEnable()
        {
            // Resets timer automatically every time a bullet leaves the pool
            _timer = 0f;
        }

        private void Update()
        {
            if (_weaponData == null) return;

            // 1. Move forward smothly based on speed and deltaTime
            transform.position += transform.forward * (_weaponData.bulletSpeed * Time.deltaTime);

            // 2. Track lifetime and rycle when expired
            _timer += Time.deltaTime;
            if (_timer >= _weaponData.bulletLifetime)
            {
                Release();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Ignore the shooter who fired the projectile
            if (_instigator != null && other.gameObject == _instigator) return;

            // Deliever damage via IDemagable
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                // Find a point on collider surface closest to the projectile's position on hit
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = (transform.position - hitPoint).normalized;

                DamageInfo hitInfo = new DamageInfo(
                    _weaponData.damage,
                    _weaponData.damageType,
                    _instigator,
                    hitPoint,
                    hitNormal
                );

                target.TakeDamage(hitInfo);
                Release();
            }
            else if(!other.isTrigger)
            {
                // If the projectile hits a non-damageable object, it should still despawn
                Release();
            }
        }

        // Called when the projectile's lifetime expires or it hits a target
        private void Release()
        {
            if (_pool != null)
                _pool.Release(this);
            else
                gameObject.SetActive(false);
        }
    }
}