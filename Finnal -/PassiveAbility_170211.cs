using System;
using UnityEngine;

namespace FinallyBeyondTheTime
{
	public class PassiveAbility_170211_Finnal : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			base.OnRoundStart();
			this.SetCard();
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
					if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != 600013)
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
						this.AddNewCard(705213, 100);
						this.AddNewCard(705214, 90);
						this.AddNewCard(705215, 80);
						this.AddNewCard(705211, 60);
						this.AddNewCard(705212, 50);
					}
					else
					{
						if (num == 1)
						{
							this.AddNewCard(705215, 100);
							this.AddNewCard(705214, 90);
							this.AddNewCard(705218, 80);
							this.AddNewCard(705211, 60);
							this.AddNewCard(705212, 40);
						}
						else
						{
							if (num == 2)
							{
								this.AddNewCard(705217, 100);
								this.AddNewCard(705213, 90);
								this.AddNewCard(705218, 80);
								this.AddNewCard(705218, 70);
								this.AddNewCard(705211, 60);
								this.AddNewCard(705212, 50);
								this.AddNewCard(705212, 40);
							}
							else
							{
								if (num == 3)
								{
									this.AddNewCard(705214, 100);
									this.AddNewCard(705215, 90);
									this.AddNewCard(705213, 80);
									this.AddNewCard(705218, 70);
								}
								else
								{
									if (num == 4)
									{
										this.AddNewCard(705216, 100);
										this.AddNewCard(705214, 90);
										this.AddNewCard(705215, 80);
										this.AddNewCard(705213, 70);
										this.AddNewCard(705218, 60);
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