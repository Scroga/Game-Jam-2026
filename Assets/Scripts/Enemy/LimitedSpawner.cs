using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LimitedSpawner : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject> onFinish;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float spawnDistance = 5f;
    [SerializeField] private int maxEnemiesCount = 10;

    private int spawnedEnemiesCount;
    private float timeSinceLastSpawn;
    private bool finished;

    private readonly List<GameObject> aliveEnemies = new();

    private void Update()
    {
        if (finished) return;

        aliveEnemies.RemoveAll(enemy => enemy == null);

        if (spawnedEnemiesCount >= maxEnemiesCount)
        {
            if (aliveEnemies.Count == 0)
            {
                Finish();
            }

            return;
        }

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnRate)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        Vector2 spawnPosition = (Vector2)transform.position + direction * spawnDistance;

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        aliveEnemies.Add(enemy);
        spawnedEnemiesCount++;
    }

    private void Finish()
    {
        finished = true;
        onFinish.Invoke(gameObject);
        gameObject.SetActive(false);
    }
}