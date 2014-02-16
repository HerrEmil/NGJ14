using UnityEngine;
using System.Collections;

public class ShowIntro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(showIntro());
	}
	
	private IEnumerator showIntro()
	{
		
		while (GameResult.result.isMobile ? Input.touches.Length == 0 : !Input.GetButton("Fire1"))
		{
			yield return new WaitForSeconds(0.1f);
		}
		
		Application.LoadLevel("level");
	}
}
