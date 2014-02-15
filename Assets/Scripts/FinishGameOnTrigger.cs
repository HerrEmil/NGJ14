using UnityEngine;
using System.Collections;

public class FinishGameOnTrigger : MonoBehaviour
{
	public GUITexture endScreenGui;
	public MusicController musicController;


	// Use this for initialization
	void OnTriggerEnter(Collider other)
	{
		endScreenGui.texture = Resources.Load<Texture>("BrowserHistory/watchKittens");
		endScreenGui.gameObject.SetActive(true);
		musicController.PlayWinAudio();
	}
}
