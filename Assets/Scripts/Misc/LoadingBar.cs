using UnityEngine;
using System.Collections;

public class LoadingBar : MonoBehaviour {

	public float width;
	public float duration = 3f;

	void Start () {
		StartCoroutine(AnimateWidth());
	}

	private IEnumerator AnimateWidth()
	{
		Vector3 startScale = transform.localScale;
		Vector3 targetScale = new Vector3(startScale.x, startScale.y, width);
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float progress = duration <= 0f ? 1f : Mathf.Clamp01(elapsed / duration);
			transform.localScale = Vector3.Lerp(startScale, targetScale, progress);
			yield return null;
		}

		transform.localScale = targetScale;
	}
}
