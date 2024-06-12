using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public float maxDuration, currentDuration;
    public Buffable target;

    public abstract void DoEffect();

    public abstract string Type();
}
