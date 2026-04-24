using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private AudioSource audioSource;
    private float maxHealth;

    public void Start()
    {
        maxHealth = health;
    }


    public void AddHealth(int amount) {
        health += amount;
        health = health > maxHealth ? maxHealth : health;

        healthSlider.value = health / maxHealth;
    }

    //Remove health
    public void RemoveHealth(int damage)
    {
        health -= damage;
        health = health < 0 ? 0 : health;

        healthSlider.value = health / maxHealth;
        if (health <= 0)
        {
            OnDeath();
        }
    }

    //Destroy object
    private void OnDeath()
    {
        if (audioSource != null)
        {
            audioSource.Play();

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, audioSource.clip.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
