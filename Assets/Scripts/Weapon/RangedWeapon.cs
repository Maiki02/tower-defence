using UnityEngine;


public class RangedWeapon : WeaponBase
{
    private float lastAttackTime;

    public override void Attack()
    {
        if (Time.time - lastAttackTime < data.cooldown) return;
        lastAttackTime = Time.time;
        // Reproduce animación
        GetComponent<Animator>().Play(data.attackAnimation.name);
        // Saca un proyectil del pool o Instantiate
        var proj = Instantiate(data.projectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Initialize(data.damage, data.range);
    }
}
