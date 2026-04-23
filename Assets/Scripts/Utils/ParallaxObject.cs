using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using MyBox;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    [SerializeField] private bool _useMouse;
    [SerializeField] private Transform _target;

    public float offsetMultiplier = 0.1f;
    public float smoothTime = 0.3f;

    private Vector2 startPosition;
    private Vector3 velocity;

    private Vector3 GetTargetPosition() {
        Vector3 targetPosition = transform.position;
        if (_useMouse)
        {
            targetPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            targetPosition.z = 0;
        }
        else if (_target != null) { 
            targetPosition = _target.position;
        }
        return targetPosition;
    }

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Vector2 offset = GetTargetPosition();
        transform.position = Vector3.SmoothDamp(transform.position, startPosition + (offset * offsetMultiplier), ref velocity, smoothTime);
    }
}
