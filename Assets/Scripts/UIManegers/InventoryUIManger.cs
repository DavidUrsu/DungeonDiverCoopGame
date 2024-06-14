using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public Image[] sprites;
    public Text[] numbers;
    public void updateUI(Dictionary<string,int > quantities, Potion[] slots)
    {
        for(int i = 0 ; i < slots.Length; i++) 
        {
            if (slots[i] != null)
            {
                sprites[i].enabled = true;
                sprites[i].sprite = slots[i].GetSprite();
                numbers[i].text = quantities[slots[i].GetEffect().Type()].ToString();
            }
            else
            {
                sprites[i].enabled = false;
                numbers[i].text = "";
            }
        }
    }
}
