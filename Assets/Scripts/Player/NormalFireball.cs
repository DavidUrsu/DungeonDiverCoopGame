using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFireball : ProjectileManger
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            collision.gameObject.SendMessage("OnHit", player);
        }
        Destroy(gameObject);
    }
}
