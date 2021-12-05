using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr;

public class JobGiver_AnimalYuk : ThinkNode_JobGiver
{
    protected override Job TryGiveJob(Pawn pawn)
    {
        if (!Settings.UseYuk || !Settings.ApplyAnimals)
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

        if (pawn.CurJobDef == BrrrJobDef.Brrr_BrrrRecovery || pawn.CurJobDef == BrrrJobDef.Brrr_GaspRecovery ||
            pawn.CurJobDef == BrrrJobDef.Brrr_PhewRecovery || pawn.CurJobDef == BrrrJobDef.Brrr_YukRecovery)
        {
            return null;
        }

        if (!pawn.health.hediffSet.HasHediff(HediffDefOf.ToxicBuildup))
        {
            return null;
        }

        var HedTox = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup);
        if (HedTox == null || HedTox.Severity < Settings.UnsafeYukSev / 100f)
        {
            return null;
        }

        Thing BrrrBed = null;
        var FindBed = RestUtility.FindBedFor(pawn, pawn, false, true);
        if (FindBed != null && FindBed.Position.Roofed(pawn.Map) &&
            pawn.ComfortableTemperatureRange().Includes(FindBed.GetRoom().Temperature))
        {
            BrrrBed = FindBed;
        }

        if (BrrrBed != null)
        {
            return new Job(BrrrJobDef.Brrr_YukRecovery, BrrrBed);
        }

        var tempRange = pawn.ComfortableTemperatureRange();
        var SafeCell = BrrrGlobals.GetNearestSafeRoofedCell(pawn, pawn.Position, pawn.Map, tempRange);
        return new Job(BrrrJobDef.Brrr_YukRecovery, SafeCell);
    }

    [DefOf]
    public static class BrrrJobDef
    {
        public static JobDef Brrr_BrrrRecovery;

        public static JobDef Brrr_PhewRecovery;

        public static JobDef Brrr_YukRecovery;

        public static JobDef Brrr_GaspRecovery;
    }
}