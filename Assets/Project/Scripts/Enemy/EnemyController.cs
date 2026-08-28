using UnityEngine;
using UnityEngine.AI;
using Assets.Project.Scripts.Combat;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class EnemyController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private float _attackCooldown = 1.0f;
    [SerializeField] private DamageType _attackType = DamageType.Physical; // Physical by default, claws, mele, bites

    [Header("Targeting & Performance")]
    [SerializeField] private float _pathUpdateInterval = 0.2f;

    private NavMeshAgent _agent;
    private Health _health;
    private Transform _playerTransform;
    private float _lastAttackTime;
    private float _nextPathUpdateTime;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache references to player via tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        { 
            _playerTransform = playerObj.transform; 
        }
    }

    private void OnEnable()
    {
        // Reset collider and navmesh state for reuse
        if (TryGetComponent(out Collider collider)) collider.enabled = true;
        if (_agent != null) _agent.enabled = true;
        {
            collider.enabled = true;
        }
        // Subscribe to Health's death event
        _health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        _health.OnDeath -= HandleDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if (_health.IsDead || _playerTransform == null) return;
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

                // Package the attack data into a stack-aalocated DamageInfo struct
                DamageInfo meeleHit = new(
                    _attackDamage,
                    _attackType,
                    gameObject,
                    contact.point,
                    contact.normal
                );

                playerDamageable.TakeDamage(meeleHit);
                _lastAttackTime = Time.time;
            }
        }
    }

    private void HandleDeath(GameObject attacker)
    {
        // Disable AI navigation logic
        if (_agent != null)
        {
            _agent.isStopped = true;
            _agent.enabled = false;
        }

        // Trigger death animation, loot here

        // Disable collider so bullets pass through and player can walk over the fallen enemy
        if (TryGetComponent(out Collider collider))
        {
            collider.enabled = false;
        }

        gameObject.SetActive(false); // More efficient than destroying and recreating the object if using object pooling

        //Destroy(gameObject, 2f ); // Or some pool release logic
    }
}
