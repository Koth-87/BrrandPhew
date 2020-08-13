using System;
using System.Reflection;
using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace Brrr
{
	// Token: 0x02000017 RID: 23
	[StaticConstructorOnStartup]
	internal static class MultiplayerSupport
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00004130 File Offset: 0x00002330
		static MultiplayerSupport()
		{
			if (!MP.enabled)
			{
				return;
			}
			MethodInfo[] array = new MethodInfo[]
			{
				AccessTools.Method(typeof(BrrrGlobals), "GetNearestSafeRoofedCell", null, null),
				AccessTools.Method(typeof(BrrrGlobals), "GenRnd100", null, null),
				AccessTools.Method(typeof(BrrrGlobals), "GenNewRRJob", null, null),
				AccessTools.Method(typeof(JobGiver_Ooh), "TryFindOohCell", null, null),
				AccessTools.Method(typeof(Toils_BrrrLayDown), "BrrrLayDown", null, null)
			};
			for (int i = 0; i < array.Length; i++)
			{
				MultiplayerSupport.FixRNG(array[i]);
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000041EE File Offset: 0x000023EE
		private static void FixRNG(MethodInfo method)
		{
			MultiplayerSupport.harmony.Patch(method, new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPre", null), new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPos", null), null, null);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00004228 File Offset: 0x00002428
		private static void FixRNGPre()
		{
			Rand.PushState(Find.TickManager.TicksAbs);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004239 File Offset: 0x00002439
		private static void FixRNGPos()
		{
			Rand.PopState();
		}

		// Token: 0x0400000C RID: 12
		private static Harmony harmony = new Harmony("rimworld.pelador.brr.multiplayersupport");
	}
}
