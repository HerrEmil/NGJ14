using UnityEngine;
using System.Collections;
using ModestTree.Zenject;

public class CameraFollow : MonoBehaviour {

	public GameObject followObject;
	public float smoothness = 10f;
	
	[Inject]
	PaperPlane plane;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = Vector3.Lerp(transform.position, followObject.transform.position, Time.deltaTime * smoothness);
//		transform.LookAt(plane.transform.position);
		transform.rotation = followObject.transform.rotation;
	}
}
