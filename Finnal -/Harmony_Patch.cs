using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
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
				MethodInfo method = typeof(Harmony_Patch).GetMethod("StageController_RoundStartPhase_UI_Post");
				harmony.Patch(typeof(StageController).GetMethod("RoundStartPhase_UI", AccessTools.all), null, new HarmonyMethod(method), null, null);
				method = typeof(Harmony_Patch).GetMethod("BattleObjectManager_InitUI_Pre");
				harmony.Patch(typeof(BattleObjectManager).GetMethod("InitUI", AccessTools.all), new HarmonyMethod(method), null, null, null);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/AgainHPerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		public static bool BattleObjectManager_InitUI_Pre()
		{
			return !(Singleton<StageController>.Instance.GetStageModel().ClassInfo.id == 600013 && Singleton<StageController>.Instance.RoundTurn >= 5);
		}

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
	}
}