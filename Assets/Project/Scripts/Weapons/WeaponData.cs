using UnityEngine;
using Assets.Project.Scripts.Combat;

namespace Assets.Project.Scripts.Weapons
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Combat/Weapon Data")]

    public class WeaponData : ScriptableObject
    {
        [Header("Display Info")]
        public string weaponName = "Vanguard Heavy Rifle";

        [Header("Combat Stats")]
        public float fireRate = 0.12f;
        public int damage = 25;
        public DamageType damageType = DamageType.Physical;
        public float bulletSpeed = 35f;
        public float bulletLifetime = 2f;
        public float spreadAngle = 2f;

        [Header("Prefab Reference")]
        public Projectile bulletPrefab;
    }
}