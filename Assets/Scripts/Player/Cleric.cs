using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : Player
{
    public GameObject LightRayPrefab;
    public GameObject AbilityTotem;
    public Transform FiringPoint;
    private ProjectileManger manager;
    private float AbilityDuration;
    void Start()
    {
        MaxHealth = 70f;
        CurrentHealth = MaxHealth;
        DamageReduction = 0.0f;
        AttackDamage = 10f;
        AbilityDamage = 0f;
        AttackCooldown = 0.4f;
        AbilityCooldown = 10f;
        MoveSpeed = 4f;
        AbilityDuration = 4f;
    }

    public override void Ability()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        GameObject relic = Instantiate(AbilityTotem, new Vector2(mousePosition.x, mousePosition.y), Quaternion.identity);
        Destroy(relic, AbilityDuration);
        Relic script = relic.GetComponent<Relic>();
        script.owner = this;
    }

    public override void Attack()
    {
        GameObject lightray = Instantiate(LightRayPrefab, FiringPoint.position, FiringPoint.rotation);
        manager = lightray.GetComponent<ProjectileManger>();
        manager.player = this;
    }
}
