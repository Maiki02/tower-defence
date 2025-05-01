using UnityEngine;

public class Enemy : CharacterBase
{
    public float moveSpeed = 3f;
    public float stoppingDistance = 0.5f;

    private Transform towerTransform;
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        towerTransform = GameObject.FindWithTag("Tower")?.transform;
    }

    void FixedUpdate()
    {
        if (towerTransform == null) return;

        // a) Vector plano XZ
        Vector3 flatDiff = towerTransform.position - rb.position;
        flatDiff.y = 0f;

        // b) Check rango
        if (flatDiff.magnitude <= stoppingDistance) return;

        // c) Normalizamos y movemos
        Vector3 dir = flatDiff.normalized;
        Vector3 nextPos = rb.position + dir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);

        // d) Rotación sólo en Y
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        rb.MoveRotation(targetRot);
    }
}
