using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x02000007 RID: 7
    public class ThinkNode_AnimalCanBrrr : ThinkNode_Conditional
    {
        // Token: 0x06000013 RID: 19 RVA: 0x000027EC File Offset: 0x000009EC
        protected override bool Satisfied(Pawn pawn)
        {
            return Settings.UseBrrr && Settings.ApplyAnimals && pawn.AnimalOrWildMan() && !pawn.IsWildMan() &&
                   pawn.Faction != null && pawn.Faction == Faction.OfPlayer && !pawn.Downed && !pawn.IsBurning() &&
                   pawn.Awake() && !pawn.InMentalState && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) &&
                   !HealthAIUtility.ShouldSeekMedicalRest(pawn) && !BrrrGlobals.BrrrAnimalIsFollowing(pawn);
        }
    }
}