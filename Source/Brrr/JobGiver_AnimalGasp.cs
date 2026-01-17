using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr;

public class JobGiver_AnimalGasp : ThinkNode_JobGiver
{
    protected override Job TryGiveJob(Pawn pawn)
    {
        if (!Settings.UseGasp || !Settings.ApplyAnimals)
        {
            return null;
        }

        if (pawn?.Map == null)
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

        if (pawn.training.CanAssignToTrain(TrainableDefOf.Obedience).Accepted &&
            pawn.training.HasLearned(TrainableDefOf.Obedience) &&
            ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn))
        {
            return null;
        }

        if (pawn.InMentalState)
        {
            return null;
        }

        var needs = pawn.needs;
        if (needs?.food != null && pawn.needs.food.CurLevelPercentage < pawn.needs.food.PercentageThreshHungry)
        {
            return null;
        }

        if (RestUtility.DisturbancePreventsLyingDown(pawn))
        {
            return null;
        }

        if (pawn.CurJobDef == BrrrJobDefOf.Brrr_BrrrRecovery || pawn.CurJobDef == BrrrJobDefOf.Brrr_GaspRecovery ||
            pawn.CurJobDef == BrrrJobDefOf.Brrr_PhewRecovery || pawn.CurJobDef == BrrrJobDefOf.Brrr_YukRecovery ||
            pawn.CurJobDef == BrrrJobDefOf.Brrr_RedRecovery)
        {
            return null;
        }

        var oxStarve = DefDatabase<HediffDef>.GetNamed("OxygenStarvation", false);
        var vacExposure = DefDatabase<HediffDef>.GetNamed("VacuumExposure", false);

        if (oxStarve == null && vacExposure == null)
        {
            return null;
        }

        if (oxStarve != null)
        {
            var hedBreath = pawn.health.hediffSet.GetFirstHediffOfDef(oxStarve);
            if (hedBreath == null || hedBreath.Severity < Settings.UnsafeGaspSev / 100f)
            {
                return null;
            }
        }

        if (vacExposure != null)
        {
            var exposure = pawn.health.hediffSet.GetFirstHediffOfDef(vacExposure);
            if (exposure == null || exposure.Severity < Settings.UnsafeGaspSev / 100f)
            {
                return null;
            }
        }

        Thing brrBed = null;
        var findBed = RestUtility.FindBedFor(pawn, pawn, false, true);
        if (findBed != null && findBed.Position.Roofed(pawn.Map) && !findBed.Position.IsPolluted(pawn.Map) &&
            pawn.ComfortableTemperatureRange().Includes(findBed.GetRoom().Temperature))
        {
            brrBed = findBed;
        }

        if (brrBed != null)
        {
            return new Job(BrrrJobDefOf.Brrr_GaspRecovery, brrBed);
        }

        var tempRange = pawn.ComfortableTemperatureRange();
        var safeCell = BrrrGlobals.GetNearestSafeRoofedCell(pawn, pawn.Position, pawn.Map, tempRange);
        return new Job(BrrrJobDefOf.Brrr_GaspRecovery, safeCell);
    }
}