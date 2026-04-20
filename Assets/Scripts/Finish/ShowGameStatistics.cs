using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowGameStatistics : MonoBehaviour {

	public Text StatusText;
	public Text resultText;

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
		EnsureTextReferences();
		resultText.text = "";
		StatusText.text = "Destroyed Evidence: " + GameResult.result.collectedPickups.Count + "/" + GameResult.result.extrasTotalCount;
		StartCoroutine (showSlideShow());
	}

	private void EnsureTextReferences()
	{
		Canvas canvas = LegacyUiTextFactory.EnsureCanvas();

		if (StatusText == null)
		{
			StatusText = LegacyUiTextFactory.CreateText(
				"Status Text",
				canvas.transform,
				new Vector2(0.5f, 0.5f),
				new Vector2(0.5f, 0.5f),
				new Vector2(0f, -32f),
				32,
				TextAnchor.MiddleCenter,
				Color.black);
		}

		if (resultText == null)
		{
			resultText = LegacyUiTextFactory.CreateText(
				"Result Text",
				canvas.transform,
				new Vector2(0.5f, 0.5f),
				new Vector2(0.5f, 0.5f),
				new Vector2(0f, -180f),
				45,
				TextAnchor.MiddleCenter,
				new Color(0f, 0f, 0f, 0f));
		}
	}

	private IEnumerator fadeInGuiText(Text text)
	{

		for (var i = 0; i < 10; i++) 
		{
			var currentColor = text.color;
			text.color = new Color(currentColor.r, currentColor.g, currentColor.b, i / 10.0f);	
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

		while (!GameInput.IsContinuePressed(GameResult.result.isMobile))
		{
			yield return null;
		}

		SceneManager.LoadScene(0);
	}
}
