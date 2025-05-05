using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [Header("Attack Setup")] 
    [Tooltip("Punto desde el cual se origina el ataque")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float knoackbackForce = 5f;

    public override float Attack()
    {
        //Debug.Log($"Melee attack - {Time.time - lastAttackTime } < {data.cooldown}");
        //if (Time.time - lastAttackTime < data.cooldown) return;

        lastAttackTime = Time.time;

        // Lanzamos la animación antes de detectar
        // GetComponent<Animator>().Play(data.attackAnimation.name);

        // Reproducimos el sonido del ataque
        AudioController.Instance.PlaySFX(SoundType.KnifeAttack);

        // esto devuelve TODOS los colliders 3D en un radio concreto
        Collider[] hits = Physics.OverlapSphere(
            attackPoint.position,
            data.range,
            damageableLayers
        );

        float total=0f;
        foreach (var hit in hits){
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null){
                Vector3 dir = (enemy.transform.position - attackPoint.position).normalized;
                enemy.GetHit(
                    data.damage,
                    attackPoint.position,
                    dir,
                    knoackbackForce
                );
                total += data.damage;
            }
        }
        return total;     
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        // Dibujamos la zona de impacto en el editor
        Gizmos.DrawWireSphere(attackPoint.position, data.range);
    }
}
