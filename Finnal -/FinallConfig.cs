using System;
using System.IO;
using UnityEngine;
using GameSave;
using Mod;

namespace FinallyBeyondTheTime {
	[Serializable]
	public class FinnalConfig {
		public bool ChildImmobilizeNerf = false;
		public bool ScatterMode = false;
		public bool PlutoOff = false;
		public bool DiceSpeedUp = true;

		public static byte HarmonyMode = 0;
		public static FinnalConfig Instance = new FinnalConfig();
	}
	internal class OldConfig {
		internal void Load() {
			if (File.Exists(configFile)) {
				try {
					JsonUtility.FromJsonOverwrite(File.ReadAllText(configFile), FinnalConfig.Instance);
				} catch (Exception ex) {
					Debug.LogError("Error reading config file");
					Debug.LogError(ex.Message + Environment.NewLine + ex.StackTrace);
					Singleton<ModContentManager>.Instance.AddErrorLog("Finnal Battle: Finnal.ini invalid, resetting it");
				}
			}
            File.WriteAllText(configFile, JsonUtility.ToJson(FinnalConfig.Instance, true));
	    }
		internal void EchoAll() {
			Debug.Log("Finall: "+JsonUtility.ToJson(FinnalConfig.Instance, true));
			Debug.Log("Finall: HarmonyMode = "+FinnalConfig.HarmonyMode);
		}
        internal string configFile = SaveManager.GetFullPath("ModConfigs\\Finnal.ini");
	}
}