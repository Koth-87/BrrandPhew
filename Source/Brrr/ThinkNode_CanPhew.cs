using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr;

public class ThinkNode_CanPhew : ThinkNode_Conditional
{
    protected override bool Satisfied(Pawn pawn)
    {
        return Settings.UsePhew && pawn.IsColonistPlayerControlled &&
               pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && !pawn.Downed && !pawn.IsBurning() &&
               !pawn.InMentalState && !pawn.Drafted && pawn.Awake() && !HealthAIUtility.ShouldSeekMedicalRest(pawn);
    }
}