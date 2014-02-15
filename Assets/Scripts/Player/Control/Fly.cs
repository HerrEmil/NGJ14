using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Fly : Controller {

	public float speed;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		transform.Translate(transform.forward * speed);
		float mx = Input.GetAxis("Mouse X");
		float my = Input.GetAxis("Mouse Y");
		
//		rigidbody.AddRelativeTorque(my * Time.deltaTime, mx * Time.deltaTime, 0f);

		transform.forward = Quaternion.AngleAxis(-my, transform.right) * transform.forward;
		transform.forward = Quaternion.AngleAxis(mx, Vector3.up) * transform.forward;
		rigidbody.velocity = transform.forward * speed;


//		rigidbody.angularVelocity = transform.TransformDirection( -my, mx, 0f );

//		transform.RotateAround(transform.right, -my * Time.deltaTime);
//		transform.RotateAround(transform.up, mx * Time.deltaTime);

	}

	void OnCollisionEnter (Collision c) {
		Debug.Log(c.gameObject.name);

	}
}
