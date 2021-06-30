using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x0200000D RID: 13
    public class JobDriver_GaspRecovery : JobDriver
    {
        // Token: 0x04000007 RID: 7
        public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000025 RID: 37 RVA: 0x00002F38 File Offset: 0x00001138
        public Building_Bed Bed => (Building_Bed) job.GetTarget(TargetIndex.A).Thing;

        // Token: 0x06000026 RID: 38 RVA: 0x00002F60 File Offset: 0x00001160
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (!job.GetTarget(TargetIndex.A).HasThing)
            {
                return true;
            }

            var localPawn = pawn;
            LocalTargetInfo target = Bed;
            var localJob = job;
            var sleepingSlotsCount = Bed.SleepingSlotsCount;
            var stackCount = 0;
            if (!localPawn.Reserve(target, localJob, sleepingSlotsCount, stackCount, null, errorOnFailed))
            {
                return false;
            }

            return true;
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00002FC0 File Offset: 0x000011C0
        public override bool CanBeginNowWhileLyingDown()
        {
            return JobInBedUtility.InBedOrRestSpotNow(pawn, job.GetTarget(TargetIndex.A));
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00002FD9 File Offset: 0x000011D9
        protected override IEnumerable<Toil> MakeNewToils()
        {
            var hasBed = job.GetTarget(TargetIndex.A).HasThing;
            if (hasBed)
            {
                yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A);
                yield return Toils_Bed.GotoBed(TargetIndex.A);
            }
            else
            {
                yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
            }

            yield return Toils_BrrrLayDown.BrrrLayDown(TargetIndex.A, hasBed);
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00002FE9 File Offset: 0x000011E9
        public override string GetReport()
        {
            if (asleep)
            {
                return "Brrr.GaspRecoverSleeping".Translate();
            }

            return "Brrr.GaspRecoverResting".Translate();
        }
    }
}