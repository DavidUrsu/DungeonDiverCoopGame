using UnityEngine;

public class GoldSystem : MonoBehaviour
{
	public int Gold { get; private set; }

	void Start()
	{
		Gold = 1000; // Start with 0 Gold
	}

	public void AddGold(int amount)
	{
		Gold += amount;
	}

	public void SubtractGold(int amount)
	{
		if (Gold >= amount)
		{
			Gold -= amount;
		}
		else
		{
			Debug.Log("Not enough Gold!");
		}
	}

	public bool CanAfford(int cost)
	{
		return Gold >= cost;
	}
}