using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public  class SwitchToggle : MonoBehaviour
{
    public GameObject ObjectMusic;
    private AudioSource audioSource;
    public Button toggleButton;
    public GameObject  on, off;
    static bool muted = false;


    public void Awake()
    {
        muted = PlayerPrefs.GetInt("Soundmuted") == 1;
        UpdateUI();

    }

    public void On()
    {
        if (!muted)
        {
            ToggleSound();
        }
    }

    public void OFF()
    {
        if (muted)
        {
            ToggleSound();
        }
    }


    private void ToggleSound()
    {
        ObjectMusic = GameObject.FindWithTag("GameMusic");
        audioSource = ObjectMusic.GetComponent<AudioSource>();
        muted = !muted;
        PlayerPrefs.SetInt("Soundmuted", muted ? 1 : 0);
        AudioListener.pause = muted;
        UpdateUI();
    }

    private void UpdateUI()
    {
        on.SetActive(!muted);
        off.SetActive(muted);
    }
}
