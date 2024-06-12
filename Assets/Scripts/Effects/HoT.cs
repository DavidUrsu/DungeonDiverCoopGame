using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HoT : Effect
{

    private float tickCooldown, lastTick, healPerTick;

    public HoT(float duration = 5f, float cooldown = 1f, float heal = 15f)
    {
        maxDuration = duration;
        tickCooldown = cooldown;
        healPerTick = heal;
        currentDuration = duration;
        lastTick = duration + tickCooldown;
    }

    public override void DoEffect()
    {
        if (lastTick - currentDuration >= tickCooldown)
        {

            lastTick = currentDuration;
            target.CurrentHealth += healPerTick;

            if (target.CurrentHealth > target.MaxHealth)
            {
                target.CurrentHealth = target.MaxHealth;
            }
        }
    }

    public override string Type()
    {
        return "hot";
    }

}
