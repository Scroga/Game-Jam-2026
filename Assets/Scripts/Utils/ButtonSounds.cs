using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    public void PlaySoundOnClick(string soundNameOnClick) {
        SoundManager.Instance.PlaySound2D(soundNameOnClick);
    }

    public void PlaySoundOnEnter(string soundNameOnEnter) {
        SoundManager.Instance.PlaySound2D(soundNameOnEnter);
    }
}
