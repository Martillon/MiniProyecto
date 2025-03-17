using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable, IHeal
{
    [Header("Player Stats")]
    public int maxHealth = 100;
    
    [Header("Audio")]
    AudioSource audioSource;
    public AudioClip healSound;
    public AudioClip deathSound;
    public AudioClip damageSound;
    
    private int currentHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HUDManager.instance.UpdateHealthBar(currentHealth, maxHealth);
        
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
        
        PlaySound(healSound);
        HUDManager.instance.UpdateHealthBar(currentHealth, maxHealth);
        
        Debug.Log(gameObject.name + " got healed " + healAmount + ". Health left: " + currentHealth);
    }
    
    private void Die()
    {
        Debug.Log(gameObject.name + " died");
    }
    
    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
}
