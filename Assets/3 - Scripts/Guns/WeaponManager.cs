using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private WeaponController[] weapons;
    
    [Header("IK Settings")]
    RightHandIkTarget rightHandIkTarget;
    LeftHandIkTarget leftHandIkTarget;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] shootClips;
    public AudioClip reloadClip;
    public AudioClip emptyClip;
    
    private int currentWeaponIndex = 0;
    private WeaponController currentWeapon;
    private Coroutine shootingCoroutine;
    private PlayerAnimatorScript playerAnimatorScript;

    private void Start()
    {
        InitializeWeapons();
        EquipWeapon(currentWeaponIndex);
        audioSource = GetComponent<AudioSource>();
        HUDManager.singleton.UpdateWeaponImage(currentWeapon.gunImage, currentWeapon.gunName);
        playerAnimatorScript = GetComponentInChildren<PlayerAnimatorScript>();
        
        rightHandIkTarget = currentWeapon.GetComponentInChildren<RightHandIkTarget>();
        leftHandIkTarget = currentWeapon.GetComponentInChildren<LeftHandIkTarget>();
        playerAnimatorScript.AssignIK(rightHandIkTarget, leftHandIkTarget);
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
        HUDManager.singleton.UpdateAmmoBar(currentWeapon.magazineAmmo, currentWeapon.currentAmmo);
        
        rightHandIkTarget = currentWeapon.GetComponentInChildren<RightHandIkTarget>();
        leftHandIkTarget = currentWeapon.GetComponentInChildren<LeftHandIkTarget>();
        playerAnimatorScript.AssignIK(rightHandIkTarget, leftHandIkTarget);
    }

    public bool GetCurrentWeapon()
    {
        return currentWeapon.automatic;
    }

    public void SetShootingState(bool isShooting)
    {
        if (currentWeapon != null && Time.timeScale > 0)
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
            if (audioSource != null && currentWeapon.magazineAmmo > 0)
            {
                PlayShootAudio();
            }
            else
            {
                PlayEmptyAudio();
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
        audioSource.pitch = Random.Range(0.4f, 0.6f);
        audioSource.volume = Random.Range(0.4f, 0.6f);
        audioSource.PlayOneShot(audioSource.clip);
    }
    
    private void PlayEmptyAudio()
    {
        Debug.Log("Playing empty audio");
        audioSource.clip = emptyClip;
        audioSource.pitch = Random.Range(0.4f, 0.6f);
        audioSource.volume = Random.Range(0.4f, 0.6f);
        audioSource.PlayOneShot(audioSource.clip);
    }

    private void PlayReloadAudio()
    {
        Debug.Log("Playing reload audio");
        audioSource.clip = reloadClip;
        audioSource.pitch = Random.Range(0.4f, 0.7f);
        audioSource.volume = Random.Range(0.4f, 0.7f);
        audioSource.PlayOneShot(audioSource.clip);
    }
}