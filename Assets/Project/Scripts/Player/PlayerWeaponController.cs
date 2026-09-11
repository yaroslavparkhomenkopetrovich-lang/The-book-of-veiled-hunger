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

            if (Time.time >= _lastFireTime + _weaponData.fireRate)
            {
                _lastFireTime = Time.time;
                _projectileSpawner.SpawnProjectile(_firePoint.position, _firePoint.rotation, _weaponData, gameObject);
            }
        }
    }
}
