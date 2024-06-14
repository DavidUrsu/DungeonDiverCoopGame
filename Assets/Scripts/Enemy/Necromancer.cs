using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Necromancer : MonoBehaviour
{
    public GameObject skeletonPrefab;
    private Enemy en;
    private GameObject target;
    private bool wantAttack, wantAbility;
    public Image healthBar;
    public GameObject particles;
    void Start()
    {
        en = GetComponent<Enemy>();
    }

    void Update()
    {
        target = GetComponent<EnemyController>().Target;
        float dist = Vector2.Distance(this.transform.position, target.transform.position);

        if (dist <= 5)
            en.controller.MoveSpeed = 0f;
        else
            en.controller.MoveSpeed = en.MoveSpeed;

        if (target != null && en.attackTimer <= 0 && dist <= 5)
        {
            en.attackTimer = en.AttackCooldown;
            wantAttack = true;
        }

        if(target != null && en.abilityTimer <= 0) 
        {
            en.abilityTimer = en.AbilityCooldown;
            wantAbility = true;
        }

        en.attackTimer -= Time.deltaTime;
        en.abilityTimer -= Time.deltaTime;

        if (en.abilityTimer < 0)
            en.abilityTimer = 0;
        if (en.attackTimer < 0)
            en.attackTimer = 0;
    }

    private void FixedUpdate()
    {
        if (wantAttack)
        {
            wantAttack = false;
            LifeDrain();
        }

        if(wantAbility)
        {
            wantAbility = false;
            SpawnMinions();
        }
    }

    private void DisableParticles()
    {
        particles.SetActive( false);
    }
    private void LifeDrain()
    {
        target.SendMessage("HitByEnemy",en.AttackDamage);

        en.CurrentHealth += en.AttackDamage * 0.5f;

        if(en.CurrentHealth > en.MaxHealth)
            en.CurrentHealth = en.MaxHealth;

        healthBar.fillAmount = en.CurrentHealth / en.MaxHealth;

        particles.SetActive(true);
        Invoke(nameof(DisableParticles), 1f);
    }
    private void SpawnMinions()
    {
        GameObject minion1 = Instantiate(skeletonPrefab, transform.position + new Vector3(1.5f,0f,0f), transform.rotation);
        GameObject minion2 = Instantiate(skeletonPrefab, transform.position + new Vector3(-1.5f, 0f, 0f), transform.rotation);

        EnemyController cont = minion1.GetComponent<EnemyController>();
        cont.Target = target;
        EnemyController cont2 = minion2.GetComponent<EnemyController>();
        cont2.Target = target;
    }

}
