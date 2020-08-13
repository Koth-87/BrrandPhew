using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x0200001C RID: 28
	public class ThinkNode_CanPhew : ThinkNode_Conditional
	{
		// Token: 0x06000055 RID: 85 RVA: 0x0000489C File Offset: 0x00002A9C
		protected override bool Satisfied(Pawn pawn)
		{
			return Settings.UsePhew && (pawn.IsColonistPlayerControlled && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving)) && (!pawn.Downed && !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake()) && !HealthAIUtility.ShouldSeekMedicalRest(pawn);
		}
	}
}
