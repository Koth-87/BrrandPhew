using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Brrr;

public class JobGiver_Ooh : ThinkNode_JobGiver
{
    private static readonly int StartRadialIndex = GenRadial.NumCellsInRadius(14f);

    private static readonly int EndRadialIndex = GenRadial.NumCellsInRadius(2f);

    private static readonly int RadialIndexStride = 3;

    protected override Job TryGiveJob(Pawn pawn)
    {
        if (!Settings.UseOoh || !pawn.IsColonistPlayerControlled)
        {
            return null;
        }

        var needs = pawn.needs;
        if (needs?.outdoors == null)
        {
            return null;
        }

        if (pawn.InMentalState)
        {
            return null;
        }

        if (pawn.Map == null)
        {
            return null;
        }

        var needs2 = pawn.needs;
        if (needs2?.food != null && pawn.needs.food.CurLevelPercentage < pawn.needs.food.PercentageThreshHungry)
        {
            return null;
        }

        var needs3 = pawn.needs;
        if (needs3?.joy != null && pawn.needs.joy.CurLevelPercentage < Settings.JoySev / 100f && Settings.AllowJoy)
        {
            return null;
        }

        if (RestUtility.DisturbancePreventsLyingDown(pawn))
        {
            return null;
        }

        if (pawn.CurJobDef == BrrrJobDefOf.Brrr_BrrrRecovery || pawn.CurJobDef == BrrrJobDefOf.Brrr_GaspRecovery ||
            pawn.CurJobDef == BrrrJobDefOf.Brrr_Skygaze || pawn.CurJobDef == BrrrJobDefOf.Brrr_GoForWalk ||
            pawn.CurJobDef == BrrrJobDefOf.Brrr_PhewRecovery || pawn.CurJobDef == BrrrJobDefOf.Brrr_YukRecovery)
        {
            return null;
        }

        if (pawn.needs.outdoors.CurLevelPercentage > Settings.OohSev / 100f)
        {
            return null;
        }

        if (Settings.UseBrrr)
        {
            var health = pawn.health;
            var HedHypo = health?.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia);
            if (HedHypo != null && HedHypo.Severity >= Settings.UnsafeBrrrSev / 100f)
            {
                return null;
            }
        }

        if (Settings.UsePhew)
        {
            var health2 = pawn.health;
            var HedHeat = health2?.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke);
            if (HedHeat != null && HedHeat.Severity >= Settings.UnsafePhewSev / 100f)
            {
                return null;
            }
        }

        if (Settings.UseYuk)
        {
            var health3 = pawn.health;
            var HedYuk = health3?.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup);
            if (HedYuk != null && HedYuk.Severity >= Settings.UnsafeYukSev / 100f)
            {
                return null;
            }
        }

        if (!TryFindOohCell(pawn.Position, pawn, out var OohStarGazeDest))
        {
            return null;
        }

        if (!TryFindOohWalkPath(pawn, OohStarGazeDest, out var OohWalkDests))
        {
            return null;
        }

        Job OohJob;
        if (BrrrGlobals.GenRnd100() <= 45)
        {
            OohJob = new Job(BrrrJobDefOf.Brrr_GoForWalk, OohWalkDests[0])
            {
                targetQueueA = new List<LocalTargetInfo>()
            };
            for (var i = 1; i < OohWalkDests.Count; i++)
            {
                OohJob.targetQueueA.Add(OohWalkDests[i]);
            }

            OohJob.locomotionUrgency = LocomotionUrgency.Walk;
        }
        else
        {
            OohJob = new Job(BrrrJobDefOf.Brrr_Skygaze, OohStarGazeDest);
        }

        return OohJob;
    }

    public static bool TryFindOohCell(IntVec3 root, Pawn searcher, out IntVec3 result)
    {
        var traverseParms = TraverseParms.For(searcher);
        if (CellFinder.TryFindClosestRegionWith(root.GetRegion(searcher.Map), traverseParms, validator, 300,
                out var result2))
        {
            return CellFinder.RandomRegionNear(result2, 14, traverseParms, validator, searcher)
                .TryFindRandomCellInRegion(cellValidator, out result);
        }

        result = root;
        return false;

        bool cellValidator(IntVec3 c)
        {
            return !c.Roofed(searcher.Map) && !c.GetTerrain(searcher.Map).avoidWander;
        }

        bool validator(Region r)
        {
            return r.Room.PsychologicallyOutdoors && r.TryFindRandomCellInRegion(cellValidator, out var unused);
        }
    }

    public static bool TryFindOohWalkPath(Pawn pawn, IntVec3 root, out List<IntVec3> result)
    {
        var list = new List<IntVec3>
        {
            root
        };
        var intVec = root;
        for (var i = 0; i < 8; i++)
        {
            var intVec2 = IntVec3.Invalid;
            var num = -1f;
            for (var num2 = StartRadialIndex; num2 > EndRadialIndex; num2 -= RadialIndexStride)
            {
                var intVec3 = intVec + GenRadial.RadialPattern[num2];
                if (!intVec3.InBounds(pawn.Map) || !intVec3.Standable(pawn.Map) ||
                    intVec3.GetTerrain(pawn.Map).avoidWander || !GenSight.LineOfSight(intVec, intVec3, pawn.Map) ||
                    intVec3.Roofed(pawn.Map) || PawnUtility.KnownDangerAt(intVec3, pawn.Map, pawn))
                {
                    continue;
                }

                var num3 = 10000f;
                foreach (var vec3 in list)
                {
                    num3 += (vec3 - intVec3).LengthManhattan;
                }

                var num4 = (float)(intVec3 - root).LengthManhattan;
                if (num4 > 40f)
                {
                    num3 *= Mathf.InverseLerp(70f, 40f, num4);
                }

                if (list.Count >= 2)
                {
                    var angleFlat = (list[list.Count - 1] - list[list.Count - 2]).AngleFlat;
                    var angleFlat2 = (intVec3 - intVec).AngleFlat;
                    float num5;
                    if (angleFlat2 > angleFlat)
                    {
                        num5 = angleFlat2 - angleFlat;
                    }
                    else
                    {
                        angleFlat -= 360f;
                        num5 = angleFlat2 - angleFlat;
                    }

                    if (num5 > 110f)
                    {
                        num3 *= 0.01f;
                    }
                }

                if (list.Count >= 4 && (intVec - root).LengthManhattan < (intVec3 - root).LengthManhattan)
                {
                    num3 *= 1E-05f;
                }

                if (!(num3 > num))
                {
                    continue;
                }

                intVec2 = intVec3;
                num = num3;
            }

            if (num < 0f)
            {
                result = null;
                return false;
            }

            list.Add(intVec2);
            intVec = intVec2;
        }

        list.Add(root);
        result = list;
        return true;
    }
}