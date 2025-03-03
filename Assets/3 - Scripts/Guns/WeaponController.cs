using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [Header("Configuración del Disparo")]
    public Transform firePoint; // Point of origin of the shot
    public float fireRange = 100f; // Max shot distance
    public LayerMask hitMask; // Hit objects
    public float fireRate = 0.2f; // Time between shots
    public int damage = 10; // Damage per shot
    public KeyCode shootKey = KeyCode.Mouse2;
    public KeyCode aimKey = KeyCode.Mouse1;
    public float recoil = 1f; // Backwards force when shooting

    private float nextFireTime = 0f;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // 1. From the camera, shoot a ray to the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, fireRange, hitMask))
        {
            targetPoint = hit.point; // Point of impact
        }
        else
        {
            targetPoint = ray.origin + ray.direction * fireRange; // Shoot to the end of the range
        }

        // 2. Now we shoot from the gun's fire point to the target point
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        if (Physics.Raycast(firePoint.position, direction, out hit, fireRange, hitMask))
        {
            Debug.Log("Impact: " + hit.collider.name);

            // Apply damage to the enemy
            /*EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }*/

            // Add SFX
        }

        // Add VFX
    }

    public void OnDrawGizmos()
    {
        if (firePoint == null || mainCamera == null) return;

        // 1. Simulate shoot from camera to the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
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

        // 2. Draw the line from the gun's fire point to the target point
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(firePoint.position, targetPoint);

        // 3. Ray from the camera to the target point
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(mainCamera.transform.position, targetPoint);
    }
}
