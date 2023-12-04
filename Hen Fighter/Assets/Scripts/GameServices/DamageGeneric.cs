using System.Collections;
using System.Collections.Generic;
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
            }
            if (is_Player && hit != null)
            {
                hit[0].GetComponentInParent<EnemyGamePlayManager>().InflictEnemyDamage();
                if (PlayerCombatManager.Instance.isHeavyAttack)
                    ScoreManager.Instance.UpdatePlayerScore("isHeavy");
                else
                    ScoreManager.Instance.UpdatePlayerScore("isLight");
            }
            gameObject.SetActive(false);
        }
    }
}
