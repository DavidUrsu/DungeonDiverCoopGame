using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float force = 20;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.TryGetComponent<Moveable>(out var entity))
        {
            entity.IsKnocked = 1;
            Vector2 dir = (collision.transform.position - collision.otherCollider.transform.position).normalized;
            entity.forceToApply += dir * force;

        }
    }
}
