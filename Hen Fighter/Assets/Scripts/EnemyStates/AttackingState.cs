using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : BaseState
{
    public override void onStateEnter()
    {
        base.onStateEnter();
        enemyGamePlayManager.currentState = enemyGamePlayManager.AState;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyGamePlayManager.inAttackRange && !enemyGamePlayManager.inChaseRange)
            enemyGamePlayManager.currentState.onStateChanged(enemyGamePlayManager.AState);
    }
}
