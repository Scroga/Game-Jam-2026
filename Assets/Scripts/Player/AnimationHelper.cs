using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    public bool infiniteLoop = true;
    public float duration = 5.5f;
    public bool destroyOnEnd = true;
    public ParticleSystem[] particles;

    private float startTime;
    private bool isPlaying = true;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (!infiniteLoop && isPlaying && Time.time >= startTime + duration)
        {
            StopVFX();
        }
    }

    public void StopVFX()
    {
        isPlaying = false;

        foreach (ParticleSystem ps in particles)
        {
            if (ps != null)
                ps.Stop();
        }

        if (destroyOnEnd)
            Destroy(gameObject, 1.0f);
    }

    public void PlayVFX()
    {
        foreach (ParticleSystem ps in particles)
        {
            if (ps != null)
                ps.Play();
        }

        startTime = Time.time;
        isPlaying = true;
    }
}
