using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
    // Token: 0x0200001B RID: 27
    public class ThinkNode_CanGasp : ThinkNode_Conditional
    {
        // Token: 0x06000053 RID: 83 RVA: 0x00004828 File Offset: 0x00002A28
        protected override bool Satisfied(Pawn pawn)
        {
            return Settings.UseGasp && pawn.IsColonistPlayerControlled &&
                   pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && !pawn.Downed && !pawn.IsBurning() &&
                   !pawn.InMentalState && !pawn.Drafted && pawn.Awake() && !HealthAIUtility.ShouldSeekMedicalRest(pawn);
        }
    }
}