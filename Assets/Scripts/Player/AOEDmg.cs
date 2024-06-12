using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDmg : MonoBehaviour
{
    public Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null )
        {
            collision.gameObject.SendMessage("OnHitAbility", player);
        }

        Destroy(gameObject);
    }
}
