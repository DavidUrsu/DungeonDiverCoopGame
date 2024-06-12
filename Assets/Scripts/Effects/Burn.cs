using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : Effect
{
    private float tickCooldown, lastTick,damagePerTick;
    public Burn(float duration = 5f, float cooldown = 2f,float damage = 5f)
    {
        maxDuration = duration;
        tickCooldown = cooldown;
        damagePerTick = damage;
        currentDuration = duration;
        lastTick = duration + tickCooldown;
    }
    public override string Type()
    {
        return "burn";
    }

    public override void DoEffect()
    {
        if(lastTick - currentDuration >= tickCooldown)
        {

              lastTick = currentDuration;
              target.CurrentHealth -= damagePerTick * (1 - target.DamageReduction);
              Debug.Log("Hit for " + damagePerTick + "at tick " + lastTick);

              if(target.CurrentHealth <= 0)
                {
                    Destroy(target.gameObject);
                }
        }


    }

}