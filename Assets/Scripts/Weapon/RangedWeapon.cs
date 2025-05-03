using UnityEngine;


public class RangedWeapon : WeaponBase
{   
    [SerializeField] private Camera cam;

    public override float Attack()
    {
        //if (Time.time - lastAttackTime < data.cooldown) return;
        //lastAttackTime = Time.time;

        // Animación
        // GetComponent<Animator>().Play(data.attackAnimation.name);

        // Hitscan
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, data.range, damageableLayers))
        {
            hit.collider.GetComponent<IDamageable>()?.TakeDamage(data.damage);
            // Aquí spawn pool de efecto de impacto en hit.point
            return data.damage;
        }

        // Pool de muzzle flash + sonido
        SpawnMuzzleFlash();
        PlayShotSound();
        return 0;
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
