using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewHenCard", menuName = "HenCards")]
public class Card : ScriptableObject
{
    public string HenName;
   // public string Description;

    public Sprite HenCard;

    public string Power;
    public string Defense;
    public string DropDown;


   
}
