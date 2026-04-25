using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float damage;
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealthScript>(out var playerHealthScript)) {
            playerHealthScript.TryToGetDamage(damage);
        }
        else if (collision.gameObject.TryGetComponent<HealthScript>(out var healthScript)) {
            healthScript.RemoveHealth(damage);
        }
    }
}
