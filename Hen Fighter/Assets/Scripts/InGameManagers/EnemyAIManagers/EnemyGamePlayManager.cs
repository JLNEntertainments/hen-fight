using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGamePlayManager : SingletonGeneric<EnemyGamePlayManager>
{
    [HideInInspector]
    public Transform playerGamePlayManager;

    Animator enemyAnimator;

    Image healthBar;

    float moveSpeed;
    float raycastDistance;
    float timeBetweenAttacks;
    float moveXPos;

    int randomAttack;
    int colCnt;
    static int cnt;

    [HideInInspector]
    public bool isAttacking;

    Vector3 minDist;
    Vector3 movePosition;

    void Start()
    {
        playerGamePlayManager = FindObjectOfType<PlayerGamePlayManager>().GetComponent<PlayerGamePlayManager>().transform;
        healthBar = GameObject.FindGameObjectWithTag("E_HealthBar").GetComponentInChildren<Image>();
        enemyAnimator = GetComponent<Animator>();

        minDist = new Vector3(4f, 0f, 0f);
        
        moveSpeed = 1f;
        raycastDistance = 10f;
        timeBetweenAttacks = 5f;
        moveXPos = 1f;
    }

    void FixedUpdate()
    {
        MoveToTarget();
        UpdateZAxis();
    }

    void MoveToTarget()
    {
        Vector3 direction = playerGamePlayManager.position - this.transform.position;
        movePosition = this.transform.position;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, raycastDistance))
        {
            Debug.DrawRay(transform.position, direction * raycastDistance, Color.red);
            if(!hit.collider.gameObject.GetComponent<PlayerGamePlayManager>() && (Mathf.Abs(direction.x) > minDist.x))
            {
                enemyAnimator.SetBool("inMotion", true);
                movePosition.x += moveXPos * moveSpeed * Time.deltaTime;
                this.transform.position = movePosition;
            }
            else if(Mathf.Abs(direction.x) <= minDist.x)
            {
                enemyAnimator.SetBool("inMotion", false);
                InvokeRepeating("AttackPlayer", 0.1f, timeBetweenAttacks);
            }
        }
    }

    void AttackPlayer()
    {
        randomAttack = Random.Range(0, 3);
        switch(randomAttack) 
        {
            case 0:
                enemyAnimator.SetTrigger("isLightAttack");
                isAttacking = true;
                cnt++;
                break;

            case 1:
                enemyAnimator.SetTrigger("isBlocking");
                isAttacking = true;
                cnt++;
                break;

            case 2:
                enemyAnimator.SetTrigger("isHeavyAttack");
                isAttacking = true;
                cnt++;
                break;
        }
        if( cnt % 3 == 0 )
            ResetAllAnimatorTriggers();
    }

    void ResetAllAnimatorTriggers()
    {
        foreach (var trigger in enemyAnimator.parameters)
        {
            if (trigger.type == AnimatorControllerParameterType.Trigger)
            {
                enemyAnimator.ResetTrigger(trigger.name);
            }
        }
    }

    void UpdateZAxis()
    {
        transform.position = new Vector3(this.transform.position.x, this.transform.position.y, playerGamePlayManager.transform.position.z);
    }

    public void InflictEnemyDamage()
    {
        enemyAnimator.SetInteger("isHurt", 1);
        healthBar.fillAmount -= 0.1f;
    }
}
