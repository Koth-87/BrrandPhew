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

    public static void DoWindowContents(Rect canvas)
    {
        var listingStandard = new Listing_Standard
        {
            ColumnWidth = (canvas.width / 2f) - 10f
        };
        listingStandard.Begin(canvas);
        listingStandard.CheckboxLabeled("Brrr.UseBrrr".Translate(), ref UseBrrr);
        listingStandard.Gap(separator);
        listingStandard.CheckboxLabeled("Brrr.UsePhew".Translate(), ref UsePhew);
        listingStandard.Gap(separator);
        listingStandard.CheckboxLabeled("Brrr.UseYuk".Translate(), ref UseYuk);
        listingStandard.Gap(separator);
        listingStandard.CheckboxLabeled("Brrr.UseGasp".Translate(), ref UseGasp);
        listingStandard.Gap(separator);
        listingStandard.CheckboxLabeled("Brrr.UseOoh".Translate(), ref UseOoh);
        listingStandard.Gap(separator);
        listingStandard.CheckboxLabeled("Brrr.AllowJoy".Translate(), ref AllowJoy);
        listingStandard.Gap(separator);
        listingStandard.CheckboxLabeled("Brrr.ApplyAnimals".Translate(), ref ApplyAnimals);
        listingStandard.Gap(separator);
        listingStandard.CheckboxLabeled("Brrr.AllowUnsafeAreas".Translate(), ref AllowUnsafeAreas);
        listingStandard.NewColumn();
        if (UseBrrr)
        {
            listingStandard.Label("Brrr.UnsafeBrrrSev".Translate() + "  " + UnsafeBrrrSev);
            UnsafeBrrrSev = listingStandard.Slider(UnsafeBrrrSev, 10f, 100f);
            listingStandard.Gap(separator);
        }

        if (UsePhew)
        {
            listingStandard.Label("Brrr.UnsafePhewSev".Translate() + "  " + UnsafePhewSev);
            UnsafePhewSev = listingStandard.Slider(UnsafePhewSev, 10f, 100f);
            listingStandard.Gap(separator);
        }

        if (UseYuk)
        {
            listingStandard.Label("Brrr.UnsafeYukSev".Translate() + "  " + UnsafeYukSev);
            UnsafeYukSev = listingStandard.Slider(UnsafeYukSev, 5f, 100f);
            listingStandard.Gap(separator);
        }

        if (UseGasp)
        {
            listingStandard.Label("Brrr.UnsafeGaspSev".Translate() + "  " + UnsafeGaspSev);
            UnsafeGaspSev = listingStandard.Slider(UnsafeGaspSev, 5f, 100f);
            listingStandard.Gap(separator);
        }

        if (UseOoh)
        {
            listingStandard.Label("Brrr.OohSev".Translate() + "  " + OohSev);
            OohSev = listingStandard.Slider(OohSev, 10f, 100f);
            listingStandard.Gap(separator);
        }

        if (AllowJoy)
        {
            listingStandard.Label("Brrr.JoySev".Translate() + "  " + JoySev);
            JoySev = listingStandard.Slider(JoySev, 5f, 100f);
            listingStandard.Gap(separator);
        }

        if (listingStandard.ButtonTextLabeled("Brrr.ResetSettings".Translate(), "Brrr.Reset".Translate()))
        {
            UnsafeBrrrSev = 20f;
            UnsafePhewSev = 20f;
            UnsafeYukSev = 10f;
            UnsafeGaspSev = 10f;
            OohSev = 12.5f;
            JoySev = 12.5f;
        }

        if (Controller.CurrentVersion != null)
        {
            listingStandard.Gap(separator);
            GUI.contentColor = Color.gray;
            listingStandard.Label("Brrr.CurrentModVersion".Translate(Controller.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
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