using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x0200000B RID: 11
	public class BrrrGlobals
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002CEC File Offset: 0x00000EEC
		public static Region BrrrClosestRegionWithinTemperatureRange(IntVec3 root, Map map, FloatRange tempRange, TraverseParms traverseParms, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = root.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return null;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
			Region foundReg = null;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r.IsDoorway)
				{
					return false;
				}
				if (tempRange.Includes(r.Room.Temperature))
				{
					foundReg = r;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, traversableRegionTypes);
			return foundReg;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002D54 File Offset: 0x00000F54
		public static IntVec3 GetNearestSafeRoofedCell(Pawn pawn, IntVec3 root, Map map, FloatRange tempRange)
		{
			Predicate<IntVec3> baseValidator = delegate(IntVec3 chkcell)
			{
				if (!chkcell.Standable(map))
				{
					return false;
				}
				if (chkcell.GetDangerFor(pawn, map) != Danger.None)
				{
					return false;
				}
				if (!chkcell.Roofed(map))
				{
					return false;
				}
				if (!tempRange.Includes(chkcell.GetRoom(map, RegionType.Set_Passable).Temperature))
				{
					return false;
				}
				if (!pawn.CanReserveAndReach(chkcell, PathEndMode.OnCell, Danger.None, 1, -1, null, false))
				{
					return false;
				}
				Room room = chkcell.GetRoom(map, RegionType.Set_Passable);
				bool flag = room != null && room.isPrisonCell;
				return pawn.IsPrisoner == flag;
			};
			IntVec3 cell;
			RCellFinder.TryFindRandomCellNearWith(root, baseValidator, map, out cell, 5, int.MaxValue);
			return cell;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002DA0 File Offset: 0x00000FA0
		public static void JoyTickOohCheckEnd(Pawn pawn, JoyTickFullJoyAction fullJoyAction = JoyTickFullJoyAction.EndJob, float extraJoyGainFactor = 1f)
		{
			Job curJob = pawn.CurJob;
			if (curJob.def.joyKind == null)
			{
				Log.Warning("This method can only be called for jobs with joyKind.", false);
				return;
			}
			pawn.needs.joy.GainJoy(extraJoyGainFactor * curJob.def.joyGainRate * 0.36f / 2500f, curJob.def.joyKind);
			if (curJob.def.joySkill != null)
			{
				pawn.skills.GetSkill(curJob.def.joySkill).Learn(curJob.def.joyXpPerTick, false);
			}
			if (!curJob.ignoreJoyTimeAssignment && !pawn.GetTimeAssignment().allowJoy)
			{
				pawn.jobs.curDriver.EndJobWith(JobCondition.InterruptForced);
			}
			if (pawn.needs.outdoors.CurLevelPercentage >= 0.75f)
			{
				if (fullJoyAction == JoyTickFullJoyAction.EndJob)
				{
					pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
					return;
				}
				if (fullJoyAction != JoyTickFullJoyAction.GoToNextToil)
				{
					return;
				}
				pawn.jobs.curDriver.ReadyForNextToil();
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002E9C File Offset: 0x0000109C
		public static bool BrrrAnimalIsFollowing(Pawn pawn)
		{
			return pawn.training.CanAssignToTrain(TrainableDefOf.Obedience).Accepted && pawn.training.HasLearned(TrainableDefOf.Obedience) && ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002EE0 File Offset: 0x000010E0
		public static int GenRnd100()
		{
			return Rand.Range(1, 100);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002EEA File Offset: 0x000010EA
		public static Job GenNewRRJob(JobDef def, Region reg)
		{
			return new Job(def, reg.RandomCell);
		}
	}
}
