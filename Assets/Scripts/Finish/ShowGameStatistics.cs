using UnityEngine;
using System.Collections;

public class ShowGameStatistics : MonoBehaviour {

	public GUIText collectibles;

	void Start () {

		collectibles.text = "Destroyed Evidence: " + GameResult.result.collectedPickups.Count;
		StartCoroutine (showStatistics(collectibles));
	}

	private IEnumerator showStatistics(GUIText text)
	{
		for (var i = 0; i < 10; i++) 
		{
			var currentColor = text.color;
			text.color = new Color(currentColor.r,currentColor.b,currentColor.g, i/10.0f);	
			yield return new WaitForSeconds(.1f);
		}
	}
}
