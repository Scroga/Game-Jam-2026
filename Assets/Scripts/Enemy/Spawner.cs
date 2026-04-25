using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate;
    public float spawnDistance;
    public int maxEnemiesCount = 10;

    private float timeSinceLastSpawn;
    private List<GameObject> enemies = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {
        enemies.RemoveAll(enemy => enemy == null);

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnRate && maxEnemiesCount > enemies.Count)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0.0f;
        }
    }

    void SpawnEnemy() {
        Vector2 spawnPosition = Random.insideUnitSphere.normalized * spawnDistance;
        spawnPosition += (Vector2)transform.position;

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemies.Add(enemy);
    }
}
