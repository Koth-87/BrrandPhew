using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr;

public class JobGiver_Red : ThinkNode_JobGiver
{
    protected override Job TryGiveJob(Pawn pawn)
    {
        if (!Settings.UseRed || !pawn.IsColonistPlayerControlled)
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

        if (!pawn.health.hediffSet.HasHediff(HediffDefOf.BloodRage))
        {
            return null;
        }

        var HedTox = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodRage);
        if (HedTox == null || HedTox.Severity < Settings.UnsafeRedSev / 100f)
        {
            return null;
        }

        Thing RedBed = null;
        Thing FindBed = Settings.AllowUnsafeAreas
            ? RestUtility.FindBedFor(pawn)
            : RestUtility.FindBedFor(pawn, pawn, false, true);

        if (FindBed != null && FindBed.Position.Roofed(pawn.Map) && !FindBed.Position.IsPolluted(pawn.Map) &&
            pawn.ComfortableTemperatureRange().Includes(FindBed.GetRoom().Temperature))
        {
            RedBed = FindBed;
        }

        if (RedBed != null)
        {
            return new Job(BrrrJobDefOf.Brrr_RedRecovery, RedBed);
        }

        var tempRange = pawn.ComfortableTemperatureRange();
        var SafeCell = BrrrGlobals.GetNearestSafeRoofedCell(pawn, pawn.Position, pawn.Map, tempRange);
        return new Job(BrrrJobDefOf.Brrr_RedRecovery, SafeCell);
    }
}