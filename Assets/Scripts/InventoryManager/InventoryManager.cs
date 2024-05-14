using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<InventoryItem> inventory = new List<InventoryItem>();
    public GameObject myInv;
    public int inventorySpace = 20;
    public List<int> quantities = null;
    public GameObject inventoryManager = null;

    public InventoryItem GetItem(int number = 0)
    {
        Debug.Log(inventory.Count);
        return inventory[number];
    }

    void Start()
    {
        
    }

    private void OnEnable()
    {
        Debug.Log(myInv.gameObject.GetComponent<InventoryBackend>());
        quantities = myInv.gameObject.GetComponent<InventoryBackend>().getQuantities();
        Debug.Log(quantities);
        int i = 0;
        foreach (Transform child in transform)
        {
            InventoryItem inventoryItem = child.GetComponent<InventoryItem>();
            if (inventoryItem != null)
            {
                inventoryItem.addToQuantity(quantities[i++]);
                inventory.Add(inventoryItem);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
