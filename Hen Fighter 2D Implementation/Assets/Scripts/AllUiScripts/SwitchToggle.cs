using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    public GameObject  on, off;
    int index;

    void Start()
    {
        
    }

    void Update()
    {
        if(index == 1)
        {
           
        }
    }

    public void On()
    {
        on.gameObject.SetActive(false);
        off.gameObject.SetActive(true);
    }

    public void OFF()
    {
        off.gameObject.SetActive(false);
        on.gameObject.SetActive(true);
    }
}
