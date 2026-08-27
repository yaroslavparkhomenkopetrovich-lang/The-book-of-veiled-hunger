using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using VeiledHunger.Core;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private float _maxHealth = 50f;
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private float _attackCooldown = 1.0f;

    [Header("Targeting & Performance")]
    [SerializeField] private float _pathUpdateInterval = 0.2f;

    private NavMeshAgent _agent;
    private Transform _playerTransform;
    private float _currentHealth;
    private float _lastAttackTime;
    private float _nextPathUpdateTime;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _currentHealth = _maxHealth;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache references to player via tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");\
        if (playerObj != null )
        { 
            _playerTransform = playerObj.transform; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerTransform == null) return;

        UpdatePathfinding();
    }

    private void UpdatePathfinding()
    {
        // Throttle path calculation (runs 5 times/sec instead of every frame)
        if (Time.time >= _nextPathUpdateTime)
        {
            _nextPathUpdateTime = Time.time + _pathUpdateInterval;

            if (_agent.isOnNavMesh)
            {
                _agent.SetDestination(_playerTransform.position);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Time.time < _lastAttackTime + _attackCooldown) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out IDamageable playerDamageable))
            {
                // Extract collision contact point and surface normal
                ContactPoint contact = collision.GetContact(0);

                playerDamageable.TakeDamage(_attackDamage, contact.point, contact.normal);
                _lastAttackTime = Time.time;
            }
        }
    }

    public void TakeDamage (int damageAmount, Vector3 hitPoint, Vector3 hitNormal)
    {
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0)
        {
            Sleep();
        }
    }

    private void Sleep()
    {
        // Destroy instance on lethal hit
        Destroy(gameObject);
    }   
}
