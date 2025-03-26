using System.Runtime.CompilerServices;
using UnityEngine;

public class Trap : MonoBehaviour, IDealDamage
{
    [Header("Trap settings")] 
    public int damage = 10;
    public float explosionRadius = 3f;
    private bool hasExploded = false;
    

    public int GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }
    
    private void Explode()
    {
        
        if (hasExploded) return; 
        hasExploded = true;
        
        Collider[] colliders = new Collider[10]; 
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, colliders);

        for (int i = 0; i < hitCount; i++)
        {
            IDamageable damageable = colliders[i].GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(GetDamage());
            }
        }
        
        Destroy(gameObject);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}