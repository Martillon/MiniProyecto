using UnityEngine;

public class Trap : MonoBehaviour, IDealDamage
{
    [Header("Trap settings")]
    public int damage = 10;
    
    public int GetDamage()
    {
        return damage;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        
        if (damageable != null)
        {
            damageable.TakeDamage(GetDamage());
        }
    }
}
