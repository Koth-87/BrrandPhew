using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr;

public class JobGiver_AnimalBrrr : ThinkNode_JobGiver
{
    protected override Job TryGiveJob(Pawn pawn)
    {
        if (!Settings.UseBrrr || !Settings.ApplyAnimals)
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

        if (!pawn.health.hediffSet.HasHediff(HediffDefOf.Hypothermia))
        {
            return null;
        }

        var HedHypo = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia);
        if (HedHypo == null || !(HedHypo.Severity >= Settings.UnsafeBrrrSev / 100f))
        {
            return null;
        }

        Thing BrrrBed = null;
        var FindBed = RestUtility.FindBedFor(pawn, pawn, false, true);
        if (FindBed != null && !FindBed.GetRoom().PsychologicallyOutdoors && !FindBed.Position.IsPolluted(pawn.Map) &&
            pawn.ComfortableTemperatureRange().Includes(FindBed.GetRoom().Temperature))
        {
            BrrrBed = FindBed;
        }

        if (BrrrBed != null)
        {
            return new Job(BrrrJobDefOf.Brrr_BrrrRecovery, BrrrBed);
        }

        var tempRange = pawn.ComfortableTemperatureRange();
        var region = BrrrGlobals.BrrrClosestRegionWithinTemperatureRange(pawn.Position, pawn.Map, tempRange,
            TraverseParms.For(pawn));
        return region != null ? BrrrGlobals.GenNewRRJob(BrrrJobDefOf.Brrr_BrrrRecovery, region) : null;
    }
}