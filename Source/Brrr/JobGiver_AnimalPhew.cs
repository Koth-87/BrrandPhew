using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x02000005 RID: 5
    public class JobGiver_AnimalPhew : ThinkNode_JobGiver
    {
        // Token: 0x0600000F RID: 15 RVA: 0x00002408 File Offset: 0x00000608
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (!Settings.UsePhew || !Settings.ApplyAnimals)
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

            if (!pawn.health.hediffSet.HasHediff(HediffDefOf.Heatstroke))
            {
                return null;
            }

            var HedHeat = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke);
            if (HedHeat == null || !(HedHeat.Severity >= Settings.UnsafePhewSev / 100f))
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
                return new Job(BrrrJobDef.Brrr_PhewRecovery, BrrrBed);
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

        // Token: 0x02000022 RID: 34
        [DefOf]
        public static class BrrrJobDef
        {
            // Token: 0x04000028 RID: 40
            public static JobDef Brrr_BrrrRecovery;

            // Token: 0x04000029 RID: 41
            public static JobDef Brrr_GaspRecovery;

            // Token: 0x0400002A RID: 42
            public static JobDef Brrr_PhewRecovery;

            // Token: 0x0400002B RID: 43
            public static JobDef Brrr_YukRecovery;
        }
    }
}