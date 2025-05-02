using UnityEngine;

public interface IDamageable
{
    public Transform transform { get; }

    void TakeDamage(float amount);
    
    void Die();
}