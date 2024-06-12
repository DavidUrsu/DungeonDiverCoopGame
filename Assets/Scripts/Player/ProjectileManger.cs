using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManger : MonoBehaviour
{
    public float speed = 7f, lifeTime = 3f;
    public Player player;
    protected Rigidbody2D rb;

    private void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

}
