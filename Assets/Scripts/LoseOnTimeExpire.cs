using UnityEngine;
using System.Collections;
using ModestTree.Zenject;

public class LoseOnTimeExpire : MonoBehaviour 
{
	public float timeLimit;
	public GUIText countDownGui;

	public float remainingTime {get; private set;}

	[Inject]
	private GameController gameController;

	void Start()
	{
		remainingTime = timeLimit;
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
