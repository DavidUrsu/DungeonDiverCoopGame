using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight :Player
{
    public MeleManager manager;
    public void Start()
    {
        MaxHealth = 200f;
        CurrentHealth = MaxHealth;
        DamageReduction = 0.3f;
        AttackDamage = 25f;
        AbilityDamage = 0f;
        AttackCooldown = 0.8f;
        AbilityCooldown = 3f;
        MoveSpeed = 5f;
    }
    private void Ena()
    {
        manager.enabled = false;
    }
    public override void Attack()
    {
        manager.enabled = true;
        Invoke(nameof(Ena),AttackCooldown);
            
    }

    public override void Ability()
    {
        Enemy[] enemys = FindObjectsOfType<Enemy>();

        foreach(Enemy enemy in enemys) 
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
 
            if (dist <= 7) 
            {
                int index = Array.IndexOf(enemy.players, gameObject);
                enemy.agro[index] += 10000;
                enemy.AddEffect(new MoveSpeedDebuff());
            }
        }
    }
}
