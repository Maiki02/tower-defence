using UnityEngine;

public class Enemy : CharacterBase
{
    public float moveSpeed = 3f;
    public float stoppingDistance = 0.5f;

    public float attackRange = 1f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;

    private float lastAttackTime = -999f;


    private IDamageable towerDamageable;
    private IDamageable playerDamageable;

    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();

        this.GetInitialTargets();
    }

    private void GetInitialTargets()
    {
        towerDamageable = GameObject.FindGameObjectWithTag("Tower").GetComponent<IDamageable>();
        playerDamageable = GameObject.FindGameObjectWithTag("Player").GetComponent<IDamageable>();
    }

    void FixedUpdate()
    {

        // a) Vector plano XZ
        Vector3 flatDiff = towerDamageable.transform.position - rb.position;
        flatDiff.y = 0f;

        // b) Check rango
        if (flatDiff.magnitude <= stoppingDistance) return;

        // c) Normalizamos y movemos
        Vector3 dir = flatDiff.normalized;
        Vector3 nextPos = rb.position + dir * moveSpeed * Time.deltaTime;
        rb.MovePosition(nextPos);

        // d) Rotación sólo en Y
        /*Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        rb.MoveRotation(targetRot);*/

        TryAttackNearbyTargets();
    }

    private void TryAttackNearbyTargets()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        this.TryAttack(playerDamageable);
        this.TryAttack(towerDamageable);
    }

    /* Intenta atacar a algo IDamageable si está cerca.
    En caso de poder atacarlo, devuelve aplica el daño y devuelve true. */
    private bool TryAttack(IDamageable target)
    {
        if (IsTargetInRange(target.transform))
        {
            target.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
            return true;
        }
        return false;
    }

    /* Comprueba si el objetivo está dentro del rango de ataque. */
    private bool IsTargetInRange(Transform target)
    {
        Vector3 flatDiff = target.position - transform.position;
        flatDiff.y = 0f;
        return flatDiff.magnitude <= attackRange;
    }

}
