using System.Collections;
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
    
    [Header("UI")]
    public GameObject deathUI;
    
    [Header("Damage Flash")]
    [SerializeField] private Renderer characterRenderer;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;
    private Color originalColor;
    
    private int currentHealth;
    private PlayerAnimatorScript animator;
    
    private void Start()
    {
        originalColor = characterRenderer.material.color;
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        HUDManager.singleton.UpdateHealthBar(currentHealth, maxHealth);
        animator = GetComponentInChildren<PlayerAnimatorScript>();
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HUDManager.singleton.UpdateHealthBar(currentHealth, maxHealth);
        
        Debug.Log(gameObject.name + " got damaged " + damage + ". Health left: " + currentHealth);
        
        PlaySound(damageSound);
        
        StartCoroutine(FlashDamage());
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashDamage()
    {
        characterRenderer.material.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        characterRenderer.material.color = originalColor;
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; //Optional: Lower the health to maxHealth if it goes over progressively
        }
        
        PlaySound(healSound);
        HUDManager.singleton.UpdateHealthBar(currentHealth, maxHealth);
        
        Debug.Log(gameObject.name + " got healed " + healAmount + ". Health left: " + currentHealth);
    }
    
    private void Die()
    {
        Debug.Log(gameObject.name + " died");
        animator.UpdateDeadAnimator(true);
        deathUI.SetActive(true);
        PlaySound(deathSound);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    
    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
}
