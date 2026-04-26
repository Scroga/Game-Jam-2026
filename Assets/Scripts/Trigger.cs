using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private UnityEvent<GameObject> onEnter;
    [SerializeField] private UnityEvent<GameObject> onExit;
    [SerializeField] private UnityEvent<GameObject> onStay;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    private bool IsPlayer(Collider2D other)
    {
        return player != null && other.CompareTag(player.tag);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayer(other))
            onEnter?.Invoke(other.gameObject);

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayer(other))
            onExit?.Invoke(other.gameObject);

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (IsPlayer(other))
            onStay?.Invoke(other.gameObject);

    }
}
