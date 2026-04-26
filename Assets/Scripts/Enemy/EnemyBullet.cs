using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 60f;
    [SerializeField] private float liftTime;
    [SerializeField] private float damage;
    [SerializeField] private bool flipSprite = true;
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] private bool isRotating = false;

    private Transform target;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        Destroy(gameObject, liftTime);
    }

    public void Setup(Transform newTarget)
    {
        target = newTarget;

        if (target != null)
        {
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    private void Update()
    {
        if (isRotating)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    private void FlipSprite()
    {
        if (!flipSprite || spriteRenderer == null) return;
        spriteRenderer.flipY = gameObject.transform.right.x < 0;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 directionToPlayer = ((Vector2)target.position - rb.position).normalized;

        Vector3 newDirection3 = Vector3.RotateTowards(
        rb.velocity.normalized,
        directionToPlayer,
        rotateSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime,
        0f
        );


        Vector2 newDirection = ((Vector2)newDirection3).normalized;

        rb.velocity = newDirection * speed;
        FlipSprite();

        float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (player && collision.gameObject.tag == player.gameObject.tag)
        {
            if (collision.gameObject.TryGetComponent(out PlayerHealthScript healthScript))
            {
                healthScript.TryToGetDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
