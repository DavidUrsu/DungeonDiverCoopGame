using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Buffable
{
    public BossCounter counter;
    public float attackTimer = 0f, abilityTimer = 0f;
    public GameObject[] players;
    public int[] agro;
    private float[] dist;
    private int maxAgro = 0, maxAgroIndex;
    public EnemyController controller;

    public Image healthBar;


    public void Start()
    {

        CurrentHealth = MaxHealth;


        controller = GetComponent<EnemyController>();
        controller.MoveSpeed = MoveSpeed;
        players = GameObject.FindGameObjectsWithTag("Player");

        agro = new int[players.Length];
        dist = new float[players.Length];
    }

    private void Update()
    {
        //Calculate agro based on distance to players
        //Get the index of the player with max agro in order to follow him
        ProcessEffects();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
                dist[i] = Vector2.Distance(this.transform.position, players[i].transform.position);
            else
            {
                dist[i] = 9999999999;
                agro[i] = 0;
            }


            if (dist[i] <= 5)
            {
                agro[i] += 5;
            }
            else
            {
                if (agro[i] == 0)
                    continue;
                else
                    if (agro[i] - 2 <= 0)
                    agro[i] = 1;
                else
                    agro[i] -= 2;
            }

            if (agro[i] > maxAgro)
            {
                maxAgro = agro[i];
                maxAgroIndex = i;
            }

        }
        if (agro[maxAgroIndex] != 0)
            controller.Target = players[maxAgroIndex];

        attackTimer -= Time.deltaTime;
        if (attackTimer < 0)
            attackTimer = 0;

        healthBar.fillAmount = CurrentHealth / MaxHealth;

        if (CurrentHealth < 0)
            Destroy(gameObject);

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player != null && controller.IsKnocked == 0 && attackTimer <= 0)
        {
            collision.gameObject.SendMessage("HitByEnemy", AttackDamage);
            attackTimer = AttackCooldown;
        }
    }
    public void OnHit((Player,string) pair)
    {
        Player player = pair.Item1;
        string name = pair.Item2;

        if(name != null) 
        {
            switch (name) 
            {
                case "burn":
                    {
                        AddEffect(new Burn());
                        break;
                    }
                case "movespeeddebuff":
                    {
                        AddEffect(new MoveSpeedDebuff());
                        break;
                    }
                case "bleed":
                    {
                        AddEffect(new Bleed());
                        break;
                    }
                case "freeze":
                    {
                        AddEffect(new Freeze());
                        break;
                    }
            }
        }

        CurrentHealth -= player.AttackDamage * (1 - DamageReduction);

        if (CurrentHealth <= 0)
        {
            Boss1 boss = GetComponent<Boss1>();
            Necromancer necro = GetComponent<Necromancer>();
            if (necro != null)
                counter.RemainingBosses--;

            if (boss != null)
            {
                Boss1.numberOfEntities--;
                if (Boss1.numberOfEntities == 0)
                    counter.RemainingBosses--;
            }

            Destroy(gameObject);
        }
        else
        {

            for(int i = 0; i < players.Length;i++)
            {
                Player playerOnObj = players[i].GetComponent<Player>();

                if (playerOnObj == player)
                {
                    agro[i] += (int)(100 * player.AttackDamage);
                }
                    
            }
        }


    }

    public void OnHitAbility((Player,string) pair)
    {
        Player player = pair.Item1;
        string name = pair.Item2;

        if (name != null)
        {
            switch (name)
            {
                case "burn":
                    {
                        AddEffect(new Burn());
                        break;
                    }
                case "movespeeddebuff":
                    {
                        AddEffect(new MoveSpeedDebuff());
                        break;
                    }
                case "bleed":
                    {
                        AddEffect(new Bleed());
                        break;
                    }
                case "freeze":
                    {
                        AddEffect(new Freeze());
                        break;
                    }
            }
        }

        CurrentHealth -= player.AbilityDamage * (1 - DamageReduction);

        if (CurrentHealth <= 0)
        {
            Boss1 boss = GetComponent<Boss1>();
            Necromancer necro = GetComponent<Necromancer>();
            if (necro != null)
                counter.RemainingBosses--;
            if (boss != null)
            {
                Boss1.numberOfEntities--;
                if (Boss1.numberOfEntities == 0)
                    counter.RemainingBosses--;
            }
            Destroy(gameObject);
        }
        else
        {


            for (int i = 0; i < players.Length; i++)
            {
                Player playerOnObj = players[i].GetComponent<Player>();

                if (playerOnObj == player)
                {
                    agro[i] += (int)(100 * player.AbilityDamage);
                }

            }
        }
    }
}
