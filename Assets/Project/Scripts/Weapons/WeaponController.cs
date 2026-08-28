using UnityEngine;
using UnityEngine.Pool;
using VeiledHunger.Weapons;

public class WeaponController : MonoBehaviour
{
    [Header("WeaponSetup")]
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform _firePoint;

    // Timer tracing the excact game time when the next round can be fired
    private float _nextFireTime = 0f;

    // Interface to Unity generic object pool
    private IObjectPool<Projectile> _projectilePool;

    private void Awake()
    {
        // 1) Initialize the object poo
        // We Configure 4 functional callbacks (create, get, release, destroy)
        _projectilePool = new ObjectPool<Projectile>(
            createFunc: CreateProjectile,
            actionOnGet: OnGetFromPool,
            actionOnRelease: OnReleaseToPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: false, // Set to false in release builds for max speed
            defaultCapacity: 20, // Pre-allocates memory for 20 projectiles
            maxSize: 100 // Hard ceilling to prevent runaway memory usage
            );
    }

    // Callback 1: How to spawn a brand new projectile when the pool is completely empty
    private Projectile CreateProjectile()
    {
        if (_weaponData == null || _weaponData.bulletPrefab == null)
        {
            Debug.LogError("WeaponData or Projectile is missing on WeaponController!", this);
            return null;
        }

        return Instantiate(_weaponData.bulletPrefab);
    }

    // Callback 2: What to do when grabbing a bullet out of standby
    private void OnGetFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    // Callback 3: What to do when a bullet hits the target or expires
    private void OnReleaseToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    // Callback 4: What to do if the pool exceeds maximum size
    private void OnDestroyPoolObject(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    /// <summary>
    /// Attempts to fire the weapon based on fireRate cadence.
    /// Can be called ontinously in update by player or AI.
    /// </summary>
    public void TryShoot()
    {
        // Guard Clause: Prevent firing if on cooldown or if no weapon is equipped
        if (_weaponData == null || Time.time < _nextFireTime) return;

        // Calculate next allowed firing timestamp
        _nextFireTime = Time.time + _weaponData.fireRate;

        ExecuteShot();
    }

    private void ExecuteShot()
    {
        Projectile bullet = _projectilePool.Get();
        if (bullet == null) return;

        // 1. Calculate random spread deviation on the Ordinate axis (horizontal sweep)
        float randomSpread = Random.Range(-_weaponData.spreadAngle, _weaponData.spreadAngle);
        Quaternion spreadRotation = Quaternion.Euler(0f, randomSpread, 0f);

        // 2. Position the bullet at the muzzle and rotate it towards the aim direction + spread
        bullet.transform.position = _firePoint.position;
        bullet.transform.rotation = _firePoint.rotation * spreadRotation;

        // 3. Initiate speed, damage, and assign the pool reference for recycling
        bullet.Initialize(_weaponData, gameObject, _projectilePool);
    }
}
