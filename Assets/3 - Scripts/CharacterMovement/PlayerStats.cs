using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable, IHeal
{
    [Header("Player Stats")]
    public int maxHealth = 100;
    
    private int currentHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        Debug.Log(gameObject.name + " got damaged " + damage + ". Health left: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; //Optional: Lower the health to maxHealth if it goes over progressively
        }
        
        Debug.Log(gameObject.name + " got healed " + healAmount + ". Health left: " + currentHealth);
    }
    
    private void Die()
    {
        Debug.Log(gameObject.name + " died");
    }
}
