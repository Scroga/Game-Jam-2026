using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionAbility : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private HealthScript healthScript;

    [SerializeField] private float frequency = 2.0f;
    [SerializeField] private float explosionScale = 2.0f;
    [SerializeField] private float explosionColliderSizeMultiplier = 2.0f;

    [SerializeField] private float growTime = 0.2f;
    [SerializeField] private float stayTime = 0.1f;
    [SerializeField] private float shrinkTime = 0.2f;

    [SerializeField] private float damage = 10.0f;

    [Header("Amount")]
    [SerializeField] private Slider amountSlider;
    [SerializeField] private float amountSpentOnExplosion = 50.0f;
    [SerializeField] private float amount = 100f;
    private float maxAmount;

    [Header("Popup")]
    [SerializeField] private PopUpDamage popUpDamagePrefab;
    [SerializeField] private Color getColor = Color.green;
    [SerializeField] private float powerOfDamageNumbersVelocity = 1.0f;

    private CircleCollider2D explosionCollider;
    private float normalRadius;
    private float explosionRadius;

    private float sinceLastExplosion;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        maxAmount = amount;

        if (amountSlider != null)
            amountSlider.value = amount / maxAmount;

        explosionCollider = GetComponent<CircleCollider2D>();

        if (explosionCollider == null)
        {
            Debug.LogError("ExplosionAbility needs CircleCollider2D.");
            enabled = false;
            return;
        }

        normalRadius = explosionCollider.radius;
        explosionRadius = normalRadius * explosionColliderSizeMultiplier;
    }

    public bool IsFull() {
        return maxAmount <= amount;
    }

    private void SpawnPopUp(float value, Color color, float scale)
    {
        if (popUpDamagePrefab == null) return;
        if (Mathf.Approximately(value, 0f)) return;

        PopUpDamage popUp = Instantiate(
            popUpDamagePrefab,
            transform.position,
            Quaternion.identity
        );

        Vector2 randomDirection = Random.insideUnitCircle;

        if (randomDirection == Vector2.zero)
            randomDirection = Vector2.up;

        Vector2 popupVelocity = randomDirection.normalized * powerOfDamageNumbersVelocity;

        popUp.Setup(value, color, popupVelocity, scale);
    }

    public void AddAmount(float value)
    {
        float oldAmount = amount;

        amount = Mathf.Clamp(amount + value, 0, maxAmount);

        float realAddedAmount = amount - oldAmount;
        float scaleMultiplier = realAddedAmount / maxAmount + 1.0f;

        SpawnPopUp(realAddedAmount, getColor, scaleMultiplier);

        UpdateSlider();
    }

    public void RemoveAmount(float value)
    {
        float oldAmount = amount;

        amount = Mathf.Clamp(amount - value, 0, maxAmount);

        float realRemovedAmount = oldAmount - amount;
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        if (amountSlider != null)
            amountSlider.value = amount / maxAmount;
    }

    protected void SpawnExplosion()
    {
        if (sinceLastExplosion < frequency) return;
        if (explosionPrefab == null) return;
        if (amount - amountSpentOnExplosion < 0f) return;

        ParticleSystem explosion = Instantiate(
            explosionPrefab,
            transform.position,
            Quaternion.identity
        );

        explosion.transform.localScale = Vector3.one * explosionScale;
        Destroy(explosion.gameObject, explosion.main.duration);

        sinceLastExplosion = 0.0f;

        RemoveAmount(amountSpentOnExplosion);

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(GrowAndShrink());
    }

    private IEnumerator GrowAndShrink()
    {
        if (healthScript != null)
            healthScript.isDamageable = false;

        yield return ChangeRadius(normalRadius, explosionRadius, growTime);
        yield return new WaitForSeconds(stayTime);
        yield return ChangeRadius(explosionRadius, normalRadius, shrinkTime);

        if (healthScript != null)
            healthScript.isDamageable = true;

        currentCoroutine = null;
    }

    private IEnumerator ChangeRadius(float from, float to, float time)
    {
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / time;
            explosionCollider.radius = Mathf.Lerp(from, to, t);

            yield return null;
        }

        explosionCollider.radius = to;
    }

    private void Update()
    {
        sinceLastExplosion += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnExplosion();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !healthScript.isDamageable)
        {
            if (collision.gameObject.TryGetComponent(out HealthScript enemyHealth))
            {
                enemyHealth.RemoveHealth(damage);
            }
        }
    }
}