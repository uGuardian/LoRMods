using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Battle.DiceAttackEffect;
using GameSave;
using HarmonyLib;
using LOR_DiceSystem;
using LOR_XML;
using Spine;
using Spine.Unity;
using StoryScene;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using WorkParser;
using Workshop;
using Mod;

public static partial class ConfigAPI {
	public class Initializer : ModInitializer {
        public override void OnInitializeMod() {
			Harmony harmony = new Harmony("LoR.uGuardian.ConfigAPI");
			HarmonyLib.Tools.HarmonyFileLog.Enabled = true;
			harmony.PatchAll();
		}
	}
	private readonly static string resources = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+"\\..\\Resource";

	[HarmonyPatch(typeof(UIOptionWindow), "Open")]
	class patch {
		[HarmonyPostfix]
		public static void UIOptionWindow_Open(UIOptionWindow __instance)
		{
			try
			{
				if (UtilTools.DefFont == null)
				{
					UtilTools.DefFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
					UtilTools.DefFontColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
				}
				if (UtilTools.DefFont_TMP == null)
				{
					UtilTools.DefFont_TMP = __instance.displayDropdown.itemText.font;
				}
				if (ModButton.btninstance == null)
				{
					Button button = UtilTools.CreateButton(__instance.root.transform, resources+"\\Image\\ModButton.png");
					button.gameObject.transform.localPosition = new Vector3(0f, -460f);
					button.gameObject.SetActive(false);
					ModButton.btninstance = button.gameObject.AddComponent<ModButton>();
					button.onClick.AddListener(delegate()
					{
						ModButton.btninstance.OnClick();
					});
					ModButton.btninstance.__instance = __instance;
					button.gameObject.SetActive(true);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
	}
}