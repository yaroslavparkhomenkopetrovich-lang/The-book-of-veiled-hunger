using System;
using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    public class HealthData : MonoBehaviour
    {
        [Header("Standard Health Limits")]
        [SerializeField] private int _maxHealth = 100;

        [Header("Overhealth Limits")]
        [SerializeField] private int _maxOverhealth = 150;

        [Header("Initial State")]
        [SerializeField] private bool _isInvulnerable = false;

        [Header("Regeneration Stats Passive Data")]
        [SerializeField] private bool _canRegenerate = false;
        [SerializeField] private int _regenAmountPerTick = 2;
        [SerializeField] private float _regenTickInterval = 1.0f;
        [SerializeField] private float _regenDelayAfterDamage = 3.0f;

        public int CurrentHealth { get; set; }
        public int MaxHealth => _maxHealth;
        public int MaxOverhealth => _maxOverhealth;
        public bool IsInvulnerable { get; set; }

        public bool CanRegenerate => _canRegenerate;
        public int RegenAmountPerTick => _regenAmountPerTick;
        public float RegenTickInterval => _regenTickInterval;
        public float RegenDelayAfterDamage => _regenDelayAfterDamage;

        // State Flags
        public bool IsDead => CurrentHealth <= 0;
        public bool IsFullHealth => CurrentHealth >= _maxHealth;
        public bool IsOverhealth => CurrentHealth > _maxHealth;

        // Derived numeric Readouts
        public int OverhealAmount => Mathf.Max(0, CurrentHealth - _maxHealth);
        public float HealthPercentage => Mathf.Clamp01((float)CurrentHealth/_maxHealth);

        // Events
        public event Action<int, int> OnHealthChanged; // (current, max)
        public event Action<bool> OnOverhealthStateChanged;
        public event Action<bool> OnInvulnerabilityChanged;
        public event Action<GameObject> OnDeath;

        private void Awake()
        {
            CurrentHealth = _maxHealth;
            IsInvulnerable = _isInvulnerable;
        }

        public void SetInvulnerable(bool state)
        {
            if (IsInvulnerable == state) return;
            IsInvulnerable = state;
            OnInvulnerabilityChanged?.Invoke(IsInvulnerable);
        }

        public void NotifyChanged(bool previousOverhealState)
        {
            OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);

            if (previousOverhealState != IsOverhealth)
            {
                OnOverhealthStateChanged?.Invoke(IsOverhealth);
            }
        }

        public void NotifyDeath(GameObject attacker)
        {
            OnDeath?.Invoke(attacker);
        }
    }
}