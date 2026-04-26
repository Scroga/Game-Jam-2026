using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitch : MonoBehaviour
{
    [SerializeField] private Sprite idle;
    [SerializeField] private Sprite attack;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetIdle() {
        spriteRenderer.sprite = idle;
    }
    public void SetAttack()
    {
        spriteRenderer.sprite = attack;
    }
}
