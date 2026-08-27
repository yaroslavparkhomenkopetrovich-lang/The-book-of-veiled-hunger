using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace VeiledHunger.Core
{
    public class Health : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private int _maxHealth = 100;
        public int CurrentHealth { get; private set; }
        public int MaxHealth => _maxHealth;
        public bool IsDead { get; private set; }

        // Events for UI bars, sound effects, or destruction logic to listen to
        public event Action<int, int> OnHealthChanged;
        public event Action<DamageInfo> OnHitTaken; // (current, max)
        public event Action OnDeath;

        private void Awake()
        {
            CurrentHealth = _maxHealth;
        }

        public void TakeDamage(DamageInfo info)
        {
            if (IsDead || info.Amount <=0) return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - info.Amount);
            OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void ApplyModifiedDamage(int calculatedAmount, DamageInfo originalInfo)
        {
            if (IsDead || calculatedAmount <= 0) return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - calculatedAmount);

            OnHitTaken?.Invoke(originalInfo);
            OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(int amount, bool allowOverheal = false)
        {
            if (IsDead || amount <= 0) return;

            CurrentHealth += amount;
            if (!allowOverheal && CurrentHealth > _maxHealth)
            {
                CurrentHealth = _maxHealth;
            }

            OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);
        }

        public void Kill()
        {
            if (IsDead) return;

            CurrentHealth = 0;
            OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);
            Die();
        }

        private void Die()
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}