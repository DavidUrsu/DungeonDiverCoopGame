using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    public int IsKnocked, IsFrozen;
    public float forceDamping;
    public Vector2 forceToApply;

    public void Initialize()
    {
        IsKnocked = 0;
        IsFrozen = 0;
        forceDamping = 1.2f;
        forceToApply = Vector2.zero;
    }
}
