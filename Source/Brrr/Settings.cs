using System;
using UnityEngine;
using Verse;

namespace Brrr
{
	// Token: 0x02000018 RID: 24
	public class Settings : ModSettings
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00004240 File Offset: 0x00002440
		public void DoWindowContents(Rect canvas)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = canvas.width;
			listing_Standard.Begin(canvas);
			listing_Standard.Gap(12f);
			listing_Standard.CheckboxLabeled("Brrr.UseBrrr".Translate(), ref Settings.UseBrrr, null);
			listing_Standard.Gap(2f);
			listing_Standard.Label("Brrr.UnsafeBrrrSev".Translate() + "  " + Settings.UnsafeBrrrSev, -1f, null);
			Settings.UnsafeBrrrSev = listing_Standard.Slider(Settings.UnsafeBrrrSev, 10f, 30f);
			listing_Standard.Gap(10f);
			listing_Standard.CheckboxLabeled("Brrr.UsePhew".Translate(), ref Settings.UsePhew, null);
			listing_Standard.Gap(2f);
			listing_Standard.Label("Brrr.UnsafePhewSev".Translate() + "  " + Settings.UnsafePhewSev, -1f, null);
			Settings.UnsafePhewSev = listing_Standard.Slider(Settings.UnsafePhewSev, 10f, 30f);
			listing_Standard.Gap(10f);
			listing_Standard.CheckboxLabeled("Brrr.UseYuk".Translate(), ref Settings.UseYuk, null);
			listing_Standard.Gap(2f);
			listing_Standard.Label("Brrr.UnsafeYukSev".Translate() + "  " + Settings.UnsafeYukSev, -1f, null);
			Settings.UnsafeYukSev = listing_Standard.Slider(Settings.UnsafeYukSev, 5f, 15f);
			listing_Standard.Gap(10f);
			listing_Standard.CheckboxLabeled("Brrr.UseGasp".Translate(), ref Settings.UseGasp, null);
			listing_Standard.Gap(2f);
			listing_Standard.Label("Brrr.UnsafeGaspSev".Translate() + "  " + Settings.UnsafeGaspSev, -1f, null);
			Settings.UnsafeYukSev = listing_Standard.Slider(Settings.UnsafeYukSev, 5f, 15f);
			listing_Standard.Gap(10f);
			listing_Standard.CheckboxLabeled("Brrr.UseOoh".Translate(), ref Settings.UseOoh, null);
			listing_Standard.Gap(2f);
			listing_Standard.Label("Brrr.OohSev".Translate() + "  " + Settings.OohSev, -1f, null);
			Settings.OohSev = listing_Standard.Slider(Settings.OohSev, 10f, 30f);
			listing_Standard.Gap(10f);
			listing_Standard.CheckboxLabeled("Brrr.AllowJoy".Translate(), ref Settings.AllowJoy, null);
			listing_Standard.Gap(2f);
			listing_Standard.Label("Brrr.JoySev".Translate() + "  " + Settings.JoySev, -1f, null);
			Settings.JoySev = listing_Standard.Slider(Settings.JoySev, 5f, 25f);
			listing_Standard.Gap(10f);
			listing_Standard.CheckboxLabeled("Brrr.ApplyAnimals".Translate(), ref Settings.ApplyAnimals, null);
			listing_Standard.Gap(2f);
			listing_Standard.End();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000459C File Offset: 0x0000279C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref Settings.UseBrrr, "UseBrrr", true, false);
			Scribe_Values.Look<float>(ref Settings.UnsafeBrrrSev, "UnsafeBrrrSev", 20f, false);
			Scribe_Values.Look<bool>(ref Settings.UsePhew, "UsePhew", true, false);
			Scribe_Values.Look<float>(ref Settings.UnsafePhewSev, "UnsafePhewSev", 20f, false);
			Scribe_Values.Look<bool>(ref Settings.UseYuk, "UseYuk", true, false);
			Scribe_Values.Look<float>(ref Settings.UnsafeYukSev, "UnsafeYukSev", 10f, false);
			Scribe_Values.Look<bool>(ref Settings.UseGasp, "UseGasp", true, false);
			Scribe_Values.Look<float>(ref Settings.UnsafeGaspSev, "UnsafeGaspSev", 10f, false);
			Scribe_Values.Look<bool>(ref Settings.UseOoh, "UseOoh", true, false);
			Scribe_Values.Look<float>(ref Settings.OohSev, "OohSev", 20f, false);
			Scribe_Values.Look<bool>(ref Settings.AllowJoy, "AllowJoy", true, false);
			Scribe_Values.Look<float>(ref Settings.JoySev, "JoySev", 12.5f, false);
			Scribe_Values.Look<bool>(ref Settings.ApplyAnimals, "ApplyAnimals", true, false);
		}

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
	}
}
