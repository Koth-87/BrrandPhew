using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr;

public class JobGiver_Brrr : ThinkNode_JobGiver
{
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

        if (pawn.Map == null)
        {
            return null;
        }

        var needs = pawn.needs;
        if (needs?.food != null && pawn.needs.food.CurLevelPercentage < pawn.needs.food.PercentageThreshHungry)
        {
            return null;
        }

        var needs2 = pawn.needs;
        if (needs2?.joy != null && pawn.needs.joy.CurLevelPercentage < Settings.JoySev / 100f && Settings.AllowJoy)
        {
            return null;
        }

        if (RestUtility.DisturbancePreventsLyingDown(pawn))
        {
            return null;
        }

        if (pawn.CurJobDef == BrrrJobDef.Brrr_BrrrRecovery || pawn.CurJobDef == BrrrJobDef.Brrr_GaspRecovery ||
            pawn.CurJobDef == BrrrJobDef.Brrr_Skygaze || pawn.CurJobDef == BrrrJobDef.Brrr_GoForWalk ||
            pawn.CurJobDef == BrrrJobDef.Brrr_PhewRecovery || pawn.CurJobDef == BrrrJobDef.Brrr_YukRecovery)
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
        Thing FindBed = Settings.AllowUnsafeAreas
            ? RestUtility.FindBedFor(pawn)
            : RestUtility.FindBedFor(pawn, pawn, false, true);
        if (FindBed != null && !FindBed.GetRoom().PsychologicallyOutdoors &&
            pawn.ComfortableTemperatureRange().Includes(FindBed.GetRoom().Temperature))
        {
            BrrrBed = FindBed;
        }

        if (BrrrBed != null)
        {
            return new Job(BrrrJobDef.Brrr_BrrrRecovery, BrrrBed);
        }

        var tempRange = pawn.ComfortableTemperatureRange();
        var region = BrrrGlobals.BrrrClosestRegionWithinTemperatureRange(pawn.Position, pawn.Map, tempRange,
            TraverseParms.For(pawn));
        return region != null ? BrrrGlobals.GenNewRRJob(BrrrJobDef.Brrr_BrrrRecovery, region) : null;
    }

    [DefOf]
    public static class BrrrJobDef
    {
        public static JobDef Brrr_BrrrRecovery;

        public static JobDef Brrr_GaspRecovery;

        public static JobDef Brrr_PhewRecovery;

        public static JobDef Brrr_YukRecovery;

        public static JobDef Brrr_Skygaze;

        public static JobDef Brrr_GoForWalk;
    }
}