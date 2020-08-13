using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x02000005 RID: 5
	public class JobGiver_AnimalPhew : ThinkNode_JobGiver
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002408 File Offset: 0x00000608
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!Settings.UsePhew || !Settings.ApplyAnimals)
			{
				return null;
			}
			if (((pawn != null) ? pawn.Map : null) == null)
			{
				return null;
			}
			if (!pawn.AnimalOrWildMan() || pawn.IsWildMan())
			{
				return null;
			}
			if (pawn.Faction == null)
			{
				return null;
			}
			if (pawn.Faction != Faction.OfPlayer)
			{
				return null;
			}
			if (pawn.training.CanAssignToTrain(TrainableDefOf.Obedience).Accepted && pawn.training.HasLearned(TrainableDefOf.Obedience) && ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn))
			{
				return null;
			}
			if (pawn.InMentalState)
			{
				return null;
			}
			Pawn_NeedsTracker needs = pawn.needs;
			if (((needs != null) ? needs.food : null) != null && pawn.needs.food.CurLevelPercentage < pawn.needs.food.PercentageThreshHungry)
			{
				return null;
			}
			if (RestUtility.DisturbancePreventsLyingDown(pawn))
			{
				return null;
			}
			if (pawn.CurJobDef == JobGiver_AnimalPhew.BrrrJobDef.Brrr_BrrrRecovery || pawn.CurJobDef == JobGiver_AnimalPhew.BrrrJobDef.Brrr_GaspRecovery || pawn.CurJobDef == JobGiver_AnimalPhew.BrrrJobDef.Brrr_PhewRecovery || pawn.CurJobDef == JobGiver_AnimalPhew.BrrrJobDef.Brrr_YukRecovery)
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
					return new Job(JobGiver_AnimalPhew.BrrrJobDef.Brrr_PhewRecovery, BrrrBed);
				}
				FloatRange tempRange = pawn.ComfortableTemperatureRange();
				Region region = BrrrGlobals.BrrrClosestRegionWithinTemperatureRange(pawn.Position, pawn.Map, tempRange, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable);
				if (region != null)
				{
					return BrrrGlobals.GenNewRRJob(JobGiver_AnimalPhew.BrrrJobDef.Brrr_BrrrRecovery, region);
				}
			}
			return null;
		}

		// Token: 0x02000022 RID: 34
		[DefOf]
		public static class BrrrJobDef
		{
			// Token: 0x04000028 RID: 40
			public static JobDef Brrr_BrrrRecovery;

			// Token: 0x04000029 RID: 41
			public static JobDef Brrr_GaspRecovery;

			// Token: 0x0400002A RID: 42
			public static JobDef Brrr_PhewRecovery;

			// Token: 0x0400002B RID: 43
			public static JobDef Brrr_YukRecovery;
		}
	}
}
