using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private float speed = 10f;
    private float maxDistance;
    private Vector3 origin;

    public void Initialize(float dmg, float range)
    {
        damage = dmg;
        maxDistance = range;
        origin = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (Vector3.Distance(origin, transform.position) >= maxDistance)
            Destroy(gameObject); // o devuélvelo al pool
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var dmg))
            dmg.TakeDamage(damage);
        Destroy(gameObject);
    }
}
