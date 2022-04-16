using MicroscopicContentExpansion.Config;
using TabletopTweaks.Core.ModLogic;
using static UnityModManagerNet.UnityModManager;

namespace MicroscopicContentExpansion.ModLogic {
    internal class ModContextMCEBase : ModContextBase {
        public AddedContent AddedContent;
        public Fixes Fixes;

        public ModContextMCEBase(ModEntry ModEntry) : base(ModEntry) {
            LoadAllSettings();
#if DEBUG
            Debug = true;
#endif
        }
        public override void LoadAllSettings() {
            LoadSettings("AddedContent.json", "MicroscopicContentExpansion.Config", ref AddedContent);
            LoadSettings("Fixes.json", "MicroscopicContentExpansion.Config", ref Fixes);
            LoadBlueprints("MicroscopicContentExpansion.Config", Main.MCEContext);
            LoadLocalization("MicroscopicContentExpansion.Localization");
        }

        public override void AfterBlueprintCachePatches() {
            base.AfterBlueprintCachePatches();
        }

        public override void SaveAllSettings() {
            base.SaveAllSettings();
            SaveSettings("AddedContent.json", AddedContent);
            SaveSettings("Fixes.json", Fixes);
        }
    }
}
