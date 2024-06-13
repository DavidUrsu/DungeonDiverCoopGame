using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyableItem : MonoBehaviour
{
	public TextMesh priceText;
	public GoldSystem playerGoldSystem;
	public GameObject player;

	private int price;
	private GameObject item;
	private ItemPool itemPool;

	void Start()
	{
		playerGoldSystem = GameObject.Find("Game Manager").GetComponent<GoldSystem>();

		// Get the ItemPool from the GameManager
		itemPool = GameObject.Find("Game Manager").GetComponent<ItemPool>();
		Debug.Log(itemPool.items.Count);

		// Create a new TextMesh object
		GameObject textObject = new GameObject("PriceText");
		priceText = textObject.AddComponent<TextMesh>();

		// Set the TextMesh properties
		priceText.fontSize = 20;
		priceText.color = Color.white;
		priceText.anchor = TextAnchor.MiddleCenter;

		// Set the TextMesh as a child of the BuyableItem
		textObject.transform.SetParent(transform, false);
		textObject.transform.localPosition = new Vector3(0, -0.75f, 0); // Position the text at the center of the BuyableItem
		textObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // Scale down the text

		// Choose a random item from the item pool
		Item chosenItem = itemPool.items[Random.Range(0, itemPool.items.Count)];
		
		item = Instantiate(chosenItem.item, transform.position, Quaternion.identity, transform);

		// Set the price to a random value within 10% of the item's base price
		float priceModifier = Random.Range(-0.1f, 0.1f);
		price = Mathf.RoundToInt(chosenItem.basePrice * (1 + priceModifier));

		// Display the price
		priceText.text = "Price: " + price;

		// Find the player based on the selected character type
		string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "Mage");
		GameObject[] potentialPlayers = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject potentialPlayer in potentialPlayers)
		{
			if (potentialPlayer.name.Contains(selectedCharacter))
			{
				player = potentialPlayer;
				break; // Found the player, no need to continue the loop
			}
		}

		if (player == null && potentialPlayers.Length > 0)
		{
			// Fallback to the first player object if no specific match is found
			player = potentialPlayers[0];
		}
	}

	void Update()
	{

		// Check if the player is near the item
		if (Vector3.Distance(transform.position, player.transform.position) < 1f)
		{
			// Change the color of the priceText to green
			if (playerGoldSystem.CanAfford(price))
			{
				priceText.color = Color.green;
			} else
			{
				priceText.color = Color.red;
			}

			// Check if the player presses space
			if (Input.GetKeyDown(KeyCode.Space))
			{
				BuyItem();
			}
		}
		else
		{
			// Change the color of the priceText back to white when the player is not near
			priceText.color = Color.white;
		}
	}

	void BuyItem()
	{
		// Check if the player has enough gold
		if (playerGoldSystem.CanAfford(price))
		{
			// Subtract the price from the player's gold
			playerGoldSystem.SubtractGold(price);

			// Give the item to the player
			// This depends on how you're handling items in your game
			// For example, you might add the item to the player's inventory

			switch (item.name)
			{
				case "DmgUp(Clone)":
					Mage playerMage = player.GetComponent<Mage>();
					Knight playerKnight = player.GetComponent<Knight>();
					if (playerMage != null)
					{
						playerMage.AttackDamage *= 1.1f;
					}
					else if (playerKnight != null)
					{
						playerKnight.AttackDamage *= 1.1f;
					}
					break;
				case "HpUp(Clone)":
					Mage mage = player.GetComponent<Mage>();
					Knight knight = player.GetComponent<Knight>();
					if (mage != null)
					{
						mage.MaxHealth *= 1.1f;
						mage.CurrentHealth = mage.MaxHealth;
					}
					else if (knight != null)
					{
						knight.MaxHealth *= 1.1f;
						knight.CurrentHealth = knight.MaxHealth;
					}
					break;
				case "HealthPotion(Clone)":
					// TODO: Add health potion to player's inventory
					break;

			}
			// Destroy the BuyableItem
			Destroy(gameObject);
			Destroy(item);
		}
	}
}