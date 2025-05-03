using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon Data", order = 1)]
public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected WeaponData data;
    protected float lastAttackTime;
    
    [SerializeField] protected LayerMask damageableLayers;

    //Devuelve la cantidad de daño que hizo en el ataque
    public float TryAttack()
    {
        if (Time.time - lastAttackTime < data.cooldown) return 0;
        lastAttackTime = Time.time;
        return Attack(); // lo implementan las subclases
    }

    // Cada arma concreta define su Attack() y devuelve el daño que hizo en el ataque
    public abstract float Attack();
}
