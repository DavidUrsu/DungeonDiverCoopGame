using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
	public GoldSystem goldSystem; // Reference to your MoneySystem script
	private Text goldText; // Reference to the Text component

	void Start()
	{
		goldText = GetComponent<Text>();
	}

	void Update()
	{
		goldText.text = (goldSystem.Gold).ToString();
	}
}