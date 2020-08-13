using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x0200000F RID: 15
	public class JobDriver_OohSkygaze : JobDriver
	{
		// Token: 0x0600002E RID: 46 RVA: 0x00003035 File Offset: 0x00001235
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003038 File Offset: 0x00001238
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil gaze = new Toil
			{
				initAction = delegate()
				{
					this.pawn.jobs.posture = PawnPosture.LayingOnGroundFaceUp;
				},
				tickAction = delegate()
				{
					BrrrGlobals.JoyTickOohCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f);
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = this.job.def.joyDuration
			};
			gaze.FailOn(() => this.pawn.Position.Roofed(this.pawn.Map));
			gaze.AddFailCondition(delegate
			{
				Pawn actor = gaze.actor;
				Pawn_NeedsTracker needs = actor.needs;
				if (((needs != null) ? needs.food : null) != null)
				{
					Pawn_NeedsTracker needs2 = actor.needs;
					float? num = (needs2 != null) ? new float?(needs2.food.CurLevelPercentage) : null;
					float percentageThreshHungry = actor.needs.food.PercentageThreshHungry;
					if (num.GetValueOrDefault() < percentageThreshHungry & num != null)
					{
						return true;
					}
				}
				return false;
			});
			yield return gaze;
			yield break;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003048 File Offset: 0x00001248
		public override string GetReport()
		{
			if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
			{
				return "Brrr.OohEclipse".Translate();
			}
			if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Aurora))
			{
				return "Brrr.OohAurora".Translate();
			}
			float num = GenCelestial.CurCelestialSunGlow(base.Map);
			if (num < 0.1f)
			{
				return "Brrr.OohStarGazing".Translate();
			}
			if (num >= 0.65f)
			{
				return "Brrr.OohClouds".Translate();
			}
			if (GenLocalDate.DayPercent(this.pawn) < 0.5f)
			{
				return "Brrr.OohSunrise".Translate();
			}
			return "Brrr.OohSunset".Translate();
		}
	}
}
