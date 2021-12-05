using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Brrr;

public class JobDriver_OohGoForWalk : JobDriver
{
    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return true;
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        var goToil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
        goToil.tickAction = delegate
        {
            if (Find.TickManager.TicksGame > startTick + job.def.joyDuration)
            {
                EndJobWith(JobCondition.Succeeded);
                return;
            }

            BrrrGlobals.JoyTickOohCheckEnd(pawn);
        };
        goToil.AddFailCondition(delegate
        {
            var actor = goToil.actor;
            var needs = actor.needs;
            return needs?.food != null &&
                   actor.needs.food.CurLevelPercentage < actor.needs.food.PercentageThreshHungry;
        });
        yield return goToil;
        yield return new Toil
        {
            initAction = delegate
            {
                if (job.targetQueueA.Count <= 0)
                {
                    return;
                }

                var targetA = job.targetQueueA[0];
                job.targetQueueA.RemoveAt(0);
                job.targetA = targetA;
                JumpToToil(goToil);
            }
        };
    }
}