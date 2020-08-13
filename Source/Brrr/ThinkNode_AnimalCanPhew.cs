using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x02000008 RID: 8
	public class ThinkNode_AnimalCanPhew : ThinkNode_Conditional
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002880 File Offset: 0x00000A80
		protected override bool Satisfied(Pawn pawn)
		{
			return Settings.UsePhew && Settings.ApplyAnimals && pawn.AnimalOrWildMan() && !pawn.IsWildMan() && pawn.Faction != null && pawn.Faction == Faction.OfPlayer && !pawn.Downed && !pawn.IsBurning() && pawn.Awake() && !pawn.InMentalState && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && !HealthAIUtility.ShouldSeekMedicalRest(pawn) && !BrrrGlobals.BrrrAnimalIsFollowing(pawn);
		}
	}
}
