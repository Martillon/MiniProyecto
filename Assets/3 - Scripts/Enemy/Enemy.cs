using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
     [Header("Enemy settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Death drops")]
    public GameObject[] drops;

    // Event to notify when an enemy dies
    public static event Action<Enemy> OnEnemyDeath;

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

    private void Die()
    {
        if (drops.Length > 0)
        {
            int randomIndex = Random.Range(0, drops.Length + 2);
            if(randomIndex < drops.Length) Instantiate(drops[randomIndex], transform.position, Quaternion.identity);
        }

        // Play Event
        Debug.Log("Enemy died, launching event");
        OnEnemyDeath?.Invoke(this);

        Debug.Log(gameObject.name + " died");
        Destroy(gameObject);
    }
}
