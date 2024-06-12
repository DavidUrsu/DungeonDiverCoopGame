using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    public GameObject fireballPrefab;
    public GameObject AbilityFireballPrefab;
    public Transform firingPoint;
    private ProjectileManger manager;
    void Start()
    {
        MaxHealth = 100f;
        CurrentHealth = MaxHealth;
        DamageReduction = 0.1f;
        AttackDamage = 40f;
        AbilityDamage = 70f;
        AttackCooldown = 0.6f;
        AbilityCooldown = 3f;
        MoveSpeed = 5;
    }

    public override void Attack()
    {
        GameObject fireball = Instantiate(fireballPrefab,firingPoint.position,firingPoint.rotation);
        manager = fireball.GetComponent<ProjectileManger>();
        manager.player = this;
    }

    public override void Ability()
    {
        GameObject abilityFireball = Instantiate(AbilityFireballPrefab, firingPoint.position, firingPoint.rotation);
        manager = abilityFireball.GetComponent<ProjectileManger>();
        manager.player = this;
    }
}
