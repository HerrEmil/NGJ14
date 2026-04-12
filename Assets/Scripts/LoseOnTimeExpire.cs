using UnityEngine;
using System.Collections;
using ModestTree.Zenject;
using UnityEngine.UI;

public class LoseOnTimeExpire : MonoBehaviour 
{
	public float timeLimit;
	public Text countDownGui;

	public float remainingTime {get; private set;}

	[Inject]
	private GameController gameController;

	void Start()
	{
		EnsureCountdownReference();
		remainingTime = timeLimit;
	}

	private void EnsureCountdownReference()
	{
		if (countDownGui != null)
		{
			return;
		}

		Canvas canvas = LegacyUiTextFactory.EnsureCanvas();
		countDownGui = LegacyUiTextFactory.CreateText(
			"Countdown",
			canvas.transform,
			new Vector2(0f, 1f),
			new Vector2(0f, 1f),
			new Vector2(220f, -40f),
			28,
			TextAnchor.MiddleLeft,
			Color.white);
	}

	void FixedUpdate()
	{
		if (gameController.gameover)
			return;
		remainingTime = Mathf.Max(0f, remainingTime - Time.deltaTime);

		var roundedRestMilis = Mathf.Round(remainingTime * 100) % 100;
		countDownGui.text = "Remaining Time: " + Mathf.Floor(remainingTime) + "." + roundedRestMilis;

		if (remainingTime <= 0)
		{
			gameController.gameover = true;
		}
	}


}
