using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField] GameObject bossPrefab;
    [SerializeField] float delay = 1f;

    public void Start()
    {
        bossPrefab.SetActive(false);
    }

    public void Spawn()
    {
        StartCoroutine(SpawnWithDelay());
    }

    private IEnumerator SpawnWithDelay()
    {
        MusicManager.Instance.PlayMusic("Boss", 0.5f);

        yield return new WaitForSeconds(delay);

        bossPrefab.SetActive(true);
        gameObject.SetActive(false);
    }
}
