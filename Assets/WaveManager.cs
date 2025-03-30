using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [Header("Oleadas")]
    public int totalRounds = 3;
    public List<WaveSettings> waves; // min and max enemies
    public float timeBetweenRounds = 5f;
    public float timeBetweenSpawns = 1f;

    [Header("Spawns")]
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public static event Action OnAllWavesCompleted;

    public int currentRound = 0;
    public int enemiesRemaining;
    public bool waveInProgress = false;
    
    private bool isSpawning = false;
    
    public void StartCombat()
    {
        Debug.Log("OnEnable subscribed to enemy death event");
        Enemy.OnEnemyDeath += HandleEnemyDeath;
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(timeBetweenRounds);
        
        if(isSpawning) yield break; // Prevents starting a new wave if one is already in progress
        isSpawning = true;
        
        while (currentRound < totalRounds)
        {
            waveInProgress = true;

            Debug.Log($"Starting round {currentRound + 1}");

            WaveSettings wave = waves[0]; // change for current if more settings are added
            int enemyCount = Random.Range(wave.minEnemies, wave.maxEnemies + 1);
            enemiesRemaining = enemyCount;

            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            // Wait until all enemies are defeated
            yield return new WaitUntil(() => enemiesRemaining <= 0);

            waveInProgress = false;
            Debug.Log($"Round {currentRound + 1} completed");

            yield return new WaitForSeconds(timeBetweenRounds);
            currentRound++;
        }

        Debug.Log("All waves defeated");
        OnAllWavesCompleted?.Invoke();
        isSpawning = false;
    }

    private void SpawnEnemy()
    {
        // Choose a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Choose a random enemy prefab
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Instantiate the enemy
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        
        Debug.Log("Enemy spawned");
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        Debug.Log($"Event called. Remaining enemies: {enemiesRemaining}");
        enemiesRemaining--;
        Debug.Log($"Enemies left: {enemiesRemaining}");
        
        if (enemiesRemaining <= 0)
            Debug.Log($"Round finished. Waiting {timeBetweenRounds} seconds...");
    }
}
