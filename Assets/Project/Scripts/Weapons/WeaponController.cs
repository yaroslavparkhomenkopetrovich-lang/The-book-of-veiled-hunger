using UnityEngine;
using Assets.Project.Scripts.Core;

namespace Assets.Project.Scripts.Weapons
{
    /// <summary>
    /// WeaponController is responsible for managing the firing of projectiles.
    /// It uses an object pool to efficiently reuse projectile instances.
    /// </summary>
    public class WeaponController : MonoBehaviour
    {
        [Header("WeaponSetup")]
        [SerializeField] private WeaponData _weaponData;
        [SerializeField] private Transform _firePoint;

        // Timer tracing the excact game time when the next round can be fired
        private float _nextFireTime = 0f;

        [Header("Spawner Reference")]
        [SerializeField] private ProjectileSpawner _projectileSpawner;

        private void Awake()
        {
            if (_projectileSpawner == null)
            {
                _projectileSpawner = FindAnyObjectByType<ProjectileSpawner>();
            }
        }

        /// <summary>
        /// Attempts to fire the weapon based on fireRate cadence.
        /// Can be called ontinously in update by player or AI.
        /// </summary>
        public void TryShoot()
        {
            // Guard Clause: Prevent firing if on cooldown or if no weapon is equipped
            if (_weaponData == null || _projectileSpawner == null || _firePoint == null || Time.time < _nextFireTime) return;

            // Calculate next allowed firing timestamp
            _nextFireTime = Time.time + _weaponData.FireInterval;

            ExecuteShot();
        }

        private void ExecuteShot()
        {
            // 1. Calculate random spread deviation on the Ordinate axis (horizontal sweep)
            float randomSpread = Random.Range(-_weaponData.SpreadAngle, _weaponData.SpreadAngle);
            Quaternion spreadRotation = Quaternion.Euler(0f, randomSpread, 0f);

            // 2. Combine the fire point's rotation with the calculated spread rotation
            Quaternion finalRotation = _firePoint.rotation * spreadRotation;

            _projectileSpawner.SpawnProjectile(_firePoint.position, finalRotation, _weaponData, gameObject);
        }
    }
}
