using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy settings")]
    public int maxHealth = 100;
    private int currentHealth;
    
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
            Die();
        }
    }
    
    private void Die()
    {
        Debug.Log(gameObject.name + " died");
        Destroy(gameObject);
    }
}
