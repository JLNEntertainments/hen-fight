using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGeneric : MonoBehaviour
{
    int cnt = 0;
    private void OnTriggerEnter(Collider collision)
    {
        
        //When Enemy attacks
        if (collision.gameObject.CompareTag("Player") && EnemyGamePlayManager.Instance.isAttacking)
        {
            collision.gameObject.GetComponentInParent<PlayerGamePlayManager>().InflictPlayerDamage();
        }
        //When Player attacks
        else if (collision.gameObject.CompareTag("EnemyAI") && PlayerCombatManager.Instance.isAttacking && cnt == 0)
        {
            cnt++;
            collision.gameObject.GetComponentInParent<EnemyGamePlayManager>().InflictEnemyDamage();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerGamePlayManager>() != null)
        {
            collision.gameObject.GetComponentInParent<PlayerGamePlayManager>().GetComponent<Animator>().SetInteger("isHurt", 0);
            PlayerCombatManager.Instance.isAttacking = false;
        }
        else if (collision.gameObject.GetComponentInParent<EnemyGamePlayManager>() != null)
        {
            collision.gameObject.GetComponentInParent<EnemyGamePlayManager>().GetComponent<Animator>().SetInteger("isHurt", 0);
            EnemyGamePlayManager.Instance.isAttacking = false;
        }
        cnt = 0;
    }
}
