using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	public Transform smoke;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hitInfo;
		bool isHit = Physics.Raycast(transform.position, transform.forward, out hitInfo);

		if(isHit){
			smoke.gameObject.SetActive(true);
			smoke.position = hitInfo.point;

			LaserInteractable interactable = hitInfo.collider.GetComponent<LaserInteractable>();
			if(interactable){
				interactable.SendMessage("Interact");
			}
		}
		else{
			smoke.gameObject.SetActive(false);
		}
	}
	void OnDisable () {
		smoke.gameObject.SetActive(false);
	}
}
