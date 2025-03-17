using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private WeaponController[] weapons;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] shootClips;
    public AudioClip reloadClip;
    
    private int currentWeaponIndex = 0;
    private WeaponController currentWeapon;
    private Coroutine shootingCoroutine;

    private void Start()
    {
        InitializeWeapons();
        EquipWeapon(currentWeaponIndex);
        audioSource = GetComponent<AudioSource>();
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
        HUDManager.singleton.UpdateWeaponImage(currentWeapon.gunImage, currentWeapon.gunName);
    }

    public bool GetCurrentWeapon()
    {
        return currentWeapon.automatic;
    }

    public void SetShootingState(bool isShooting)
    {
        if (currentWeapon != null)
        {
            if (isShooting && shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(ShootingCoroutine());
            }
            else if (!isShooting && shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
        }
    }

    // Control shooting rate synced with weapon fire rate
    private IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            currentWeapon.Shoot();

            // If audioManager plays the shoot audio
            if (audioSource != null)
            {
                PlayShootAudio();
            }

            yield return new WaitForSeconds(1f / currentWeapon.fireRate); // Sync with weapon fire rate
        }
    }
    
    public void ReloadWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Reload();

            if (audioSource != null)
            {
                PlayReloadAudio();
            }
        }
    }
    
    public void AddAmmo(int amount)
    {
        if (currentWeapon != null)
        {
            currentWeapon.AddAmmo(amount);
            Debug.Log("Ammo added: " + amount);
        }
    }
    
    private void PlayShootAudio()
    {
        Debug.Log("Playing shoot audio");
        audioSource.clip = shootClips[Random.Range(0, shootClips.Length)];
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.volume = Random.Range(0.8f, 1f);
        audioSource.PlayOneShot(audioSource.clip);
    }

    private void PlayReloadAudio()
    {
        Debug.Log("Playing reload audio");
        audioSource.clip = reloadClip;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.volume = Random.Range(0.8f, 1f);
        audioSource.PlayOneShot(audioSource.clip);
    }
}