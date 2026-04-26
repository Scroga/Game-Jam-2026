using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 2f;

    private Vector3 closedPosition;

    private Coroutine moveCoroutine;

    private void Start()
    {
        closedPosition = transform.position;
    }

    public void Open()
    {
        MoveTo(target.position);
    }

    public void Close()
    {
        MoveTo(closedPosition);
    }

    private void MoveTo(Vector3 destination)
    {
        if (transform.position == destination) return;
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveDoor(destination));
    }

    private IEnumerator MoveDoor(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                speed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = destination;
        moveCoroutine = null;
    }
}
