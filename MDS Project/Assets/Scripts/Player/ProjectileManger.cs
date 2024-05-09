using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManger : MonoBehaviour
{
    public float speed = 10f, lifeTime = 3f;
    public Mage player;
    private Rigidbody2D rb;

    private void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            collision.gameObject.SendMessage("OnHit", player.attackDmg);
        }
        Destroy(gameObject);
    }
}
