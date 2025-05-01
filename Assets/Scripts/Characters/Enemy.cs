using UnityEngine;

public class Enemy : CharacterBase
{
    public float moveSpeed = 3f;

    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        // Buscar al Player por Tag
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;
    }

    void Update()
    {
        if (playerTransform == null) return;
        MoveTowardsPlayer();
    }

    /// Se acerca al jugador
    private void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}
