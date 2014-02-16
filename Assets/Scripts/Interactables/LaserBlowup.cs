using UnityEngine;
using System.Collections;
using ModestTree.Zenject;

public class LaserBlowup : LaserInteractable {

	public GameObject SmokeSystem;
	public GameObject FireSystem;
	public AudioClip explosionSound;

	[Inject]
	public AudioManager audioManager;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Interact () {
		if(this.enabled){
			RaycastHit hitInfo;
			bool hit = Physics.Raycast(transform.position, Vector3.down, out hitInfo);
			Debug.Log(hitInfo);
			if(hit){
				rigidbody.AddExplosionForce(100f, transform.position - transform.up * 1f, 4f);
				GameObject.Instantiate(SmokeSystem, hitInfo.point, Quaternion.identity);
				GameObject.Instantiate(FireSystem, hitInfo.point, Quaternion.identity);
				audioManager.Play(explosionSound, hitInfo.point);
				this.enabled = false;
			}
		}
	}
}
