using UnityEngine;
using System.Collections;

public class FinishGameOnTrigger : MonoBehaviour
{
	public GUIText BrowserHistoryGui;

	// Use this for initialization
	void OnTriggerEnter(Collider other)
	{
		BrowserHistoryGui.text = Resources.Load<TextAsset>("BrowserHistory").text;
		BrowserHistoryGui.gameObject.SetActive(true);
	}
}
