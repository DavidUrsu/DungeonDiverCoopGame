using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Buffable
{
    public float  attackTimer = 0f;
    public GameObject[] players;
    public int[] agro;
    private float[] dist;
    private int maxAgro = 0, maxAgroIndex;
    private EnemyController controller;


    public void Start()
    {
        MaxHealth = 150;
        CurrentHealth = MaxHealth;
        AbilityDamage = 0;
        AttackDamage = 15;
        MoveSpeed = 2f;
        DamageReduction = 0.05f;
        AttackCooldown = 1f;
        AbilityCooldown = 0f;


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
    public void OnHit(Player player)
    {
        CurrentHealth -= player.AttackDamage * (1 - DamageReduction);

        if (CurrentHealth <= 0)
        {
            Debug.Log("Bleeah");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Hit for " + player.AttackDamage * (1 - DamageReduction) + " " + CurrentHealth + " left");

            for(int i = 0; i < players.Length;i++)
            {
                Player playerOnObj = players[i].GetComponent<Player>();

                if (playerOnObj == player)
                {
                    agro[i] += (int)(100 * player.AttackDamage);
                    Debug.Log(agro[i] + "HIT");
                }
                    
            }
        }

    }

    public void OnHitAbility(Player player)
    {
        CurrentHealth -= player.AbilityDamage * (1 - DamageReduction);

        if (CurrentHealth <= 0)
        {
            Debug.Log("Bleeah");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Hit for " + player.AbilityDamage * (1 - DamageReduction) + " " + CurrentHealth + " left");

            for (int i = 0; i < players.Length; i++)
            {
                Player playerOnObj = players[i].GetComponent<Player>();

                if (playerOnObj == player)
                {
                    agro[i] += (int)(100 * player.AbilityDamage);
                    Debug.Log(agro[i] + "HIT");
                }

            }
        }

    }
}
