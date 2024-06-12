using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBuff : Effect
{
    private bool aplied = false;
    private float percentage, amountAttack,amountAbility;
    
    public AttackBuff(float duration = 5f,float p = 0.1f)
    {
        maxDuration = duration;
        percentage = p;
        currentDuration = maxDuration;
    }

    public override string Type()
    {
        return "attackbuff";
    }
    public override void DoEffect()
    {
        Debug.Log(currentDuration);
        if (currentDuration <= 0) 
        {
            target.AttackDamage -= amountAttack;
            target.AbilityDamage -= amountAbility;
        }
        else if (aplied)
        {
            return;
        }
        else
        {
            amountAttack = percentage * target.AttackDamage;
            amountAbility = percentage * target.AbilityDamage;
            aplied = true;
            target.AttackDamage += amountAttack;
            target.AbilityDamage += amountAbility;
        }

    }
}
