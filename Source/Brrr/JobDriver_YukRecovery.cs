using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x02000010 RID: 16
    public class JobDriver_YukRecovery : JobDriver
    {
        // Token: 0x04000008 RID: 8
        public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000032 RID: 50 RVA: 0x00003118 File Offset: 0x00001318
        public Building_Bed Bed => (Building_Bed) job.GetTarget(TargetIndex.A).Thing;

        // Token: 0x06000033 RID: 51 RVA: 0x00003140 File Offset: 0x00001340
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

        // Token: 0x06000034 RID: 52 RVA: 0x000031A0 File Offset: 0x000013A0
        public override bool CanBeginNowWhileLyingDown()
        {
            return JobInBedUtility.InBedOrRestSpotNow(pawn, job.GetTarget(TargetIndex.A));
        }

        // Token: 0x06000035 RID: 53 RVA: 0x000031B9 File Offset: 0x000013B9
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

        // Token: 0x06000036 RID: 54 RVA: 0x000031C9 File Offset: 0x000013C9
        public override string GetReport()
        {
            if (asleep)
            {
                return "Brrr.YukRecoverSleeping".Translate();
            }

            return "Brrr.YukRecoverResting".Translate();
        }
    }
}