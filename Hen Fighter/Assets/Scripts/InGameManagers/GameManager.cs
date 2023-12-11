using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonGeneric<GameManager>
{
    public void Assignplayer(GameObject Player)
    {
        Player.transform.position = new Vector3(-3.3f, 2f, 1.25f);
        Player.transform.localScale = new Vector3(20f, 20f, 20f);
        Instantiate(Player, Player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
