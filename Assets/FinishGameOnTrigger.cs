using UnityEngine;
using System.Collections;

public class FinishGameOnTrigger : MonoBehaviour
{
	public GUITexture BrowserHistoryGui;

	// Use this for initialization
	void OnTriggerEnter(Collider other)
	{
		BrowserHistoryGui.texture = Resources.Load<Texture>("BrowserHistory/watchKittens");
		BrowserHistoryGui.gameObject.SetActive(true);
	}
}
