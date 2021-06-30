using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x0200000E RID: 14
    public class JobDriver_OohGoForWalk : JobDriver
    {
        // Token: 0x0600002B RID: 43 RVA: 0x0000301A File Offset: 0x0000121A
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        // Token: 0x0600002C RID: 44 RVA: 0x0000301D File Offset: 0x0000121D
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
}