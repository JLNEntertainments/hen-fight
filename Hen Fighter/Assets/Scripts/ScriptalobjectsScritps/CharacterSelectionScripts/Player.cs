using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public CharacterDataBase CharacterDB;

    public TMP_Text nameOfTheHen;
    public GameObject networkHenGameObject;

    private int selectedOption = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            load();
        }
        updateCharacter(selectedOption);
    }

    public void AssignPlayer()
    {

    }

    private void updateCharacter(int selectedOption)
    {
        Character character = CharacterDB.Getcharacter(selectedOption);
        networkHenGameObject = character.CharacterofHen;
        nameOfTheHen.text = character.characterName;
       // Assignplayer(networkHenGameObject);
    }


    private void load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
}
