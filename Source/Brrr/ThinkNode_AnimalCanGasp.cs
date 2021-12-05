using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr;

public class ThinkNode_AnimalCanGasp : ThinkNode_Conditional
{
    protected override bool Satisfied(Pawn pawn)
    {
        return Settings.UseGasp && Settings.ApplyAnimals && pawn.AnimalOrWildMan() && !pawn.IsWildMan() &&
               pawn.Faction != null && pawn.Faction == Faction.OfPlayer && !pawn.Downed && !pawn.IsBurning() &&
               pawn.Awake() && !pawn.InMentalState && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) &&
               !HealthAIUtility.ShouldSeekMedicalRest(pawn) && !BrrrGlobals.BrrrAnimalIsFollowing(pawn);
    }
}