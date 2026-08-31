using System;
using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    public class Armor : MonoBehaviour, IArmor
    {
        [Header("Armor Settings")]
        [SerializeField] private int _maxArmor = 100;

        [Range(0f, 1f)]
        [Tooltip("Percentage of incomming damage that is absorbed by the armor before breaking (e.g., 0.7 means 70% absorbed, 30% goes directly to HP")]
        [SerializeField] private float _absorptionRate = 0.75f;

        [Header("Flat Resistance")]
        [Tooltip("Flat damage reduction deducted before absorption calculations")]
        [SerializeField] private int _flatDamageReduction = 2;

        private int _currentArmor;

        // Event for UI and audio triggers
        public event Action<int, int> OnArmorChanged; // (current, max)
        public event Action OnArmorBroken;

        public int CurrentArmor => _currentArmor;
        public int MaxArmor => _maxArmor;

        private void Awake()
        {
            _currentArmor = _maxArmor;
        }

        public int MitigateDamage(DamageInfo info)
        {
            // If armor is fully depleted, 100% of damage goes to health
            if (_currentArmor <= 0)
            {
                return info.Amount;
            }

            // 1) Apply treshold reduction, (e.g., small caliber bullets deal less base damage)
            int effectiveDamage = Mathf.Max(1, info.Amount - _flatDamageReduction);

            // 2) Calculate how much damage armor atempts to absorb vs how much damage goes to HP
            int damageToArmor = Mathf.RoundToInt(effectiveDamage * _absorptionRate);
            int passingDamage = effectiveDamage - damageToArmor;

            // 3) Substract from current armor pool
            if (_currentArmor >= damageToArmor)
            {
                _currentArmor -= damageToArmor;
            }
            else
            {
                // Armor is depleted, remaining damage goes to health
                int leftoverDamage = damageToArmor - _currentArmor;
                passingDamage += leftoverDamage;
                _currentArmor = 0;
                OnArmorBroken?.Invoke();
            }

            OnArmorChanged?.Invoke(CurrentArmor, _maxArmor);
            return passingDamage;
        }

        public void RepairArmor(int amount)
        {
            if (amount <= 0 || _currentArmor >= _maxArmor) return;

            _currentArmor = Mathf.Min(_maxArmor, _currentArmor + amount);
            OnArmorChanged?.Invoke(_currentArmor, _maxArmor);
        }
    }
}