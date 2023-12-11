using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageGeneric : MonoBehaviour
{
    [SerializeField]
    LayerMask collisionLayer;
    
    [SerializeField]
    float colliderRadius;

    [SerializeField]
    bool is_Player, is_Enemy;

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
                    hit[0].GetComponentInParent<PlayerGamePlayManager>().InflictHeavyDamage();
                    ScoreManager.Instance.UpdateEnemyScore("isHeavy");
                }
                else if(hit[0].GetComponentInParent<PlayerGamePlayManager>().enemyGamePlayManager.isLightAttack)
                {
                    hit[0].GetComponentInParent<PlayerGamePlayManager>().InflictLightDamage();
                    ScoreManager.Instance.UpdateEnemyScore("isLight");
                }
                    
            }
            if (is_Player && hit != null)
            {
                hit[0].GetComponentInParent<EnemyGamePlayManager>().InflictEnemyDamage();
                if(hit[0].GetComponentInParent<EnemyGamePlayManager>().playerGamePlayManager.isHeavyAttack)
                    ScoreManager.Instance.UpdatePlayerScore("isHeavy");
                else
                    ScoreManager.Instance.UpdatePlayerScore("isLight");
            }
            this.gameObject.SetActive(false);
        }
    }
}
