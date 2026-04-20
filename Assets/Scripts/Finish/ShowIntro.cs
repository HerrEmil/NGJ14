using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShowIntro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(showIntro());
	}
	
	private IEnumerator showIntro()
	{
		
		while (!GameInput.IsContinuePressed(GameResult.result.isMobile))
		{
			yield return null;
		}
		
		SceneManager.LoadScene("level");
	}
}
