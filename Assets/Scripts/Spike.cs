using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float damage;
    [SerializeField] private bool isRunning = true;
    void Update()
    {
        if (isRunning)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    public void Run()
    {
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isRunning)
        {
            if (collision.gameObject.TryGetComponent<PlayerHealthScript>(out var playerHealthScript))
            {
                playerHealthScript.TryToGetDamage(damage);
            }
            else if (collision.gameObject.TryGetComponent<HealthScript>(out var healthScript))
            {
                healthScript.RemoveHealth(damage);
            }
        }
    }
}
