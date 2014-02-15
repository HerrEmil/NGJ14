using UnityEngine;
using System.Collections;

public class LoseOnTimeExpire : MonoBehaviour 
{
	public float timeLimit;
	public GUITexture endScreenGui;
	public GUIText countDownGui;

	public float remainingTime {get; private set;}

	void Start()
	{
		remainingTime = timeLimit;
	}

	void FixedUpdate()
	{
		if (endScreenGui.gameObject.activeInHierarchy)
			return;
		remainingTime = Mathf.Max(0f, remainingTime - Time.deltaTime);

		var roundedRestMilis = Mathf.Round(remainingTime * 100) % 100;
		countDownGui.text = "Remaining Time: " + Mathf.Floor(remainingTime) + "." + roundedRestMilis;

		if (remainingTime <= 0)
		{
			endScreenGui.texture = Resources.Load<Texture>("Lose");
			endScreenGui.gameObject.SetActive(true);
		}
	}


}
