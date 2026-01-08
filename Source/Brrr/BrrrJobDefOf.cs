using RimWorld;
using Verse;

namespace Brrr;

[DefOf]
public static class BrrrJobDefOf
{
    public static JobDef Brrr_RedRecovery;
    public static JobDef Brrr_BrrrRecovery;

    public static JobDef Brrr_GaspRecovery;

    public static JobDef Brrr_PhewRecovery;

    public static JobDef Brrr_YukRecovery;

    public static JobDef Brrr_Skygaze;

    public static JobDef Brrr_GoForWalk;

    static BrrrJobDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(BrrrJobDefOf));
    }
}