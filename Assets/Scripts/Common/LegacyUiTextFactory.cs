using UnityEngine;
using UnityEngine.UI;

public static class LegacyUiTextFactory
{
	private static Font cachedFont;

	public static Canvas EnsureCanvas(string name = "Legacy UI Canvas")
	{
		Canvas canvas = Object.FindFirstObjectByType<Canvas>();
		if (canvas != null)
		{
			return canvas;
		}

		GameObject canvasObject = new GameObject(name);
		canvas = canvasObject.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvasObject.AddComponent<CanvasScaler>();
		canvasObject.AddComponent<GraphicRaycaster>();
		return canvas;
	}

	public static Text CreateText(
		string name,
		Transform parent,
		Vector2 anchorMin,
		Vector2 anchorMax,
		Vector2 anchoredPosition,
		int fontSize,
		TextAnchor alignment,
		Color color)
	{
		GameObject textObject = new GameObject(name);
		textObject.transform.SetParent(parent, false);

		RectTransform rectTransform = textObject.AddComponent<RectTransform>();
		rectTransform.anchorMin = anchorMin;
		rectTransform.anchorMax = anchorMax;
		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		rectTransform.anchoredPosition = anchoredPosition;
		rectTransform.sizeDelta = new Vector2(900f, 200f);

		Text text = textObject.AddComponent<Text>();
		text.font = GetFallbackFont();
		text.fontSize = fontSize;
		text.alignment = alignment;
		text.color = color;
		text.horizontalOverflow = HorizontalWrapMode.Wrap;
		text.verticalOverflow = VerticalWrapMode.Overflow;
		return text;
	}

	private static Font GetFallbackFont()
	{
		if (cachedFont != null)
		{
			return cachedFont;
		}

		string[] preferredFonts =
		{
			"Arial",
			"Segoe UI",
			"Tahoma",
			"Verdana"
		};

		cachedFont = Font.CreateDynamicFontFromOSFont(preferredFonts, 16);

		if (cachedFont == null)
		{
			Debug.LogError("Could not create a fallback UI font from OS fonts.");
		}

		return cachedFont;
	}
}
