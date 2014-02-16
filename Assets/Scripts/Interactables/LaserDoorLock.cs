using UnityEngine;
using System.Collections;

public class LaserDoorLock : LaserInteractable {

	public GameObject bar;
	public Door door;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Interact() {
		if(this.enabled){
			door.SendMessage("Open");
			bar.SetActive(true);
			this.enabled = false;
		}
	}
}
