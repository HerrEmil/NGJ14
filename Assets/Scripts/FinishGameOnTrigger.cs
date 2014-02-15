
using UnityEngine;
using System.Collections;
using ModestTree.Zenject;

public class FinishGameOnTrigger : MonoBehaviour
{	
	[Inject]
	private AudioSource gameMusic;
	[Inject]
	private GameController gameController;

	// Use this for initialization
	void OnTriggerEnter(Collider other)
	{
		gameController.isFinished = true;
	}

}
