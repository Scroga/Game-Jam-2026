using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElonMaskSpawn : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] float delay = 1f;
    private bool wasSpawned = false;

    private void Start()
    {
        boss.SetActive(false);
    }

    public void Spawn()
    {
        if (wasSpawned) return;
        wasSpawned = true;
        StartCoroutine(SpawnWithDelay());
    }

    private IEnumerator SpawnWithDelay()
    {
        SoundManager.Instance.PlaySound2D("Elon");

        yield return new WaitForSeconds(delay);

        boss.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
