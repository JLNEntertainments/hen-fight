using UnityEngine;

[RequireComponent(typeof(EnemyGamePlayManager))]

public class BaseState : MonoBehaviour
{
    protected EnemyGamePlayManager enemyGamePlayManager;

    void Awake()
    {
        enemyGamePlayManager = GetComponent<EnemyGamePlayManager>();
    }
    
    public virtual void onStateEnter()
    {
        this.enabled = true;
    }

    public virtual void onStateExit()
    {
        this.enabled = false;
    }

    public void onStateChanged(BaseState newState)
    {
        if(enemyGamePlayManager.currentState != null)
        {
            enemyGamePlayManager.currentState.onStateExit();
        }

        enemyGamePlayManager.currentState = newState;
        enemyGamePlayManager.currentState.onStateEnter();
    }
}
