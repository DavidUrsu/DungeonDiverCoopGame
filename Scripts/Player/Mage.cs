using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    public GameObject fireballPrefab;
    public Transform firingPoint;
    private ProjectileManger manager;
    void Start()
    {
        attackCooldown = 0.6f;
        abilityCooldown = 3f;
        moveSpeed = 5;
    }

    public override void Attack()
    {
        GameObject fireball = Instantiate(fireballPrefab,firingPoint.position,firingPoint.rotation);
        manager = fireball.GetComponent<ProjectileManger>();
        manager.player = this;
    }

    public override void Ability()
    {
        throw new System.NotImplementedException();
    }
}
