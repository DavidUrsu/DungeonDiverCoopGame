using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFireball : ProjectileManger
{
    public string Name;
    public float chance;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        (Player, string) pair;
        if (enemy != null)
        {
            if (Random.Range(0f, 1f) < chance)
            {
                pair = (player, Name);
            }
            else
            {
                pair = (player, "");
            }




            collision.gameObject.SendMessage("OnHit", pair);
        }
        Destroy(gameObject);
    }
}
