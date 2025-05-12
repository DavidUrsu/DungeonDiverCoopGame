using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Moveable
{
    private Rigidbody2D rb;
    public float MoveSpeed, rotationSpeed;
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 0.05f;
        Initialize();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Target != null && IsKnocked == 0 && IsFrozen == 0)
        {
            RotateTowards(Target.transform, rotationSpeed);
        }
    }
    private void FixedUpdate()
    {
        if (Target != null)
            rb.velocity = (IsFrozen ^ 1) * (IsKnocked ^ 1) * MoveSpeed * new Vector2(transform.up.x, transform.up.y) + forceToApply;

        forceToApply /= forceDamping;
        if (Mathf.Abs(forceToApply.x) <= 0.1f && Mathf.Abs(forceToApply.y) <= 0.1f)
        {
            IsKnocked = 0;
        }
        if (Mathf.Abs(forceToApply.x) <= 0.01f && Mathf.Abs(forceToApply.y) <= 0.01f)
        {
            forceToApply = Vector2.zero;
        }

    }

    private void RotateTowards(Transform target, float roatationSpeed)
    {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, roatationSpeed);

		// Force the rotation of the child object (Mage sprite) to be fixed
		GameObject textureSprite = transform.GetChild(1).gameObject;
		textureSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
        GameObject healthBar = transform.GetChild(0).gameObject;
		healthBar.transform.rotation = Quaternion.Euler(0, 0, 0);
        healthBar.transform.localPosition = new Vector3(0, -0.61f, 0);
	}
}
