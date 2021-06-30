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
        // Token: 0x0400000C RID: 12
        private static readonly Harmony harmony = new Harmony("rimworld.pelador.brr.multiplayersupport");

        // Token: 0x06000047 RID: 71 RVA: 0x00004130 File Offset: 0x00002330
        static MultiplayerSupport()
        {
            if (!MP.enabled)
            {
                return;
            }

            var array = new[]
            {
                AccessTools.Method(typeof(BrrrGlobals), "GetNearestSafeRoofedCell"),
                AccessTools.Method(typeof(BrrrGlobals), "GenRnd100"),
                AccessTools.Method(typeof(BrrrGlobals), "GenNewRRJob"),
                AccessTools.Method(typeof(JobGiver_Ooh), "TryFindOohCell"),
                AccessTools.Method(typeof(Toils_BrrrLayDown), "BrrrLayDown")
            };
            foreach (var methodInfo in array)
            {
                FixRNG(methodInfo);
            }
        }

        // Token: 0x06000048 RID: 72 RVA: 0x000041EE File Offset: 0x000023EE
        private static void FixRNG(MethodInfo method)
        {
            harmony.Patch(method, new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPre"),
                new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPos"));
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
    }
}