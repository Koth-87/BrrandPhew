using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x02000015 RID: 21
	public class JobGiver_Ooh : ThinkNode_JobGiver
	{
		// Token: 0x06000040 RID: 64 RVA: 0x000039BC File Offset: 0x00001BBC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!Settings.UseOoh || !pawn.IsColonistPlayerControlled)
			{
				return null;
			}
			Pawn_NeedsTracker needs = pawn.needs;
			if (((needs != null) ? needs.outdoors : null) == null)
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
			Pawn_NeedsTracker needs2 = pawn.needs;
			if (((needs2 != null) ? needs2.food : null) != null && pawn.needs.food.CurLevelPercentage < pawn.needs.food.PercentageThreshHungry)
			{
				return null;
			}
			Pawn_NeedsTracker needs3 = pawn.needs;
			if (((needs3 != null) ? needs3.joy : null) != null && pawn.needs.joy.CurLevelPercentage < Settings.JoySev / 100f && Settings.AllowJoy)
			{
				return null;
			}
			if (RestUtility.DisturbancePreventsLyingDown(pawn))
			{
				return null;
			}
			if (pawn.CurJobDef == JobGiver_Ooh.BrrrJobDef.Brrr_BrrrRecovery || pawn.CurJobDef == JobGiver_Ooh.BrrrJobDef.Brrr_GaspRecovery || pawn.CurJobDef == JobGiver_Ooh.BrrrJobDef.Brrr_Skygaze || pawn.CurJobDef == JobGiver_Ooh.BrrrJobDef.Brrr_GoForWalk || pawn.CurJobDef == JobGiver_Ooh.BrrrJobDef.Brrr_PhewRecovery || pawn.CurJobDef == JobGiver_Ooh.BrrrJobDef.Brrr_YukRecovery)
			{
				return null;
			}
			if (pawn.needs.outdoors.CurLevelPercentage > Settings.OohSev / 100f)
			{
				return null;
			}
			if (Settings.UseBrrr)
			{
				Pawn_HealthTracker health = pawn.health;
				Hediff HedHypo = (health != null) ? health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false) : null;
				if (HedHypo != null && HedHypo.Severity >= Settings.UnsafeBrrrSev / 100f)
				{
					return null;
				}
			}
			if (Settings.UsePhew)
			{
				Pawn_HealthTracker health2 = pawn.health;
				Hediff HedHeat = (health2 != null) ? health2.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, false) : null;
				if (HedHeat != null && HedHeat.Severity >= Settings.UnsafePhewSev / 100f)
				{
					return null;
				}
			}
			if (Settings.UseYuk)
			{
				Pawn_HealthTracker health3 = pawn.health;
				Hediff HedYuk = (health3 != null) ? health3.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false) : null;
				if (HedYuk != null && HedYuk.Severity >= Settings.UnsafeYukSev / 100f)
				{
					return null;
				}
			}
			IntVec3 OohStarGazeDest;
			if (!JobGiver_Ooh.TryFindOohCell(pawn.Position, pawn, out OohStarGazeDest))
			{
				return null;
			}
			List<IntVec3> OohWalkDests;
			if (!JobGiver_Ooh.TryFindOohWalkPath(pawn, OohStarGazeDest, out OohWalkDests))
			{
				return null;
			}
			Job OohJob;
			if (BrrrGlobals.GenRnd100() <= 45)
			{
				OohJob = new Job(JobGiver_Ooh.BrrrJobDef.Brrr_GoForWalk, OohWalkDests[0]);
				if (OohJob != null)
				{
					OohJob.targetQueueA = new List<LocalTargetInfo>();
					for (int i = 1; i < OohWalkDests.Count; i++)
					{
						OohJob.targetQueueA.Add(OohWalkDests[i]);
					}
					OohJob.locomotionUrgency = LocomotionUrgency.Walk;
				}
			}
			else
			{
				OohJob = new Job(JobGiver_Ooh.BrrrJobDef.Brrr_Skygaze, OohStarGazeDest);
			}
			if (OohJob != null)
			{
				return OohJob;
			}
			return null;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003C50 File Offset: 0x00001E50
		public static bool TryFindOohCell(IntVec3 root, Pawn searcher, out IntVec3 result)
		{
			Predicate<IntVec3> cellValidator = (IntVec3 c) => !c.Roofed(searcher.Map) && !c.GetTerrain(searcher.Map).avoidWander;
			IntVec3 unused;
			Predicate<Region> validator = (Region r) => r.Room.PsychologicallyOutdoors && r.TryFindRandomCellInRegion(cellValidator, out unused);
			TraverseParms traverseParms = TraverseParms.For(searcher, Danger.Deadly, TraverseMode.ByPawn, false);
			Region result2;
			if (!CellFinder.TryFindClosestRegionWith(root.GetRegion(searcher.Map, RegionType.Set_Passable), traverseParms, validator, 300, out result2, RegionType.Set_Passable))
			{
				result = root;
				return false;
			}
			return CellFinder.RandomRegionNear(result2, 14, traverseParms, validator, searcher, RegionType.Set_Passable).TryFindRandomCellInRegion(cellValidator, out result);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003CE4 File Offset: 0x00001EE4
		public static bool TryFindOohWalkPath(Pawn pawn, IntVec3 root, out List<IntVec3> result)
		{
			List<IntVec3> list = new List<IntVec3>();
			list.Add(root);
			IntVec3 intVec = root;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec2 = IntVec3.Invalid;
				float num = -1f;
				for (int num2 = JobGiver_Ooh.StartRadialIndex; num2 > JobGiver_Ooh.EndRadialIndex; num2 -= JobGiver_Ooh.RadialIndexStride)
				{
					IntVec3 intVec3 = intVec + GenRadial.RadialPattern[num2];
					if (intVec3.InBounds(pawn.Map) && intVec3.Standable(pawn.Map) && !intVec3.GetTerrain(pawn.Map).avoidWander && GenSight.LineOfSight(intVec, intVec3, pawn.Map, false, null, 0, 0) && !intVec3.Roofed(pawn.Map) && !PawnUtility.KnownDangerAt(intVec3, pawn.Map, pawn))
					{
						float num3 = 10000f;
						for (int j = 0; j < list.Count; j++)
						{
							num3 += (float)(list[j] - intVec3).LengthManhattan;
						}
						float num4 = (float)(intVec3 - root).LengthManhattan;
						if (num4 > 40f)
						{
							num3 *= Mathf.InverseLerp(70f, 40f, num4);
						}
						if (list.Count >= 2)
						{
							float angleFlat = (list[list.Count - 1] - list[list.Count - 2]).AngleFlat;
							float angleFlat2 = (intVec3 - intVec).AngleFlat;
							float num5;
							if (angleFlat2 > angleFlat)
							{
								num5 = angleFlat2 - angleFlat;
							}
							else
							{
								angleFlat -= 360f;
								num5 = angleFlat2 - angleFlat;
							}
							if (num5 > 110f)
							{
								num3 *= 0.01f;
							}
						}
						if (list.Count >= 4 && (intVec - root).LengthManhattan < (intVec3 - root).LengthManhattan)
						{
							num3 *= 1E-05f;
						}
						if (num3 > num)
						{
							intVec2 = intVec3;
							num = num3;
						}
					}
				}
				if (num < 0f)
				{
					result = null;
					return false;
				}
				list.Add(intVec2);
				intVec = intVec2;
			}
			list.Add(root);
			result = list;
			return true;
		}

		// Token: 0x04000009 RID: 9
		private static readonly int StartRadialIndex = GenRadial.NumCellsInRadius(14f);

		// Token: 0x0400000A RID: 10
		private static readonly int EndRadialIndex = GenRadial.NumCellsInRadius(2f);

		// Token: 0x0400000B RID: 11
		private static readonly int RadialIndexStride = 3;

		// Token: 0x02000031 RID: 49
		[DefOf]
		public static class BrrrJobDef
		{
			// Token: 0x0400006A RID: 106
			public static JobDef Brrr_BrrrRecovery;

			// Token: 0x0400006B RID: 107
			public static JobDef Brrr_GaspRecovery;

			// Token: 0x0400006C RID: 108
			public static JobDef Brrr_PhewRecovery;

			// Token: 0x0400006D RID: 109
			public static JobDef Brrr_YukRecovery;

			// Token: 0x0400006E RID: 110
			public static JobDef Brrr_Skygaze;

			// Token: 0x0400006F RID: 111
			public static JobDef Brrr_GoForWalk;
		}
	}
}
