using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamepass : MonoBehaviour
{
    public GameObject pausePanel,gamequitPanel;
    

    public void PauseButoon()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueButtoon()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitPlaneButton()
    {
        pausePanel.SetActive(false);
        gamequitPanel.SetActive(true);

    }
    public void NoquitButton()
    {
        ContinueButtoon();
        gamequitPanel.SetActive(false);
    }
    public void GameQuitButton()
    {
        Application.Quit();
    }
}
