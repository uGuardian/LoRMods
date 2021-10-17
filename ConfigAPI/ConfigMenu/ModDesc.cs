using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static partial class ConfigAPI {
	// Token: 0x02002064 RID: 8292
	public class ModDesc : MonoBehaviour
	{
		// Token: 0x060095F7 RID: 38391 RVA: 0x002C4CC8 File Offset: 0x002C2EC8
		public void Init()
		{
			this.moddesc = UtilTools.CreateText_TMP(base.transform, new Vector2(0f, 0f), 25, new Vector2(0.02f, 0.02f), new Vector2(0.98f, 0.98f), new Vector2(0f, 0f), TextAlignmentOptions.TopLeft, UtilTools.DefFontColor, UtilTools.DefFont_TMP);
			// this.modinput = UtilTools.CreateInputField(base.transform, resources+"\\Image\\ModListFrame.png", new Vector2(0f, 0f), TextAnchor.UpperRight, 25, UtilTools.DefFontColor, UtilTools.DefFont);
		}

		// Token: 0x060095F8 RID: 38392 RVA: 0x002C4D34 File Offset: 0x002C2F34
		public void SetDesc(ModInfo info)
		{
			string temp = "";
			foreach (var entry in info.modinfo) {
				// temp += entry.Key+Environment.NewLine;
				temp += entry.Key+": "+entry.Value+Environment.NewLine;
			}
			this.moddesc.text = temp;
		}

		// Token: 0x040073B7 RID: 29623
		public TextMeshProUGUI moddesc;
		// public InputField modinput;
	}
}
