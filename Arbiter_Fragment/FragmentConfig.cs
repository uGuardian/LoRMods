using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using GameSave;
using Mod;

// Your namespace here
namespace ArbiterFragment {
	public class ConfigInitializer : ModInitializer {
		public override void OnInitializeMod() {
			List<String> assembly = new List<String>();
			foreach (var a in AppDomain.CurrentDomain.GetAssemblies()) {
				assembly.Add(a.GetName().Name);
			}

			// Get rid of old config
			string oldConfig = SaveManager.GetFullPath("Arbiter_Fragment.ini");
			if (File.Exists(oldConfig)) {
				Singleton<ModContentManager>.Instance.AddWarningLog("ArbiterFragment: Config has been reset."+Environment.NewLine+"Now supports ConfigAPI for in-game settings.");
				try {File.Delete(oldConfig);} catch {Singleton<ModContentManager>.Instance.AddErrorLog("ArbiterFragment: Failed to delete old config");}
			}
			// Init config
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
			ConfigAPI.Init("ArbiterFragment", FragmentConfig.Instance);
		}
	}

	[Serializable]
	public class FragmentConfig {
		public int FragmentEmoLevel = 3;
        public int FragmentBonusHP = 15;
        public int FragmentBonusStagger = 15;
        public bool FragmentActivationHP = true;
        public bool FragmentActivationStagger = true;

		public static FragmentConfig Instance = new FragmentConfig();
	}

	internal class OldConfig {
		internal void Load() {
			if (File.Exists(configFile)) {
				try {
					JsonUtility.FromJsonOverwrite(File.ReadAllText(configFile), FragmentConfig.Instance);
				} catch (Exception ex) {
					Debug.LogError("Error reading config file");
					Debug.LogError(ex.Message + Environment.NewLine + ex.StackTrace);
					Singleton<ModContentManager>.Instance.AddErrorLog("ArbiterFragment: ArbiterFragment.ini invalid, resetting it");
				}
			}
            File.WriteAllText(configFile, JsonUtility.ToJson(FragmentConfig.Instance, true));
	    }
		internal void EchoAll() {
			Debug.Log("ArbiterFragment: "+JsonUtility.ToJson(FragmentConfig.Instance, true));
		}
        internal string configFile = SaveManager.GetFullPath("ModConfigs\\ArbiterFragment.ini");
	}
}