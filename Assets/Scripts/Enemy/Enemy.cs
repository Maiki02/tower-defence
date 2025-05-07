using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 3f; //Velocidad de movimiento del enemigo
    [SerializeField] private float rotationSpeed = 10f; //Para que el enemigo rote cuando se mueve
    [SerializeField] private float stoppingDistance = 0.5f; //Distancia en la que el enemigo para
    [SerializeField] private float attackRange = 1f; //Distancia de ataque del enemigo
    [SerializeField] private float attackDamage = 10f; //Daño del ataque del enemigo
    [SerializeField] private float attackCooldown = 1.5f; //Cooldown entre ataques
    [SerializeField] private float maxHealth = 50f; //Vida máxima del enemigo
    [SerializeField] private EnemyType enemyType = EnemyType.Normal; //Tipo de enemigo

    [Header("Target")]
    [SerializeField] private Transform followTarget; //El enemigo sigue a este objeto (jugador o torre)

    [Header("Effects")]
    [SerializeField] private float flashDuration = 0.2f; //Duración del efecto de flash al recibir daño
    [SerializeField] private Color flashColor = new Color(1, 0, 0, 0.5f); //Color del efecto de flash al recibir daño

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
            this.PlaySound();
            animator.SetTrigger("Attack");
            targetDamageable.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
    }

    private void PlaySound()
    {
        if (enemyType == EnemyType.Dwarf)
            AudioController.Instance.PlaySFX(SoundType.EnemyAttack);
        else
            AudioController.Instance.PlaySFX(SoundType.TowerAttack);
    }

    public override void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0f)
        {
            Die();
            return;
        }
        //Debug.Log($"Enemy took {amount} damage, remaining health: {CurrentHealth}");
    }

    public void GetHit(float amount, Vector3 attackOrigin, Vector3 attackDir, float knockbackStrength)
    {
        // Flash feedback
        if (rend != null) StartCoroutine(FlashRoutine());

        // Apply damage
        TakeDamage(amount);

        // Knock-back y un pequeño salto, para todos los enemigos igual
        const float jumpForce = 2f;
        Vector3 impulse = attackDir.normalized * knockbackStrength + Vector3.up * jumpForce;
        rb.AddForce(impulse, ForceMode.Impulse);
    }

    public override void Die()
    {
        // Efectos de muerte (animación)    
        animator.SetTrigger("Death");
        AudioController.Instance.PlaySFX(SoundType.EnemyDeath);
        StartCoroutine(WaitForDeathAnimation());


    }


    private IEnumerator WaitForDeathAnimation()
    {
        // Marcar al enemigo como “muerto” para impedir otras acciones
        isDeath = true;

        // Duración total de la animación y contador de tiempo transcurrido
        float dur = 2f;
        float elapsed = 0f;

        // Posición inicial y posición final (caída al suelo)
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(startPos.x, GetDeathYPosition(), startPos.z);

        // Preparar el material para el fade-out
        Material mat = rend.material;
        Color orig = mat.color;

        // Bucle de animación: mientras no se cumpla la duración
        while (elapsed < dur)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dur);

            // Fade-out del color del material (de opaco a transparente)
            mat.color = new Color(orig.r, orig.g, orig.b, 1f - t);

            // Movemos la posición (bajar hacia endPos)
            transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        // Una vez terminada la animación:
        // - Despawn del enemigo
        // - Reset de su vida para cuando se reutilice desde el pool
        EnemySpawner.Instance.DespawnEnemy(gameObject, enemyType);
        CurrentHealth = maxHealth;
        isDeath = false;
        rend.material.color = originalColor; // Reset color
    }


    private float GetDeathYPosition()
    {
        // Devuelve la posición Y a la que queremos que caiga el enemigo
        if (enemyType == EnemyType.Normal)
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
