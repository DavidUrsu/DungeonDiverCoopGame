using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Effect
{
    private bool aplied = false;
    private Moveable controller;


    public Freeze(float duration = 2f) 
    {
        maxDuration = duration;
        currentDuration = duration;
        
    }
    public override string Type()
    {
        return "freeze";
    }

    public override void DoEffect()
    {
        controller = target.gameObject.GetComponent<Moveable>();
        Debug.Log(currentDuration);
        if (currentDuration <= 0)
        {
            controller.IsFrozen = 0;

        }
        else if (aplied)
        {
            return;
        }
        else
        {
            aplied = true;
            controller.IsFrozen = 1;
        }

    }
}
