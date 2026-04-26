using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject> onDeath;
    [SerializeField] public float health;
    [SerializeField] protected Slider healthSlider;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected PopUpDamage popUpDamagePrefab;
    [SerializeField] protected Color damageColor;
    [SerializeField] protected Color healColor;
    [SerializeField] public bool isDamageable = true;
    protected float maxHealth;
    protected float powerOfDamageNumbersVelocity = 3.0f;

    protected virtual void Start()
    {
        maxHealth = health;

        if (healthSlider != null)
            healthSlider.value = health / maxHealth;
    }

    protected virtual void Update()
    {
    }
    private void SpawnPopUp(float amount, Color color, float scale)
    {
        if (popUpDamagePrefab == null) return;

        PopUpDamage popUp = Instantiate(
            popUpDamagePrefab,
            transform.position,
            Quaternion.identity
        );

        Vector2 damagePopupVelocity = Random.insideUnitCircle.normalized * powerOfDamageNumbersVelocity;

        popUp.Setup(amount, color, damagePopupVelocity, scale);
    }

    public void AddHealth(float amount)
    {
        float oldHealth = health;

        health = Mathf.Clamp(health + amount, 0, maxHealth);

        float currentHeal = health - oldHealth;
        float scaleMultiplier = currentHeal / maxHealth + 1.0f;

        SpawnPopUp(currentHeal, healColor, scaleMultiplier);

        if (healthSlider != null)
            healthSlider.value = health / maxHealth;
    }

    //Remove health
    public void RemoveHealth(float damage)
    {
        if (!isDamageable) return;

        float oldHealth = health;

        health = Mathf.Clamp(health - damage, 0, maxHealth);

        float currentDamage = oldHealth - health;
        float scaleMultiplier = currentDamage / maxHealth + 1.0f;

        SpawnPopUp(currentDamage, damageColor, scaleMultiplier);

        if (healthSlider != null)
            healthSlider.value = health / maxHealth;

        if (health <= 0)
        {
            OnDeath();
        }
    }

    public bool IsFull() {
        return health >= maxHealth;
    }

    //Destroy object
    protected virtual void OnDeath()
    {

        if (gameObject.TryGetComponent<DropItem>(out var dropScript))
        {
            dropScript.Drop();
        }

        onDeath.Invoke(gameObject);
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
