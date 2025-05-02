using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected WeaponData data;
    private float lastAttackTime;

    public float Cooldown => data.cooldown;


    public void TryAttack()
    {
        if (Time.time - lastAttackTime < data.cooldown) return;
        lastAttackTime = Time.time;
        Attack(); // lo implementan las subclases
    }

    // Cada arma concreta define su Attack()
    public abstract void Attack();
}
