using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedBuff : Effect
{
    private bool aplied = false;
    private float percentage, amount;

    public MoveSpeedBuff(float duration = 5f, float p = 0.1f)
    {
        maxDuration = duration;
        percentage = p;
        currentDuration = maxDuration;
    }

    public override string Type()
    {
        return "movespeedbuff";
    }
    public override void DoEffect()
    {
        Debug.Log(currentDuration);
        if (currentDuration <= 0)
        {
            target.MoveSpeed -= amount;
        }
        else if (aplied)
        {
            return;
        }
        else
        {
            amount = percentage * target.MoveSpeed;
            aplied = true;
            target.MoveSpeed += amount;

        }

    }
}
