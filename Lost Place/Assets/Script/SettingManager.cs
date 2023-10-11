using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingManager : MonoBehaviour
{
    public string VolumeKey = "Volume";
    public string SliderKey = "SliderValue";
    public Slider volumeSlider;
    public AudioManager audioManager;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }
    public float GetVolume()
    {
        return PlayerPrefs.GetFloat(VolumeKey, 1f);
    }
    public float GetSliderValue()
    {
        return PlayerPrefs.GetFloat(SliderKey, 1f);
    }

    public void SetVolume()
    {
        PlayerPrefs.SetFloat(VolumeKey, Mathf.Log(volumeSlider.value) * 10f);
        PlayerPrefs.SetFloat(SliderKey, volumeSlider.value);
        PlayerPrefs.Save();
        audioManager.UpdateVolume();
    }
}
