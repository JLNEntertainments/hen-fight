using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : BaseState
{
    public override void onStateEnter()
    {
        base.onStateEnter();
        enemyGamePlayManager.activeState = EnemyStates.Chasing;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyGamePlayManager.inChaseRange && !enemyGamePlayManager.inAttackRange)
            enemyGamePlayManager.currentState.onStateChanged(enemyGamePlayManager.CState);
    }
}
