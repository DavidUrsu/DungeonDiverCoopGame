using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion
{
    private Effect effect;
    private Sprite sprite;

    public Potion(Effect effect, Sprite sp)
    {
        this.effect = effect;
        this.sprite = sp;
    }

    public Effect GetEffect() 
    {
        return effect;
    }

    public Sprite GetSprite() 
    {
        return sprite;
    }

}
