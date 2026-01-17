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

        if (pawn.CurJobDef == BrrrJobDefOf.Brrr_BrrrRecovery || pawn.CurJobDef == BrrrJobDefOf.Brrr_GaspRecovery ||
            pawn.CurJobDef == BrrrJobDefOf.Brrr_Skygaze || pawn.CurJobDef == BrrrJobDefOf.Brrr_GoForWalk ||
            pawn.CurJobDef == BrrrJobDefOf.Brrr_PhewRecovery || pawn.CurJobDef == BrrrJobDefOf.Brrr_YukRecovery
            || pawn.CurJobDef == BrrrJobDefOf.Brrr_RedRecovery)
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