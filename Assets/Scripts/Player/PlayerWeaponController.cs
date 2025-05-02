using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerWeaponController : MonoBehaviour
{
    //Observers para notificar el cambio de arma y mostrarlo en la pantalla
    private readonly List<IWeaponObserver> observers = new List<IWeaponObserver>();
    
    

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
        if (Input.GetButtonDown("Fire"))
            CurrentWeapon.Attack();
    }

    private void EquipWeapon(WeaponBase weapon)
    {
        if (CurrentWeapon == weapon) return;

        CurrentWeapon = weapon;
        // Activo sólo el GameObject del arma seleccionada
        meleeWeapon.gameObject.SetActive(weapon == meleeWeapon);
        rangedWeapon.gameObject.SetActive(weapon == rangedWeapon);

        // Notifico a los observers
        CurrentWeapon = weapon;
        NotifyObservers();
    }

    public void RegisterObserver(IWeaponObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void UnregisterObserver(IWeaponObserver observer)
    {
        observers.Remove(observer);
    }

    // Llama a cada observador
    private void NotifyObservers()
    {
        foreach (var obs in observers)
            obs.OnWeaponChanged(CurrentWeapon);
    }
}
