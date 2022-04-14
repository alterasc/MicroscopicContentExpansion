using MicroscopicContentExpansion.Base.Config;
using TabletopTweaks.Core.ModLogic;
using static UnityModManagerNet.UnityModManager;

namespace MicroscopicContentExpansion.Base.ModLogic {
    internal class ModContextMCEBase : ModContextBase {
        public AddedContent AddedContent;

        public ModContextMCEBase(ModEntry ModEntry) : base(ModEntry) {
            LoadAllSettings();
#if DEBUG
            Debug = true;
#endif
        }
        public override void LoadAllSettings() {
            LoadSettings("AddedContent.json", "MicroscopicContentExpansion.Base.Config", ref AddedContent);
            LoadBlueprints("MicroscopicContentExpansion.Base.Config", Main.MCEContext);
            LoadLocalization("MicroscopicContentExpansion.Base.Localization");
        }

        public override void AfterBlueprintCachePatches() {
            base.AfterBlueprintCachePatches();
        }

        public override void SaveAllSettings() {
            base.SaveAllSettings();
            SaveSettings("AddedContent.json", AddedContent);
        }
    }
}
