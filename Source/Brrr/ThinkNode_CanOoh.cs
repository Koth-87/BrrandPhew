using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr;

public class ThinkNode_CanOoh : ThinkNode_Conditional
{
    protected override bool Satisfied(Pawn pawn)
    {
        return Settings.UseOoh && pawn.IsColonistPlayerControlled && !pawn.IsSlave &&
               pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && !pawn.Downed && !pawn.IsBurning() &&
               !pawn.InMentalState && !pawn.Drafted && pawn.Awake() && !HealthAIUtility.ShouldSeekMedicalRest(pawn);
    }
}