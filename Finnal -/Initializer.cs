using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using GameSave;
using Mod;

namespace FinallyBeyondTheTime {
    public class Initializer : ModInitializer {
        public override void OnInitializeMod() {
            List<String> assembly = new List<String>();
			foreach (var a in AppDomain.CurrentDomain.GetAssemblies()) {
				assembly.Add(a.GetName().Name);
			}
			if (assembly.Contains("0Harmony")) {
				Debug.Log("Finall: Harmony Found");
				FinallHarmony.Load();
			} else {
				Debug.Log("Finall: Harmony Unavailable");
            }

			// Old to new config migration
			string oldConfig = SaveManager.GetFullPath("Finnal.ini");
			if (File.Exists(oldConfig)) {
				Singleton<ModContentManager>.Instance.AddWarningLog("Finnal Battle: Now supports ConfigAPI for in-game settings. It can be found in the workshop.");
				Directory.CreateDirectory(SaveManager.GetFullPath("ModConfigs"));
				string newConfig = SaveManager.GetFullPath("ModConfigs\\Finnal.ini");
				try {File.Delete(newConfig);} catch {}
				File.Move(oldConfig, newConfig);
			}
			if (assembly.Contains("ConfigAPI")) {
				// Slightly easier on memory than handling it as a static
				var tempInstance = new InitConfig();
				tempInstance.Init();
			} else {
				// Slightly easier on memory than handling it as a static
				var tempInstance = new OldConfig();
				tempInstance.Load();
				tempInstance.EchoAll();
			}
        }
    }
	internal class InitConfig {
		internal void Init() {
			ConfigAPI.Init("Finnal", FinnalConfig.Instance, "HarmonyMode = "+FinnalConfig.HarmonyMode);
		}
	}
}