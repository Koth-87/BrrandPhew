using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr;

public class JobGiver_Phew : ThinkNode_JobGiver
{
    protected override Job TryGiveJob(Pawn pawn)
    {
        if (!Settings.UsePhew || !pawn.IsColonistPlayerControlled)
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

        if (!pawn.health.hediffSet.HasHediff(HediffDefOf.Heatstroke))
        {
            return null;
        }

        var HedHeat = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke);
        if (HedHeat == null || !(HedHeat.Severity >= Settings.UnsafePhewSev / 100f))
        {
            return null;
        }

        // Check for swimming option (Odyssey DLC)
        if (pawn.Faction == Faction.OfPlayer &&
            ModLister.GetActiveModWithIdentifier("ludeon.rimworld.royalty", true) != null)
        {
            var waterCell = FindNearestWaterCell(pawn);
            if (waterCell.IsValid)
            {
                var job = new Job(BrrrJobDefOf.Brrr_PhewRecovery, IntVec3.Invalid, waterCell);
                return job;
            }
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
            return new Job(BrrrJobDefOf.Brrr_PhewRecovery, BrrrBed);
        }

        var tempRange = pawn.ComfortableTemperatureRange();
        var region = BrrrGlobals.BrrrClosestRegionWithinTemperatureRange(pawn.Position, pawn.Map, tempRange,
            TraverseParms.For(pawn));
        return region != null ? new Job(BrrrJobDefOf.Brrr_PhewRecovery, region.RandomCell) : null;
    }

    private static IntVec3 FindNearestWaterCell(Pawn pawn)
    {
        var map = pawn.Map;
        var maxDistance = 30; // Search radius for water

        for (var i = 0; i < maxDistance; i++)
        {
            var cells = GenRadial.RadialCellsAround(pawn.Position, i, true);
            foreach (var cell in cells)
            {
                if (!cell.InBounds(map))
                {
                    continue;
                }

                if (cell.GetTerrain(map).HasTag("Water") && pawn.CanReach(cell, PathEndMode.OnCell, Danger.None))
                {
                    return cell;
                }
            }
        }

        return IntVec3.Invalid;
    }
}