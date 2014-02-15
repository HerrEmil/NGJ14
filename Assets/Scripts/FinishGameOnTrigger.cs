
using UnityEngine;
using System.Collections;
using ModestTree.Zenject;

public class FinishGameOnTrigger : MonoBehaviour
{
	public GUITexture endScreenGui;
	
	[Inject]
	private AudioSource gameMusic;
	[Inject]
	private GameController gameController;


	// Use this for initialization
	void OnTriggerEnter(Collider other)
	{
		endScreenGui.texture = Resources.Load<Texture>("BrowserHistory/watchKittens");
		endScreenGui.gameObject.SetActive(true);
		gameController.isFinished = true;
	}

}
