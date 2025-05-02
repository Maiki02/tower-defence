using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon Data", order = 1)]
public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected WeaponData data;
    protected float lastAttackTime;
    
    [SerializeField] protected LayerMask damageableLayers;

    public void TryAttack()
    {
        if (Time.time - lastAttackTime < data.cooldown) return;
        lastAttackTime = Time.time;
        Attack(); // lo implementan las subclases
    }

    // Cada arma concreta define su Attack()
    public abstract void Attack();
}
