using UnityEngine;
using System.Collections;

public class LoadingBar : MonoBehaviour {

	public float width;

	// Use this for initialization
	void Start () {
		iTween.ScaleTo(gameObject, new Vector3(transform.localScale.x, transform.localScale.y, width), 3f);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
