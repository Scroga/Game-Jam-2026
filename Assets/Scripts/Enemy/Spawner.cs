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
    
    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnRate)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0.0f;
        }
    }

    void SpawnEnemy() {
        Vector2 spawnPosition = Random.insideUnitSphere.normalized * spawnDistance;
        spawnPosition += (Vector2)transform.position;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
