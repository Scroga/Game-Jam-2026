using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float damageFrequency = 1.0f;
    private float timeSinceLastDamage = 0;
    private float maxHealth;

    public void Start()
    {
        maxHealth = health;
    }

    public void Update()
    {
        timeSinceLastDamage += Time.deltaTime;
    }

    public void TryToGetDamage(float amount)
    {
        if (timeSinceLastDamage >= damageFrequency) {
            RemoveHealth(amount);
            timeSinceLastDamage = 0.0f;
        }
    }

    public void AddHealth(float amount)
    {
        health += amount;
        health = health > maxHealth ? maxHealth : health;

        healthSlider.value = health / maxHealth;
    }

    //Remove health
    public void RemoveHealth(float damage)
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

            //Destroy(gameObject, audioSource.clip.length);
            gameObject.SetActive(false);
        }
        else
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
