using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour, IDealDamage
{
    [Header("Shoot settings")]
    public Transform firePoint; // Point of origin of the shot
    public float fireRange = 100f; // Max shot distance
    public LayerMask hitMask; // Hit objects
    public float fireRate = 0.2f; // Time between shots
    public int damage = 10; // Damage per shot
    public KeyCode shootKey = KeyCode.Mouse0;
    public KeyCode aimKey = KeyCode.Mouse1;
    public float spread = 0.02f; // Backwards force when shooting
    public bool automatic = false;
    
    private Camera mainCamera;
    private bool canShoot = true;

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
            spread = 0.02f;
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
            spread = 0.02f;
            Shoot();
            Invoke(nameof(ResetShoot), fireRate);
        }
        
    }
    
    public int GetDamage()
    {
        return damage;
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

    public void ResetShoot()
    {
        canShoot = true;
    }

    public void OnDrawGizmos()
    {
        if (firePoint == null || mainCamera == null) return;

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
