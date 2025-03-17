using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;
    
    [Header("Healthbar")]
    public Slider healthbar;
    public Text healthText;
    
    [Header("Ammo")]
    public Text ammoText;
    
    [Header("Weapon")]
    public Image weaponImage;
    public Text weaponName;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthbar.value = currentHealth;
        healthText.text = currentHealth + " / " + maxHealth;
    }

    public void UpdateAmmoBar(float currentAmmo, float maxAmmo)
    {
        ammoText.text = currentAmmo + " / " + maxAmmo;
    }
    
    public void UpdateWeaponImage(Sprite weaponSprite, string weapon)
    {
        weaponImage.sprite = weaponSprite;
        weaponName.text = weapon;
    }
}
