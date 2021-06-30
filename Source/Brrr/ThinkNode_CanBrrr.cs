using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x0200001A RID: 26
    public class ThinkNode_CanBrrr : ThinkNode_Conditional
    {
        // Token: 0x06000051 RID: 81 RVA: 0x000047B4 File Offset: 0x000029B4
        protected override bool Satisfied(Pawn pawn)
        {
            return Settings.UseBrrr && pawn.IsColonistPlayerControlled &&
                   pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && !pawn.Downed && !pawn.IsBurning() &&
                   !pawn.InMentalState && !pawn.Drafted && pawn.Awake() && !HealthAIUtility.ShouldSeekMedicalRest(pawn);
        }
    }
}