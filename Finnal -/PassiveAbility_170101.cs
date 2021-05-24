using System;
using System.Collections.Generic;

namespace FinallyBeyondTheTime
{
	public class PassiveAbility_170101 : PassiveAbilityBase
	{
		public override void OnWaveStart()
		{
		}

		public override void OnRoundEndTheLast()
		{
			base.OnRoundEndTheLast();
			this.CreateEnemys();
		}

		public void CreateEnemys()
		{
			int num = 0;
			if (!(this.owner.hp <= 300f))
			{
				List<BattleUnitModel> list = BattleObjectManager.instance.GetList(Faction.Enemy).FindAll((BattleUnitModel x) => x.IsDead() && x != this.owner);
				if (!(list.Count == 0))
				{
					foreach (BattleUnitModel battleUnitModel in list)
					{
						num++;
						if (num > 4)
						{
							break;
						}
						BattleUnitModel battleUnitModel2 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, this.CREATE_ENEMY_ID, battleUnitModel.index, -1);
						if (battleUnitModel2 != null)
						{
							battleUnitModel2.SetDeadSceneBlock(false);
							this.owner.SetHp((int)this.owner.hp - 50);
						}
					}
				}
			}
		}

		private readonly int CREATE_ENEMY_ID = 60106;
	}
}