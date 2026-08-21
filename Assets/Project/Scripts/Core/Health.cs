using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 100;
    public int CurrentHealth { get; private set; }

    // Events for UI bars, sound effects, or destruction logic to listen to
    public event Action<int, int> OnHealthChanged; // (current, max)
    public event Action OnDeath;
    private void Awake()
    {
        CurrentHealth = _maxHealth;
    }

    public void TakeDamage(int damageAmount, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - damageAmount);
        OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
