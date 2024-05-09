using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Health, DmgReduction, MoveSpeed = 1f, rotationSpeed = 0.1f, Dmg = 10f, attackTimer = 0f, attackCooldown = 1f;
    private GameObject[] players;
    private int[] agro;
    private float[] dist;
    private int maxAgro = 0, maxAgroIndex;
    private Rigidbody2D rb;
    private int IsKnocked = 0;
    private float damping = 1.2f;
    private Vector2 force;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (players == null)
            players = GameObject.FindGameObjectsWithTag("Player");

        agro = new int[players.Length];
        dist = new float[players.Length];
    }

    private void Update()
    {
        //Calculate agro based on distance to players
        //Get the index of the player with max agro in order to follow him
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
            RotateTowards(players[maxAgroIndex].transform, rotationSpeed);

        attackTimer -= Time.deltaTime;
        if (attackTimer < 0)
            attackTimer = 0;

    }

    private void FixedUpdate()
    {
        if (agro[maxAgroIndex] != 0)
            rb.velocity = new Vector2(transform.up.x,transform.up.y) * MoveSpeed * (IsKnocked ^ 1) + force;

        force /= damping;
        if (Mathf.Abs(force.x) <= 0.1f && Mathf.Abs(force.y) <= 0.1f)
        {
            IsKnocked = 0;
        }
        if (Mathf.Abs(force.x) <= 0.01f && Mathf.Abs(force.y) <= 0.01f)
        {
            force = Vector2.zero;
        }

    }

    private void RotateTowards(Transform target,float roatationSpeed)
    {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, q , roatationSpeed);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var knockback = collision.gameObject.GetComponent<Knockback>();


        Debug.Log("Collision detected Enemy");

        if (knockback != null)
        {
            IsKnocked = 1;
            Vector2 dir = (collision.otherCollider.transform.position - collision.transform.position).normalized;
            force += dir * knockback.force;
            Debug.Log(force);
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player != null && IsKnocked == 0 && attackTimer <= 0) 
        { 
            collision.gameObject.SendMessage("HitByEnemy", Dmg);
            attackTimer = attackCooldown;
        }
    }
    public void OnHit(float dmg)
    {
        Health -= dmg * (1 - DmgReduction);

        if (Health <= 0)
        {
            Debug.Log("Bleeah");
            Destroy(gameObject);
        }
        else
            Debug.Log("Hit for " + dmg * (1 - DmgReduction) + " " + Health + " left");
    }
}
