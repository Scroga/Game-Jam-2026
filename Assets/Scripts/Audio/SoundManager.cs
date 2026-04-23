using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SmartSingleton<SoundManager>
{
    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx2DSource;

    public void PlaySound3D(AudioClip clip, Vector3 positon)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, positon);
        }
    }

    public void PlaySound3D(string soundName, Vector3 positon)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), positon);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName)); 
    }
}
