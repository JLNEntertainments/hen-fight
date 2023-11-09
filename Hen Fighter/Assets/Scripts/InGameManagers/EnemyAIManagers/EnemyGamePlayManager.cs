using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGamePlayManager : MonoBehaviour
{
    Animator enemyAnimator;
    
    //Parameters for changing state of enemy during gameplay
    [SerializeField]
    EnemyStates initialState;
    public EnemyStates activeState;
    public BaseState currentState;
    public AttackingState AState;
    public ChasingState CState;

    //Parameters for chasing and attacking the player during gameplay
    public float chaseRange;
    public float attackRange;
    public bool inChaseRange;
    public bool inAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        chaseRange = 15.0f;
        attackRange = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
