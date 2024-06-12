using System.Collections;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
	public Sprite sprite1; // Assign this in the Inspector
	public Sprite sprite2; // Assign this in the Inspector
	public float switchTime = 3f; // Time in seconds to switch between sprites

	private SpriteRenderer spriteRenderer;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		StartCoroutine(SwitchSprites());
	}

	IEnumerator SwitchSprites()
	{
		while (true)
		{
			spriteRenderer.sprite = sprite1;
			yield return new WaitForSeconds(switchTime);
			spriteRenderer.sprite = sprite2;
			yield return new WaitForSeconds(switchTime);
		}
	}
}