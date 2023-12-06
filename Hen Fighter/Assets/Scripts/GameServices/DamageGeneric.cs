using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageGeneric : MonoBehaviour
{
    public LayerMask collisionLayer;
    public float radius;
    public float damage;

    public bool is_Player, is_Enemy;

    void Update()
    {
        DetectCollision();
    }

    void DetectCollision()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, radius, collisionLayer);
        if (hit.Length > 0)
        {
            if (is_Enemy && hit != null)
            {
                hit[0].GetComponentInParent<PlayerGamePlayManager>().InflictPlayerDamage();
                if (hit[0].GetComponentInParent<PlayerGamePlayManager>().enemyGamePlayManager.isHeavyAttack)
                    ScoreManager.Instance.UpdateEnemyScore("isHeavy");
                else
                    ScoreManager.Instance.UpdateEnemyScore("isLight");
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
