using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
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
        nameText.text = HenCarPrefab[dataCnt].name.ToString();
        PowerText.text = HenCarPrefab[dataCnt].Power.ToString();
        DespText.text = HenCarPrefab[dataCnt].Defense.ToString();
        DropDownText.text = HenCarPrefab[dataCnt].DropDown.ToString();
        henImage.sprite = HenCarPrefab[dataCnt].HenCard;
        
    }
    public void Next()
    {
        isNextPressed = true;
        isPreviousPressed = false;
        if (dataCnt >= HenCarPrefab.Length - 1 && isNextPressed)
        {
            dataCnt = 0;
            nameText.text = HenCarPrefab[dataCnt].name.ToString();
            PowerText.text = HenCarPrefab[dataCnt].Power.ToString();
            DespText.text = HenCarPrefab[dataCnt].Defense.ToString();
            DropDownText.text = HenCarPrefab[dataCnt].DropDown.ToString();
            henImage.sprite = HenCarPrefab[dataCnt].HenCard;
        }
        else if (isNextPressed)
        {
            dataCnt++;
            nameText.text = HenCarPrefab[dataCnt].name.ToString();
            PowerText.text = HenCarPrefab[dataCnt].Power.ToString();
            DespText.text = HenCarPrefab[dataCnt].Defense.ToString();
            DropDownText.text = HenCarPrefab[dataCnt].DropDown.ToString();
            henImage.sprite = HenCarPrefab[dataCnt].HenCard;
        }
    }

    public void Previous()
    {
        
        isNextPressed = false;
        isPreviousPressed = true;
        if (dataCnt == 0 && isPreviousPressed)
        {
            dataCnt = HenCarPrefab.Length - 1;
            nameText.text = HenCarPrefab[dataCnt].name.ToString();
            PowerText.text = HenCarPrefab[dataCnt].Power.ToString();
            DespText.text = HenCarPrefab[dataCnt].Defense.ToString();
            DropDownText.text = HenCarPrefab[dataCnt].DropDown.ToString();
            henImage.sprite = HenCarPrefab[dataCnt].HenCard;
            
        }
        else if (dataCnt < HenCarPrefab.Length && isPreviousPressed)
        {
            dataCnt--;
            nameText.text = HenCarPrefab[dataCnt].name.ToString();
            PowerText.text = HenCarPrefab[dataCnt].Power.ToString();
            DespText.text = HenCarPrefab[dataCnt].Defense.ToString();
            DropDownText.text = HenCarPrefab[dataCnt].DropDown.ToString();
            henImage.sprite = HenCarPrefab[dataCnt].HenCard;
            
        }


    }

   
}
