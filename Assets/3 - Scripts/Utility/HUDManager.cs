using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager singleton;
    
    [Header("Healthbar")]
    public Slider healthbar;
    public TMP_Text healthText;
    
    [Header("Ammo")]
    public TMP_Text ammoText;
    
    [Header("Weapon")]
    public Image weaponImage;
    public TMP_Text weaponName;

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
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
