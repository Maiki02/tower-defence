using System;
using System.Collections;
using UnityEngine;

public class Player : CharacterBase, ILives
{
    private Rigidbody rb;
    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;

    public int MaxLives { get; private set; } = 3;
    public int CurrentLives { get; private set; } = 3;

    public static event Action<float> OnHealthChanged;
    public static event Action<int> OnLivesChanged;
    public static event Action OnPlayerDied;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
    }

    public override void TakeDamage(float amount)
    {
        if (CurrentHealth <= 0) return;
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f); //Para evitar valores negativos
        OnHealthChanged?.Invoke(CurrentHealth); //Llamamos al evento de cambio de vida
        if (CurrentHealth == 0f) LoseLife(); //Si la vida es 0, perdemos una vida

        Debug.Log($"Player took {amount} damage. Current health: {CurrentHealth}");
    }

    public void LoseLife()
    {
        CurrentLives--;
        OnLivesChanged?.Invoke(CurrentLives); //Llamamos al evento de cambio de vidas
        if (CurrentLives > 0)
        {
            StartCoroutine(RespawnRoutine()); //Iniciamos la rutina de respawn
        }
        else
        {
            OnPlayerDied?.Invoke(); //Llamamos al evento de muerte del jugador
        }
    }

    private IEnumerator RespawnRoutine()
    {
        // bloqueo, animación de fade-out…
        yield return new WaitForSeconds(1f);
        transform.position = spawnPoint.position;
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth);
        // invulnerabilidad temporal…
        yield return new WaitForSeconds(1f);
        // fin de invulnerabilidad, reactivar controles…
    }

}
