using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float followDistance = 0.2f;
    [SerializeField] private Transform target;
    [SerializeField] private bool follow = true;
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool flipSprite = true;
    [SerializeField] private string soundName;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        rb.constraints = follow
            ? RigidbodyConstraints2D.FreezeRotation
            : RigidbodyConstraints2D.FreezeAll;
    }

    public void StartFollow() {
        if (follow) return;

        if (soundName != null)
            SoundManager.Instance.PlaySound2D(soundName);

        follow = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void FlipSprite(Vector2 dir)
    {
        if (!flipSprite || spriteRenderer == null) return;

        spriteRenderer.flipX = dir.x < 0;
    }

    public void StopFollow() {
        follow = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void FixedUpdate()
    {
        if (target == null) return;
        Vector2 dir = ((Vector2)target.position - rb.position);
        float dist = dir.magnitude;
        FlipSprite(dir.normalized);
        if (follow)
        {
            if (dist > followDistance)
            {
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.MovePosition(rb.position + dir.normalized * speed * Time.fixedDeltaTime);
            }
            else {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }
}
