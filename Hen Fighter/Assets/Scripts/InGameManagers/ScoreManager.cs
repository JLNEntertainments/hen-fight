using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingletonGeneric<ScoreManager>
{
    static int enemyScore, playerScore;

    void Start()
    {
        enemyScore = 0;
        playerScore = 0;
    }

    public void UpdateEnemyScore(string attackType)
    {
        if(attackType.Equals("isLight"))
            enemyScore += 20;
        else if(attackType.Equals("isHeavy"))
            enemyScore += 40;

        Debug.Log("Enemy : " + enemyScore);
    }

    public void UpdatePlayerScore(string attackType)
    {
        if (attackType.Equals("isLight"))
            playerScore += 20;
        else if (attackType.Equals("isHeavy"))
            playerScore += 40;

        Debug.Log("Player : " + playerScore);
    }
}
