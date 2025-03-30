using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int currentRound = 0;
    public int enemiesRemaining;
    public bool waveInProgress = false;
    
    public void StartCombat()
    {
        Debug.Log("OnEnable subscribed to enemy death event");
        Enemy.OnEnemyDeath += HandleEnemyDeath;
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        if (currentRound >= totalRounds)
        {
            Debug.Log("All rounds finished!");
            yield break;
        }

        waveInProgress = true;

        yield return new WaitForSeconds(timeBetweenRounds);

        WaveSettings wave = waves[Mathf.Clamp(currentRound, 0, waves.Count - 1)];
        int enemyCount = Random.Range(wave.minEnemies, wave.maxEnemies);
        enemiesRemaining = enemyCount;

        Debug.Log($"Round {currentRound + 1} - Enemies: {enemyCount}");

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        // Wait until all enemies are defeated
        while (enemiesRemaining > 0)
        {
            yield return null;
        }

        currentRound++;
        waveInProgress = false;

        StartCoroutine(StartNextWave());
    }

    private void SpawnEnemy()
    {
        // Choose a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Choose a random enemy prefab
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Instantiate the enemy
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
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
    }
}
