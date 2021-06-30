using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x02000002 RID: 2
    public class JobDriver_BrrrRecovery : JobDriver
    {
        // Token: 0x04000001 RID: 1
        public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public Building_Bed Bed => (Building_Bed) job.GetTarget(TargetIndex.A).Thing;

        // Token: 0x06000002 RID: 2 RVA: 0x00002078 File Offset: 0x00000278
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

        // Token: 0x06000003 RID: 3 RVA: 0x000020D8 File Offset: 0x000002D8
        public override bool CanBeginNowWhileLyingDown()
        {
            return JobInBedUtility.InBedOrRestSpotNow(pawn, job.GetTarget(TargetIndex.A));
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000020F1 File Offset: 0x000002F1
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

        // Token: 0x06000005 RID: 5 RVA: 0x00002101 File Offset: 0x00000301
        public override string GetReport()
        {
            if (asleep)
            {
                return "Brrr.BrrrRecoverSleeping".Translate();
            }

            return "Brrr.BrrrRecoverResting".Translate();
        }
    }
}