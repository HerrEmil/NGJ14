using UnityEngine;
using System.Collections;
using ModestTree.Zenject;


enum FlyState {
	NORMAL,
	BOOST,
	AIM
}

[RequireComponent(typeof(Rigidbody))]

public class Fly : Controller {

	public float normalSpeed;
	public float boostSpeed;
	public float hRotationSpeed = 1f;
	public float vRotationSpeed = 1f;
	public Animator animator;

	// For debugging, comment out later
//	public string accelerationString;

	float speed;
	FlyState state;

	[Inject]
	Laser laser;

	// Use this for initialization
	void Start () {
		speed = boostSpeed;
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		
		if(Input.GetKeyDown(KeyCode.A)){
			laser.gameObject.SetActive(true);
			state = FlyState.AIM;
			animator.SetInteger("state", 2);
		}
		else if(Input.GetKeyDown(KeyCode.S)){
			state = FlyState.NORMAL;
			animator.SetInteger("state", 0);
		}
		else if(Input.GetKeyDown(KeyCode.D)){
			state = FlyState.BOOST;
			animator.SetInteger("state", 1);
		}

		if(state != FlyState.AIM){
			laser.gameObject.SetActive(false);
		}

		if(state == FlyState.BOOST){
			speed = Mathf.Lerp(speed, boostSpeed, Time.deltaTime);
		}
		else if(state == FlyState.NORMAL){
			speed = Mathf.Lerp(speed, normalSpeed, Time.deltaTime);
		}
		else if(state == FlyState.AIM){
			speed = Mathf.Lerp(speed, 0, Time.deltaTime*3);
		}

//		transform.Translate(transform.forward * speed);
		float mx = Input.GetAxis("Mouse X");
		float my = Input.GetAxis("Mouse Y");

		// Comment out mx and my above and uncomment below before building mobile version
//		float mx = Input.acceleration.x;
//		float my = -Input.acceleration.y;
//		my -= 0.6f;

		// Needed for on-screen print of acceleration
//		Rect rect = new Rect(10, 10, 50, 50);
//		accelerationString = "Device acceleration (" + Input.acceleration.x + ", " + Input.acceleration.y + ", " + Input.acceleration.z + ")";
		
//		rigidbody.AddRelativeTorque(my * Time.deltaTime, mx * Time.deltaTime, 0f);
		float angle =Vector3.Angle(transform.forward, Vector3.up);
		if(angle > 30 && my > 0){
			transform.forward = Quaternion.AngleAxis(-my * vRotationSpeed, transform.right) * transform.forward;
		}
		else if(angle < 150 && my < 0){
			transform.forward = Quaternion.AngleAxis(-my * vRotationSpeed, transform.right) * transform.forward;
		}
		transform.forward = (Quaternion.AngleAxis(mx * hRotationSpeed, Vector3.up) * transform.forward).normalized;
		rigidbody.velocity = transform.forward * speed;

		//Prevent random rotation after hitting other colliders
		rigidbody.angularVelocity = Vector3.zero;


//		rigidbody.angularVelocity = transform.TransformDirection( -my, mx, 0f );

//		transform.RotateAround(transform.right, -my * Time.deltaTime);
//		transform.RotateAround(transform.up, mx * Time.deltaTime);

	}
//
//	void OnGUI () {
//		GUI.Label(new Rect(0,0,500,50),accelerationString);
//	}

	void OnCollisionEnter (Collision c) {
		Debug.Log(c.gameObject.name);

		Vector3 reflected = Vector3.Reflect(transform.forward, c.contacts[0].normal);
		transform.forward = (Quaternion.AngleAxis(Vector3.Angle(transform.forward, reflected), Vector3.Cross(transform.forward, reflected)) * transform.forward).normalized;

	}
}
