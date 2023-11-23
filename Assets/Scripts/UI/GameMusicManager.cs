using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMusicManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider slider;
    public MusicManager musicManager;

    private void Start()
    {
        slider.value = musicManager.mainMenuVolume;
        audioMixer.SetFloat("Volume", musicManager.mainMenuVolume);
    }

    public void SetVolume(float volume)
    {
        volume = slider.value;
        audioMixer.SetFloat("Volume", volume);
    }
}
