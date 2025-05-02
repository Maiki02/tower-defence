using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, IHealth, IDamageable
{
    public float MaxHealth { get; private set; } = 100f; // Salud máxima del personaje
    public float CurrentHealth { get; private set; } // Salud actual del personaje

    protected virtual void Awake()
    {
        // Inicializa la salud actual al valor máximo
        CurrentHealth = MaxHealth;
    }

    public virtual void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    // Se llama cuando el personaje muere
    public virtual void Die()
    {
        Destroy(gameObject);
    }

}
