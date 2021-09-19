using System;

namespace FinallyBeyondTheTime
{
	public class PassiveAbility_180002_Finnal : PassiveAbilityBase
	{
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00004A5C File Offset: 0x00002C5C
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

		private void SetStartingCards()
		{
			this.owner.allyCardDetail.AddNewCard(706001, false);
			this.owner.allyCardDetail.AddNewCard(706001, false);
			this.owner.allyCardDetail.AddNewCard(706001, false);
			this.owner.allyCardDetail.AddNewCard(706002, false);
			this.owner.allyCardDetail.AddNewCard(706003, false);
			this.owner.allyCardDetail.AddNewCard(706004, false);
			this.owner.allyCardDetail.AddNewCard(706005, false);
			this.owner.allyCardDetail.AddNewCard(706006, false);
		}

		private void ResetPriorityAdder()
		{
			foreach (BattleDiceCardModel battleDiceCardModel in this.owner.allyCardDetail.GetAllDeck())
			{
				battleDiceCardModel.SetPriorityAdder(-10000);
			}
		}

		public override void OnRoundStart()
		{
			this.owner.allyCardDetail.DrawCards(4);
			this.owner.cardSlotDetail.RecoverPlayPoint(6);
		}

		public override void OnRoundStartAfter()
		{
			// if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != 600013)
			if (false)
			{
				this.ResetPriorityAdder();
				if (this._stageManager != null)
				{
					if (this._stageManager.CurrentPhase == EnemyTeamStageManager_FinalFinal.FinalPhase.RolandOnly)
					{
						this.PrepareCard(706004);
					}
					else
					{
						if (this._stageManager.CurrentPhase == EnemyTeamStageManager_FinalFinal.FinalPhase.GeburahEnterBattle)
						{
							int num = this._stageManager.GeburahTurnCount % 4;
							this.PrepareCard(706001);
							this.PrepareCard(706002);
						}
						else
						{
							if (this._stageManager.CurrentPhase == EnemyTeamStageManager_FinalFinal.FinalPhase.BinahEnterBattle)
							{
								int finalTurnCount = this._stageManager.FinalTurnCount;
								if (finalTurnCount == 0)
								{
									this.PrepareCard(706001);
									this.PrepareCard(706003);
								}
								else
								{
									if (finalTurnCount == 1)
									{
										this.PrepareCard(706002);
										this.PrepareCard(706006);
									}
									else
									{
										if (finalTurnCount == 2)
										{
											this.PrepareCard(706001);
											this.PrepareCard(706004);
										}
										else
										{
											if (finalTurnCount == 3)
											{
												this.PrepareCard(706001);
												this.PrepareCard(706005);
											}
											else
											{
												if (finalTurnCount == 4)
												{
													this.PrepareCard(706002);
													this.PrepareCard(706003);
												}
												else
												{
													if (finalTurnCount == 5)
													{
														this.PrepareCard(706001);
														this.PrepareCard(706004);
													}
													else
													{
														if (finalTurnCount == 6)
														{
															this.PrepareCard(706001);
															this.PrepareCard(706005);
														}
														else
														{
															this.PrepareCard(706002);
															this.PrepareCard(706004);
														}
													}
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
						this.PrepareCard(706001);
					}
				}
			}
		}

		private void PrepareCard(int id)
		{
			BattleDiceCardModel battleDiceCardModel = this.owner.allyCardDetail.GetHand().Find((BattleDiceCardModel x) => x.GetPriorityAdder() <= -10000 && x.GetID() == id);
			if (battleDiceCardModel != null)
			{
				battleDiceCardModel.SetPriorityAdder(0);
			}
		}

		public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
		{
			// if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != 600013)
			if (false)
			{
				if (this._stageManager != null && this._stageManager.GetBinahUnit() == null)
				{
					return this._stageManager.GetRolandUnit();
				}
			}
			return base.ChangeAttackTarget(card, idx);
		}

		private const int _NORMAL1_CARD = 706001;

		private const int _NORMAL2_CARD = 706002;

		private const int _R_CARD = 706003;

		private const int _W_CARD = 706004;

		private const int _ALL_CARD = 706005;

		private const int _K_CARD = 706006;

		private EnemyTeamStageManager_FinalFinal _stageManager;
	}
}