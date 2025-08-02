using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr;

public class JobDriver_BrrrRecovery : JobDriver
{
    public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;

    private Building_Bed Bed => (Building_Bed)job.GetTarget(TargetIndex.A).Thing;

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
        return localPawn.Reserve(target, localJob, sleepingSlotsCount, stackCount, null, errorOnFailed);
    }

    public override bool CanBeginNowWhileLyingDown()
    {
        return JobInBedUtility.InBedOrRestSpotNow(pawn, job.GetTarget(TargetIndex.A));
    }

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

    public override string GetReport()
    {
        return asleep ? "Brrr.BrrrRecoverSleeping".Translate() : "Brrr.BrrrRecoverResting".Translate();
    }
}