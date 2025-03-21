using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamageable , IDealDamage
{
    [Header("Explosive Barrel Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int explosionDamage = 100;
    [SerializeField] private float explosionRadius = 5f;
    
    private int currentHealth;
    private bool hasExploded = false;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }
    
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        Debug.Log(gameObject.name + " got damaged" + damage + ". Health left: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Explode();
        }
    }
    
    public int GetDamage()
    {
        return explosionDamage;
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
