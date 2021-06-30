using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x0200000F RID: 15
    public class JobDriver_OohSkygaze : JobDriver
    {
        // Token: 0x0600002E RID: 46 RVA: 0x00003035 File Offset: 0x00001235
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        // Token: 0x0600002F RID: 47 RVA: 0x00003038 File Offset: 0x00001238
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
            var gaze = new Toil
            {
                initAction = delegate { pawn.jobs.posture = PawnPosture.LayingOnGroundFaceUp; },
                tickAction = delegate { BrrrGlobals.JoyTickOohCheckEnd(pawn); },
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = job.def.joyDuration
            };
            gaze.FailOn(() => pawn.Position.Roofed(pawn.Map));
            gaze.AddFailCondition(delegate
            {
                var actor = gaze.actor;
                var needs = actor.needs;
                if (needs?.food == null)
                {
                    return false;
                }

                var needs2 = actor.needs;
                var num = needs2 != null ? new float?(needs2.food.CurLevelPercentage) : null;
                var percentageThreshHungry = actor.needs.food.PercentageThreshHungry;
                if ((num.GetValueOrDefault() < percentageThreshHungry) & (num != null))
                {
                    return true;
                }

                return false;
            });
            yield return gaze;
        }

        // Token: 0x06000030 RID: 48 RVA: 0x00003048 File Offset: 0x00001248
        public override string GetReport()
        {
            if (Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
            {
                return "Brrr.OohEclipse".Translate();
            }

            if (Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Aurora))
            {
                return "Brrr.OohAurora".Translate();
            }

            var num = GenCelestial.CurCelestialSunGlow(Map);
            if (num < 0.1f)
            {
                return "Brrr.OohStarGazing".Translate();
            }

            if (num >= 0.65f)
            {
                return "Brrr.OohClouds".Translate();
            }

            if (GenLocalDate.DayPercent(pawn) < 0.5f)
            {
                return "Brrr.OohSunrise".Translate();
            }

            return "Brrr.OohSunset".Translate();
        }
    }
}