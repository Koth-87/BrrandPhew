using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x0200001D RID: 29
	public class ThinkNode_CanOoh : ThinkNode_Conditional
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00004910 File Offset: 0x00002B10
		protected override bool Satisfied(Pawn pawn)
		{
			return Settings.UseOoh && (pawn.IsColonistPlayerControlled && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving)) && (!pawn.Downed && !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake()) && !HealthAIUtility.ShouldSeekMedicalRest(pawn);
		}
	}
}
