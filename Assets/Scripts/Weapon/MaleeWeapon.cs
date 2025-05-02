using UnityEngine;


public class MeleeWeapon : WeaponBase
{
    private float lastAttackTime;


    public override void Attack()
    {
        if (Time.time - lastAttackTime < data.cooldown) return;
        lastAttackTime = Time.time;
        // Reproduce animación
        GetComponent<Animator>().Play(data.attackAnimation.name);
        // Detecta enemigos
        var hits = Physics2D.OverlapCircleAll(transform.position, data.range);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var dmg))
                dmg.TakeDamage(data.damage);
        }
    }

    // Para ver el área de golpe en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.range);
    }
}
