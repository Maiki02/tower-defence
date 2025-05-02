using UnityEngine;

public interface IDamageable
{
    public Transform transform { get; } // Necesito el transform para saber si puede ser dañado o no

    void TakeDamage(float amount);
    
    void Die();
}