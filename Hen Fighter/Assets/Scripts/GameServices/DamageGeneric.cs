using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGeneric : MonoBehaviour
{
    public LayerMask collisionLayer;
    public float radius = 1f;
    public float damage = 2f;

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
            }
            gameObject.SetActive(false);
        }
    }
}
