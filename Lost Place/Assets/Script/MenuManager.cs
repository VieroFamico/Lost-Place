using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public GameObject Setting; 
    public void StartGame()
    {
        Debug.Log("Start");
        SceneManager.LoadSceneAsync("TutorialMenu");
    }
    public void ActivateSettings()
    {
        Setting.SetActive(true);
        Debug.Log("Settings");
    }
    public void DeactivateSettings()
    {
        Setting.SetActive(false);
        Debug.Log("Settings");
    }
    public void TrueStartGame()
    {
        Debug.Log("ActualStart");
        SceneManager.LoadSceneAsync("MainGame");

    }
    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }


}
