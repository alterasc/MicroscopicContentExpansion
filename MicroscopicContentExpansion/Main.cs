using HarmonyLib;
using MicroscopicContentExpansion.Base.ModLogic;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;

namespace MicroscopicContentExpansion.Base {
    static class Main {
        public static bool Enabled;
        public static ModContextMCEBase MCEContext;
        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            MCEContext = new ModContextMCEBase(modEntry);
            MCEContext.LoadAllSettings();
            MCEContext.ModEntry.OnSaveGUI = OnSaveGUI;
            MCEContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
            harmony.PatchAll();
            PostPatchInitializer.Initialize(MCEContext);
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            MCEContext.SaveAllSettings();
        }
    }
}
