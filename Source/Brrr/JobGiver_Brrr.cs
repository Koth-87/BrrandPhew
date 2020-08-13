using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x02000012 RID: 18
	public class JobGiver_Brrr : ThinkNode_JobGiver
	{
		// Token: 0x0600003A RID: 58 RVA: 0x000033FC File Offset: 0x000015FC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!Settings.UseBrrr || !pawn.IsColonistPlayerControlled)
			{
				return null;
			}
			if (pawn.InMentalState)
			{
				return null;
			}
			if (((pawn != null) ? pawn.Map : null) == null)
			{
				return null;
			}
			Pawn_NeedsTracker needs = pawn.needs;
			if (((needs != null) ? needs.food : null) != null && pawn.needs.food.CurLevelPercentage < pawn.needs.food.PercentageThreshHungry)
			{
				return null;
			}
			Pawn_NeedsTracker needs2 = pawn.needs;
			if (((needs2 != null) ? needs2.joy : null) != null && pawn.needs.joy.CurLevelPercentage < Settings.JoySev / 100f && Settings.AllowJoy)
			{
				return null;
			}
			if (RestUtility.DisturbancePreventsLyingDown(pawn))
			{
				return null;
			}
			if (pawn.CurJobDef == JobGiver_Brrr.BrrrJobDef.Brrr_BrrrRecovery || pawn.CurJobDef == JobGiver_Brrr.BrrrJobDef.Brrr_GaspRecovery || pawn.CurJobDef == JobGiver_Brrr.BrrrJobDef.Brrr_Skygaze || pawn.CurJobDef == JobGiver_Brrr.BrrrJobDef.Brrr_GoForWalk || pawn.CurJobDef == JobGiver_Brrr.BrrrJobDef.Brrr_PhewRecovery || pawn.CurJobDef == JobGiver_Brrr.BrrrJobDef.Brrr_YukRecovery)
			{
				return null;
			}
			if (!pawn.health.hediffSet.HasHediff(HediffDefOf.Hypothermia, false))
			{
				return null;
			}
			Hediff HedHypo = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
			if (HedHypo != null && HedHypo.Severity >= Settings.UnsafeBrrrSev / 100f)
			{
				Thing BrrrBed = null;
				Building_Bed FindBed = RestUtility.FindBedFor(pawn, pawn, false, true, false);
				if (FindBed != null && !FindBed.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors && pawn.ComfortableTemperatureRange().Includes(FindBed.GetRoom(RegionType.Set_Passable).Temperature))
				{
					BrrrBed = FindBed;
				}
				if (BrrrBed != null)
				{
					return new Job(JobGiver_Brrr.BrrrJobDef.Brrr_BrrrRecovery, BrrrBed);
				}
				FloatRange tempRange = pawn.ComfortableTemperatureRange();
				Region region = BrrrGlobals.BrrrClosestRegionWithinTemperatureRange(pawn.Position, pawn.Map, tempRange, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable);
				if (region != null)
				{
					return BrrrGlobals.GenNewRRJob(JobGiver_Brrr.BrrrJobDef.Brrr_BrrrRecovery, region);
				}
			}
			return null;
		}

		// Token: 0x0200002E RID: 46
		[DefOf]
		public static class BrrrJobDef
		{
			// Token: 0x04000058 RID: 88
			public static JobDef Brrr_BrrrRecovery;

			// Token: 0x04000059 RID: 89
			public static JobDef Brrr_GaspRecovery;

			// Token: 0x0400005A RID: 90
			public static JobDef Brrr_PhewRecovery;

			// Token: 0x0400005B RID: 91
			public static JobDef Brrr_YukRecovery;

			// Token: 0x0400005C RID: 92
			public static JobDef Brrr_Skygaze;

			// Token: 0x0400005D RID: 93
			public static JobDef Brrr_GoForWalk;
		}
	}
}
