using UnityEngine;
using System.Collections;


enum FlyState {
	NORMAL,
	BOOST,
	AIM
}

[RequireComponent(typeof(Rigidbody))]

public class Fly : Controller {

	public float normalSpeed;
	public float boostSpeed;

	float speed;
	FlyState state;

	// Use this for initialization
	void Start () {
		speed = boostSpeed;
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		
		if(Input.GetKeyDown(KeyCode.A)){
			state = FlyState.AIM;
		}
		else if(Input.GetKeyDown(KeyCode.S)){
			state = FlyState.NORMAL;
		}
		else if(Input.GetKeyDown(KeyCode.D)){
			state = FlyState.BOOST;
		}

		if(state == FlyState.BOOST){
			speed = Mathf.Lerp(speed, boostSpeed, Time.deltaTime);
		}
		if(state == FlyState.NORMAL){
			speed = Mathf.Lerp(speed, normalSpeed, Time.deltaTime);
		}
		if(state == FlyState.AIM){
			speed = Mathf.Lerp(speed, 0, Time.deltaTime);
		}

//		transform.Translate(transform.forward * speed);
		float mx = Input.GetAxis("Mouse X");
		float my = Input.GetAxis("Mouse Y");
		
//		rigidbody.AddRelativeTorque(my * Time.deltaTime, mx * Time.deltaTime, 0f);

		transform.forward = Quaternion.AngleAxis(-my, transform.right) * transform.forward;
		transform.forward = (Quaternion.AngleAxis(mx, Vector3.up) * transform.forward).normalized;
		rigidbody.velocity = transform.forward * speed;

		//Prevent random rotation after hitting other colliders
		rigidbody.angularVelocity = Vector3.zero;


//		rigidbody.angularVelocity = transform.TransformDirection( -my, mx, 0f );

//		transform.RotateAround(transform.right, -my * Time.deltaTime);
//		transform.RotateAround(transform.up, mx * Time.deltaTime);

	}

	void OnCollisionEnter (Collision c) {
		Debug.Log(c.gameObject.name);

	}
}
