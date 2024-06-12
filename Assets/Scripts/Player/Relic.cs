using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour
{
    private GameObject[] players;
    public Cleric owner;
    private float HealTimer, HealCooldown, HealAmount;
    void Start()
    {

        players = GameObject.FindGameObjectsWithTag("Player");
        HealTimer = 0f;
        HealCooldown = 1f;
        HealAmount = owner.AttackDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if(HealTimer <= 0)
        {
            HealTimer = HealCooldown;

            foreach(GameObject player in players)
            {
                if(Vector2.Distance(player.transform.position,transform.position) <= 5)
                {
                    Player cls = player.GetComponent<Player>();

                    if(cls.CurrentHealth + HealAmount >cls.MaxHealth)
                    {
                        cls.CurrentHealth = cls.MaxHealth;
                    }
                    else
                        cls.CurrentHealth += HealAmount;
                }
            }
        }

        HealTimer -= Time.deltaTime;
    }
}
