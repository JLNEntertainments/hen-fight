using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterDataBase : ScriptableObject
{
    public Character[] Character;


    public int characterCount
    {
        get
        {
            return Character.Length;
        }
    }

    public Character Getcharacter(int index)
    {
        return Character[index];
    }
}
