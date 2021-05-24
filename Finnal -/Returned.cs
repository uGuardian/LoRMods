using System;

namespace FinallyBeyondTheTime
{
	public class Returned : BattleUnitBuf
	{
		public override void OnRoundEnd()
		{
			this.Destroy();
		}
	}
}