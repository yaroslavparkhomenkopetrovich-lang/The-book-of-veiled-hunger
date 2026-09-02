using System;
using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    public class ArmorData : MonoBehaviour
    {

        [Header("Armor Pool Limits")]
        [SerializeField] private int _maxArmor = 100;

        [Header("Mitigation Stats Passive Data")]
        [Range(0f, 1f)]
        [Tooltip("Percentage of damage absorbed by armor before passing to health; e.g., 0.5 means 50% of damage is absorbed")]
        [SerializeField] private float _absorptionRate = 0.75f;

        [Tooltip("Flat damage value substracted before percentage absorption")]
        [SerializeField] private int _flatDamageReduction = 2;

        public int CurrentArmor { get; set; }
        public int MaxArmor => _maxArmor;
        public float AbsorptionRate => _absorptionRate;
        public int FlatDamageReduction => _flatDamageReduction;

        // State Flags
        public bool IsDepleted => CurrentArmor <= 0;
        public bool IsFullArmor => CurrentArmor >= _maxArmor;
        public float ArmorPercentage => _maxArmor > 0 ? Mathf.Clamp01((float)CurrentArmor / _maxArmor) : 0f;

        // Events
        public event Action<int, int> OnArmorChanged; // (current, max)
        public event Action OnArmorBroken;

        // Methods

        private void Awake()
        {
            CurrentArmor = _maxArmor;
        }

        public void NotifyChanged()
        {
            OnArmorChanged?.Invoke(CurrentArmor, _maxArmor);
        }

        public void NotifyBroken()
        {
            OnArmorBroken?.Invoke();
        }
    }
}
