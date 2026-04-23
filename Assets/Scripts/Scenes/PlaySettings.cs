using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlaySettings : MonoBehaviour
{
    public UnityEvent PauseOn;
    public UnityEvent PauseOff;

    public GameObject SettingsCanvas;

    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider sfxSlider;
    public static bool IsPaused { get; private set; } = false;

    private void Start()
    {
        SettingsCanvas.SetActive(false);
        LoadVolume();
    }

    public void OnPause() {
        PauseOn?.Invoke();
        SettingsCanvas.SetActive(true);
    }

    public void OffPause() {
        PauseOff?.Invoke();
        SettingsCanvas.SetActive(false);
        SaveSettings();
    }

    public void TogglePause() {
        if (IsPaused) OffPause();
        else OnPause();
        IsPaused = !IsPaused;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            TogglePause();
        }
    }

    private void OnDisable()
    {
        if (IsPaused)
        {
            IsPaused = false;
        }
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void Next() {
        LevelManager.Instance.LoadScene("Cameras", "CrossFade");
        MusicManager.Instance.StopMusic();
    }

    public void Exit() {
        LevelManager.Instance.LoadScene("Menu", "CircleWipe");
        MusicManager.Instance.PlayMusic("Menu");
    }

    public void SaveSettings()
    {

        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}
