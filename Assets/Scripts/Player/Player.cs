using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Buffable
{

    private Dictionary<string ,int> Inventory = new Dictionary<string, int>();
    private Potion[] InventorySlots;
    private int numSlots = 4, emptySlots;
    public InventoryUIManager man;
    public abstract void Attack();

    public abstract void Ability();

    public void Update()
    {
        if (CurrentHealth <= 0)
            Destroy(gameObject);
    }
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
        if (Inventory.ContainsKey(pot.GetEffect().Type())) 
           {
            Debug.Log("Adding extra potion");
            Inventory[pot.GetEffect().Type()] += 1;
           }
        else 
           {
            if (emptySlots == 0)
            {
                throw new System.Exception("Inventory is full");
            }
            else
            {
                Debug.Log("Adding potion");
                for (int i = 0; i < numSlots; i++) 
                {
                    if (InventorySlots[i] == null)
                    {
                        InventorySlots[i] = pot;
                        emptySlots--;
                        break;
                    }
                }

                Inventory.Add(pot.GetEffect().Type(), 1);
            }
           }

        man.updateUI(Inventory, InventorySlots);
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
            Inventory[pot.GetEffect().Type()] -= 1;

            AddEffect(pot.GetEffect());

            if (Inventory[pot.GetEffect().Type()] <= 0)
            {
                InventorySlots[Slot] = null;
                emptySlots++;
                Inventory.Remove(pot.GetEffect().Type());
            }
        }
        man.updateUI(Inventory, InventorySlots);
    }
    public void HitByEnemy(float dmg)
    {
        CurrentHealth -= dmg * (1 - DamageReduction);
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);

            // Load the Main Menu scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}

