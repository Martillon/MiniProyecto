using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    [Header("Ammo Settings")]
    [SerializeField] private int ammoAmount = 30;

    public int ProvideAmmo()
    {
        Destroy(gameObject); // Destruir el objeto después de recogerlo
        return ammoAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        WeaponManager weaponManager = other.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.AddAmmo(ammoAmount);
            Destroy(gameObject);
        }
    }
}
