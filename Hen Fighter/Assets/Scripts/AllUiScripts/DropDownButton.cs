using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownButton : MonoBehaviour
{
    public GameObject DropDown;
    public bool Ison = false;
   
    public void DropDownButoon()
    {
        {
            if (!Ison) {
                DropDown.SetActive(true);
                Ison = true;
            }
            else
            {
               
                {
                    DropDown.SetActive(false);
                    Ison = false;
                }
            
            }
        }
    }
}
