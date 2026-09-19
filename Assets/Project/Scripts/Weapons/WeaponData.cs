using UnityEngine;
using Assets.Project.Scripts.Combat;
using UnityEngine.Serialization;

namespace Assets.Project.Scripts.Weapons
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Combat/Weapon Data")]

    public class WeaponData : ScriptableObject
    {
        [Header("Display Info")]
        [SerializeField] private string _weaponName = "Vanguard Heavy Rifle";
        public string WeaponName => _weaponName;

        [Header("Combat Stats")]
        [FormerlySerializedAs("fireRate")]
        [SerializeField] private float _fireInterval = 0.12f;
        public float FireInterval => _fireInterval;
        [FormerlySerializedAs("damage")]
        [SerializeField] private int _damage = 25;
        public int Damage => _damage;
        [SerializeField] private DamageType _damageKind = DamageType.Physical;
        public DamageType DamageKind => _damageKind;

        [Header("Projectile")]
        [SerializeField] private float _bulletSpeed = 35f;
        public float BulletSpeed => _bulletSpeed;
        [SerializeField] private float _bulletLifetime = 2f;
        public float BulletLifetime => _bulletLifetime;
        [SerializeField] private float _spreadAngle = 2f;
        public float SpreadAngle => _spreadAngle;

        // --- Retro-inspired Stopping Power and Knockback effects ---

        [Header("Hit Reaction")]
        [Tooltip("Duration of the stun effect")]
        [SerializeField] private float _stunDuration;
        public float StunDuration => _stunDuration;

        [Tooltip("Force of the knockback effect")]
        [SerializeField] private float _knockbackForce;
        public float KnockbackForce => _knockbackForce;

        [Tooltip("Direction of push or pull")]
        [SerializeField] private ImpulseType _impulseKind = ImpulseType.PushAwayFromBullet;
        public ImpulseType ImpulseKind => _impulseKind;

        [Header("Prefab Reference")]
        [SerializeField] private Projectile _bulletPrefab;
        public Projectile BulletPrefab => _bulletPrefab;
    }
}