using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private MeleeWeapon meleeWeapon;
    [SerializeField] private RangedWeapon rangedWeapon;

    public WeaponBase CurrentWeapon {get; set; }

    void Start()
    {
        // Equipo por defecto el cuerpo a cuerpo
        EquipWeapon(meleeWeapon);
    }

    void Update()
    {
        HandleWeaponSelection();
        HandleAttackInput();
    }

    private void HandleWeaponSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EquipWeapon(meleeWeapon);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            EquipWeapon(rangedWeapon);
    }

    private void HandleAttackInput()
    {
        if (CurrentWeapon == null) return;

        // Unificamos el disparo en Fire1
        if (Input.GetButtonDown("Fire1")){ 
            var damage= CurrentWeapon.TryAttack();
            //Sumamos el daño al score
            ScoreController.Instance.AddScore((int)damage);

        }
    }

    private void EquipWeapon(WeaponBase weapon)
    {
        if (CurrentWeapon == weapon) return;

        CurrentWeapon = weapon;
        // Activo sólo el GameObject del arma seleccionada
        meleeWeapon.gameObject.SetActive(weapon == meleeWeapon);
        rangedWeapon.gameObject.SetActive(weapon == rangedWeapon);

        CurrentWeapon = weapon;
    }


}
