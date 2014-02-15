using UnityEngine;
using System.Collections;
using ModestTree.Zenject;

public class PickupOnContact : MonoBehaviour 
{
	[Inject]
	private GameController gameController;

	void OnTriggerEnter(Collider other)
	{
		gameController.collectedPickups.Add(this.gameObject);
		Destroy(this.gameObject);
	}

}
