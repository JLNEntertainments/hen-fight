using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{   
    //for character selection management
    public CharacterDataBase CharacterDB;
    public TMP_Text nameOfTheHen;
    public GameObject networkHenGameObject;
    private int selectedOption;


    void Start()
    {
        selectedOption = 0;
        
        updateCharacter(Display.Instance.tempDataCnt);
    }

    private void Update()
    {
        updateCharacter(Display.Instance.tempDataCnt);
    }

   
    private void updateCharacter(int selectedOption)
    {
        Character character = CharacterDB.Getcharacter(selectedOption);
        networkHenGameObject = character.CharacterofHen;
        nameOfTheHen.text = character.characterName;
    }


    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        
    }
   
}
