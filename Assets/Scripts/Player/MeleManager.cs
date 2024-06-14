using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MeleManager : MonoBehaviour
{
     public BoxCollider2D AttackCollider;
     public Player player;
     public string Name;
     public float chance;
    private void OnEnable()
    {
        AttackCollider.enabled = true;
    }

    private void OnDisable()
    {
        AttackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            (Player, string) pair;
            if (Random.Range(0f, 1f) < chance)
                pair = (player, Name);
            else
                pair = (player, "");

            collision.gameObject.SendMessage("OnHit", pair);
        }

    }

}
