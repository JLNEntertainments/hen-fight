using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamepass : MonoBehaviour
{
    public GameObject pausePanel,gamequitPanel;

    public GameObject DoYouWantToExitGame;

    public AudioSource gameAudioSource;

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            
            // Application.Quit();
            DoYouWantToExitGame.SetActive(true);
            
            Debug.LogError("ApplicationQuit");
            Time.timeScale = 0;
            gameAudioSource.Stop();
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
        DoYouWantToExitGame.SetActive(false);
        Time.timeScale = 0;

    }
    public void NoquitButton()
    {
        ContinueButtoon();
        gamequitPanel.SetActive(false);
        DoYouWantToExitGame.SetActive(false);


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
        gameAudioSource.Play();
        gamequitPanel.SetActive(false);
        DoYouWantToExitGame.SetActive(false);
        pausePanel.SetActive(false);
        Time.timeScale = 1;

    }
}
