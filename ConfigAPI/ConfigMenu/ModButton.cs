using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

public static partial class ConfigAPI {
	// Token: 0x0200205F RID: 8287
	public class ModButton : MonoBehaviour
	{
		// Token: 0x060095E4 RID: 38372 RVA: 0x002C43E8 File Offset: 0x002C25E8
		public void Awake()
		{
			if (this.modlist == null)
			{
				Image image = UtilTools.CreateImage(this.__instance.root.transform, resources+"\\Image\\ModListFrame.png", new Vector2(1.7f, 1.7f), new Vector2(0f, 110f));
				this.modlist = image.gameObject.AddComponent<ModList>();
				this.modlist.__instance = this.__instance;
				this.modlist.Init();
				this.modlist.gameObject.SetActive(false);
			}
		}

		// Token: 0x060095E5 RID: 38373 RVA: 0x002C4490 File Offset: 0x002C2690
		public void OnClick()
		{
			if (this.modlist.gameObject.activeSelf)
			{
				this.modlist.gameObject.SetActive(false);
			}
			else
			{
				this.modlist.gameObject.SetActive(true);
				this.modlist.InitModInfos();
			}
		}

		// Token: 0x040073A3 RID: 29603
		public static ModButton btninstance;

		// Token: 0x040073A4 RID: 29604
		public UIOptionWindow __instance;

		// Token: 0x040073A5 RID: 29605
		public ModList modlist;
	}
}