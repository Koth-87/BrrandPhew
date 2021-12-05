using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr;

public class JobGiver_Gasp : ThinkNode_JobGiver
{
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

        var OxStarve = DefDatabase<HediffDef>.GetNamed("OxygenStarvation", false);
        if (OxStarve == null || !pawn.health.hediffSet.HasHediff(OxStarve))
        {
            return null;
        }

        var HedBreath = pawn.health.hediffSet.GetFirstHediffOfDef(OxStarve);
        if (HedBreath == null || HedBreath.Severity < Settings.UnsafeGaspSev / 100f)
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
            return new Job(BrrrJobDef.Brrr_GaspRecovery, BrrrBed);
        }

        var tempRange = pawn.ComfortableTemperatureRange();
        var SafeCell = BrrrGlobals.GetNearestSafeRoofedCell(pawn, pawn.Position, pawn.Map, tempRange);
        return new Job(BrrrJobDef.Brrr_GaspRecovery, SafeCell);
    }

    [DefOf]
    public static class BrrrJobDef
    {
        public static JobDef Brrr_BrrrRecovery;

        public static JobDef Brrr_PhewRecovery;

        public static JobDef Brrr_YukRecovery;

        public static JobDef Brrr_GaspRecovery;

        public static JobDef Brrr_Skygaze;

        public static JobDef Brrr_GoForWalk;
    }
}