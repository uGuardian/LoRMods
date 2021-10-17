using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using GameSave;
using HarmonyLib;
using StoryScene;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public static partial class ConfigAPI {
	// Token: 0x0200205A RID: 8282
	public class UtilTools
	{
		// Token: 0x060095CB RID: 38347 RVA: 0x002C2E70 File Offset: 0x002C1070
		public static InputField CreateInputField(Transform parent, string Imagepath, Vector2 position, TextAnchor tanchor, int fsize, Color tcolor, Font font)
		{
			GameObject gameObject = UtilTools.CreateImage(parent, Imagepath, new Vector2(1f, 1f), position).gameObject;
			Text text = UtilTools.CreateText(gameObject.transform, new Vector2(0f, 0f), fsize, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0f, 0f), tanchor, tcolor, font);
			text.text = "";
			InputField inputField = gameObject.AddComponent<InputField>();
			inputField.targetGraphic = gameObject.GetComponent<Image>();
			inputField.textComponent = text;
			return inputField;
		}

		// Token: 0x060095CC RID: 38348 RVA: 0x002C2F18 File Offset: 0x002C1118
		public static Button AddButton(Image target)
		{
			Button button = target.gameObject.AddComponent<Button>();
			button.targetGraphic = target;
			return button;
		}

		// Token: 0x060095CD RID: 38349 RVA: 0x002C2F40 File Offset: 0x002C1140
		public static Image CreateImage(Transform parent, string Imagepath, Vector2 scale, Vector2 position)
		{
			GameObject gameObject = new GameObject("Image");
			Image image = gameObject.AddComponent<Image>();
			image.transform.SetParent(parent);
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(File.ReadAllBytes(Imagepath));
			Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
			image.sprite = sprite;
			image.rectTransform.sizeDelta = new Vector2((float)texture2D.width, (float)texture2D.height);
			gameObject.SetActive(true);
			gameObject.transform.localScale = scale;
			gameObject.transform.localPosition = position;
			return image;
		}

		// Token: 0x060095CE RID: 38350 RVA: 0x002C3010 File Offset: 0x002C1210
		public static Text CreateText(Transform target, Vector2 position, int fsize, Vector2 anchormin, Vector2 anchormax, Vector2 anchorposition, TextAnchor anchor, Color tcolor, Font font)
		{
			GameObject gameObject = new GameObject("Text");
			Text text = gameObject.AddComponent<Text>();
			gameObject.transform.SetParent(target);
			text.rectTransform.sizeDelta = Vector2.zero;
			text.rectTransform.anchorMin = anchormin;
			text.rectTransform.anchorMax = anchormax;
			text.rectTransform.anchoredPosition = anchorposition;
			text.text = " ";
			text.font = font;
			text.fontSize = fsize;
			text.color = tcolor;
			text.alignment = anchor;
			gameObject.transform.localScale = new Vector3(1f, 1f);
			gameObject.transform.localPosition = position;
			gameObject.SetActive(true);
			return text;
		}

		// Token: 0x060095CF RID: 38351 RVA: 0x002C30E0 File Offset: 0x002C12E0
		public static TextMeshProUGUI CreateText_TMP(Transform target, Vector2 position, int fsize, Vector2 anchormin, Vector2 anchormax, Vector2 anchorposition, TextAlignmentOptions anchor, Color tcolor, TMP_FontAsset font)
		{
			GameObject gameObject = new GameObject("Text");
			TextMeshProUGUI textMeshProUGUI = gameObject.AddComponent<TextMeshProUGUI>();
			gameObject.transform.SetParent(target);
			textMeshProUGUI.rectTransform.sizeDelta = Vector2.zero;
			textMeshProUGUI.rectTransform.anchorMin = anchormin;
			textMeshProUGUI.rectTransform.anchorMax = anchormax;
			textMeshProUGUI.rectTransform.anchoredPosition = anchorposition;
			textMeshProUGUI.text = " ";
			textMeshProUGUI.font = font;
			textMeshProUGUI.fontSize = (float)fsize;
			textMeshProUGUI.color = tcolor;
			textMeshProUGUI.alignment = anchor;
			gameObject.transform.localScale = new Vector3(1f, 1f);
			gameObject.transform.localPosition = position;
			gameObject.SetActive(true);
			return textMeshProUGUI;
		}

		// Token: 0x060095D0 RID: 38352 RVA: 0x002C31B4 File Offset: 0x002C13B4
		public static Text CreateText(Transform target)
		{
			return UtilTools.CreateText(target, new Vector2(0f, 0f), 10, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0f, 0f), TextAnchor.UpperLeft, Color.black, UtilTools.DefFont);
		}

		// Token: 0x060095D1 RID: 38353 RVA: 0x002C3218 File Offset: 0x002C1418
		public static Button CreateButton(Transform parent, string Imagepath, Vector2 scale, Vector2 position)
		{
			Image image = UtilTools.CreateImage(parent, Imagepath, scale, position);
			GameObject gameObject = image.gameObject;
			Button button = gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			return button;
		}

		// Token: 0x060095D2 RID: 38354 RVA: 0x002C324C File Offset: 0x002C144C
		public static Button CreateButton(Transform parent, string Imagepath)
		{
			return UtilTools.CreateButton(parent, Imagepath, new Vector2(1f, 1f), new Vector2(0f, 0f));
		}

		// Token: 0x04007393 RID: 29587
		public static Font DefFont;

		// Token: 0x04007394 RID: 29588
		public static TMP_FontAsset DefFont_TMP;

		// Token: 0x04007395 RID: 29589
		public static Color DefFontColor;
	}
}
