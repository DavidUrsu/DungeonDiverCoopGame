using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootObject : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] players;
    private int quantity;
    private InventoryManager inventoryManager;
    private InventoryItem inventoryItemReff;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void collected()
    {
        inventoryManager = GameObject.FindAnyObjectByType<InventoryManager>();
        inventoryItemReff = inventoryManager.GetItem(1); // value should be randomized;
        Debug.Log(inventoryItemReff);
        inventoryItemReff.addToQuantity(1);
        Destroy(gameObject);
    }
}
