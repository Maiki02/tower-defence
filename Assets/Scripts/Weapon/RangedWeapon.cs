using UnityEngine;


public class RangedWeapon : WeaponBase
{   
    [SerializeField] private Camera cam;

    public override void Attack()
    {
        //if (Time.time - lastAttackTime < data.cooldown) return;
        //lastAttackTime = Time.time;

    Debug.Log("Vamo a atacar con Ranged");
        // Animación
        // GetComponent<Animator>().Play(data.attackAnimation.name);

        // Hitscan
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, data.range, damageableLayers))
        {
            Debug.Log("Hay damageable players");
            hit.collider.GetComponent<IDamageable>()?.TakeDamage(data.damage);
            // Aquí spawn pool de efecto de impacto en hit.point
        }

        // Pool de muzzle flash + sonido
        SpawnMuzzleFlash();
        PlayShotSound();
    }

    private void SpawnMuzzleFlash()
    {
        // Spawn pool de efecto de disparo
        // Instantiate(muzzleFlashPrefab, camera.transform.position, Quaternion.identity);
    }

    private void PlayShotSound()
    {
        // Spawn pool de sonido de disparo
        // AudioSource.PlayClipAtPoint(shotSound, camera.transform.position);
    }


}
