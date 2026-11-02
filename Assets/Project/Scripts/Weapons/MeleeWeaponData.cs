using UnityEngine;

namespace Assets.Project.Scripts.Weapons
{
    [CreateAssetMenu(fileName = "Melee_", menuName = "Combat/Melee Weapon Data")]
    public class MeleeWeaponData : ScriptableObject
    {
        [SerializeField] private string _weaponName = "Knife";
        public string WeaponName => _weaponName;

        [SerializeField] private float _attackInterval = 0.5f;
        public float AttackInterval => _attackInterval;

        [SerializeField] private float _attackRange = 1.4f;
        public float AttackRange => _attackRange;

        [SerializeField] private float _hitRadius = 0.5f;
        public float HitRadius => _hitRadius;

        [Header("Hit Effects")]
        [SerializeField] private HitEffectProfile _hitEffects;
        public HitEffectProfile HitEffects => _hitEffects;
    }
}
