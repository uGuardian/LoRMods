using System;

namespace FinallyBeyondTheTime
{
	public class PassiveAbility_180001_Finnal : PassiveAbilityBase
	{
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00004504 File Offset: 0x00002704
		public override bool isHide
		{
			get
			{
				return true;
			}
		}

		public override int GetMaxHpBonus()
		{
			return 800;
		}

		public override int GetMaxBpBonus()
		{
			return 450;
		}

		// (get) Token: 0x0600002B RID: 43 RVA: 0x00004548 File Offset: 0x00002748
		public override bool isTargetable
		{
			get
			{
				// return this._stageManager == null || this._stageManager.CurrentPhase != EnemyTeamStageManager_FinalFinal.FinalPhase.RolandOnly || Singleton<StageController>.Instance.GetStageModel().ClassInfo.id == 600013;
				return true;
			}
		}

		public override int SpeedDiceNumAdder()
		{
			int num = 1;
			if (Singleton<StageController>.Instance.RoundTurn >= 5)
			{
				num++;
			}
			return num;
		}

		public override void OnWaveStart()
		{
			// if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != 600013)
			if (false)
			{
				this._stageManager = (Singleton<StageController>.Instance.EnemyStageManager as EnemyTeamStageManager_FinalFinal);
			}
			this.SetStartingCards();
			this.owner.view.EnableStatNumber(false);
			this.owner.RecoverHP(this.owner.MaxHp);
		}

		public override void OnRoundStart()
		{
			this.owner.allyCardDetail.DrawCards(4);
			this.owner.cardSlotDetail.RecoverPlayPoint(6);
		}

		private void SetStartingCards()
		{
			this.owner.allyCardDetail.AddNewCard(706011, false);
			this.owner.allyCardDetail.AddNewCard(706011, false);
			this.owner.allyCardDetail.AddNewCard(706011, false);
			this.owner.allyCardDetail.AddNewCard(706011, false);
			this.owner.allyCardDetail.AddNewCard(706012, false);
			this.owner.allyCardDetail.AddNewCard(706015, false);
			this.owner.allyCardDetail.AddNewCard(706014, false);
			this.owner.allyCardDetail.AddNewCard(706013, false);
		}

		private void ResetPriorityAdder()
		{
			foreach (BattleDiceCardModel battleDiceCardModel in this.owner.allyCardDetail.GetAllDeck())
			{
				battleDiceCardModel.SetPriorityAdder(-10000);
			}
		}

		public override void OnRoundStartAfter()
		{
			// if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != 600013)
			if (false)
			{
				this.ResetPriorityAdder();
				if (this._stageManager != null)
				{
					if (this._stageManager.CurrentPhase == EnemyTeamStageManager_FinalFinal.FinalPhase.GeburahEnterBattle)
					{
						this.PrepareCard(706013);
						this.PrepareCard(706012);
					}
					else
					{
						if (this._stageManager.CurrentPhase == EnemyTeamStageManager_FinalFinal.FinalPhase.BinahEnterBattle)
						{
							int num = this._stageManager.FinalTurnCount % 10;
							if (num == 0)
							{
								this.PrepareCard(706011);
								this.PrepareCard(706015);
							}
							else
							{
								if (num == 1)
								{
									this.PrepareCard(706011);
									this.PrepareCard(706012);
								}
								else
								{
									if (num == 2)
									{
										this.PrepareCard(706011);
										this.PrepareCard(706013);
									}
									else
									{
										if (num == 3)
										{
											this.PrepareCard(706011);
											this.PrepareCard(706012);
										}
										else
										{
											if (num == 4)
											{
												this.PrepareCard(706014);
												return;
											}
											if (num == 5)
											{
												this.PrepareCard(706011);
												this.PrepareCard(706015);
											}
											else
											{
												if (num == 6)
												{
													this.PrepareCard(706011);
													this.PrepareCard(706012);
												}
												else
												{
													this.PrepareCard(706013);
													this.PrepareCard(706012);
												}
											}
										}
									}
								}
							}
						}
					}
					int num2 = this.SpeedDiceNumAdder();
					for (int i = 0; i < num2 - 1; i++)
					{
						this.PrepareCard(706011);
					}
				}
			}
		}

		public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
		{
			// if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != 600013)
			if (false)
			{
				if (this._stageManager != null && this._stageManager.GetBinahUnit() == null)
				{
					return this._stageManager.GetGeburahUnit();
				}
			}
			return base.ChangeAttackTarget(card, idx);
		}

		private void PrepareCard(int id)
		{
			BattleDiceCardModel battleDiceCardModel = this.owner.allyCardDetail.GetHand().Find((BattleDiceCardModel x) => x.GetPriorityAdder() <= -10000 && x.GetID() == id);
			if (battleDiceCardModel != null)
			{
				battleDiceCardModel.SetPriorityAdder(0);
			}
		}

		private const int _LINE1_CARD = 706011;

		private const int _LINE2_CARD = 706012;

		private const int _STR_LINE_CARD = 706013;

		private const int _AREA_CARD = 706014;

		private const int _LINE_LOCK_CARD = 706015;

		private EnemyTeamStageManager_FinalFinal _stageManager;

		private int _currentAdder = 1;
	}
}