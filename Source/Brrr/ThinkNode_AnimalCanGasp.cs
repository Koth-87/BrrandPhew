using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x02000019 RID: 25
	public class ThinkNode_AnimalCanGasp : ThinkNode_Conditional
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00004720 File Offset: 0x00002920
		protected override bool Satisfied(Pawn pawn)
		{
			return Settings.UseGasp && Settings.ApplyAnimals && pawn.AnimalOrWildMan() && !pawn.IsWildMan() && pawn.Faction != null && pawn.Faction == Faction.OfPlayer && !pawn.Downed && !pawn.IsBurning() && pawn.Awake() && !pawn.InMentalState && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && !HealthAIUtility.ShouldSeekMedicalRest(pawn) && !BrrrGlobals.BrrrAnimalIsFollowing(pawn);
		}
	}
}
