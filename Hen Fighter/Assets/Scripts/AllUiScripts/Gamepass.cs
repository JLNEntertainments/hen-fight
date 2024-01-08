using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamepass : MonoBehaviour
{
    public GameObject pausePanel,gamequitPanel;

    public GameObject DoYouWantToExitGame;

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            // Application.Quit();
            DoYouWantToExitGame.SetActive(true);
            Time.timeScale = 0;
            Debug.LogError("ApplicationQuit");
        }
    }

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
        Debug.LogError("gameExit");
    }



    public void DoYouWantToQuit_Yes()
    {
        Application.Quit();
        Debug.LogError("gameExit");
    }

    public void DoYouWantToQuit_NO()
    {
        DoYouWantToExitGame.SetActive(false);
        pausePanel.SetActive(false);
        Time.timeScale = 1;

    }
}
