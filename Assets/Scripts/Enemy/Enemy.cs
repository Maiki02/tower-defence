using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 10f; //Para que el enemigo rote cuando se mueve
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

    private bool isDeath = false;

    private float lastAttackTime = Mathf.NegativeInfinity;
    private Rigidbody rb;
    private Renderer rend;
    private Animator animator;
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
        animator = GetComponentInChildren<Animator>();

        SetTargetDamageableByEnemyType(enemyType);
    }

    private void FixedUpdate()
    {
        if (targetDamageable == null) return;
        if (isDeath) return; // No se mueve si está muerto

        var isMoving = this.GoToTarget();
        animator.SetBool("isWalking", isMoving);

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
    private float GetColliderDistance()
    {
        Collider myCol = GetComponent<Collider>();
        Collider targetCol = followTarget.GetComponent<Collider>();
        Vector3 myPoint = myCol.ClosestPoint(followTarget.position);
        Vector3 tgtPoint = targetCol.ClosestPoint(transform.position);

        Vector3 diff = tgtPoint - myPoint;
        diff.y = 0f;
        return diff.magnitude;
    }

    // Retorna true si se mueve
    private bool GoToTarget()
    {
        float dist = GetColliderDistance();
        if (dist > stoppingDistance)
        {
            // dirección para mover/rotar
            Vector3 rawDir = followTarget.position - transform.position;
            rawDir.y = 0f;
            Vector3 dir = rawDir.normalized;

            // rotar suavemente
            Quaternion targetRot = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));

            // avanzar
            rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    private void TryAttackTarget()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        if (GetColliderDistance() <= attackRange)
        {
            animator.SetTrigger("Attack");
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
        animator.SetTrigger("Death");
        StartCoroutine(WaitForDeathAnimation());


    }


    private IEnumerator WaitForDeathAnimation()
    {
        isDeath = true;
        float dur = 2f; // duración de tu animación Death
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(startPos.x, GetDeathYPosition(), startPos.z);
        //Le ponemos una y de -0.5 para que se vea que cae al suelo y no se quede flotando

        // Le aplicamos un fade out al material del enemigo
        Material mat = rend.material;
        Color orig = mat.color;

        while (elapsed < dur)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dur);

            // Fade out
            mat.color = new Color(orig.r, orig.g, orig.b, 1f - t);

            // Bajar hasta y=0
            transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        // Al final, despawnea
        EnemySpawner.Instance.DespawnEnemy(gameObject, enemyType);
        CurrentHealth = maxHealth;
        isDeath = false;
    }

    private float GetDeathYPosition()
    {
        // Devuelve la posición Y a la que queremos que caiga el enemigo
        if(enemyType == EnemyType.Normal)
            return -1f;
        else if (enemyType == EnemyType.Giant)
            return -2f;
        else if (enemyType == EnemyType.Dwarf)
            return -0.5f;
        return 0f;
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
