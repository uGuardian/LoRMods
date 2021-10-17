using System;
using System.Collections.Generic;
using System.IO;
using UI;
using UnityEngine;
using UnityEngine.UI;

public static partial class ConfigAPI {
	// Token: 0x02002060 RID: 8288
	public class ModList : MonoBehaviour
	{
		// Token: 0x060095E7 RID: 38375 RVA: 0x002C44F4 File Offset: 0x002C26F4
		public void InitModInfos()
		{
			this.ModInfos = new List<ModInfo>();
			foreach (var config in configs)
			{
				ModInfo modInfo = new ModInfo(config);
				this.ModInfos.Add(modInfo);
			}
		}

		// Token: 0x060095E8 RID: 38376 RVA: 0x002C4574 File Offset: 0x002C2774
		public void Init()
		{
			this.currentpage = 0;
			this.InitModInfos();
			this.slots = new List<ModSlot>();
			for (int i = 0; i < 5; i++)
			{
				Button button = UtilTools.CreateButton(base.transform, resources+"\\Image\\ModSlot.png");
				button.gameObject.transform.localPosition = new Vector3(-450f, (float)(160 - i * 70));
				button.gameObject.transform.localScale = new Vector3(0.5f, 0.5f);
				ModSlot modSlot = button.gameObject.AddComponent<ModSlot>();
				modSlot.parent = this;
				modSlot.Init();
				int j = i;
				button.onClick.AddListener(delegate()
				{
					this.OnClickSlot(j);
				});
				this.slots.Add(modSlot);
			}
			Image image = UtilTools.CreateImage(base.transform, resources+"\\Image\\ModDesc.png", new Vector2(0.5f, 0.5f), new Vector2(75f, 0f));
			this.desc = image.gameObject.AddComponent<ModDesc>();
			this.desc.Init();
			this.down = UtilTools.CreateButton(base.transform, resources+"\\Image\\Arrow.png");
			this.down.gameObject.transform.localScale = new Vector3(0.5f, 0.5f);
			this.down.gameObject.transform.localPosition = new Vector3(-450f, -170f);
			this.down.onClick.AddListener(delegate()
			{
				this.PageUpDown(false);
			});
			this.up = UtilTools.CreateButton(base.transform, resources+"\\Image\\Arrow.png");
			this.up.gameObject.transform.localScale = new Vector3(0.5f, -0.5f);
			this.up.gameObject.transform.localPosition = new Vector3(-450f, 205f);
			this.up.onClick.AddListener(delegate()
			{
				this.PageUpDown(true);
			});
			this.SetPage();
		}

		// Token: 0x060095E9 RID: 38377 RVA: 0x002C47D8 File Offset: 0x002C29D8
		public void SetPage()
		{
			for (int i = 0; i < 5; i++)
			{
				if (this.ModInfos.Count > i + this.currentpage * 5)
				{
					this.slots[i].gameObject.SetActive(true);
					this.slots[i].SetModInfo(this.ModInfos[i + this.currentpage * 5]);
				}
				else
				{
					this.slots[i].gameObject.SetActive(false);
				}
			}
			this.up.gameObject.SetActive(true);
			this.down.gameObject.SetActive(true);
			if (this.currentpage == 0)
			{
				this.up.gameObject.SetActive(false);
			}
			if (this.ModInfos.Count <= (this.currentpage + 1) * 5)
			{
				this.down.gameObject.SetActive(false);
			}
		}

		// Token: 0x060095EA RID: 38378 RVA: 0x002C48EC File Offset: 0x002C2AEC
		public void PageUpDown(bool isUp)
		{
			if (isUp)
			{
				this.currentpage--;
			}
			else
			{
				this.currentpage++;
			}
			this.SetPage();
		}

		// Token: 0x060095EB RID: 38379 RVA: 0x002C4928 File Offset: 0x002C2B28
		public void OnClickSlot(int i)
		{
			this.desc.SetDesc(this.ModInfos[i + this.currentpage * 5]);
		}

		// Token: 0x040073A6 RID: 29606
		public List<ModInfo> ModInfos;

		// Token: 0x040073A7 RID: 29607
		public UIOptionWindow __instance;

		// Token: 0x040073A8 RID: 29608
		public List<ModSlot> slots;

		// Token: 0x040073A9 RID: 29609
		public ModDesc desc;

		// Token: 0x040073AA RID: 29610
		public Button up;

		// Token: 0x040073AB RID: 29611
		public Button down;

		// Token: 0x040073AC RID: 29612
		public int currentpage;
	}
}
