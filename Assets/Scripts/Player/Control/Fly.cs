using UnityEngine;
using System.Collections;
using ModestTree.Zenject;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


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
	public ParticleSystem system;

	public int collisionCount = 0;

	// For debugging, comment out later
	public string mousePositionString;
	public string debugString = "finger: NOT SET";
	int lastFingerIndex = 0;

	float speed;
	FlyState state;

	[Inject]
	Laser laser;

	private readonly bool isMobile =
	#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
			true;
	#else
			false;
	#endif

	// Use this for initialization
	void Start () {
		speed = normalSpeed;
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
		}
	}


// Update is called once per frame
	void FixedUpdate () {

		if(state != FlyState.AIM){
			laser.gameObject.SetActive(false);
		}

		if(state == FlyState.BOOST){
			speed = Mathf.Lerp(speed, boostSpeed, Time.deltaTime);
			var emission = system.emission;
			emission.enabled = true;
		}
		else if(state == FlyState.NORMAL){
			speed = Mathf.Lerp(speed, normalSpeed, Time.deltaTime);
			var emission = system.emission;
			emission.enabled = false;
		}
		else if(state == FlyState.AIM){
			speed = Mathf.Lerp(speed, 0, Time.deltaTime*3);
			var emission = system.emission;
			emission.enabled = false;
		}

//		transform.Translate(transform.forward * speed);
		Vector2 lookInput = isMobile ? Vector2.zero : GameInput.ReadMouseLookAxis();
		Vector3 acceleration = isMobile ? GameInput.ReadAcceleration() : Vector3.zero;
		float mx = isMobile ? acceleration.x : lookInput.x;
		float my = isMobile ? -acceleration.y - 0.5f : lookInput.y;

		// String for on-screen info
//		mousePositionString = "Input.mousePosition (" + Input.mousePosition[0] + ", " + Input.mousePosition[1] + ")";
//      debugString = "Device acceleration (" + Input.acceleration.x + ", " + Input.acceleration.y + ", " + Input.acceleration.z + ")";
//		debugString += "\nisMobile: "+isMobile + "(" + mx + "," + my + ")";

		
//		rigidbody.AddRelativeTorque(my * Time.deltaTime, mx * Time.deltaTime, 0f);
		float angle =Vector3.Angle(transform.forward, Vector3.up);
		if(angle > 30 && my > 0){
			transform.forward = Quaternion.AngleAxis(-my * vRotationSpeed, transform.right) * transform.forward;
		}
		else if(angle < 150 && my < 0){
			transform.forward = Quaternion.AngleAxis(-my * vRotationSpeed, transform.right) * transform.forward;
		}
		transform.forward = (Quaternion.AngleAxis(mx * hRotationSpeed, Vector3.up) * transform.forward).normalized;
		GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;

		//Prevent random rotation after hitting other colliders
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

//		rigidbody.angularVelocity = transform.TransformDirection( -my, mx, 0f );

//		transform.RotateAround(transform.right, -my * Time.deltaTime);
//		transform.RotateAround(transform.up, mx * Time.deltaTime);

	}

	void OnGUI () {
//		string str = mousePositionString + "\n" + fingerString;
//		GUI.Label(new Rect(0,0,1000,1000),debugString);
//		GUI.Label(new Rect(0, 0, 200, 40), collisionCount + "");
	}

	void OnCollisionEnter (Collision c) {

		Vector3 reflected = Vector3.Reflect(transform.forward, c.contacts[0].normal);
		transform.forward = (Quaternion.AngleAxis(Vector3.Angle(transform.forward, reflected), Vector3.Cross(transform.forward, reflected)) * transform.forward).normalized;
		if(++collisionCount > 10){
			SceneManager.LoadScene("Lose");
		}
	}

	void UpdateStateMobile()
	{
		// Grab touches in current frame
		int fingerCount = GameInput.TouchCount;
		
		debugString = "";
		
		if (fingerCount > 0) {
			print ("User has " + fingerCount + " finger(s) touching the screen");
			debugString = "User has " + fingerCount + " finger(s) touching the screen";
			
			for (int i = 0; i < fingerCount; i++) {
				GameTouchData touch = GameInput.GetTouch(i);
				if (touch.Position.x < (Screen.width / 2f)) {
					// Last tap is on left half???
					debugString = "Last tap: LEFT";
					laser.gameObject.SetActive(true);
					state = FlyState.AIM;
					animator.SetInteger("state", 2);
				} else {
					// Last tap is on right half???
					debugString = "Last tap: RIGHT";
					state = FlyState.BOOST;
					animator.SetInteger("state", 1);
				}
				debugString += ("\n Touch " + i + ".position: " + touch.Position);
			}
		} else {
			state = FlyState.NORMAL;
			animator.SetInteger("state", 0);
		}
		
		
		
		for(int i = 0; i < fingerCount; i++) {
			GameTouchData touch = GameInput.GetTouch(i);
			debugString += ("\n Touch " + i + ".phase: " + touch.Phase);
			if(touch.Phase == GameTouchPhase.Began) 
			{ 
				lastFingerIndex = touch.TouchId; 
			}
		} 
		// Touch lastTouch = Input.touches.ToList().Find(v => v.fingerId == lastFingerIndex);
		
		debugString += ("\n lastFingerIndex: " + lastFingerIndex);
	}

	void UpdateStatePC()
	{
		if(GameInput.WasKeyboardKeyPressed(Key.A, KeyCode.A)){
			laser.gameObject.SetActive(true);
			state = FlyState.AIM;
			animator.SetInteger("state", 2);
		}
		else if(GameInput.WasKeyboardKeyPressed(Key.S, KeyCode.S)){
			state = FlyState.NORMAL;
			animator.SetInteger("state", 0);
		}
		else if(GameInput.WasKeyboardKeyPressed(Key.D, KeyCode.D)){
			state = FlyState.BOOST;
			animator.SetInteger("state", 1);
		}
	}
}
