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
	public string mousePositionString;
	public string fingerString = "finger: NOT SET";
	int lastFingerIndex = 0;

	float speed;
	FlyState state;

	[Inject]
	Laser laser;

	public bool isMobile =
	#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
			true;
	#else
			false;
	#endif

	// Use this for initialization
	void Start () {
		speed = boostSpeed;
	}

	void Update() {
		if (isMobile)
		{
			UpdateStateMobile();
			mousePositionString = "";
		}
		else
		{
			UpdateStatePC();
			fingerString = "";
		}
	}


// Update is called once per frame
	void FixedUpdate () {

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
		float mx = isMobile ? Input.acceleration.x : Input.GetAxis("Mouse X");
		float my = isMobile ? -Input.acceleration.y - 0.5f : Input.GetAxis("Mouse Y");

		// String for on-screen info
//		mousePositionString = "Input.mousePosition (" + Input.mousePosition[0] + ", " + Input.mousePosition[1] + ")";
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

	void OnGUI () {
//		string str = mousePositionString + "\n" + fingerString;
		GUI.Label(new Rect(0,0,1000,1000),fingerString);
	}

	void OnCollisionEnter (Collision c) {
		Debug.Log(c.gameObject.name);

		Vector3 reflected = Vector3.Reflect(transform.forward, c.contacts[0].normal);
		transform.forward = (Quaternion.AngleAxis(Vector3.Angle(transform.forward, reflected), Vector3.Cross(transform.forward, reflected)) * transform.forward).normalized;

	}

	void UpdateStateMobile()
	{
		// Grab touches in current frame
		int fingerCount = 0;
		foreach (Touch touch in Input.touches) {
			// if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
			fingerCount++;
		}
		
		fingerString = "";
		
		if (fingerCount > 0) {
			print ("User has " + fingerCount + " finger(s) touching the screen");
			fingerString = "User has " + fingerCount + " finger(s) touching the screen";
			
			for (int i = 0; i < fingerCount; i++) {
				if (Input.GetTouch(i).position[0] < (Screen.width / 2)) {
					// Last tap is on left half???
					fingerString = "Last tap: LEFT";
					laser.gameObject.SetActive(true);
					state = FlyState.AIM;
					animator.SetInteger("state", 2);
				} else {
					// Last tap is on right half???
					fingerString = "Last tap: RIGHT";
					state = FlyState.BOOST;
					animator.SetInteger("state", 1);
				}
				fingerString += ("\n Input.GetTouch(" + i + ").position: " + Input.GetTouch(i).position);
			}
		} else {
			state = FlyState.NORMAL;
			animator.SetInteger("state", 0);
		}
		
		
		
		for(int i = 0; i < fingerCount; i++) {
			fingerString += ("\n Input.touches[" + i + "].phase: " + Input.touches[i].phase);
			if(Input.touches[i].phase == TouchPhase.Began) 
			{ 
				lastFingerIndex = Input.touches[i].fingerId; 
			}
		} 
		// Touch lastTouch = Input.touches.ToList().Find(v => v.fingerId == lastFingerIndex);
		
		fingerString += ("\n lastFingerIndex: " + lastFingerIndex);
	}

	void UpdateStatePC()
	{
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
	}
}
