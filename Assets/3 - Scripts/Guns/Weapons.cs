using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptable Objects/Weapons")]
public class Weapons : ScriptableObject
{
    [Header("General Stats")]
    public string weaponName;
    public Sprite weaponIcon; // HUD Icon
    public GameObject weaponPrefab; // 3D Model
    public GunType type;
    
    public enum GunType
    {
        Rifle,
        Handgun
    }

    [Header("Shooting Stats")]
    public float damage = 10f;
    public float fireRate = 0.2f; // Time between shots
    public float range = 100f; // Raycast distance
    public int magazineSize = 30;
    public float reloadTime = 2f;
    
    [Header("Recoil & Spread")]
    public float recoil = 1f; // Backwards force when shooting
    public float spread = 0f; // Random spread when shooting

    [Header("Effects & Layers")]
    public LayerMask hitMask; // What the weapon hits
}
