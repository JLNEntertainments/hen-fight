using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    //[RequireComponent(typeof(Card))]
    
    public Card[] HenCarPrefab ;
   
    public Card card;

    public TMP_Text nameText;
    

    public Image henImage;

    bool isPreviousPressed, isNextPressed;
    static int dataCnt; //for keeping track by counter of data
    public TMP_Text PowerText;
    public TMP_Text DespText;
    public TMP_Text DropDownText;

   
    void Start()
    {
        /* nameText.text = card.name;
         dataCnt = 1;
         henImage.sprite = card.HenCard;
         PowerText.text = card.Power.ToString();
         DespText.text = card.Defense.ToString();
         DropDownText.text = card.DropDown.ToString();*/
        nameText.text = HenCarPrefab[dataCnt].name.ToString();
        PowerText.text = HenCarPrefab[dataCnt].Power.ToString();
        DespText.text = HenCarPrefab[dataCnt].Defense.ToString();
        DropDownText.text = HenCarPrefab[dataCnt].DropDown.ToString();
        henImage.sprite = HenCarPrefab[dataCnt].HenCard;
        dataCnt = 1;

    }

    

    


    

    public void Next()
    {
        
        isNextPressed = true;
        isPreviousPressed = false;
        if (dataCnt == HenCarPrefab.Length-1 && isNextPressed)
        {
            nameText.text = HenCarPrefab[dataCnt].name.ToString();
            PowerText.text = HenCarPrefab[dataCnt].Power.ToString();
            DespText.text = HenCarPrefab[dataCnt].Defense.ToString();
            DropDownText.text = HenCarPrefab[dataCnt].DropDown.ToString();
            henImage.sprite = HenCarPrefab[dataCnt].HenCard;
            dataCnt = 0;
        }
        else if (isNextPressed)
        {
            nameText.text = HenCarPrefab[dataCnt].name.ToString();
            PowerText.text = HenCarPrefab[dataCnt].Power.ToString();
            DespText.text = HenCarPrefab[dataCnt].Defense.ToString();
            DropDownText.text = HenCarPrefab[dataCnt].DropDown.ToString();
            henImage.sprite = HenCarPrefab[dataCnt].HenCard;
            dataCnt++;
        }
    }

    public void Previous()
    {
        
        isNextPressed = false;
        isPreviousPressed = true;


        if (dataCnt == 0 && isPreviousPressed)
        {
            nameText.text = HenCarPrefab[dataCnt].name.ToString();
            PowerText.text = HenCarPrefab[dataCnt].Power.ToString();
            DespText.text = HenCarPrefab[dataCnt].Defense.ToString();
            DropDownText.text = HenCarPrefab[dataCnt].DropDown.ToString();
            henImage.sprite = HenCarPrefab[dataCnt].HenCard;
            dataCnt = HenCarPrefab.Length - 1;
        }
        else if (dataCnt < HenCarPrefab.Length && isPreviousPressed)
        {
            nameText.text = HenCarPrefab[dataCnt].name.ToString();
            PowerText.text = HenCarPrefab[dataCnt].Power.ToString();
            DespText.text = HenCarPrefab[dataCnt].Defense.ToString();
            DropDownText.text = HenCarPrefab[dataCnt].DropDown.ToString();
            henImage.sprite = HenCarPrefab[dataCnt].HenCard;
            dataCnt--;
        }


    }

   
}
