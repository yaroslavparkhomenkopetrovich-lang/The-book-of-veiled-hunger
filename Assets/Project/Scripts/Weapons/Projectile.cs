using UnityEngine;
using UnityEngine.Pool;
using Assets.Project.Scripts.Combat;

namespace Assets.Project.Scripts.Weapons
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        [Header("Projectile Base Settings")]
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
            transform.position += transform.forward * (_weaponData.BulletSpeed * Time.deltaTime);

            // 2. Track lifetime and rycle when expired
            _timer += Time.deltaTime;
            if (_timer >= _weaponData.BulletLifetime)
            {
                Release();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // 1) Ignore the shooter who fired the projectile
            if (_instigator != null && other.gameObject == _instigator) return;

            // 2) Ignore allies with the same tag unless CanHitAllies is set to true
            if (ShouldIgnoreAlly(other.gameObject)) return;

            // 3) Deliver damage via IDamageable
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(DamageInfo.FromProjectile(_weaponData, _instigator, transform, other));

                Release();
            }
            else if(!other.isTrigger)
            {
                // If the projectile hits a non-damageable object, it should still despawn
                Release();
            }
        }

        private bool ShouldIgnoreAlly(GameObject hitObject)
        {
            if (_weaponData == null || _weaponData.HitEffects == null) return false;

            if (_weaponData.HitEffects.CanHitAllies) return false;

            // Check if the other object has the same tag as the instigator
            if (_instigator == null || _weaponData == null || _weaponData.HitEffects == null)
                return false;
            
            return !_instigator.CompareTag("Untagged") && hitObject.CompareTag(_instigator.tag);
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