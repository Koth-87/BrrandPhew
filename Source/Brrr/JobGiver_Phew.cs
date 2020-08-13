using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x02000014 RID: 20
	public class JobGiver_Phew : ThinkNode_JobGiver
	{
		// Token: 0x0600003E RID: 62 RVA: 0x000037D0 File Offset: 0x000019D0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!Settings.UsePhew || !pawn.IsColonistPlayerControlled)
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
			if (pawn.CurJobDef == JobGiver_Phew.BrrrJobDef.Brrr_BrrrRecovery || pawn.CurJobDef == JobGiver_Phew.BrrrJobDef.Brrr_GaspRecovery || pawn.CurJobDef == JobGiver_Phew.BrrrJobDef.Brrr_Skygaze || pawn.CurJobDef == JobGiver_Phew.BrrrJobDef.Brrr_GoForWalk || pawn.CurJobDef == JobGiver_Phew.BrrrJobDef.Brrr_PhewRecovery || pawn.CurJobDef == JobGiver_Phew.BrrrJobDef.Brrr_YukRecovery)
			{
				return null;
			}
			if (!pawn.health.hediffSet.HasHediff(HediffDefOf.Heatstroke, false))
			{
				return null;
			}
			Hediff HedHeat = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, false);
			if (HedHeat != null && HedHeat.Severity >= Settings.UnsafePhewSev / 100f)
			{
				Thing BrrrBed = null;
				Building_Bed FindBed = RestUtility.FindBedFor(pawn, pawn, false, true, false);
				if (FindBed != null && !FindBed.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors && pawn.ComfortableTemperatureRange().Includes(FindBed.GetRoom(RegionType.Set_Passable).Temperature))
				{
					BrrrBed = FindBed;
				}
				if (BrrrBed != null)
				{
					return new Job(JobGiver_Phew.BrrrJobDef.Brrr_PhewRecovery, BrrrBed);
				}
				FloatRange tempRange = pawn.ComfortableTemperatureRange();
				Region region = BrrrGlobals.BrrrClosestRegionWithinTemperatureRange(pawn.Position, pawn.Map, tempRange, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable);
				if (region != null)
				{
					return new Job(JobGiver_Phew.BrrrJobDef.Brrr_PhewRecovery, region.RandomCell);
				}
			}
			return null;
		}

		// Token: 0x02000030 RID: 48
		[DefOf]
		public static class BrrrJobDef
		{
			// Token: 0x04000064 RID: 100
			public static JobDef Brrr_BrrrRecovery;

			// Token: 0x04000065 RID: 101
			public static JobDef Brrr_GaspRecovery;

			// Token: 0x04000066 RID: 102
			public static JobDef Brrr_PhewRecovery;

			// Token: 0x04000067 RID: 103
			public static JobDef Brrr_YukRecovery;

			// Token: 0x04000068 RID: 104
			public static JobDef Brrr_Skygaze;

			// Token: 0x04000069 RID: 105
			public static JobDef Brrr_GoForWalk;
		}
	}
}
