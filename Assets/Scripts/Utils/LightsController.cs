using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LightsController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> lights = new();

    public float Duration = 0.8f;

    private bool isBlinkingMode = false;
    private const float blinkingDuration = 0.4f;
    private const float alternatingDuration = 0.2f;

    private void Start()
    {
        SetAll(true);
        RestartRoutine();
    }

    private Coroutine running;

    private void OnDisable()
    {
        if (running != null) StopCoroutine(running);
        running = null;
    }

    public void SwitchMode() {
        if (isBlinkingMode)
        {
            isBlinkingMode = false;
            Duration = blinkingDuration;
        }
        else {
            isBlinkingMode = true;
            Duration = alternatingDuration;
        }
        RestartRoutine();
    }

    private void RestartRoutine()
    {
        if (!isActiveAndEnabled) return;

        if (running != null) StopCoroutine(running);
        running = StartCoroutine(isBlinkingMode ? BlinkingModeLoop() : AlternatingFlashingLoop());
    }

    private void SetAll(bool active)
    {
        foreach (var light in lights)
            if (light != null) light.SetActive(active);
    }

    private IEnumerator AlternatingFlashingLoop()
    {
        while (!isBlinkingMode)
        {
            SetAll(true);
            yield return new WaitForSecondsRealtime(Duration);

            SetAll(false);
            yield return new WaitForSecondsRealtime(Duration);
        }
    }

    private IEnumerator BlinkingModeLoop()
    {
        SetAll(false);

        int i = 0;
        while (isBlinkingMode)
        {
            int j = (i - 1 + lights.Count) % lights.Count;

            if (lights[i] != null) lights[i].SetActive(false);
            if (lights[j] != null) lights[j].SetActive(true);

            i = (i + 1) % lights.Count;
            yield return new WaitForSecondsRealtime(Duration);
        }
    }
}
