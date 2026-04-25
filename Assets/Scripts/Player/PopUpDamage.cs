using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpDamage : MonoBehaviour
{
    public TMP_Text amountText;
    public Vector2 initialVelocity;
    public float lifeTime = 1.0f;
    private Vector2 originalScale;

    private void Awake()
    {
        originalScale = amountText.transform.localScale;
    }

    public void Setup(float amount, Color color, Vector2 velocity, float scaleMultiplier)
    {
        amountText.text = amount.ToString("0");
        amountText.color = color;
        amountText.transform.localScale *= originalScale * scaleMultiplier;

        if (gameObject.TryGetComponent<Rigidbody2D>(out var rb)) {
            rb.velocity = velocity;
        }

        Destroy(gameObject, lifeTime);
    }
}
