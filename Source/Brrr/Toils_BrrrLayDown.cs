using System;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x0200000A RID: 10
	public static class Toils_BrrrLayDown
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000029A8 File Offset: 0x00000BA8
		public static Toil BrrrLayDown(TargetIndex bedOrRestSpotIndex, bool hasBed, bool lookForOtherJobs = false, bool canSleep = true, bool gainRestAndHealth = true)
		{
			Toil layDown = new Toil();
			layDown.initAction = delegate()
			{
				Pawn actor3 = layDown.actor;
				actor3.pather.StopDead();
				JobDriver curDriver3 = actor3.jobs.curDriver;
				if (hasBed)
				{
					if (!((Building_Bed)actor3.CurJob.GetTarget(bedOrRestSpotIndex).Thing).OccupiedRect().Contains(actor3.Position))
					{
						Log.Error("Can't start LayDown toil because pawn is not in the bed. pawn=" + actor3, false);
						actor3.jobs.EndCurrentJob(JobCondition.Errored, true, true);
						return;
					}
					actor3.jobs.posture = PawnPosture.LayingInBed;
				}
				else
				{
					actor3.jobs.posture = PawnPosture.LayingOnGroundNormal;
				}
				curDriver3.asleep = false;
				if (actor3.mindState.applyBedThoughtsTick == 0)
				{
					actor3.mindState.applyBedThoughtsTick = Find.TickManager.TicksGame + Rand.Range(2500, 10000);
					actor3.mindState.applyBedThoughtsOnLeave = false;
				}
				if (actor3.ownership != null && actor3.CurrentBed() != actor3.ownership.OwnedBed)
				{
					ThoughtUtility.RemovePositiveBedroomThoughts(actor3);
				}
			};
			layDown.tickAction = delegate()
			{
				Pawn actor2 = layDown.actor;
				Job curJob = actor2.CurJob;
				JobDriver curDriver2 = actor2.jobs.curDriver;
				Building_Bed building_Bed = (Building_Bed)curJob.GetTarget(bedOrRestSpotIndex).Thing;
				actor2.GainComfortFromCellIfPossible(false);
				if (!curDriver2.asleep)
				{
					if (canSleep && ((actor2.needs.rest != null && actor2.needs.rest.CurLevel < RestUtility.FallAsleepMaxLevel(actor2)) || curJob.forceSleep))
					{
						curDriver2.asleep = true;
					}
				}
				else if (!canSleep)
				{
					curDriver2.asleep = false;
				}
				else if ((actor2.needs.rest == null || actor2.needs.rest.CurLevel >= RestUtility.WakeThreshold(actor2)) && !curJob.forceSleep)
				{
					curDriver2.asleep = false;
				}
				if ((curDriver2.asleep & gainRestAndHealth) && actor2.needs.rest != null)
				{
					float restEffectiveness = (building_Bed == null || !building_Bed.def.statBases.StatListContains(StatDefOf.BedRestEffectiveness)) ? 0.8f : building_Bed.GetStatValue(StatDefOf.BedRestEffectiveness, true);
					actor2.needs.rest.TickResting(restEffectiveness);
				}
				if (actor2.mindState.applyBedThoughtsTick != 0 && actor2.mindState.applyBedThoughtsTick <= Find.TickManager.TicksGame)
				{
					Toils_BrrrLayDown.ApplyBedThoughts(actor2);
					actor2.mindState.applyBedThoughtsTick += 60000;
					actor2.mindState.applyBedThoughtsOnLeave = true;
				}
				if (actor2.IsHashIntervalTick(100) && !actor2.Position.Fogged(actor2.Map))
				{
					if (curDriver2.asleep)
					{
						MoteMaker.ThrowMetaIcon(actor2.Position, actor2.Map, ThingDefOf.Mote_SleepZ);
					}
					if (gainRestAndHealth && actor2.health.hediffSet.GetNaturallyHealingInjuredParts().Any<BodyPartRecord>())
					{
						MoteMaker.ThrowMetaIcon(actor2.Position, actor2.Map, ThingDefOf.Mote_HealingCross);
					}
				}
				if (actor2.ownership != null && building_Bed != null && !building_Bed.Medical && !building_Bed.OwnersForReading.Contains(actor2))
				{
					if (actor2.Downed)
					{
						actor2.Position = CellFinder.RandomClosewalkCellNear(actor2.Position, actor2.Map, 1, null);
					}
					actor2.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				if (lookForOtherJobs && actor2.IsHashIntervalTick(211))
				{
					actor2.jobs.CheckForJobOverride();
				}
			};
			layDown.defaultCompleteMode = ToilCompleteMode.Never;
			if (hasBed)
			{
				layDown.FailOnBedNoLongerUsable(bedOrRestSpotIndex);
			}
			layDown.AddFailCondition(delegate
			{
				Pawn actor = layDown.actor;
				Pawn_NeedsTracker needs = actor.needs;
				if (((needs != null) ? needs.food : null) != null && actor.needs.food.CurLevelPercentage < actor.needs.food.PercentageThreshHungry)
				{
					return true;
				}
				Pawn_NeedsTracker needs2 = actor.needs;
				return ((needs2 != null) ? needs2.joy : null) != null && !actor.RaceProps.Animal && actor.needs.joy.CurLevelPercentage < Settings.JoySev / 100f && Settings.AllowJoy;
			});
			layDown.AddFailCondition(delegate
			{
				Pawn actor = layDown.actor;
				Hediff hypoHed = actor.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
				Hediff heatHed = actor.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, false);
				Hediff ToxHed = actor.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
				Hediff BreathHed = actor.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("OxygenStarvation", false), false);
				return hypoHed == null && heatHed == null && ToxHed == null && BreathHed == null;
			});
			layDown.AddFinishAction(delegate
			{
				Pawn actor = layDown.actor;
				JobDriver curDriver = actor.jobs.curDriver;
				if (actor.mindState.applyBedThoughtsOnLeave)
				{
					Toils_BrrrLayDown.ApplyBedThoughts(actor);
				}
				curDriver.asleep = false;
			});
			return layDown;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002A88 File Offset: 0x00000C88
		private static void ApplyBedThoughts(Pawn actor)
		{
			if (actor.needs.mood == null)
			{
				return;
			}
			Building_Bed building_Bed = actor.CurrentBed();
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInBedroom);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInBarracks);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptOutside);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptOnGround);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInCold);
			actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInHeat);
			if (actor.GetRoom(RegionType.Set_Passable).PsychologicallyOutdoors)
			{
				actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptOutside, null);
			}
			if (building_Bed == null || building_Bed.CostListAdjusted().Count == 0)
			{
				actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptOnGround, null);
			}
			if (actor.AmbientTemperature < actor.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null))
			{
				actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInCold, null);
			}
			if (actor.AmbientTemperature > actor.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null))
			{
				actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInHeat, null);
			}
			if (building_Bed == null || building_Bed != actor.ownership.OwnedBed || building_Bed.ForPrisoners || actor.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				return;
			}
			ThoughtDef thoughtDef = null;
			if (building_Bed.GetRoom(RegionType.Set_Passable).Role == RoomRoleDefOf.Bedroom)
			{
				thoughtDef = ThoughtDefOf.SleptInBedroom;
			}
			else if (building_Bed.GetRoom(RegionType.Set_Passable).Role == RoomRoleDefOf.Barracks)
			{
				thoughtDef = ThoughtDefOf.SleptInBarracks;
			}
			if (thoughtDef != null)
			{
				int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(building_Bed.GetRoom(RegionType.Set_Passable).GetStat(RoomStatDefOf.Impressiveness));
				if (thoughtDef.stages[scoreStageIndex] != null)
				{
					actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtDef, scoreStageIndex), null);
				}
			}
		}

		// Token: 0x04000003 RID: 3
		private const int TicksBetweenSleepZs = 100;

		// Token: 0x04000004 RID: 4
		public const float GroundRestEffectiveness = 0.8f;

		// Token: 0x04000005 RID: 5
		private const int GetUpOrStartJobWhileInBedCheckInterval = 211;
	}
}
