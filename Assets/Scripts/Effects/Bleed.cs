using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bleed : Effect
{
    private float tickCooldown, lastTick, damagePerTick;

    public Bleed(float duration = 3f, float cooldown = 0.75f, float damage = 7f)
    {
        maxDuration = duration;
        tickCooldown = cooldown;
        damagePerTick = damage;
        currentDuration = duration;
        lastTick = duration + tickCooldown;
    }

    public override string Type()
    {
        return "bleed";
    }

    public override void DoEffect()
    {
        if (lastTick - currentDuration >= tickCooldown)
        {

            lastTick = currentDuration;
            target.CurrentHealth -= damagePerTick * (1 - target.DamageReduction);

        }

    }
}
