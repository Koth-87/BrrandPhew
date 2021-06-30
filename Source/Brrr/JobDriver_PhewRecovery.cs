using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x02000003 RID: 3
    public class JobDriver_PhewRecovery : JobDriver
    {
        // Token: 0x04000002 RID: 2
        public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000007 RID: 7 RVA: 0x00002134 File Offset: 0x00000334
        public Building_Bed Bed => (Building_Bed) job.GetTarget(TargetIndex.A).Thing;

        // Token: 0x06000008 RID: 8 RVA: 0x0000215C File Offset: 0x0000035C
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

        // Token: 0x06000009 RID: 9 RVA: 0x000021BC File Offset: 0x000003BC
        public override bool CanBeginNowWhileLyingDown()
        {
            return JobInBedUtility.InBedOrRestSpotNow(pawn, job.GetTarget(TargetIndex.A));
        }

        // Token: 0x0600000A RID: 10 RVA: 0x000021D5 File Offset: 0x000003D5
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

        // Token: 0x0600000B RID: 11 RVA: 0x000021E5 File Offset: 0x000003E5
        public override string GetReport()
        {
            if (asleep)
            {
                return "Brrr.PhewRecoverSleeping".Translate();
            }

            return "Brrr.PhewRecoverResting".Translate();
        }
    }
}