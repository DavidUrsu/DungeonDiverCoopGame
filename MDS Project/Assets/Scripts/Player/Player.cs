using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public float health, attackDmg,abilityDmg,dmgRed,moveSpeed, attackCooldown, abilityCooldown;

    public abstract void Attack();

    public abstract void Ability();

    public void HitByEnemy(float dmg)
    {
        health -= dmg * (1 - dmgRed);
        Debug.Log("Player has " + health + "Health remaning");
        if (health <= 0)
            Destroy(gameObject);
    }
}

