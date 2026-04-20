using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowGameover : MonoBehaviour {
	
	public Text message;
	
	void Start () 
	{
		EnsureMessageReference();
		StartCoroutine (showStatistics(message));
	}

	private void EnsureMessageReference()
	{
		if (message != null)
		{
			return;
		}

		Canvas canvas = LegacyUiTextFactory.EnsureCanvas();
		message = LegacyUiTextFactory.CreateText(
			"Gameover Message",
			canvas.transform,
			new Vector2(0.5f, 0.5f),
			new Vector2(0.5f, 0.5f),
			new Vector2(0f, 120f),
			30,
			TextAnchor.MiddleCenter,
			new Color(1f, 1f, 1f, 0f));
		message.text = "Browser history was discovered...";
	}
	
	private IEnumerator showStatistics(Text text)
	{
		for (var i = 0; i < 10; i++) 
		{
			var currentColor = text.color;
			text.color = new Color(currentColor.r, currentColor.g, currentColor.b, i / 10.0f);	
			yield return new WaitForSeconds(.1f);
		}

		while (!GameInput.IsContinuePressed(GameResult.result.isMobile))
		{
			yield return null;
		}
		
		SceneManager.LoadScene(0);
	}
}
