global using static MicroscopicContentExpansion.Main;
using HarmonyLib;
using MicroscopicContentExpansion.ModLogic;
using MicroscopicContentExpansion.NewContent.AnimalCompanions;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;

namespace MicroscopicContentExpansion;
static class Main
{
    public static ModContextMCEBase MCEContext;
    public static Harmony HarmonyInstance;
    static bool Load(UnityModManager.ModEntry modEntry)
    {
        HarmonyInstance = new Harmony(modEntry.Info.Id);
        MCEContext = new ModContextMCEBase(modEntry);
        MCEContext.LoadAllSettings();
        MCEContext.ModEntry.OnSaveGUI = OnSaveGUI;
        MCEContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
        HarmonyInstance.CreateClassProcessor(typeof(BlueprintInitLoader)).Patch();
        HarmonyInstance.CreateClassProcessor(typeof(NightmareMountOffsetPatch)).Patch();
        PostPatchInitializer.Initialize(MCEContext);
        return true;
    }

    static void OnSaveGUI(UnityModManager.ModEntry modEntry)
    {
        MCEContext.SaveAllSettings();
    }
}
