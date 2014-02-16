using UnityEngine;
using System.Collections;

public class ShowGameover : MonoBehaviour {
	
	public GUIText message;
	
	void Start () 
	{
		StartCoroutine (showStatistics(message));
	}
	
	private IEnumerator showStatistics(GUIText text)
	{
		for (var i = 0; i < 10; i++) 
		{
			var currentColor = text.color;
			text.color = new Color(currentColor.r,currentColor.b,currentColor.g, i/10.0f);	
			yield return new WaitForSeconds(.1f);
		}

		while (GameResult.result.isMobile ? Input.touches.Length == 0 : !Input.GetButton("Fire1"))
		{
			yield return new WaitForSeconds(0.1f);
		}
		
		Application.LoadLevel(0);
	}
}
