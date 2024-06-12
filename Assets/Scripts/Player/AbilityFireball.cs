using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityFireball : ProjectileManger
{
    public GameObject explosionPrefab;
    private void Start()
    {
        speed = 10f;
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject explosion = Instantiate(explosionPrefab,transform.position,transform.rotation);
        AOEDmg aoe = explosion.GetComponent<AOEDmg>();
        aoe.player = player;
        Destroy(gameObject);
    }
}
