using UnityEngine;
using System.Collections;

public class LaserDoorLock : LaserInteractable {

	public GameObject bar;
//	public Door door;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Interact() {
		Debug.Log("Unlocked");
		bar.SetActive(true);
	}
}
