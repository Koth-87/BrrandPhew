using UnityEngine;
using Verse;

namespace Brrr
{
    // Token: 0x0200000C RID: 12
    public class Controller : Mod
    {
        // Token: 0x04000006 RID: 6
        public static Settings Settings;

        // Token: 0x06000024 RID: 36 RVA: 0x00002F23 File Offset: 0x00001123
        public Controller(ModContentPack content) : base(content)
        {
            Settings = GetSettings<Settings>();
        }

        // Token: 0x06000022 RID: 34 RVA: 0x00002F05 File Offset: 0x00001105
        public override string SettingsCategory()
        {
            return "Brrr.Name".Translate();
        }

        // Token: 0x06000023 RID: 35 RVA: 0x00002F16 File Offset: 0x00001116
        public override void DoSettingsWindowContents(Rect canvas)
        {
            Settings.DoWindowContents(canvas);
        }
    }
}