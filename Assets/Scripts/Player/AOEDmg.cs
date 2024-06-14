using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDmg : MonoBehaviour
{
    public Player player;
    public string Name;
    public float chance;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null )
        {
            (Player, string) pair;

            if (Random.Range(0f, 1f) < chance)
                pair = (player, Name);
            else
                pair = (player, "");

            collision.gameObject.SendMessage("OnHitAbility", pair);
        }

        Destroy(gameObject);
    }
}
