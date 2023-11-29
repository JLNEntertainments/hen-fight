using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Staminbar : MonoBehaviour
{
    public GameObject CharactherObj;
   [SerializeField] Animator CharactherAnima;
    public Image StaminaBarImage;

    [Space]
    public bool isBlocking;

    [Space]
    [Range(0,100f)]
    public float playerHealth;
    float playerhealthValue;

    [Space]

    public float LightAttackDamage;
    public float MediumAttackDamage;
    public float HeavyAttackDamage;

    [Space]
    public float BlockDamageOffset;

   

    // Start is called before the first frame update


     
    void Start()
    {
        playerHealth = 100f;
        CharactherAnima = CharactherObj.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerhealthValue = playerHealth * .01f;
        StaminaBarImage.fillAmount = playerhealthValue;
    }

    public void LightAtatck()
    {
        if (isBlocking)
        {
            playerHealth -= (LightAttackDamage / BlockDamageOffset);
        }else
        playerHealth -= LightAttackDamage;
        CharactherAnima.SetTrigger("isLightAttack");
    }
    public void MediumAttack()
    {
        if (isBlocking)
        {
            playerHealth -= (MediumAttackDamage / BlockDamageOffset);
        }else
        playerHealth -= MediumAttackDamage;
        CharactherAnima.SetTrigger("isLightAttack");
    }

    public void HeavyAttack()
    {
        if (isBlocking)
        {
            playerHealth -= (HeavyAttackDamage / BlockDamageOffset);
        }
        else
            playerHealth -= HeavyAttackDamage;
        CharactherAnima.SetTrigger("isHeavyAttack");
    }

    public void SetBlocking()
    {
        if (isBlocking)
        {
            isBlocking = false;
        }
        else if (!isBlocking)
        {
            isBlocking = true;
        }
    }
}


