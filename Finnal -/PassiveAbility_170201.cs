using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinallyBeyondTheTime
{
	public class PassiveAbility_170201_Finnal : PassiveAbilityBase
	{
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00005274 File Offset: 0x00003474
		public override bool isHide
		{
			get
			{
				return this._bHide;
			}
		}

		public override void OnCreated()
		{
			base.OnCreated();
			this._bHide = false;
		}

		public override void OnRoundStart()
		{
			this._bHide = this.IsAngelicaDead();
			this.SetCard();
		}

		private bool IsAngelicaDead()
		{
			bool result = false;
			if (!this.owner.bufListDetail.GetActivatedBufList().Exists((BattleUnitBuf x) => x is BattleUnitBuf_SpiritLink))
			{
				List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
				aliveList.Remove(this.owner);
				if (aliveList.Count == 0)
				{
					result = true;
				}
			}
			return result;
		}

		private void SetCard()
		{
			bool bHide = this._bHide;
			if (!bHide)
			{
				if (!(this.owner.IsBreakLifeZero()))
				{
					this.owner.allyCardDetail.ExhaustAllCards();
					int num = -1;
					// if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != 600013)
					if (false)
					{
						EnemyTeamStageManager enemyStageManager = Singleton<StageController>.Instance.EnemyStageManager;
						if (enemyStageManager is EnemyTeamStageManager_BlackSilence)
						{
							num = (enemyStageManager as EnemyTeamStageManager_BlackSilence).thirdPhaseElapsed;
						}
					}
					else
					{
						num = RandomUtil.Range(0, 4);
					}
					if (num == 0)
					{
						this.AddNewCard(705203, 100);
						this.AddNewCard(705204, 90);
						this.AddNewCard(705205, 80);
						this.AddNewCard(705201, 60);
						this.AddNewCard(705202, 50);
					}
					else
					{
						if (num == 1)
						{
							this.AddNewCard(705205, 100);
							this.AddNewCard(705204, 90);
							this.AddNewCard(705206, 80);
							this.AddNewCard(705201, 60);
							this.AddNewCard(705202, 50);
						}
						else
						{
							if (num == 2)
							{
								this.AddNewCard(705209, 100);
								this.AddNewCard(705203, 90);
								this.AddNewCard(705206, 80);
								this.AddNewCard(705206, 70);
								this.AddNewCard(705201, 60);
								this.AddNewCard(705201, 50);
								this.AddNewCard(705202, 40);
							}
							else
							{
								if (num == 3)
								{
									this.AddNewCard(705207, 100);
									this.AddNewCard(705208, 90);
									this.AddNewCard(705207, 80);
									this.AddNewCard(705208, 70);
								}
								else
								{
									if (num == 4)
									{
										this.AddNewCard(705207, 100);
										this.AddNewCard(705208, 90);
										this.AddNewCard(705207, 80);
										this.AddNewCard(705208, 70);
										this.AddNewCard(705206, 60);
									}
									else
									{
										Debug.Log("SetCard Phase Error in " + base.GetType().ToString());
									}
								}
							}
						}
					}
				}
			}
		}

		private void AddNewCard(int id, int priorityAdder)
		{
			BattleDiceCardModel battleDiceCardModel = this.owner.allyCardDetail.AddNewCard(id, false);
			if (battleDiceCardModel != null)
			{
				battleDiceCardModel.SetCostToZero(true);
				battleDiceCardModel.SetPriorityAdder(priorityAdder);
				battleDiceCardModel.temporary = true;
			}
		}
	}
}