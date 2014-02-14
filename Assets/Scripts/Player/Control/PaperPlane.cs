using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PaperPlane : MonoBehaviour {

	public float vSensitivity = 1f;
	public float hSensitivity = 1f;

	public float power = 5f;
	public float weakpower = 2f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float mx = Input.GetAxis("Mouse X");
		float my = Input.GetAxis("Mouse Y");

		rigidbody.AddRelativeTorque(new Vector3(my*hSensitivity, 0, -mx*vSensitivity));
		rigidbody.velocity = transform.forward * power;

		power -= transform.forward.y;
		power = Mathf.Max(power, 0);

		if(power < weakpower){
			Debug.Log((weakpower-power) / weakpower);
			rigidbody.AddRelativeTorque( (weakpower-power)/weakpower * Time.deltaTime * 35f, 0, 0);
		}
	}
}
