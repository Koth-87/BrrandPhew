using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x0200001E RID: 30
	public class ThinkNode_CanYuk : ThinkNode_Conditional
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00004984 File Offset: 0x00002B84
		protected override bool Satisfied(Pawn pawn)
		{
			return Settings.UseYuk && (pawn.IsColonistPlayerControlled && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving)) && (!pawn.Downed && !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake()) && !HealthAIUtility.ShouldSeekMedicalRest(pawn);
		}
	}
}
