using UnityEngine;
using System.Collections;

public class Fly : Controller {

	public float speed;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate(transform.forward * speed);
		float mx = Input.GetAxis("Mouse X");
		float my = Input.GetAxis("Mouse Y");

		transform.RotateAround(transform.right, -my * Time.deltaTime);
		transform.RotateAround(transform.up, mx * Time.deltaTime);
	}
}
