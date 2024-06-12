using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion
{
    private Effect effect;

    public Potion(Effect effect)
    {
        this.effect = effect;
    }

    public Effect GetEffect() 
    {
        return effect;
    }
}
