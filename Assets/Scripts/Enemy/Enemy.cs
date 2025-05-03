using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private EnemyType enemyType = EnemyType.Normal;

    [Header("Target")]
    [SerializeField] private Transform followTarget;

    [Header("Effects")]
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private Color flashColor = new Color(1, 0, 0, 0.5f);

    private float lastAttackTime = Mathf.NegativeInfinity;
    private Rigidbody rb;
    private Renderer rend;
    private Color originalColor;
    private IDamageable targetDamageable;

    protected override void Awake()
    {
        base.Awake();

        // Initialize health
        CurrentHealth = maxHealth;

        // Cache components
        rb = GetComponent<Rigidbody>();
        rend = GetComponentInChildren<Renderer>();
        originalColor = rend.material.color;

        SetTargetDamageableByEnemyType(enemyType);
    }

    private void FixedUpdate()
    {
        if (targetDamageable == null) return;

        this.GoToTarget();

        TryAttackTarget();
    }

    public void SetTargetDamageableByEnemyType(EnemyType enemyType)
    {
        var target = GetTargetByEnemyType(enemyType);
        if (target != null)
        {
            SetTargetDamageable(target);
        }
    }

    private IDamageable GetTargetByEnemyType(EnemyType enemyType)
    {
        if (enemyType == EnemyType.Dwarf)
        {
            return FindObjectOfType<Player>();
        }
        else if (enemyType == EnemyType.Normal || enemyType == EnemyType.Giant)
        {
            return FindObjectOfType<Tower>();
        }
        return null;
    }

    private void SetTargetDamageable(IDamageable target)
    {
        targetDamageable = target;
        followTarget = target.transform;
    }

    private void GoToTarget()
    {
        Vector3 direction = followTarget.position - rb.position;
        direction.y = 0f; //Para que no suba ni baje

        direction.Normalize(); // Para que no se mueva más rápido en diagonal

        if (direction.magnitude > stoppingDistance)
        {
            Vector3 move = direction.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
    }

    private void TryAttackTarget()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;
        Debug.Log("Intentamos ataque");
        Vector3 diff = followTarget.position - transform.position;
        diff.y = 0f;
        Debug.Log($"diff.magnitude <= attackRange: {diff.magnitude} <= {attackRange}");
        if (diff.magnitude <= attackRange)
        {
            targetDamageable.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
    }

    public override void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0f)
        {
            Die();
            return;
        }
        Debug.Log($"Enemy took {amount} damage, remaining health: {CurrentHealth}");
    }

    public void GetHit(float amount, Vector3 attackOrigin, Vector3 attackDir, float knockbackStrength)
    {
        // Flash feedback
        if (rend != null) StartCoroutine(FlashRoutine());

        // Apply damage
        TakeDamage(amount);

        // Knock-back
        rb.AddForce(attackDir.normalized * knockbackStrength, ForceMode.Impulse);

    }

    public override void Die()
{
    // Efectos de muerte (animación)
    //GetComponent<Animator>()?.Play("Death");

    // Devolver al pool en vez de destruir
    EnemySpawner.Instance.DespawnEnemy(gameObject, this.enemyType);

    // Reset de estado interno 
    CurrentHealth = maxHealth;
}

    private IEnumerator FlashRoutine()
    {
        rend.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = originalColor;
    }

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }
}
