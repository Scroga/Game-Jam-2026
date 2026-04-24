using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate;
    public float spawnDistance;
    private float timeSinceLastSpawn;
    public int maxEnemiesCount = 10;
    private int currentEnemiesCount = 0;
    
    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnRate && maxEnemiesCount > currentEnemiesCount)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0.0f;
        }
    }

    void SpawnEnemy() {
        currentEnemiesCount++;
        Vector2 spawnPosition = Random.insideUnitSphere.normalized * spawnDistance;
        spawnPosition += (Vector2)transform.position;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
