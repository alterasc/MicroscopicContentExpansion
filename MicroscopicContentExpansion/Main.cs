global using static MicroscopicContentExpansion.Main;
using HarmonyLib;
using MicroscopicContentExpansion.ModLogic;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;

namespace MicroscopicContentExpansion;
static class Main
{
    public static ModContextMCEBase MCEContext;
    public static Harmony HarmonyInstance;
    static bool Load(UnityModManager.ModEntry modEntry)
    {
        var harmony = new Harmony(modEntry.Info.Id);
        HarmonyInstance = harmony;
        MCEContext = new ModContextMCEBase(modEntry);
        MCEContext.LoadAllSettings();
        MCEContext.ModEntry.OnSaveGUI = OnSaveGUI;
        MCEContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
        harmony.PatchAll();
        PostPatchInitializer.Initialize(MCEContext);
        return true;
    }

    static void OnSaveGUI(UnityModManager.ModEntry modEntry)
    {
        MCEContext.SaveAllSettings();
    }
}
