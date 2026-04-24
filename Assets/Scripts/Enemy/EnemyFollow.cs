using UnityEngine;
public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 1f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    //private void Update()
    //{
    //    if (player == null) return;

    //    Vector3 direction = player.position - transform.position;
    //    direction.z = 0;

    //    transform.position += direction.normalized * speed * Time.deltaTime;
    //}

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
}
