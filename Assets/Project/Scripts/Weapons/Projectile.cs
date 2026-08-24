using System.Diagnostics.Tracing;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    private float _speed;
    private int _damage;
    private float _lifeTime;
    private float _timer;

    private IObjectPool<Projectile> _pool;

    public void Initialize(WeaponData data, IObjectPool<Projectile> originatingPool)
    {
        _speed = data.bulletSpeed;
        _damage = data.damage;
        _lifeTime = data.bulletLifeTime;
        _pool = originatingPool;
        _timer = 0f;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime));

        _timer = Time.deltaTime;
        if (_timer >= _lifeTime)
        {
            Release();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Deliever damage via IDemagable
        if (other.TryGetComponent<IDemageable>(out var target))
        {
            target.TakeDamage(_damage, transform.position, -transform.forward);
        }

        Release();
    }

    private void Release()
    {
        if (_pool != null)
            _pool.Release(this);
        else
            Destroy(gameObject);
    }
}