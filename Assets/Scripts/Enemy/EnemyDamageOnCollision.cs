using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageOnCollision : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float damage = 10f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == player.gameObject.tag)
        {
            if (collision.gameObject.TryGetComponent(out PlayerHealthScript healthScript))
            {
                healthScript.TryToGetDamage(damage);
            }
        }
    }
}
