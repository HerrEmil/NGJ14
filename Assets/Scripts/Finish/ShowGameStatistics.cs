using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowGameStatistics : MonoBehaviour {

	public GUIText collectibles;
	public Renderer backGroundText;
	public Texture[] textures;

	public float transitionTime;

	void Start () 
	{
		collectibles.text = "Destroyed Evidence: " + GameResult.result.collectedPickups.Count;
		StartCoroutine (showSlideShow());
	}

	private IEnumerator showStatistics(GUIText text)
	{

		for (var i = 0; i < 10; i++) 
		{
			var currentColor = text.color;
			text.color = new Color(currentColor.r,currentColor.b,currentColor.g, i/10.0f);	
			yield return new WaitForSeconds(.1f);
		}
	}

	private IEnumerator showSlideShow()
	{
		foreach (var texture in textures)
		{
			backGroundText.material.mainTexture = texture;
			Debug.Log(texture.name);
			yield return new WaitForSeconds(transitionTime);
		}
	}
}
