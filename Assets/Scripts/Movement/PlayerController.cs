using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Moveable
{
    //I recommend 7 for the move speed, and 1.2 for the force damping
    public Rigidbody2D rb;
    private Player cls;
    public float  dashSpeed;
    public float dashCooldown,dashTimer = 0, dashDuration,dashT = 0,attackTimer = 0,  abilityTimer = 0;
    Vector2 PlayerInput, PlayerInputDash;
    bool  wantDash = false;
    bool doAttack = false , doAbility = false;
    readonly bool[] useItems = new bool[4];


    public GameObject mainCamera;
    public Boolean isCameraLocked = false;

    private void Start()
    {
        Initialize();
        cls = gameObject.GetComponent<Player>();

        for(int i = 0;i<4;i++)
            useItems[i] = false;

        if (isCameraLocked == false)
		{
			// Position the camera relative to the player
			mainCamera.transform.localPosition = new Vector3(0, 0, 0);

			// Set the camera's rotation
			mainCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
		}
	}


	void Update()
    {
        cls.ProcessEffects();

        if (IsKnocked == 0 && IsFrozen == 0)
            PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        else
            PlayerInput = Vector2.zero;

        if ( Input.GetKeyDown(KeyCode.Space) && dashTimer <= 0 && IsKnocked == 0 && IsFrozen == 0)
        {
            PlayerInputDash = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            dashTimer = dashCooldown;
            dashT = dashDuration;
            wantDash = true;
        }
        //Left click
        if ( IsFrozen == 0 && IsKnocked == 0 && Input.GetMouseButtonDown(0) && attackTimer <= 0)
        {
            doAttack = true;
            attackTimer = cls.AttackCooldown;
        }
        //RightClick
        if(IsFrozen == 0 && IsKnocked == 0 && Input.GetMouseButtonDown(1) && abilityTimer <= 0)
        {
            doAbility = true;
            abilityTimer = cls.AbilityCooldown;
        }

        for(int i = 0;i<4;i++)
            if(IsFrozen == 0 && IsKnocked == 0 && Input.GetKeyDown("" + (i+1)))
            {
                useItems[i] = true;
            }



        dashTimer -= Time.deltaTime;
        dashT -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
        abilityTimer -= Time.deltaTime;

        if (attackTimer < 0)
            attackTimer = 0;
        if (abilityTimer < 0)
            abilityTimer = 0;
        if (dashTimer < 0)
            dashTimer = 0;
        if (dashT < 0)
            dashT = 0;

        if (isCameraLocked == false)
		{
			// lock the camera to the player
			mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
			mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
		}
    }
    void FixedUpdate()
    {
        Vector2 moveForce = new Vector2();

        for (int i = 0; i < 4;i++)
        {
            if (useItems[i])
            {
                useItems[i] = false;
                Debug.Log("Use Item on Slot " + (i+1));
                if (i == 0)
                    cls.AddEffect(new Freeze());
            }
        }


        if(doAttack)
        {
            doAttack = false;
            cls.Attack();
        }

        if (doAbility) 
        {
            doAbility = false;
            cls.Ability();
        }

        if (wantDash)
        {
            
            if (dashT > 0 && IsKnocked == 0)
            {
                moveForce = PlayerInputDash * dashSpeed;
            }
            else
            {
                wantDash = false;
            }
        }
        else
        {
            moveForce = PlayerInput * cls.MoveSpeed;
        }

        moveForce += forceToApply;
        forceToApply /= forceDamping;
        if (Mathf.Abs(forceToApply.x) <= 0.1f && Mathf.Abs(forceToApply.y) <= 0.1f)
        {
            IsKnocked = 0;     
        }
        if (Mathf.Abs(forceToApply.x) <= 0.01f && Mathf.Abs(forceToApply.y) <= 0.01f)
        {
            forceToApply = Vector2.zero;
        }
        rb.velocity = moveForce;

        faceMouse();
    }

    void faceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        transform.up = direction;
    }
}
