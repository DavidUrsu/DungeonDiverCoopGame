using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceBuff : Effect
{
    private bool aplied = false;
    private float percentage;

    public ResistanceBuff(float duration = 5f, float p = 0.1f)
    {
        maxDuration = duration;
        percentage = p;
        currentDuration = maxDuration;
    }

    public override string Type()
    {
        return "resbuff";
    }
    public override void DoEffect()
    {
        Debug.Log(currentDuration);
        if (currentDuration <= 0)
        {
            target.DamageReduction -= percentage;
        }
        else if (aplied)
        {
            return;
        }
        else
        {
            
            aplied = true;
            target.DamageReduction += percentage;
        }

    }
}
