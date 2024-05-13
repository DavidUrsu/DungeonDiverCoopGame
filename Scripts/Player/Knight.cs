using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight :Player
{
    public MeleManager manager;

    public void Start()
    {
        attackCooldown = 0.8f;
        abilityCooldown = 3f;
        moveSpeed = 5;
    }
    public void Ena()
    {
        manager.enabled = false;
    }
    public override void Attack()
    {
        manager.enabled = true;
        Invoke(nameof(Ena),attackCooldown);
            
    }

    public override void Ability()
    {
        throw new System.NotImplementedException();
    }
}
