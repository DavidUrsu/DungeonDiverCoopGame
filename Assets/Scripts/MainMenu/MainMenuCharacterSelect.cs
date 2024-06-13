using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCharacterSelect : MonoBehaviour
{
	public GameObject mageSelector;
	public GameObject knightSelector;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject == mageSelector)
		{
			SelectCharacter("Mage");
		}
		else if (other.gameObject == knightSelector)
		{
			SelectCharacter("Knight");
		}
	}

	private void SelectCharacter(string characterType)
	{
		PlayerPrefs.SetString("SelectedCharacter", characterType);
		Debug.Log("Selected character: " + characterType);
		Debug.Log("Loading game scene...");
		SceneManager.LoadScene("DungeonGenerator");
	}
}
