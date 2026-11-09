using UnityEngine;
using Assets.Project.Scripts.Core;
using Assets.Project.Scripts.Weapons;

namespace Assets.Project.Scripts.Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [Header("Weapon Configuration")]
        [SerializeField] private WeaponData _weaponData;
        [SerializeField] private Transform _firePoint;

        [Header("Spawner Reference")]
        [SerializeField] private ProjectileSpawner _projectileSpawner;

        private float _lastFireTime;

        /// <summary>
        ///  Called by PlayerController when the player presses the fire button.
        ///  Checks fire rate and spawns a projectile if enough time has passed since the last shot.
        ///  </summary>
        ///  
        public void TryFire()
        {
            if (_weaponData == null || _projectileSpawner == null || _firePoint == null)
            {
                Debug.LogWarning("WeaponData or ProjectileSpawner/FirePoint/WeaponData is not assigned.");
                return;
            }

            if (Time.time >= _lastFireTime + _weaponData.FireInterval)
            {
                _lastFireTime = Time.time;

                // 1) Calculate random angle Y-axis for bullet spread
                float randomAngle = Random.Range(-_weaponData.SpreadAngle, _weaponData.SpreadAngle);
                Quaternion spreadRotation = Quaternion.Euler(0f, randomAngle, 0f);

                // 2) Combine fire point rotation with spread rotation
                Quaternion finalRotation = _firePoint.rotation * spreadRotation;

                _projectileSpawner.SpawnProjectile(_firePoint.position, finalRotation, _weaponData, gameObject);
            }
        }
    }
}
