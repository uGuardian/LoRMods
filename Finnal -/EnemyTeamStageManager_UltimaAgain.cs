using System;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using UI;
using UnityEngine;

namespace FinallyBeyondTheTime
{
	public class EnemyTeamStageManager_UltimaAgain : EnemyTeamStageManager
	{
		public static bool FinallConfig(string settingKey)
		{
			string configFile = (Application.dataPath + "/BaseMods/Finnal -/Finnal.ini");
			string[] config = new string[4];
			if (!File.Exists(configFile))
			{
				// If the file somehow does not exist, which should never be the case due to generation during the Harmony Patch, assume false for all.
				Debug.LogError("Finall: Config file does not exist!" + Environment.NewLine + "Assuming " + settingKey + " = False");
				return false;
			} else {
				config = File.ReadAllLines(configFile);
			}
			try {
				bool settingResult = System.Convert.ToBoolean(config[Array.IndexOf(config, settingKey)+1]);
				Debug.LogError("Finall: " + settingKey + " = " + settingResult);
				return settingResult;
			} catch {
				Debug.LogError(ex.Message + Environment.NewLine + ex.StackTrace);
				Debug.LogError("Finall: Error occured in config check for variable " + settingKey + "!" + Environment.NewLine + "Assuming " + settingKey + " = False");
				return false;
			}
		}
		public override void OnWaveStart()
		{
			this.phase = 0;
			this.currentFloor = Singleton<StageController>.Instance.GetCurrentStageFloorModel().Sephirah;
			Debug.LogError("Finall: Initial floor is " + this.currentFloor);
			FinallConfig("diceSpeedUp"); // Outputs the current setting into the log, does nothing else
			this._finished = false;
			this._angelaappears = false;
			this.remains.Clear();
			foreach (LibraryFloorModel libraryFloorModel in LibraryModel.Instance.GetOpenedFloorList())
			{
				if (libraryFloorModel.GetUnitDataList().Count > 0)
				{
					StageLibraryFloorModel stageLibraryFloorModel = new StageLibraryFloorModel();
					stageLibraryFloorModel.Init(Singleton<StageController>.Instance.GetStageModel(), libraryFloorModel, false);
					if (stageLibraryFloorModel.Sephirah != this.currentFloor || this.currentFloor == SephirahType.Keter)
					{
						this.remains.Add(stageLibraryFloorModel);
					} else {
						Debug.LogError("Finall: Floor list skipping over " + this.currentFloor);
					}
				}
			}
			if (this.currentFloor != SephirahType.Keter)
			{
				Debug.LogError("Finall: Inserting Keter at top of floor list");
				this.remains.Insert(0, this.remains[this.remains.Count - 1]);
			}
		}

		public override void OnRoundEndTheLast()
		{
			this.CheckPhase();
			this.CheckFloor();
			// Phases from T3 SotC onwards have more complicated mechanics and less characters, so we stop cleaning every round just to be sure
			if (this.phase < 7)
			{
				this.CleanUp();
			}
		}

		public override bool IsStageFinishable()
		{
			return this._finished;
		}

		public override void OnRoundStart()
		{
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
			{
				PassiveAbilityBase passiveAbilityBase = battleUnitModel.passiveDetail.PassiveList.Find((PassiveAbilityBase x) => x is PassiveAbility_240528);
				if (passiveAbilityBase != null && !passiveAbilityBase.destroyed)
				{
					List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(Faction.Player);
					if (aliveList.Count > 1 || !FinallConfig("childImmobilizeNerf") && aliveList.Count > 0)
					{
						RandomUtil.SelectOne<BattleUnitModel>(aliveList).bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Stun, 1, null);
						break;
					}
					break;
				}
			}
		}

		public override void OnRoundStart_After()
		{
			if (this.phase == 12)
			{
				List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(Faction.Player);
				using (List<BattleUnitModel>.Enumerator enumerator = BattleObjectManager.instance.GetAliveList(Faction.Enemy).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BattleUnitModel battleUnitModel2 = enumerator.Current;
						if (!aliveList.Exists((BattleUnitModel x) => x.IsTargetable(battleUnitModel2)))
						{
							foreach (BattleUnitModel battleUnitModel in aliveList)
							{
								battleUnitModel.bufListDetail.AddBuf(new PassiveAbility_1306012.BattleUnitBuf_nullfyNotTargetable());
								battleUnitModel.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 2, null);
								battleUnitModel.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.BreakProtection, 2, null);
							}
						}
					}
				}
			}
		}

		public override bool HideEnemyTarget()
		{
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
			{
				PassiveAbilityBase passiveAbilityBase = battleUnitModel.passiveDetail.PassiveList.Find((PassiveAbilityBase x) => x is PassiveAbility_240428);
				if (passiveAbilityBase != null && !passiveAbilityBase.destroyed)
				{
					return true;
				}
			}
			return false;
		}

		public override bool BlockEnemyAggroChange()
		{
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
			{
				PassiveAbilityBase passiveAbilityBase = battleUnitModel.passiveDetail.PassiveList.Find((PassiveAbilityBase x) => x is PassiveAbility_240428);
				if (passiveAbilityBase != null && !passiveAbilityBase.destroyed)
				{
					return true;
				}
			}
			return false;
		}

		public override bool IsHideDiceAbilityInfo()
		{
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
			{
				PassiveAbilityBase passiveAbilityBase = battleUnitModel.passiveDetail.PassiveList.Find((PassiveAbilityBase x) => x is PassiveAbility_240328);
				if (passiveAbilityBase != null && !passiveAbilityBase.destroyed)
				{
					return true;
				}
			}
			return false;
		}

		private List<int> GetPhaseGuest(int phase)
		{
			List<int> result;
			switch (phase)
			{
			case 1:
				result = new List<int>
				{
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8,
					9,
					10,
					11,
					12,
					1001,
					1002,
					1003,
					1004
				};
				break;
			case 2:
				result = new List<int>
				{
					10001,
					10002,
					10003,
					10004,
					10005,
					10006,
					11001,
					11002,
					11003,
					11004,
					11005,
					11006
				};
				break;
			case 3:
				result = new List<int>
				{
					20001,
					20002,
					20003,
					20004,
					20005,
					20006,
					20007,
					20008,
					20009,
					20010,
					21001,
					21002,
					21003,
					21004,
					21005,
					21006,
					21007,
					21008,
					21009,
					21010,
					21011,
					21012,
					21013
				};
				break;
			case 4:
				result = new List<int>
				{
					30001,
					30002,
					30003,
					30004,
					30005,
					30006,
					30007,
					30008,
					30009,
					30010,
					30011,
					30012,
					30013,
					30014,
					30015,
					30016,
					30018,
					30019,
					30020,
					30021,
					30022,
					30023,
					30024,
					30025,
					30026,
					30027,
					30028,
					31001,
					31002,
					31003,
					31004
				};
				break;
			case 5:
				result = new List<int>
				{
					40001,
					40002,
					40003,
					40004,
					40005,
					40006,
					40007,
					40008,
					40011,
					40012,
					40013,
					40015,
					40016,
					40017,
					40018,
					40019,
					40020,
					40021,
					40022,
					40023,
					40024,
					40025,
					40026,
					42001,
					42002,
					42003,
					42004,
					42005,
					42006,
					42007,
					42008,
					41001,
					41002,
					43001,
					43002,
					43003,
					43004
				};
				break;
			case 6:
				result = new List<int>
				{
					50001,
					50002,
					50003,
					50004,
					50005,
					50006,
					50007,
					50008,
					50009,
					50105,
					50010,
					50011,
					50012,
					50013,
					50015,
					50016,
					50017,
					51001,
					51002,
					50018,
					50019,
					50020,
					50021,
					50023,
					50024,
					50025,
					50026,
					50101,
					50102,
					50103,
					43005
				};
				break;
			case 7:
				result = new List<int>
				{
					50022,
					50031,
					50032,
					50033,
					50034,
					50101,
					50102,
					50103,
					50035,
					50036,
					50037,
					50038,
					50051,
					50039,
					50040
				};
				break;
			case 8:
				result = new List<int>
				{
					60001,
					60101,
					60002,
					60003,
					60004
				};
				break;
			case 9:
				result = new List<int>
				{
					1301011,
					1301021,
					1302011,
					1302021,
					1303011,
					1303021,
					1304011,
					1304021,
					1304031,
					1305011,
					1305021,
					1305031,
					1307011,
					1307021,
					1307031,
					1307041,
					1307051,
					1308011,
					1308021,
					1306011,
					1310011,
					1309011,
					1309021
				};
				break;
			case 10:
				result = new List<int>
				{
					60005,
					60006,
					60007,
					60107
				};
				break;
			case 11:
				result = new List<int>
				{
					1408011,
					1410011,
					1409011,
					1409021,
					1405011,
					1405021,
					1405031,
					1405041,
					1407011,
					1406011,
					1403011,
					1404011,
					1401011,
					1402011
				};
				break;
			case 12:
				result = new List<int>
				{
					80001,
					80002
				};
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		private void CheckPhase()
		{
			if (BattleObjectManager.instance.GetAliveList(Faction.Enemy).Count <= 0)
			{
				this.phase++;
				Debug.LogError("Finall: Starting Phase Transition, new phase is " + this.phase);
				if (this.phase <= 12)
				{
					foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(Faction.Player))
					{
						battleUnitModel.RecoverHP(20);
						battleUnitModel.breakDetail.RecoverBreak(20);
						battleUnitModel.RecoverBreakLife(1, false);
						battleUnitModel.cardSlotDetail.RecoverPlayPoint(4);
						battleUnitModel.allyCardDetail.DrawCards(2);
					}
					BattleTeamModel battleTeamModel = (BattleTeamModel)typeof(StageController).GetField("_enemyTeam", AccessTools.all).GetValue(Singleton<StageController>.Instance);
					foreach (int num in this.GetPhaseGuest(this.phase))
					{
						EnemyUnitClassInfo data = Singleton<EnemyUnitClassInfoList>.Instance.GetData(num);
						UnitBattleDataModel unitBattleDataModel = UnitBattleDataModel.CreateUnitBattleDataByEnemyUnitId(Singleton<StageController>.Instance.GetStageModel(), data.id);
						UnitDataModel unitData = unitBattleDataModel.unitData;
						BattleUnitModel battleUnitModel2 = BattleObjectManager.CreateDefaultUnit(Faction.Enemy);
						battleUnitModel2.index = 0;
						battleUnitModel2.formation = Singleton<StageController>.Instance.GetStageModel().GetWave(1).GetFormationPosition(battleUnitModel2.index);
						if (!unitBattleDataModel.isDead)
						{
							battleUnitModel2.grade = unitData.grade;
							battleUnitModel2.SetUnitData(unitBattleDataModel);
							battleUnitModel2.OnDispose();
							battleTeamModel.AddUnit(battleUnitModel2);
							BattleObjectManager.instance.RegisterUnit(battleUnitModel2);
							battleUnitModel2.passiveDetail.OnUnitCreated();
							battleUnitModel2.passiveDetail.OnWaveStart();
							battleUnitModel2.emotionDetail.SetEmotionLevel(Mathf.Min(this.phase + 1, 5));
							battleUnitModel2.cardSlotDetail.RecoverPlayPoint(5);
							battleUnitModel2.allyCardDetail.DrawCards(4);
						}
						if (num == 50035)
						{
							this.pt = battleUnitModel2;
						}
					}
					// We're between phases, so clean up if it won't be done on Round End
					if (this.phase >= 7)
					{
						this.CleanUp();
					}
				}
				else
				{
					if (this.phase == 13)
					{
						BattleObjectManager.instance.RegisterUnit(this.pt);
						this.pt.view.EnableView(false);
						this.pt.bufListDetail.AddBuf(new Returned());
					}
					else
					{
						if (this.phase > 13)
						{
							this._finished = true;
						}
					}
				}
			}
		}

		private void CheckFloor()
		{
			if (BattleObjectManager.instance.GetAliveList(Faction.Player).Count <= 0)
			{
				Debug.LogError("Finall: Starting Floor Transition, changing from " + this.currentFloor + " to " + this.remains[0].Sephirah);
				Debug.LogError("Finall: Cleaning current floor...");
				foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList(Faction.Player))
				{
					BattleObjectManager.instance.UnregisterUnit(battleUnitModel);
					// Debug.LogError("Finall: Unregistered Librarian " + battleUnitModel.id);
				}
				Debug.LogError("Finall: Setting up floor...");
				BattleTeamModel battleTeamModel = (BattleTeamModel)typeof(StageController).GetField("_librarianTeam", AccessTools.all).GetValue(Singleton<StageController>.Instance);
				/* Debug.LogError("Finall: battleTeamModel includes:");
				foreach (BattleUnitModel battleUnitModel in battleTeamModel.GetList())
				{
					Debug.LogError("Finall: battleUnitModel.id: " + battleUnitModel.id);
				} */
				if (this.remains.Count > 1)
				{
					Singleton<StageController>.Instance.SetCurrentSephirah(this.remains[0].Sephirah);
					StageLibraryFloorModel currentStageFloorModel = Singleton<StageController>.Instance.GetCurrentStageFloorModel();
					// Debug.LogError("Finall: currentStageFloorModel.GetUnitBattleDataList includes:");
					for (int i = 0; i < currentStageFloorModel.GetUnitBattleDataList().Count; i++)
					{
						// Debug.LogError("Finall: Count Index: " + i);
						BattleUnitModel battleUnitModel = Singleton<StageController>.Instance.CreateLibrarianUnit_fromBattleUnitData(i);
						// Debug.LogError("Finall: CreateLibrarianUnit: " + battleUnitModel.id);
						battleUnitModel.OnWaveStart();
						battleUnitModel.emotionDetail.SetEmotionLevel(Mathf.Min(this.phase + 1, 5));
						battleUnitModel.cardSlotDetail.RecoverPlayPoint(5);
						battleUnitModel.allyCardDetail.DrawCards(6);
						SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel.UnitData.unitData, i, true);
					}
					// MapChange needs to be called before remains is updated
					this.MapChange();
					this.remains.Remove(this.remains[0]);
				}
				else
				{
					if (!this._angelaappears)
					{
						this._angelaappears = true;
						Singleton<StageController>.Instance.SetCurrentSephirah(SephirahType.Keter);
						StageLibraryFloorModel currentStageFloorModel2 = Singleton<StageController>.Instance.GetCurrentStageFloorModel();
						UnitDataModel unitDataModel = new UnitDataModel(9100501, SephirahType.Keter, true);
						BattleUnitModel battleUnitModel2 = BattleObjectManager.CreateDefaultUnit(Faction.Player);
						battleUnitModel2.index = 0;
						battleUnitModel2.grade = unitDataModel.grade;
						battleUnitModel2.formation = currentStageFloorModel2.GetFormationPosition(battleUnitModel2.index);
						UnitBattleDataModel unitBattleDataModel = new UnitBattleDataModel(Singleton<StageController>.Instance.GetStageModel(), unitDataModel);
						unitBattleDataModel.Init();
						battleUnitModel2.SetUnitData(unitBattleDataModel);
						battleUnitModel2.OnDispose();
						battleTeamModel.AddUnit(battleUnitModel2);
						BattleObjectManager.instance.RegisterUnit(battleUnitModel2);
						battleUnitModel2.passiveDetail.OnUnitCreated();
						battleUnitModel2.passiveDetail.OnWaveStart();
						battleUnitModel2.emotionDetail.SetEmotionLevel(5);
						battleUnitModel2.cardSlotDetail.RecoverPlayPoint(5);
						battleUnitModel2.allyCardDetail.DrawCards(4);
						SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel2.UnitData.unitData, 0, true);
						this.MapChange();
					}
				}
				// Refresh UI after floor setup is complete
				BattleObjectManager.instance.InitUI();
				Debug.LogError("Finall: Floor Setup Complete");
			}
			bool angelaappears = this._angelaappears;
			if (angelaappears)
			{
				BattleUnitModel battleUnitModel3 = BattleObjectManager.instance.GetAliveList(Faction.Player).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId() == 9100501);
				foreach (BattleDiceCardModel battleDiceCardModel in battleUnitModel3.personalEgoDetail.GetHand())
				{
					battleUnitModel3.personalEgoDetail.RemoveCard(battleDiceCardModel.GetID());
				}
				if (this.phase >= 9)
				{
					battleUnitModel3.personalEgoDetail.AddCard(9910020);
					battleUnitModel3.personalEgoDetail.AddCard(9910011);
					battleUnitModel3.personalEgoDetail.AddCard(9910012);
					battleUnitModel3.personalEgoDetail.AddCard(9910013);
					battleUnitModel3.personalEgoDetail.AddCard(9910014);
				}
				if (this.phase >= 10)
				{
					battleUnitModel3.personalEgoDetail.AddCard(9910015);
					battleUnitModel3.personalEgoDetail.AddCard(9910016);
					battleUnitModel3.personalEgoDetail.AddCard(9910017);
				}
				if (this.phase >= 11)
				{
					battleUnitModel3.personalEgoDetail.AddCard(9910018);
					battleUnitModel3.personalEgoDetail.AddCard(9910019);
				}
				if (this.phase >= 12)
				{
					if (!battleUnitModel3.bufListDetail.GetActivatedBufList().Exists((BattleUnitBuf x) => x is BattleUnitBuf_KeterFinal_Cogito))
					{
						battleUnitModel3.bufListDetail.AddBuf(new BattleUnitBuf_KeterFinal_Cogito());
					}
				}
				if (this.phase >= 13)
				{
					battleUnitModel3.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_KeterFinal_Cogito).Destroy();
				}
			}
		}
		private void CleanUp()
		{
			Debug.LogError("Finall: Cleaning dead enemies...");
			int i = 5;
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList(Faction.Enemy))
			{
				if (battleUnitModel.IsDead())
				{
					BattleObjectManager.instance.UnregisterUnit(battleUnitModel);
					// Debug.LogError("Finall: Unregistered Enemy: " + battleUnitModel.id);
				} else {
					// We use this opportunity to reregister living units to lower registers for partial UI functionality, because we might as well
					// Debug.LogError("Finall: Count Index: " + i);
					SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel.UnitData.unitData, i++, true);
					// Debug.LogError("Finall: SetCharacter Enemy: " + battleUnitModel.id);
					// i = ((i+1)%6)+5; // Looped register function, slightly changes display bugs
				}
			}
			// We refresh the UI after the registrations are all done
			BattleObjectManager.instance.InitUI();
			Debug.LogError("Finall: Cleaning Finished");
		}
		private void MapChange()
		{
			//If the floor is angela, make it Keter, otherwise base on this.remains
			if (!this._angelaappears)
			{
				this.currentFloor = this.remains[0].Sephirah;
			} else {
				this.currentFloor = SephirahType.Keter;
			}
			Debug.LogError("Finall: Changing map, new map is " + this.currentFloor);
			// ChangeToSephirahMap called for transition effect
			//SingletonBehavior<BattleSceneRoot>.Instance.ChangeToSephirahMap(this.currentFloor, true);
			// EndBattle only terminates the map structure, not the battle itself. Used to clear the map effectively.
			SingletonBehavior<BattleSceneRoot>.Instance.EndBattle(true);
			// Immediately CheckTheme to make the music not temporarily stall.
			SingletonBehavior<BattleSoundManager>.Instance.CheckTheme();
			// Emulate map related init functions.
			Singleton<StageController>.Instance.InitializeMap();
			Singleton<StageController>.Instance.GetStageModel().GetFloor(this.currentFloor).SetEmotionTeamUnit();
			SingletonBehavior<HexagonalMapManager>.Instance.OnRoundStart();
			SingletonBehavior<HexagonalMapManager>.Instance.ResetMapSetting();
			SingletonBehavior<BattleCamManager>.Instance.ResetCamSetting();
		}

		private BattleUnitModel pt = new BattleUnitModel(50035);

		private int phase;
		private SephirahType currentFloor;

		private bool _finished;

		private bool _angelaappears;

		private List<StageLibraryFloorModel> remains = new List<StageLibraryFloorModel>();
	}
}
