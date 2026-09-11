using Assets.Project.Scripts.Weapons;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Project.Scripts.Core
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [Header("Pool Configuration")]
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private int _defaultPoolSize = 30;
        [SerializeField] private int _maxPoolSize = 100;

        private IObjectPool<Projectile> _projectilePool;

        private void Awake()
        {
            // Initialize the projectile pool with the specified prefab and pool sizes
            _projectilePool = new ObjectPool<Projectile>(
                createFunc: () => Instantiate(_projectilePrefab),
                actionOnGet: projectile => projectile.gameObject.SetActive(true),
                actionOnRelease: projectile => projectile.gameObject.SetActive(false),
                actionOnDestroy: projectile => Destroy(projectile.gameObject),
                collectionCheck: false, // Set to true if you want to check for duplicates in the pool (useful for debugging)
                defaultCapacity: _defaultPoolSize,
                maxSize: _maxPoolSize
            );
        }

        public Projectile SpawnProjectile(Vector3 position, Quaternion rotation, WeaponData weaponData, GameObject owner)
        {
            // Get a projectile from the pool
            Projectile projectile = _projectilePool.Get();

            projectile.transform.SetPositionAndRotation(position, rotation);

            projectile.Initialize(weaponData, owner, _projectilePool);
            return projectile;
        }
    }
}
