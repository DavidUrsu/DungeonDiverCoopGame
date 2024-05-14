using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBackend : MonoBehaviour
{
    // Start is called before the first frame update
    private List<int> quantities = new List<int>(new int[20]);
    public GameObject inventoryManagerUI = null;
    public bool state = false;
    void Start()
    {
        
    }

    public List<int> getQuantities()
    {
        return quantities;
    }

    public void setQuantities(List<int> quantities)
    {
        this.quantities = quantities;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            state = !state;
            if (state)
            {
                inventoryManagerUI.SetActive(state);
            }
            else
            {
                this.setQuantities(inventoryManagerUI.GetComponent<InventoryManager>().quantities);
                inventoryManagerUI.SetActive(state);
            }
        }
    }
}
