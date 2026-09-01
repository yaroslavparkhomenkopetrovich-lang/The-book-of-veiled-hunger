using System;
using UnityEngine;

namespace Assets.Project.Scripts.Combat
{
    public class Health : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [SerializeField] private int _maxHealth = 100;
        private int _currentHealth;

        // Optional armor reference (cached via interface on Awake)
        private IArmor _armor;

        // Events for UI bars, sound effects, or destruction logic to listen to
        public event Action<int, int> OnHealthChanged; // (current, max)
        public event Action<DamageInfo> OnHitTaken; 
        public event Action<GameObject> OnDeath;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;
        public bool IsDead { get; private set; }

        private void Awake()
        {
            _currentHealth = _maxHealth;
            // Automatically find an armor component on the same GameObject if it exists
            _armor = GetComponent<IArmor>();
        }

        public void TakeDamage(DamageInfo info)
        {
            if (IsDead || info.Amount <=0) return;

            // 1) Filter damage through armor if available
            int finalDamage = _armor != null ? _armor.MitigateDamage(info) : info.Amount;

            // 2) Apply the final damage to health
            _currentHealth = Mathf.Max(0, _currentHealth - finalDamage);

            // 3) Notify listeners (ui, vfx, sound, etc.)
            OnHitTaken?.Invoke(info);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            //// 4) Check for death
            //if (_currentHealth <= 0)
            //{
            //    Die(info.Instigator);
            //}
        }

        public void Die(GameObject attacker = null)
        {
            if (IsDead) return;
            IsDead = true;

            //OnDeath?.Invoke(attacker);
            //gameObject.SetActive(false); // Optional: disable the GameObject on death
        }

        public void Heal(int amount, bool allowOverheal = false)
        {
            if (IsDead || amount <= 0) return;

            _currentHealth += amount;
            if (!allowOverheal && _currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }

            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }
    }
}