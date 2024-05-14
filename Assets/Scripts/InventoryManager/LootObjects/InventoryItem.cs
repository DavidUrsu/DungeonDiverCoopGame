using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    public int quantity;
    private bool removable;
    private TMP_Text text;
    private Image image;

    public InventoryItem(int quantity = 0, bool removable = true)
    {
        this.quantity = quantity;
        this.removable = removable;
    }

    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        Debug.Log(text);
        this.image = transform.gameObject.GetComponent<Image>();
        if (this.quantity == 0)
        {
            if (this.text != null)
                text.gameObject.SetActive(false);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addToQuantity(int n)
    {
        this.quantity += n;
        if (this.quantity > 0)
        {
            text.gameObject.SetActive(true);
            text.text = this.quantity.ToString();
        }
    }

}
