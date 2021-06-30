using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x02000016 RID: 22
    public class JobGiver_Yuk : ThinkNode_JobGiver
    {
        // Token: 0x06000045 RID: 69 RVA: 0x00003F48 File Offset: 0x00002148
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (!Settings.UseYuk || !pawn.IsColonistPlayerControlled)
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

        // Token: 0x02000033 RID: 51
        [DefOf]
        public static class BrrrJobDef
        {
            // Token: 0x04000073 RID: 115
            public static JobDef Brrr_BrrrRecovery;

            // Token: 0x04000074 RID: 116
            public static JobDef Brrr_GaspRecovery;

            // Token: 0x04000075 RID: 117
            public static JobDef Brrr_PhewRecovery;

            // Token: 0x04000076 RID: 118
            public static JobDef Brrr_YukRecovery;

            // Token: 0x04000077 RID: 119
            public static JobDef Brrr_Skygaze;

            // Token: 0x04000078 RID: 120
            public static JobDef Brrr_GoForWalk;
        }
    }
}