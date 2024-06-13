using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Effect
{
    public float maxDuration, currentDuration;
    public Buffable target;

    public abstract void DoEffect();

    public abstract string Type();

}
