using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x0200000E RID: 14
	public class JobDriver_OohGoForWalk : JobDriver
	{
		// Token: 0x0600002B RID: 43 RVA: 0x0000301A File Offset: 0x0000121A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000301D File Offset: 0x0000121D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil goToil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			goToil.tickAction = delegate()
			{
				if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
				{
					this.EndJobWith(JobCondition.Succeeded);
					return;
				}
				BrrrGlobals.JoyTickOohCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f);
			};
			goToil.AddFailCondition(delegate
			{
				Pawn actor = goToil.actor;
				Pawn_NeedsTracker needs = actor.needs;
				return ((needs != null) ? needs.food : null) != null && actor.needs.food.CurLevelPercentage < actor.needs.food.PercentageThreshHungry;
			});
			yield return goToil;
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.job.targetQueueA.Count > 0)
					{
						LocalTargetInfo targetA = this.job.targetQueueA[0];
						this.job.targetQueueA.RemoveAt(0);
						this.job.targetA = targetA;
						this.JumpToToil(goToil);
					}
				}
			};
			yield break;
		}
	}
}
