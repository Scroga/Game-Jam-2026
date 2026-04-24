using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float damage = 10f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.z = 0;

        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
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
