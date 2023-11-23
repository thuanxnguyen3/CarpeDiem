using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{


    public void Resume()
    {
        Time.timeScale = 1f;
        Player player = GetComponentInParent<Player>();
        player.GetComponent<PlayerStats>().isPaused = false;
        gameObject.SetActive(false);
        
    }


    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
