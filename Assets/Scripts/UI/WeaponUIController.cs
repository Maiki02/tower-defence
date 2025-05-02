using UnityEngine;
using TMPro;

public class WeaponUIController : MonoBehaviour, IWeaponObserver
{
    [SerializeField] private PlayerWeaponController playerWeaponController;
    [SerializeField] private TextMeshProUGUI weaponNameText;

    private void OnEnable()
    {
        playerWeaponController.RegisterObserver(this);
    }

    private void OnDisable()
    {
        playerWeaponController.UnregisterObserver(this);
    }


    public void OnWeaponChanged(WeaponBase newWeapon)
    {
        // Si tus WeaponData tienen un campo 'weaponName', y tu WeaponBase expone ese dato:
        /*if (newWeapon is WeaponBase wb && wb.data != null)
        {
            weaponNameText.text = wb.Data.weaponName;
        }
        else
        {
            // Fallback: el nombre de la clase
            weaponNameText.text = newWeapon.GetType().Name;
        }*/
    }
}
