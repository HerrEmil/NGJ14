using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Open () {
		GetComponent<AudioSource>().Play();
		GetComponent<Animation>().Play();
	}
}
