using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageGeneric : MonoBehaviour
{
    [SerializeField]
    LayerMask collisionLayer;
    
    float colliderRadius;

    [SerializeField]
    bool is_Player, is_Enemy;

    void Start()
    {
        colliderRadius = 0.8f;
    }

    void FixedUpdate()
    {
        DetectCollision();
    }

    void DetectCollision()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, colliderRadius, collisionLayer);
        if (hit.Length > 0)
        {
            if (is_Enemy && hit != null && !hit[0].GetComponentInParent<PlayerGamePlayManager>().isBlocking)
            {
                if (hit[0].GetComponentInParent<PlayerGamePlayManager>().enemyGamePlayManager.isHeavyAttack)
                {
                    hit[0].GetComponentInParent<PlayerGamePlayManager>().InflictPlayerDamage("isHeavy");
                    ScoreManager.Instance.UpdateEnemyScore("isHeavy");
                }
                else if(hit[0].GetComponentInParent<PlayerGamePlayManager>().enemyGamePlayManager.isLightAttack)
                {
                    hit[0].GetComponentInParent<PlayerGamePlayManager>().InflictPlayerDamage("isLight");
                    ScoreManager.Instance.UpdateEnemyScore("isLight");
                }
                else if (hit[0].GetComponentInParent<PlayerGamePlayManager>().enemyGamePlayManager.isSpecialAttack)
                {
                    hit[0].GetComponentInParent<PlayerGamePlayManager>().InflictPlayerDamage("isSpecialAttack");
                    ScoreManager.Instance.UpdateEnemyScore("isSpecialAttack");
                }
            }
            if (is_Player && hit != null)
            {
                if(hit[0].GetComponentInParent<EnemyGamePlayManager>().playerGamePlayManager.isHeavyAttack)
                {
                    hit[0].GetComponentInParent<EnemyGamePlayManager>().InflictEnemyDamage("isHeavy");
                    ScoreManager.Instance.UpdatePlayerScore("isHeavy");
                }
                else if (hit[0].GetComponentInParent<EnemyGamePlayManager>().playerGamePlayManager.isLightAttack)
                {
                    hit[0].GetComponentInParent<EnemyGamePlayManager>().InflictEnemyDamage("isLight");
                    ScoreManager.Instance.UpdatePlayerScore("isLight");
                }
                else if (hit[0].GetComponentInParent<EnemyGamePlayManager>().playerGamePlayManager.isSpecialAttack)
                {
                    hit[0].GetComponentInParent<EnemyGamePlayManager>().InflictEnemyDamage("isSpecialAttack");
                    ScoreManager.Instance.UpdatePlayerScore("isSpecialAttack");
                }
            }
            this.gameObject.SetActive(false);
        }
    }
}
