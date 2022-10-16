using UnityEngine;
using Verse;

namespace Brrr;

public class Settings : ModSettings
{
    public static bool UseBrrr = true;

    public static float UnsafeBrrrSev = 20f;

    public static bool UsePhew = true;

    public static float UnsafePhewSev = 20f;

    public static bool UseYuk = true;

    public static float UnsafeYukSev = 10f;

    public static bool UseGasp = true;

    public static float UnsafeGaspSev = 10f;

    public static bool UseOoh = true;

    public static float OohSev = 20f;

    public static bool AllowJoy = true;

    public static float JoySev = 12.5f;

    public static bool ApplyAnimals = true;

    public static bool AllowUnsafeAreas = true;

    private static readonly float separator = 7f;

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
        listing_Standard.Gap(separator);
        listing_Standard.CheckboxLabeled("Brrr.UsePhew".Translate(), ref UsePhew);
        listing_Standard.Gap(2f);
        listing_Standard.Label("Brrr.UnsafePhewSev".Translate() + "  " + UnsafePhewSev);
        UnsafePhewSev = listing_Standard.Slider(UnsafePhewSev, 10f, 30f);
        listing_Standard.Gap(separator);
        listing_Standard.CheckboxLabeled("Brrr.UseYuk".Translate(), ref UseYuk);
        listing_Standard.Gap(2f);
        listing_Standard.Label("Brrr.UnsafeYukSev".Translate() + "  " + UnsafeYukSev);
        UnsafeYukSev = listing_Standard.Slider(UnsafeYukSev, 5f, 15f);
        listing_Standard.Gap(separator);
        listing_Standard.CheckboxLabeled("Brrr.UseGasp".Translate(), ref UseGasp);
        listing_Standard.Gap(2f);
        listing_Standard.Label("Brrr.UnsafeGaspSev".Translate() + "  " + UnsafeGaspSev);
        UnsafeYukSev = listing_Standard.Slider(UnsafeYukSev, 5f, 15f);
        listing_Standard.Gap(separator);
        listing_Standard.CheckboxLabeled("Brrr.UseOoh".Translate(), ref UseOoh);
        listing_Standard.Gap(2f);
        listing_Standard.Label("Brrr.OohSev".Translate() + "  " + OohSev);
        OohSev = listing_Standard.Slider(OohSev, 10f, 30f);
        listing_Standard.Gap(separator);
        listing_Standard.CheckboxLabeled("Brrr.AllowJoy".Translate(), ref AllowJoy);
        listing_Standard.Gap(2f);
        listing_Standard.Label("Brrr.JoySev".Translate() + "  " + JoySev);
        JoySev = listing_Standard.Slider(JoySev, 5f, 25f);
        listing_Standard.Gap(separator);
        listing_Standard.CheckboxLabeled("Brrr.ApplyAnimals".Translate(), ref ApplyAnimals);
        listing_Standard.Gap(separator);
        listing_Standard.CheckboxLabeled("Brrr.AllowUnsafeAreas".Translate(), ref AllowUnsafeAreas);
        if (Controller.currentVersion != null)
        {
            listing_Standard.Gap(separator);
            GUI.contentColor = Color.gray;
            listing_Standard.Label("Brrr.CurrentModVersion".Translate(Controller.currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }

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
        Scribe_Values.Look(ref AllowUnsafeAreas, "AllowUnsafeAreas", true);
    }
}