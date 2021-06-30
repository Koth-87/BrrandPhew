using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x02000009 RID: 9
    public class ThinkNode_AnimalCanYuk : ThinkNode_Conditional
    {
        // Token: 0x06000017 RID: 23 RVA: 0x00002914 File Offset: 0x00000B14
        protected override bool Satisfied(Pawn pawn)
        {
            return Settings.UseYuk && Settings.ApplyAnimals && pawn.AnimalOrWildMan() && !pawn.IsWildMan() &&
                   pawn.Faction != null && pawn.Faction == Faction.OfPlayer && !pawn.Downed && !pawn.IsBurning() &&
                   pawn.Awake() && !pawn.InMentalState && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) &&
                   !HealthAIUtility.ShouldSeekMedicalRest(pawn) && !BrrrGlobals.BrrrAnimalIsFollowing(pawn);
        }
    }
}