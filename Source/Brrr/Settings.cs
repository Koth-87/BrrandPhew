using UnityEngine;
using Verse;

namespace Brrr
{
    // Token: 0x02000018 RID: 24
    public class Settings : ModSettings
    {
        // Token: 0x0400000D RID: 13
        public static bool UseBrrr = true;

        // Token: 0x0400000E RID: 14
        public static float UnsafeBrrrSev = 20f;

        // Token: 0x0400000F RID: 15
        public static bool UsePhew = true;

        // Token: 0x04000010 RID: 16
        public static float UnsafePhewSev = 20f;

        // Token: 0x04000011 RID: 17
        public static bool UseYuk = true;

        // Token: 0x04000012 RID: 18
        public static float UnsafeYukSev = 10f;

        // Token: 0x04000013 RID: 19
        public static bool UseGasp = true;

        // Token: 0x04000014 RID: 20
        public static float UnsafeGaspSev = 10f;

        // Token: 0x04000015 RID: 21
        public static bool UseOoh = true;

        // Token: 0x04000016 RID: 22
        public static float OohSev = 20f;

        // Token: 0x04000017 RID: 23
        public static bool AllowJoy = true;

        // Token: 0x04000018 RID: 24
        public static float JoySev = 12.5f;

        // Token: 0x04000019 RID: 25
        public static bool ApplyAnimals = true;

        // Token: 0x0600004B RID: 75 RVA: 0x00004240 File Offset: 0x00002440
        public void DoWindowContents(Rect canvas)
        {
            var listing_Standard = new Listing_Standard
            {
                ColumnWidth = canvas.width
            };
            listing_Standard.Begin(canvas);
            listing_Standard.Gap();
            listing_Standard.CheckboxLabeled("Brrr.UseBrrr".Translate(), ref UseBrrr);
            listing_Standard.Gap(2f);
            listing_Standard.Label("Brrr.UnsafeBrrrSev".Translate() + "  " + UnsafeBrrrSev);
            UnsafeBrrrSev = listing_Standard.Slider(UnsafeBrrrSev, 10f, 30f);
            listing_Standard.Gap(10f);
            listing_Standard.CheckboxLabeled("Brrr.UsePhew".Translate(), ref UsePhew);
            listing_Standard.Gap(2f);
            listing_Standard.Label("Brrr.UnsafePhewSev".Translate() + "  " + UnsafePhewSev);
            UnsafePhewSev = listing_Standard.Slider(UnsafePhewSev, 10f, 30f);
            listing_Standard.Gap(10f);
            listing_Standard.CheckboxLabeled("Brrr.UseYuk".Translate(), ref UseYuk);
            listing_Standard.Gap(2f);
            listing_Standard.Label("Brrr.UnsafeYukSev".Translate() + "  " + UnsafeYukSev);
            UnsafeYukSev = listing_Standard.Slider(UnsafeYukSev, 5f, 15f);
            listing_Standard.Gap(10f);
            listing_Standard.CheckboxLabeled("Brrr.UseGasp".Translate(), ref UseGasp);
            listing_Standard.Gap(2f);
            listing_Standard.Label("Brrr.UnsafeGaspSev".Translate() + "  " + UnsafeGaspSev);
            UnsafeYukSev = listing_Standard.Slider(UnsafeYukSev, 5f, 15f);
            listing_Standard.Gap(10f);
            listing_Standard.CheckboxLabeled("Brrr.UseOoh".Translate(), ref UseOoh);
            listing_Standard.Gap(2f);
            listing_Standard.Label("Brrr.OohSev".Translate() + "  " + OohSev);
            OohSev = listing_Standard.Slider(OohSev, 10f, 30f);
            listing_Standard.Gap(10f);
            listing_Standard.CheckboxLabeled("Brrr.AllowJoy".Translate(), ref AllowJoy);
            listing_Standard.Gap(2f);
            listing_Standard.Label("Brrr.JoySev".Translate() + "  " + JoySev);
            JoySev = listing_Standard.Slider(JoySev, 5f, 25f);
            listing_Standard.Gap(10f);
            listing_Standard.CheckboxLabeled("Brrr.ApplyAnimals".Translate(), ref ApplyAnimals);
            listing_Standard.Gap(2f);
            listing_Standard.End();
        }

        // Token: 0x0600004C RID: 76 RVA: 0x0000459C File Offset: 0x0000279C
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref UseBrrr, "UseBrrr", true);
            Scribe_Values.Look(ref UnsafeBrrrSev, "UnsafeBrrrSev", 20f);
            Scribe_Values.Look(ref UsePhew, "UsePhew", true);
            Scribe_Values.Look(ref UnsafePhewSev, "UnsafePhewSev", 20f);
            Scribe_Values.Look(ref UseYuk, "UseYuk", true);
            Scribe_Values.Look(ref UnsafeYukSev, "UnsafeYukSev", 10f);
            Scribe_Values.Look(ref UseGasp, "UseGasp", true);
            Scribe_Values.Look(ref UnsafeGaspSev, "UnsafeGaspSev", 10f);
            Scribe_Values.Look(ref UseOoh, "UseOoh", true);
            Scribe_Values.Look(ref OohSev, "OohSev", 20f);
            Scribe_Values.Look(ref AllowJoy, "AllowJoy", true);
            Scribe_Values.Look(ref JoySev, "JoySev", 12.5f);
            Scribe_Values.Look(ref ApplyAnimals, "ApplyAnimals", true);
        }
    }
}