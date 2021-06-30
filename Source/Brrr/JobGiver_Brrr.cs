using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x02000012 RID: 18
    public class JobGiver_Brrr : ThinkNode_JobGiver
    {
        // Token: 0x0600003A RID: 58 RVA: 0x000033FC File Offset: 0x000015FC
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
            var FindBed = RestUtility.FindBedFor(pawn, pawn, false, true);
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
            if (region != null)
            {
                return BrrrGlobals.GenNewRRJob(BrrrJobDef.Brrr_BrrrRecovery, region);
            }

            return null;
        }

        // Token: 0x0200002E RID: 46
        [DefOf]
        public static class BrrrJobDef
        {
            // Token: 0x04000058 RID: 88
            public static JobDef Brrr_BrrrRecovery;

            // Token: 0x04000059 RID: 89
            public static JobDef Brrr_GaspRecovery;

            // Token: 0x0400005A RID: 90
            public static JobDef Brrr_PhewRecovery;

            // Token: 0x0400005B RID: 91
            public static JobDef Brrr_YukRecovery;

            // Token: 0x0400005C RID: 92
            public static JobDef Brrr_Skygaze;

            // Token: 0x0400005D RID: 93
            public static JobDef Brrr_GoForWalk;
        }
    }
}