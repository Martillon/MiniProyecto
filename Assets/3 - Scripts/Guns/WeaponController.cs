using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WeaponController : MonoBehaviour, IDealDamage
{
    [Header("Gun Settings")]
    public LayerMask hitMask;
    public float fireRange = 100f;
    public float fireRate = 2f;
    public int damage = 10;
    public float spread = 0.02f;
    public bool automatic = false;

    [Header("Ammo Settings")]
    public int magazineSize = 30;
    public int magazineAmmo = 0;
    public int currentAmmo = 300;
    public int maxAmmo = 300;
    public float reloadTime = 2f;
    
    [Header("UI Settings")]
    public Sprite gunImage;
    public string gunName = "Unknown Gun";
    private string currentAmmoText;
    private string maxAmmoText;

    private Camera mainCamera;
    private bool canShootBecauseTime = true;
    private bool shootPressed = false;
    private bool isReloading = false;

    
    
    private void Start()
    {
        mainCamera = Camera.main;
        magazineAmmo = magazineSize;
    }

    private void Update()
    {
        
        if(isReloading) return;
        
        if (shootPressed && canShootBecauseTime)
        {
            Shoot();
            canShootBecauseTime = false;
            StartCoroutine(ResetShoot());
        }
    }

    public void Shoot()
    {
        if (magazineAmmo <= 0) return;

        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        Vector3 direction = ray.direction;
        direction.x += Random.Range(-spread, spread);
        direction.y += Random.Range(-spread, spread);

        RaycastHit hit;
        if (Physics.Raycast(ray.origin, direction, out hit, fireRange, hitMask))
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(GetDamage());
            }
        }
        
        HUDManager.singleton.UpdateAmmoBar(currentAmmo, magazineSize);
        magazineAmmo--;
    }

    private IEnumerator ResetShoot()
    {
        yield return new WaitForSeconds(1f / fireRate);
        canShootBecauseTime = true;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void ActivateWeapon(bool active)
    {
        gameObject.SetActive(active);
    }

    public void Reload()
    {
        if (isReloading) return;
        if (currentAmmo <= 0 || magazineAmmo == magazineSize) return;

        StartCoroutine(ReloadCoroutine());
    }
    
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        Debug.Log("Ammo picked up. New ammo amount: " + currentAmmo);
    }
    
    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = magazineSize - magazineAmmo;

        magazineAmmo += ammoNeeded;
        currentAmmo -= ammoNeeded;

        isReloading = false;

        HUDManager.singleton.UpdateAmmoBar(currentAmmo, magazineSize);
        Debug.Log("Reload complete. Ammo left: " + currentAmmo);
    }
    
    public void OnDrawGizmos()
    {
        if (mainCamera == null) return;
        // 1. Simulate shoot from camera to the center of the screen
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, fireRange, hitMask))
        {
            targetPoint = hit.point; // Point of hit
            Gizmos.color = Color.red; // Red if it hits something
            Gizmos.DrawSphere(targetPoint, 0.1f); // Draw a sphere where it hits
        }
        else
        {
            targetPoint = ray.origin + ray.direction * fireRange; // If nothing is hit, draw to the end of the range
        }

        // 3. Ray from the camera to the target point
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(mainCamera.transform.position, targetPoint);
    }
}
