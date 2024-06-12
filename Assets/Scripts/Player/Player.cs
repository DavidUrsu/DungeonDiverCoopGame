using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Buffable
{
    public abstract void Attack();

    public abstract void Ability();

    public void HitByEnemy(float dmg)
    {
        CurrentHealth -= dmg * (1 - DamageReduction);
        Debug.Log("Player has " + CurrentHealth + "Health remaning");
        if (CurrentHealth <= 0)
            Destroy(gameObject);
    }
}

