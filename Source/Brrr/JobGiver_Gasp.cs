using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x02000013 RID: 19
	public class JobGiver_Gasp : ThinkNode_JobGiver
	{
		// Token: 0x0600003C RID: 60 RVA: 0x000035E0 File Offset: 0x000017E0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!Settings.UseGasp || !pawn.IsColonistPlayerControlled)
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
			if (pawn.CurJobDef == JobGiver_Gasp.BrrrJobDef.Brrr_BrrrRecovery || pawn.CurJobDef == JobGiver_Gasp.BrrrJobDef.Brrr_GaspRecovery || pawn.CurJobDef == JobGiver_Gasp.BrrrJobDef.Brrr_Skygaze || pawn.CurJobDef == JobGiver_Gasp.BrrrJobDef.Brrr_GoForWalk || pawn.CurJobDef == JobGiver_Gasp.BrrrJobDef.Brrr_PhewRecovery || pawn.CurJobDef == JobGiver_Gasp.BrrrJobDef.Brrr_YukRecovery)
			{
				return null;
			}
			HediffDef OxStarve = DefDatabase<HediffDef>.GetNamed("OxygenStarvation", false);
			if (OxStarve == null || !pawn.health.hediffSet.HasHediff(OxStarve, false))
			{
				return null;
			}
			Hediff HedBreath = pawn.health.hediffSet.GetFirstHediffOfDef(OxStarve, false);
			if (HedBreath == null || HedBreath.Severity < Settings.UnsafeGaspSev / 100f)
			{
				return null;
			}
			Thing BrrrBed = null;
			Building_Bed FindBed = RestUtility.FindBedFor(pawn, pawn, false, true, false);
			if (FindBed != null && FindBed.Position.Roofed((pawn != null) ? pawn.Map : null) && pawn.ComfortableTemperatureRange().Includes(FindBed.GetRoom(RegionType.Set_Passable).Temperature))
			{
				BrrrBed = FindBed;
			}
			if (BrrrBed != null)
			{
				return new Job(JobGiver_Gasp.BrrrJobDef.Brrr_GaspRecovery, BrrrBed);
			}
			FloatRange tempRange = pawn.ComfortableTemperatureRange();
			IntVec3 SafeCell = BrrrGlobals.GetNearestSafeRoofedCell(pawn, pawn.Position, pawn.Map, tempRange);
			return new Job(JobGiver_Gasp.BrrrJobDef.Brrr_GaspRecovery, SafeCell);
		}

		// Token: 0x0200002F RID: 47
		[DefOf]
		public static class BrrrJobDef
		{
			// Token: 0x0400005E RID: 94
			public static JobDef Brrr_BrrrRecovery;

			// Token: 0x0400005F RID: 95
			public static JobDef Brrr_PhewRecovery;

			// Token: 0x04000060 RID: 96
			public static JobDef Brrr_YukRecovery;

			// Token: 0x04000061 RID: 97
			public static JobDef Brrr_GaspRecovery;

			// Token: 0x04000062 RID: 98
			public static JobDef Brrr_Skygaze;

			// Token: 0x04000063 RID: 99
			public static JobDef Brrr_GoForWalk;
		}
	}
}
