using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class SlotMachineBotton : MonoBehaviour
{
    private Camera _camera;

    [SerializeField]
    private LightsController _lightsController;

    [SerializeField]
    private UnityEvent action;

    [SerializeField]
    private GameObject light;

    private SpriteRenderer _sprite;
    private Color _original;
    [SerializeField]
    private Color onPressColor;
    [SerializeField]
    private Color DeactivatedColor;

    [SerializeField]
    private float duration = 0.15f;


    private bool isActive = true;

    private void Awake()
    {
        _camera = Camera.main;

        light.SetActive(true);
        _sprite = GetComponent<SpriteRenderer>();
        _original = _sprite.color;
    }

    void Update()
    {
        if (!isActive || !Input.GetMouseButtonDown(0)) return;

        Vector2 mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mouseWorld);

        if (hit != null && hit.attachedRigidbody == GetComponent<Rigidbody2D>())
        {
            OnClick();
            action.Invoke();
        }
    }

    public void Activate() {
        if (isActive) return;

        _lightsController.SwitchMode();

        isActive = true;
        light.SetActive(true);
        Transition(DeactivatedColor, _original);
    }

    public void Deactivate() {
        if (!isActive) return;

        _lightsController.SwitchMode();

        isActive = false;
        light.SetActive(false);
        Transition(_original, DeactivatedColor);
    }

    public void Transition(Color from, Color to)
    {
        StopAllCoroutines();
        StartCoroutine(DoTransition(from, to));
    }

    private IEnumerator DoTransition(Color from, Color to)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / duration);
            _sprite.color = Color.Lerp(from, to, a);
            yield return null;
        }
        _sprite.color = to;
    }

    public void Flash(Color normal, Color pressed)
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine(normal, pressed));
    }

    private IEnumerator FlashRoutine(Color normal, Color pressed)
    {
        yield return DoTransition(normal, pressed);
        yield return DoTransition(pressed, normal);
    }

    private void OnClick()
    {
        Flash(_original, onPressColor);
    }
}
