using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private Toggle m_MenuToggle;
	private float m_TimeScaleRef = 1f;
    private float m_VolumeRef = 1f;
    private bool m_Paused;
    public GameObject pauseMenu;


    void Awake()
    {
        m_MenuToggle = GetComponent <Toggle> ();
	}


    private void MenuOn ()
    {
        m_TimeScaleRef = Time.timeScale;
        Time.timeScale = 0f;

        m_VolumeRef = AudioListener.volume;
        AudioListener.volume = 0f;

        m_Paused = true;
    }


    public void MenuOff ()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }


    public void OnMenuStatusChange ()
    {
        if (m_MenuToggle.isOn && !m_Paused)
        {
            MenuOn();
        }
        else if (!m_MenuToggle.isOn && m_Paused)
        {
            MenuOff();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }


#if !MOBILE_INPUT
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            Player player = GetComponentInParent<Player>();
            player.cam.lockCursor = false;
            player.cam.canMove = false;
            //m_MenuToggle.isOn = !m_MenuToggle.isOn;
            //Cursor.visible = m_MenuToggle.isOn;//force the cursor visible if anythign had hidden it


        }
	}
#endif

}
