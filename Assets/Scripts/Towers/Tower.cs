using System;
using UnityEngine;

public class Tower : MonoBehaviour, IDamageable, IHealth
{
    public static event Action<float> OnHealthChanged;     // evento para la barra

    public new Transform transform => base.transform;
    public float MaxHealth { get; private set; } = 2000f;
    public float CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        CurrentHealth = Mathf.Max(CurrentHealth, 0f);
        OnHealthChanged?.Invoke(CurrentHealth);

        Debug.Log($"Torre recibió {amount} de daño. Vida restante: {CurrentHealth}");

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        GameController.Instance.FinishGame(GameOverType.TowerDEAD);
        Debug.Log("Torre destruida");
        Destroy(gameObject);
    }
}
