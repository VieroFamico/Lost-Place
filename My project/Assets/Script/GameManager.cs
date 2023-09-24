using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Slider hpBar;
    public TextMeshProUGUI hpValue;
    public Slider staminaBar;
    public TextMeshProUGUI bulletCounter;
    public TextMeshProUGUI medkitCounter;
    public TextMeshProUGUI glowstickCounter;
    public TextMeshProUGUI notification;
    public GameObject deathscreen;
    public GameObject winscreen;
    public GameObject pausescreen;

    private GameObject player;
    private PlayerProperties playerprop;
    private GameObject door;
    private SettingManager settingManager;
    public bool pause;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerprop = player.GetComponent<PlayerProperties>();
        door = GameObject.FindGameObjectWithTag("Door");
        settingManager = GetComponent<SettingManager>();
        settingManager.audioManager.UpdateVolume();
    }

    public void UpdateHP(float hp)
    {
        hpBar.value = hp;
        hpValue.text = hp.ToString();
    }
    public void UpdateStamina(float stamina)
    {
        staminaBar.value = stamina;
    }
    public void UpdateBullet(float ammo, float totalammo)
    {
        bulletCounter.text = ammo.ToString() + " / " + totalammo.ToString();
    }
    public void UpdateMedkit(float medkit)
    {
        medkitCounter.text = medkit.ToString();
    }
    public void UpdateGlowStick(float glowstick)
    {
        glowstickCounter.text = glowstick.ToString();
    }
    public IEnumerator Notification(string message)
    {
        notification.text += message + "\n";
        yield return new WaitForSeconds(5f);
        notification.text = "";
    }
    public void DeathScreen()
    {
        deathscreen.SetActive(true);
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            if (audio.enabled)
            {
                audio.Stop();
            }
        }
        Time.timeScale = 0f;
    }
    public void PauseScreen()
    {
        if (!pause)
        {
            Debug.Log(pause);
            pausescreen.SetActive(true);
            Time.timeScale = 0f;
            pause = true;
            Debug.Log(pause + "lal");
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach(AudioSource audio in audios)
            {
                if (audio.enabled)
                {
                    audio.Pause();
                }
            }
        }
        else if (pause && GetComponent<MenuManager>().Setting.activeSelf == false)
        {
            Debug.Log(pause);
            pausescreen.SetActive(false);
            Time.timeScale = 1f;
            pause = false;
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in audios)
            {
                if (audio.enabled && audio.transform.gameObject.activeSelf == true)
                {
                    audio.UnPause();
                }
            }
            
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Start");
        SceneManager.LoadSceneAsync("MainGame");

        player = GameObject.FindGameObjectWithTag("Player");
        playerprop = player.GetComponent<PlayerProperties>();
        door = GameObject.FindGameObjectWithTag("Door");

        playerprop.StartingSetUp();
        playerprop.manager = this;

        deathscreen.SetActive(false);
        winscreen.SetActive(false);
        pausescreen.SetActive(false);
        pause = false;

        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            audio.Stop();
        }

    }
    public void Escape()
    {
        winscreen.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
    public void UnlockDoor()
    {
        door = GameObject.FindGameObjectWithTag("Door");
        door.SetActive(false);
    }
}
