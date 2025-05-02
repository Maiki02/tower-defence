using UnityEngine;
using System;

public class PlayerWeaponController : MonoBehaviour
{
    // Evento para que otros sistemas (UI, VFX) sepan cuándo cambias de arma
    public event Action<IWeapon> OnWeaponChanged;

    [SerializeField] private MeleeWeapon meleeWeapon;
    [SerializeField] private RangedWeapon rangedWeapon;

    private WeaponBase currentWeapon;

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
        if (currentWeapon == null) return;

        // Unificamos el disparo en Fire1
        if (Input.GetButtonDown("Fire"))
            currentWeapon.Attack();
    }

    private void EquipWeapon(WeaponBase weapon)
    {
        if (currentWeapon == weapon) return;

        currentWeapon = weapon;
        // Activo sólo el GameObject del arma seleccionada
        meleeWeapon.gameObject.SetActive(weapon == meleeWeapon);
        rangedWeapon.gameObject.SetActive(weapon == rangedWeapon);
        // Notifico a los observers
        OnWeaponChanged?.Invoke(currentWeapon);
    }

    // Por si quieres consultarlo desde otro script
    public IWeapon GetCurrentWeapon() => currentWeapon;
}
