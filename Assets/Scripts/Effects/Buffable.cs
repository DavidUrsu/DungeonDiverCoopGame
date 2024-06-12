using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffable : MonoBehaviour
{
    public float MaxHealth,CurrentHealth, AttackDamage, DamageReduction, AbilityDamage, MoveSpeed, AttackCooldown, AbilityCooldown;
    public List<Effect> effects = new();

    public void ProcessEffects()
    {
        foreach (Effect effect in effects) 
        {
            effect.target = this;
            effect.currentDuration -= Time.deltaTime;
            effect.DoEffect();

            if (effect.currentDuration <= 0)
                effects.Remove( effect );
        }
    }

    public void AddEffect(Effect effect)
    {
        bool refresh = false;
        foreach(Effect ef in effects)
        {
            if (ef.Type() == effect.Type())
            {
                ef.currentDuration = ef.maxDuration;
                refresh = true;
            }
        }

        if(!refresh)
        {
            effects.Add( effect );
        }
    }
}
