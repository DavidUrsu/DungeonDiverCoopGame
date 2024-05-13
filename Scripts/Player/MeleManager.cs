using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MeleManager : MonoBehaviour
{
     public BoxCollider2D AttackCollider;
     public Player player;
    private void OnEnable()
    {

        if (AttackCollider == null)
            Debug.LogWarning("Attack collider null");
        else
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
            collision.gameObject.SendMessage("OnHit", player);
            Debug.Log("Hit Something");
        }

    }

}
