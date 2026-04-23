using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : SmartSingleton<MusicManager>
{
    [SerializeField]
    private MusicLibrary musicLibrary;
    [SerializeField]
    private AudioSource musicSource;

    public void PlayMusic(string trackName, float fadeDuration = 0.5f) {
        StartCoroutine(AnimaterMusicCrossFage(musicLibrary.GetClipFromName(trackName), fadeDuration));
    }

    IEnumerator AnimaterMusicCrossFage(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }

        musicSource.clip = nextTrack;
        musicSource.Play();
        percent = 0;
        while (percent < 1) {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    }

    Coroutine musicRoutine;

    public void StopMusic(float fadeDuration = 0.5f)
    {
        if (musicRoutine != null) StopCoroutine(musicRoutine);
        musicRoutine = StartCoroutine(FadeOutAndStop(fadeDuration));
    }

    IEnumerator FadeOutAndStop(float fadeDuration)
    {
        float startVol = musicSource.volume;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVol, 0f, t / fadeDuration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = null;   
        musicSource.volume = startVol;
    }
}
