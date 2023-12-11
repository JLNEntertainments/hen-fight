using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{    //for character selection management
    public CharacterDataBase CharacterDB;

    public TMP_Text nameOfTheHen;
    public GameObject networkHenGameObject;
    

    private int selectedOption = 0;


    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
         //  load();
        }
        updateCharacter(selectedOption);
    }

    public void Next()
    {

        //For hen Selection For next screen
        selectedOption++;
        if (selectedOption >= CharacterDB.characterCount)
        {
            selectedOption = 0;
        }
        updateCharacter(selectedOption);
        save();


    }



    public void Previous()
    {
        //For hen Selection For next screen
        selectedOption--;
        if (selectedOption < 0)
        {
            selectedOption = CharacterDB.characterCount - 1;
        }
        updateCharacter(selectedOption);
        save();

    }

    private void updateCharacter(int selectedOption)
    {
        Character character = CharacterDB.Getcharacter(selectedOption);
        networkHenGameObject = character.CharacterofHen;
       
        nameOfTheHen.text = character.characterName;
    }


    private void load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

    private void save()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        GameManager.Instance.Assignplayer(networkHenGameObject);
    }
   
}
