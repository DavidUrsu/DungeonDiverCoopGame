using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Triggered");
		// Check if the colliding object is the player
		if (other.CompareTag("Player")) // Make sure your player GameObject has the "Player" tag
		{
			// Load the MainMenu scene
			Debug.Log("Player entered the portal");
			SceneManager.LoadScene("MainMenu");
		}
	}
}