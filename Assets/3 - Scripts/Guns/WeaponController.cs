using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class WeaponController : MonoBehaviour, IDealDamage
{
    #region Variables
    [Header("Gun Settings")]
    public LayerMask hitMask; // Hit objects
    public float fireRange = 100f; // Max shot distance
    public float fireRate = 0.2f; // Time between shots
    public int damage = 10; // Damage per shot
    public float spread = 0.02f; // Backwards force when shooting
    public bool automatic = false;
    
    [Header("Ammo settings")]
    public int magazineSize = 30;
    public int magazineAmmo = 0;
    public int currentAmmo = 300;
    public int maxAmmo = 300;
    
    [Header("Keybindings")]
    public KeyCode shootKey = KeyCode.Mouse0;
    public KeyCode aimKey = KeyCode.Mouse1;
    
    private Camera mainCamera;
    private bool canShoot = true;

    #endregion
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        
        //Automatic shooting
        
        if (Input.GetKey(shootKey) && Input.GetKey(aimKey) && canShoot && automatic)
        {
            
            spread = 0f;
            Shoot();
            Invoke(nameof(ResetShoot), fireRate);
            
        } 
        else if(Input.GetKey(shootKey) && canShoot && automatic)
        {
            
            spread = 0f;
            Shoot();
            Invoke(nameof(ResetShoot), fireRate);
            
        }
        
        //Manual Shooting
        
        if (Input.GetKeyDown(shootKey) && Input.GetKey(aimKey) && canShoot && !automatic)
        {
            
            spread = 0f;
            Shoot();
            Invoke(nameof(ResetShoot), fireRate);
            
        } 
        else if(Input.GetKeyDown(shootKey) && canShoot && !automatic)
        {
            
            spread = 0f;
            Shoot();
            Invoke(nameof(ResetShoot), fireRate);
            
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        
        Vector3 direction = ray.direction;
        direction.x += UnityEngine.Random.Range(-spread, spread);
        direction.y += UnityEngine.Random.Range(-spread, spread);
        
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, direction, out hit, fireRange, hitMask))
        {
            Debug.Log("Impact: " + hit.collider.name);

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(GetDamage());
            }
            
            //Add sfx
        }

        // Add VFX
    }

    #region Interface & Invoke

    public int GetDamage()
    {
        return damage;
    }

    public void ResetShoot()
    {
        canShoot = true;
    }

    #endregion

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
