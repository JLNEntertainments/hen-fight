using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    [SerializeField]
    Animator playerAnimator;
    static int clicksCnt;

    // Start is called before the first frame update
    void Start()
    {
        clicksCnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLightAttackBtnPressed()
    {
        Debug.Log("Light attack!!");
        playerAnimator.SetTrigger("isLightAttack");
        clicksCnt++;
        isComboCheck();
        //call method to inflict damage on other player
    }

    public void OnHeavyAttackBtnPressed()
    {
        Debug.Log("Heavy attack!!");
        playerAnimator.SetTrigger("isHeavyAttack");
        clicksCnt++;
        isComboCheck();
        //call method to inflict damage on other player
    }

    public void OnBlockAttackBtnPressed()
    {
        Debug.Log("Player blocked attack!!");
        playerAnimator.SetTrigger("isBlocking");
        clicksCnt = 0;
    }

    public void isComboCheck()
    {
        if (clicksCnt == 3)
        {
            playerAnimator.SetTrigger("isComboAttack");
            clicksCnt = 0;
        }
    }
}
