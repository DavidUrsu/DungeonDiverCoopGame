using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Buffable
{

    private Dictionary<Potion ,int> Inventory = new Dictionary<Potion, int>();
    private Potion[] InventorySlots;
    private int numSlots = 4, emptySlots;
    public abstract void Attack();

    public abstract void Ability();

    public void InitializeInventory()
    {
        InventorySlots = new Potion[4];
        emptySlots = numSlots;
        for (int i = 0; i < numSlots; i++) 
        {
            InventorySlots[i] = null;
        }
    }
    public void AddPotion(Potion pot)
    {   
        if (Inventory.ContainsKey(pot)) 
           {
            Inventory[pot] += 1;
           }
        else 
           {
            if (emptySlots == 0)
            {
                throw new System.Exception("Inventory is full");
            }
            else
            {
                for (int i = 0; i < numSlots; i++) 
                {
                    if (InventorySlots[i] == null)
                    {
                        Debug.Log(i);
                        InventorySlots[i] = pot;
                        emptySlots--;
                        break;
                    }
                }

                Inventory.Add(pot, 1);
            }
           }
    }

    public void UsePotion(int Slot) 
    {
        if (InventorySlots[Slot] == null) 
        {
            throw new System.Exception("Slot is empty");
        }
        else
        {
            Potion pot = InventorySlots[Slot];
            Inventory[pot] -= 1;

            AddEffect(pot.GetEffect());

            if (Inventory[pot] <= 0)
            {
                InventorySlots[Slot] = null;
                emptySlots++;
                Inventory.Remove(pot);
            }
        }
    }
    public void HitByEnemy(float dmg)
    {
        CurrentHealth -= dmg * (1 - DamageReduction);
        Debug.Log("Player has " + CurrentHealth + "Health remaning");
        if (CurrentHealth <= 0)
            Destroy(gameObject);
    }
}

