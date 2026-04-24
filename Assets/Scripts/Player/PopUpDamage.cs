using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpDamage : MonoBehaviour
{
    public TMP_Text ammoText;
    public Vector2 initialVelocity;
    public float lifeTime;

    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = initialVelocity;
        Destroy(gameObject, lifeTime);
    }
}
