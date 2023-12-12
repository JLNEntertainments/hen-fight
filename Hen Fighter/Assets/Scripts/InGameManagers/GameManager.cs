using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : SingletonGeneric<GameManager>
{
    public CharacterDataBase CharacterDB;

    public TMP_Text nameOfTheHen;
    private GameObject networkHenGameObject;

    private int selectedOption = 0;

    public void Assignplayer(GameObject Player)
    {
        Player.transform.position = new Vector3(-3.31f, 2f, 1.25f);
        Player.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        Player.transform.localScale = new Vector3(20f, 20f, 20f);
        Instantiate(Player, Player.transform);
    }

    void Start()
    {
        
        updateCharacter(Display.Instance.tempDataCnt);
    }

    private void updateCharacter(int selectedOption)
    {
        Character character = CharacterDB.Getcharacter(selectedOption);
        networkHenGameObject = character.CharacterofHen;
        nameOfTheHen.text = character.characterName;
        Assignplayer(networkHenGameObject);
    }


    
}
