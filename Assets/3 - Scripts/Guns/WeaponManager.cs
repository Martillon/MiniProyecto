using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private WeaponController[] weapons;
    
    private int currentWeaponIndex = 0;
    private WeaponController currentWeapon;

    private void Start()
    {
        InitializeWeapons();
        EquipWeapon(currentWeaponIndex);
    }

    private void InitializeWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].ActivateWeapon(false);
        }
    }

    private void EquipWeapon(int index)
    {
        if (currentWeapon != null)
        {
            currentWeapon.ActivateWeapon(false);
        }

        currentWeapon = weapons[index];
        currentWeapon.ActivateWeapon(true);
    }

    public void SwitchWeapon()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex >= weapons.Length)
        {
            currentWeaponIndex = 0;
        }

        EquipWeapon(currentWeaponIndex);
    }

    public void SetShootingState(bool isShooting)
    {
        if (currentWeapon != null)
        {
            currentWeapon.SetShootingState(isShooting);
        }
    }
    
    public void ReloadWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Reload();
        }
    }
    
    public void AddAmmo(int amount)
    {
        if (currentWeapon != null)
        {
            currentWeapon.AddAmmo(amount);
            Debug.Log("Munición añadida: " + amount);
        }
    }
}