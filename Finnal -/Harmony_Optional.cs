using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace FinallyBeyondTheTime {
	public class FinallHarmony {
		public static void Load() {
			Harmony harmony = new Harmony("LoR.uGuardian.Finnal");
			Config.HarmonyMode = 1;
			CheckSummonLiberation();
			harmony.PatchAll();
		}
		public static void CheckSummonLiberation() {
			List<String> assembly = new List<String>();
			foreach (var a in AppDomain.CurrentDomain.GetAssemblies()) {
				assembly.Add(a.GetName().Name);
			}
			if (assembly.Contains("BaseMod")) {
				Debug.Log("Finall: BaseMod Found");
				Config.HarmonyMode = 2;
			}
		}
	}

	[HarmonyPatch(typeof(BattleEmotionCoinSlotUI))]
	class EmotionCoinExceptionSuppressor {
		[HarmonyFinalizer]
		[HarmonyPatch("Init")]
        public static Exception Init_Finalizer(Exception __exception) {
			if (Singleton<StageController>.Instance.EnemyStageManager is EnemyTeamStageManager_UltimaAgain) {
    			return null;
			} else {
				return __exception;
			}
    	}
		[HarmonyFinalizer]
		[HarmonyPatch("StartMoving")]
        public static Exception StartMoving_Finalizer(Exception __exception) {
			if (Singleton<StageController>.Instance.EnemyStageManager is EnemyTeamStageManager_UltimaAgain) {
    			return null;
			} else {
				return __exception;
			}
    	}
		[HarmonyFinalizer]
		[HarmonyPatch(typeof(BattleEmotionCoinUI), "Init")]
        public static Exception Init_Finalizer2(Exception __exception) {
			if (Singleton<StageController>.Instance.EnemyStageManager is EnemyTeamStageManager_UltimaAgain) {
    			return null;
			} else {
				return __exception;
			}
    	}
    }
	[HarmonyPatch(typeof(RencounterManager), "StartRencounter")]
    class EnableNoDelay {
        public static void Postfix(RencounterManager __instance) {
    		if (Config.instance.DiceSpeedUp == true && Singleton<StageController>.Instance.EnemyStageManager is EnemyTeamStageManager_UltimaAgain) {
    			__instance.SetNodelay(true);
    		}
    	}
    }
}