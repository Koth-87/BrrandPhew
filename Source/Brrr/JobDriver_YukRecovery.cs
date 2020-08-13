using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Brrr
{
	// Token: 0x02000010 RID: 16
	public class JobDriver_YukRecovery : JobDriver
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00003118 File Offset: 0x00001318
		public Building_Bed Bed
		{
			get
			{
				return (Building_Bed)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003140 File Offset: 0x00001340
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.job.GetTarget(TargetIndex.A).HasThing)
			{
				Pawn pawn = this.pawn;
				LocalTargetInfo target = this.Bed;
				Job job = this.job;
				int sleepingSlotsCount = this.Bed.SleepingSlotsCount;
				int stackCount = 0;
				if (!pawn.Reserve(target, job, sleepingSlotsCount, stackCount, null, errorOnFailed))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000031A0 File Offset: 0x000013A0
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000031B9 File Offset: 0x000013B9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			bool hasBed = this.job.GetTarget(TargetIndex.A).HasThing;
			if (hasBed)
			{
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.A);
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			}
			yield return Toils_BrrrLayDown.BrrrLayDown(TargetIndex.A, hasBed, false, true, true);
			yield break;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000031C9 File Offset: 0x000013C9
		public override string GetReport()
		{
			if (this.asleep)
			{
				return "Brrr.YukRecoverSleeping".Translate();
			}
			return "Brrr.YukRecoverResting".Translate();
		}

		// Token: 0x04000008 RID: 8
		public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;
	}
}
