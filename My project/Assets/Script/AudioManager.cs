using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public SettingManager settingManager;
    public AudioMixer master;
    private void Start()
    {
        if (PlayerPrefs.HasKey(settingManager.VolumeKey))
        {
            UpdateVolume();
        }
        settingManager.volumeSlider.value = settingManager.GetSliderValue();
    }

    public void UpdateVolume()
    {
        float volume = settingManager.GetVolume();
        master.SetFloat("master", volume);
    }
}
