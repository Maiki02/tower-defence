using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon Data", order = 1)]
public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected WeaponData data;
    private float lastAttackTime;

    public void TryAttack()
    {
        Debug.Log($"Intentamos ataque {lastAttackTime} {Time.time} {data.cooldown}");
        if (Time.time - lastAttackTime < data.cooldown) return;
        lastAttackTime = Time.time;
        Debug.Log($"Intentamos ataque");
        Attack(); // lo implementan las subclases
    }

    // Cada arma concreta define su Attack()
    public abstract void Attack();
}
