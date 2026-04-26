using UnityEngine;

public class PlayerHealthScript : HealthScript
{
    [SerializeField] private float currentHeal;
    [SerializeField] private float damageFrequency = 1.0f;
    private float timeSinceLastDamage = 0;

    protected override void Start()
    {
        maxHealth = health;
        health = currentHeal;

        if (healthSlider != null)
            healthSlider.value = health / maxHealth;
    }

    protected override void Update()
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

    //Destroy object
    protected override void OnDeath()
    {
        LevelManager.Instance.OnDeath();
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
