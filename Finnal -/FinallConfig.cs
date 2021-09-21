using System;
using System.IO;
using UnityEngine;
using GameSave;
using Mod;

namespace FinallyBeyondTheTime {
	[Serializable]
    public class Config {
		public bool ChildImmobilizeNerf = false;
		public bool ScatterMode = false;
		public bool PlutoOff = false;
		public bool DiceSpeedUp = true;
		
		public void Load() {
			if (File.Exists(configFile)) {
				try {
					JsonUtility.FromJsonOverwrite(File.ReadAllText(configFile), this);
				} catch (Exception ex) {
					Debug.LogError("Error reading config file");
					Debug.LogError(ex.Message + Environment.NewLine + ex.StackTrace);
					Singleton<ModContentManager>.Instance.AddErrorLog("Finnal Battle: Finnal.ini invalid, resetting it");
				}
			}
            File.WriteAllText(configFile, JsonUtility.ToJson(this, true));
	    }
		public void EchoAll() {
			Debug.Log("Finall: "+JsonUtility.ToJson(this, true));
			Debug.Log("Finall: HarmonyMode = "+HarmonyMode);
		}

        public static string configFile = SaveManager.GetFullPath("Finnal.ini");
		public static byte HarmonyMode = 0;
		static public Config instance = new Config();
    }
}