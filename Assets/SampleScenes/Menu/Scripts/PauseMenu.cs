using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    public void Resume()
    {
        Time.timeScale = 1f;
        Player player = GetComponentInParent<Player>();
        player.GetComponent<PlayerStats>().isPaused = false;
        pauseMenu.gameObject.SetActive(false);
        
    }

    public void SettingsMenu()
    {
        settingsMenu.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
    }

    public void BackToMenu()
    {
        settingsMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
    }


    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
