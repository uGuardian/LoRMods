using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using HarmonyLib;
using UI;
using UnityEngine;

namespace FinallyBeyondTheTime
{
	public class Harmony_Patch
	{
		public Harmony_Patch()
		{
			try
			{
				Harmony harmony = new Harmony("Again");
				MethodInfo stageController_RoundStartPhase_UI = typeof(Harmony_Patch).GetMethod("StageController_RoundStartPhase_UI_Post");
				harmony.Patch(typeof(StageController).GetMethod("RoundStartPhase_UI", AccessTools.all), null, new HarmonyMethod(stageController_RoundStartPhase_UI), null, null);
				// var uiCharacterRenderer_SetCharacter = typeof(Harmony_Patch).GetMethod("UICharacterRenderer_SetCharacter_Transpiler");
				// harmony.Patch(typeof(BaseMod.Harmony_Patch).GetMethod("UICharacterRenderer_SetCharacter", AccessTools.all), null, null, new HarmonyMethod(UICharacterRenderer_SetCharacter), null);
				// harmony.Patch(typeof(UI.UICharacterRenderer).GetMethod("SetCharacter", AccessTools.all), null, null, new HarmonyMethod(uiCharacterRenderer_SetCharacter), null);
				// method = typeof(Harmony_Patch).GetMethod("BattleObjectManager_InitUI_Pre");
				// harmony.Patch(typeof(BattleObjectManager).GetMethod("InitUI", AccessTools.all), new HarmonyMethod(method), null, null, null);
				MethodInfo emotionCoinExceptionSuppressor = typeof(Harmony_Patch).GetMethod("EmotionCoinExceptionSuppressor");
				harmony.Patch(typeof(BattleEmotionCoinUI).GetMethod("Init", AccessTools.all), null, null, null, new HarmonyMethod(emotionCoinExceptionSuppressor));
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Finnal -/AgainHPerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		// Unneeded with cleaning enabled
 		/* public static bool BattleObjectManager_InitUI_Pre()
		{
			return !(Singleton<StageController>.Instance.GetStageModel().ClassInfo.id == 600013 && Singleton<StageController>.Instance.RoundTurn >= 5);
		} */

		public static void StageController_RoundStartPhase_UI_Post()
		{
			if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id == 600013)
			{
				foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
				{
					battleUnitModel.view.WorldPosition = new Vector3Int(RandomUtil.Range(-48, 0), RandomUtil.Range(-8, 6), 0);
				}
			}
		}

		// Character index uncapper transpiler.
		// Currently causes CTD with no error when applied to original method
		// Currently causes white squares when applied to Harmony_Patch method
		/* public static IEnumerable<CodeInstruction> UICharacterRenderer_SetCharacter_Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/Finnal -/AgainHPTranspiler.txt", "Transpiler starting");
			var startIndex = -1;
    		var endIndex = -1;
			var codes = new List<CodeInstruction>(instructions);
			for (var i = 0; i < codes.Count; i++)
			{
				if (codes[i].opcode == OpCodes.Ldc_I4_S)
				{
					startIndex = (i-1);
					File.WriteAllText(Application.dataPath + "/BaseMods/Finnal -/AgainHPStartIndex.txt", startIndex.ToString());
				}
				if (codes[i].opcode == OpCodes.Ceq && i == (startIndex + 4))
				{
					endIndex = (i);
					File.WriteAllText(Application.dataPath + "/BaseMods/Finnal -/AgainHPEndIndex.txt", endIndex.ToString());
					break;
				}
			}
			if (startIndex > -1 && endIndex > -1)
			{
				codes[startIndex].opcode = OpCodes.Ldc_I4_0;
				File.WriteAllText(Application.dataPath + "/BaseMods/Finnal -/AgainHPComplete.txt", "Transpiler complete");
			} else {
				File.WriteAllText(Application.dataPath + "/BaseMods/Finnal -/AgainHPMissing.txt", "Transpiler position not found");
			}
			return codes.AsEnumerable();
		} */

		public static Exception EmotionCoinExceptionSuppressor()
		{
			return null;
		}
	}
}