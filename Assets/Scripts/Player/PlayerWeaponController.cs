using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private MeleeWeapon melee;
    [SerializeField] private RangedWeapon ranged;

    void Update()
    {
        /*if (Input.GetButtonDown("Fire1"))
            melee.Attack();
        if (Input.GetButtonDown("Fire2"))
            ranged.Attack();*/
    }
}
