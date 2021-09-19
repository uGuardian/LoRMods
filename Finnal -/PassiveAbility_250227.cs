using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinallyBeyondTheTime
{
	public class PassiveAbility_250227_Finnal : PassiveAbilityBase
	{
		public override int SpeedDiceNumAdder()
		{
			int result;
			if (this._patternCount <= 3)
			{
				result = 2;
			}
			else
			{
				result = 3;
			}
			return result;
		}

		public override void OnWaveStart()
		{
			this._patternCount = this.owner.UnitData.floorBattleData.param1;
			this._teleported = this.owner.UnitData.floorBattleData.param2;
			if (this._teleported > 0)
			{
				this._areaCoolTime = 1;
			}
			else
			{
				this._areaCoolTime = 0;
			}
			this._stancePassive = (this.owner.passiveDetail.PassiveList.Find((PassiveAbilityBase x) => x is PassiveAbility_250127) as PassiveAbility_250127);
		}

		public override void OnRoundEnd()
		{
			this.owner.cardSlotDetail.RecoverPlayPoint(this.owner.cardSlotDetail.GetMaxPlayPoint());
		}

		public override void OnRoundStart()
		{
			if (!(this.owner.UnitData.floorBattleData.param2 > 0))
			{
				if (this._teleportReady || this.owner.hp <= (float)this._teleportCondition)
				{
					this.owner.breakDetail.RecoverBreakLife(this.owner.MaxBreakLife, false);
					this.owner.breakDetail.nextTurnBreak = false;
					this.owner.breakDetail.RecoverBreak(this.owner.breakDetail.GetDefaultBreakGauge());
					this.owner.UnitData.floorBattleData.param2 = 1;
					// if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != 600013)
					if (false)
					{
						List<StageLibraryFloorModel> availableFloorList = Singleton<StageController>.Instance.GetStageModel().GetAvailableFloorList();
						availableFloorList.RemoveAll((StageLibraryFloorModel x) => x.Sephirah == SephirahType.Chesed);
						availableFloorList.RemoveAll((StageLibraryFloorModel x) => x.Sephirah == SephirahType.Hokma);
						availableFloorList.RemoveAll((StageLibraryFloorModel x) => x.Sephirah == Singleton<StageController>.Instance.CurrentFloor);
						if (availableFloorList.Count > 0)
						{
							SephirahType sephirah = RandomUtil.SelectOne<StageLibraryFloorModel>(availableFloorList).Sephirah;
							Singleton<StageController>.Instance.ChangeFloorForcely(sephirah, this.owner);
						}
					}
				}
			}
		}

		public override void OnFixedUpdateInWaitPhase(float delta)
		{
			base.OnFixedUpdateInWaitPhase(delta);
			if (this.owner.bufListDetail.HasBuf<Returned>())
			{
				if (this._elapsedTimeTeleport < Mathf.Epsilon)
				{
					UnityEngine.Object @object = Resources.Load("Prefabs/Battle/BufEffect/Purple_Teleport");
					if (@object != null)
					{
						Teleport_Fast component = (UnityEngine.Object.Instantiate(@object, this.owner.view.atkEffectRoot) as GameObject).GetComponent<Teleport_Fast>();
						if (component != null)
						{
							component.PlayEffect();
						}
					}
				}
				this._elapsedTimeTeleport += delta;
				if (this._elapsedTimeTeleport > 0.8f)
				{
					this.owner.view.EnableView(true);
				}
			}
			else
			{
				// if (!(Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != 600013 || (!this._teleportReady && this.owner.hp > (float)this._teleportCondition)))
				if (!(false || (!this._teleportReady && this.owner.hp > (float)this._teleportCondition)))
				{
					if (this._elapsedTimeTeleport < Mathf.Epsilon)
					{
						UnityEngine.Object object2 = Resources.Load("Prefabs/Battle/BufEffect/Purple_Teleport");
						if (object2 != null)
						{
							Teleport_Fast component2 = (UnityEngine.Object.Instantiate(object2, this.owner.view.atkEffectRoot) as GameObject).GetComponent<Teleport_Fast>();
							if (component2 != null)
							{
								component2.PlayEffect();
							}
						}
					}
					this._elapsedTimeTeleport += delta;
					if (this._elapsedTimeTeleport > 0.8f)
					{
						this.owner.view.characterTranslationCenter.gameObject.SetActive(false);
						BattleObjectManager.instance.UnregisterUnit(this.owner);
					}
				}
			}
		}

		public override void OnRoundStartAfter()
		{
			this._stanceCooltime--;
			if (this._stanceCooltime <= 0)
			{
				this.UpdateStance();
			}
			this.SetCards();
			this._patternCount++;
			this.owner.UnitData.floorBattleData.param1 = this._patternCount;
		}

		private void SetCards()
		{
			this.owner.allyCardDetail.ExhaustAllCards();
			if (this.owner.UnitData.floorBattleData.param2 > 0)
			{
				if (this._areaCoolTime <= 0)
				{
					this.AddNewCard(609013);
					this._areaCoolTime = 2;
				}
				else
				{
					this._areaCoolTime--;
				}
			}
			switch (this._stancePassive.CurrentStance)
			{
			case PurpleStance.Slash:
				this.SetCards_slash();
				break;
			case PurpleStance.Penetrate:
				this.SetCards_penetrate();
				break;
			case PurpleStance.Hit:
				this.SetCards_hit();
				break;
			case PurpleStance.Defense:
				this.SetCards_defense();
				break;
			}
			this._patternCount++;
		}

		private void SetCards_slash()
		{
			this.AddNewCard(609001);
			this.AddNewCard(609002);
			this.AddNewCard(609003);
			this.AddNewCard(609001);
			this.AddNewCard(609002);
			this.AddNewCard(609003);
		}

		private void SetCards_penetrate()
		{
			this.AddNewCard(609004);
			this.AddNewCard(609005);
			this.AddNewCard(609006);
			this.AddNewCard(609004);
			this.AddNewCard(609005);
			this.AddNewCard(609006);
		}

		private void SetCards_hit()
		{
			this.AddNewCard(609007);
			this.AddNewCard(609008);
			this.AddNewCard(609009);
			this.AddNewCard(609007);
			this.AddNewCard(609008);
			this.AddNewCard(609009);
		}

		private void SetCards_defense()
		{
			this.AddNewCard(609010);
			this.AddNewCard(609011);
			this.AddNewCard(609012);
			this.AddNewCard(609010);
			this.AddNewCard(609011);
		}

		private void UpdateStance()
		{
			if (this._alreayUsed.Count >= 4)
			{
				this._alreayUsed.Clear();
			}
			int param = 0;
			try {
				param = this.owner.UnitData.floorBattleData.param2;
			} catch { Debug.LogError("Finall: PurpleTear failed to find param2"); }
			if (param > 0 && !this._alreayUsed.Contains(PurpleStance.Defense))
			{
				this._alreayUsed.Add(PurpleStance.Defense);
			}
			List<PurpleStance> list = new List<PurpleStance>
			{
				PurpleStance.Slash,
				PurpleStance.Penetrate,
				PurpleStance.Hit,
				PurpleStance.Defense
			};
			foreach (PurpleStance item in this._alreayUsed)
			{
				list.Remove(item);
			}
			switch (RandomUtil.SelectOne<PurpleStance>(list))
			{
			case PurpleStance.Slash:
			{
				this._stancePassive.ChangeStance_slash();
				this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.SlashPowerUp, 1, null);
				if (param > 0)
				{
					this._stanceCooltime = 1;
				}
				else
				{
					this._stanceCooltime = 2;
				}
				this._alreayUsed.Add(PurpleStance.Slash);
				break;
			}
			case PurpleStance.Penetrate:
			{
				this._stancePassive.ChangeStance_penetrate();
				this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.PenetratePowerUp, 1, null);
				if (param > 0)
				{
					this._stanceCooltime = 1;
				}
				else
				{
					this._stanceCooltime = 2;
				}
				this._alreayUsed.Add(PurpleStance.Penetrate);
				break;
			}
			case PurpleStance.Hit:
			{
				this._stancePassive.ChangeStance_hit();
				this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.HitPowerUp, 1, null);
				if (param > 0)
				{
					this._stanceCooltime = 1;
				}
				else
				{
					this._stanceCooltime = 2;
				}
				this._alreayUsed.Add(PurpleStance.Hit);
				break;
			}
			case PurpleStance.Defense:
				this._stancePassive.ChangeStance_defense();
				this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.DefensePowerUp, 1, null);
				this._stanceCooltime = 1;
				this._alreayUsed.Add(PurpleStance.Defense);
				break;
			}
		}

		private int AddNewCard(int id)
		{
			BattleDiceCardModel battleDiceCardModel = this.owner.allyCardDetail.AddNewCard(id, false);
			int result;
			if (battleDiceCardModel != null)
			{
				result = battleDiceCardModel.GetOriginCost();
			}
			else
			{
				result = 1;
			}
			return result;
		}

		public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
		{
			this._dmgReduction = 0;
			if (this.owner.UnitData.floorBattleData.param2 <= 0 && (this.owner.hp <= (float)this._teleportCondition || this.owner.hp - (float)dmg <= (float)this._teleportCondition))
			{
				this._dmgReduction = (int)((float)this._teleportCondition - (this.owner.hp - (float)dmg));
				this._teleportReady = true;
			}
			return base.BeforeTakeDamage(attacker, dmg);
		}

		public override int GetDamageReductionAll()
		{
			int result;
			if (this.owner.UnitData.floorBattleData.param2 <= 0 && this.owner.hp <= (float)this._teleportCondition)
			{
				result = 9999;
				this._teleportReady = true;
			}
			else
			{
				result = this._dmgReduction;
			}
			return result;
		}

		public override void OnBattleEnd_alive()
		{
			bool teleportReady = this._teleportReady;
		}

		private float _elapsedTimeTeleport;

		private int _teleportCondition = 350;

		private int _patternCount;

		private int _teleported;

		private int _stanceCooltime;

		private List<PurpleStance> _alreayUsed = new List<PurpleStance>();

		private PassiveAbility_250127 _stancePassive;

		private int _areaCoolTime = 1;

		private bool _teleportReady = false;

		private int _dmgReduction;
	}
}