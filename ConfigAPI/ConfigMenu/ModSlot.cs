using System.Collections.Generic;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;

public static partial class ConfigAPI {
	// Token: 0x02002062 RID: 8290
	public class ModSlot : MonoBehaviour
	{
		// Token: 0x060095F1 RID: 38385 RVA: 0x002C4994 File Offset: 0x002C2B94
		public void Init()
		{
			this.modname = UtilTools.CreateText_TMP(base.transform, new Vector2(0f, 0f), 30, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0f, 0f), TextAlignmentOptions.MidlineJustified, UtilTools.DefFontColor, UtilTools.DefFont_TMP);
		}

		// Token: 0x060095F2 RID: 38386 RVA: 0x002C4A00 File Offset: 0x002C2C00
		public void SetModInfo(ModInfo info)
		{
			this.modname.text = info.modname;
		}

		// Token: 0x040073AF RID: 29615
		public ModList parent;

		// Token: 0x040073B0 RID: 29616
		public TextMeshProUGUI modname;
	}

	public class ModInfo {
		public ModInfo(KeyValuePair<string, object> config) {
			modname = config.Key;
			Type type = config.Value.GetType();
			foreach (var f in type.GetFields(BindingFlags.Public | BindingFlags.Instance)) {
				if (!f.IsStatic) {
					try {
						modinfo.Add(f.Name, f.GetValue(config.Value));
					} catch {
						Debug.Log("ConfigAPI: Failed to access variable "+f.Name);
					}
				}
			}
		}
		public string modname;
		public Dictionary<string, object> modinfo = new Dictionary<string, object>();
	}
}
