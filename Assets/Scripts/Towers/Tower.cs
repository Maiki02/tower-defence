using UnityEngine;

public class Tower : MonoBehaviour, IDamageable, IHealth
{
    public new Transform transform => base.transform; // Necesito el transform para saber si puede ser dañado o no

    public float MaxHealth { get; private set; } = 10000f; // Salud máxima de la torre
    public float CurrentHealth { get; private set; } // Salud actual de la torre

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    // Interfaz IDamageable
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        Debug.Log($"Torre recibió {amount} de daño. Vida restante: {CurrentHealth}");

        if (CurrentHealth <= 0f){            
            Die();
        }
    }

    public void Die()
    {            
        GameController.Instance.FinishGame();

        Debug.Log("Torre destruida");
        Destroy(gameObject);
    }

    // Detectar colisiones con proyectiles (suponiendo tag "Projectile")
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Colisión con: {other.name}");
        if (other.CompareTag("Projectile"))
        {
            // Asumimos que el proyectil tiene un componente que indica daño
            /*var proj = other.GetComponent<Projectile>();
            if (proj != null)
            {
                TakeDamage(proj.Damage);
                Destroy(other.gameObject);
            }*/
        }
    }
}
