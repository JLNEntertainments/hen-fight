using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    [SerializeField]
    EnemyGamePlayManager enemyPrefab;
    PlayerGamePlayManager playerPrefab;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (timer == 4f)
            enemyPrefab = FindObjectOfType<EnemyGamePlayManager>();
        else
            timer += 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyPrefab.enemyAIDecision.backWalkToggle = false;
            enemyPrefab.enemy_Unfollow_Time = 0f;
        }
            
    }
}
