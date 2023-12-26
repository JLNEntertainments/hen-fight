using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    public AudioSource audioSource;
    public Button toggleButton;
    

    public GameObject  on, off;
    int index;
    private bool muted = false;

    public void On()
    {
            muted = true;
            AudioListener.pause = true;
            on.gameObject.SetActive(false);
            off.gameObject.SetActive(true);
     }

    public void OFF()
    {
        
            muted = false;
            AudioListener.pause = false;
            on.gameObject.SetActive(false);
            off.gameObject.SetActive(true);
        
    }

    
}
