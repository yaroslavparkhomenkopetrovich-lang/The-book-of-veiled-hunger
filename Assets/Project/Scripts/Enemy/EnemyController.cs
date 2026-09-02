using UnityEngine;
using Assets.Project.Scripts.Enemy;
using Assets.Project.Scripts.Combat;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(HealthData))]
[RequireComponent(typeof(EnemyMeleeAttack))]
public class EnemyController : MonoBehaviour
{
    private EnemyMovement _movement;
    private EnemyMeleeAttack _attack;
    private HealthData _healthData;
    private Transform _playerTransform;
    private IDamageable _playerDamageable;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
        _attack = GetComponent<EnemyMeleeAttack>();
        _healthData = GetComponent<HealthData>();   
    }

    public void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)   
        {
            _playerTransform = player.transform;
            _playerDamageable = player.GetComponent<IDamageable>();
        }
    }

    // If a player within range, stop and attack
    private void Update()
    {
        if (_playerTransform == null || _playerDamageable == null || _healthData.IsDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        if (_attack.CanAttack(distanceToPlayer) && _playerDamageable != null)
        {
            _movement.Stop();
            _attack.Attack(_playerDamageable);
        }
        else
        {
            _movement.Resume();
            _movement.MoveTowards(_playerTransform.position);
        }
    }
}
