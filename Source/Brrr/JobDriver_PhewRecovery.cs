using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr;

public class JobDriver_PhewRecovery : JobDriver
{
    public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;
    public const TargetIndex SwimSpotIndex = TargetIndex.B;

    private Building_Bed Bed => job.GetTarget(BedOrRestSpotIndex).Thing as Building_Bed;

    private bool IsSwimming => job.GetTarget(SwimSpotIndex).IsValid;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        if (!job.GetTarget(BedOrRestSpotIndex).HasThing && !IsSwimming)
        {
            return true;
        }

        if (IsSwimming)
        {
            return pawn.Reserve(job.GetTarget(SwimSpotIndex), job, errorOnFailed: errorOnFailed);
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
        return !IsSwimming && JobInBedUtility.InBedOrRestSpotNow(pawn, job.GetTarget(BedOrRestSpotIndex));
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        if (IsSwimming)
        {
            yield return Toils_Goto.GotoCell(SwimSpotIndex, PathEndMode.OnCell);
            var swimToil = Toils_Goto.GotoCell(SwimSpotIndex, PathEndMode.OnCell);
            swimToil.tickIntervalAction = _ =>
            {
                CheckForSwimmingPose();
                pawn.mindState.lastSwamTick = GenTicks.TicksGame;
            };
            yield return swimToil;
        }
        else
        {
            var hasBed = job.GetTarget(BedOrRestSpotIndex).HasThing;
            if (hasBed)
            {
                yield return Toils_Bed.ClaimBedIfNonMedical(BedOrRestSpotIndex);
                yield return Toils_Bed.GotoBed(BedOrRestSpotIndex);
            }
            else
            {
                yield return Toils_Goto.GotoCell(BedOrRestSpotIndex, PathEndMode.OnCell);
            }

            yield return Toils_BrrrLayDown.BrrrLayDown(BedOrRestSpotIndex, hasBed);
        }
    }

    public override string GetReport()
    {
        if (IsSwimming)
        {
            return "Brrr.PhewRecoverSwimming".Translate();
        }

        return asleep ? "Brrr.PhewRecoverSleeping".Translate() : "Brrr.PhewRecoverResting".Translate();
    }

    private void CheckForSwimmingPose()
    {
        job.swimming = pawn.Position.GetTerrain(pawn.Map).IsWater;
    }
}