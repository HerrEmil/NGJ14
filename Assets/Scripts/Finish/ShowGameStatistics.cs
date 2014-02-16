using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowGameStatistics : MonoBehaviour {

	public GUIText StatusText;
	public GUIText resultText;

	public Renderer backGroundRenderer;
	public Texture statusTexture;
	public Texture destroyAllTexture;
	public Texture destroySomeTexture;
	public TextAsset destroyAllText;
	public TextAsset destroySomeText;
	public TextAsset destroyAllResult;
	public TextAsset destroySomeResult;

	public float transitionTime;

	void Start () 
	{
		resultText.text = "";
		StatusText.text = "Destroyed Evidence: " + GameResult.result.collectedPickups.Count + "/" + GameResult.result.extrasTotalCount;
		StartCoroutine (showSlideShow());
	}

	private IEnumerator fadeInGuiText(GUIText text)
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
		backGroundRenderer.material.mainTexture = statusTexture;
		yield return new WaitForSeconds(transitionTime);

		bool destroyedAll = GameResult.result.collectedPickups.Count == GameResult.result.extrasTotalCount;
		backGroundRenderer.material.mainTexture = destroyedAll ? destroyAllTexture : destroySomeTexture;
		StatusText.text = destroyedAll ? destroyAllText.text : destroySomeText.text;
		yield return new WaitForSeconds(2f);

		resultText.text = destroyedAll ? destroyAllResult.text : destroySomeResult.text;
		StartCoroutine (fadeInGuiText(resultText));

		while (GameResult.result.isMobile ? Input.touches.Length == 0 : !Input.GetButton("Fire1"))
		{
			yield return new WaitForSeconds(0.1f);
		}

		Application.LoadLevel(0);
	}
}