using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private List<Item> itemPrefabs;
    [Range(0f, 1f)]
    [SerializeField] private float dropChance = 0.5f;

    public void Drop()
    {
        if (itemPrefabs == null || itemPrefabs.Count == 0)
            return;

        if (Random.value > dropChance)
            return;

        int randomIndex = Random.Range(0, itemPrefabs.Count);
        Item randomItem = itemPrefabs[randomIndex];

        Instantiate(randomItem, transform.position, Quaternion.identity);
    }
}
