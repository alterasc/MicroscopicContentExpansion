using TabletopTweaks.Core.UMMTools;
using UnityModManagerNet;

namespace MicroscopicContentExpansion {
    internal static class UMMSettingsUI {
        private static int selectedTab;
        public static void OnGUI(UnityModManager.ModEntry modEntry) {
            UI.AutoWidth();
            UI.TabBar(ref selectedTab,
                    () => UI.Label("SETTINGS WILL NOT BE UPDATED UNTIL YOU RESTART YOUR GAME.".yellow().bold()),
                    new NamedAction("Added Content", () => SettingsTabs.AddedContent())
            );
        }
    }

    internal static class SettingsTabs {

        public static void AddedContent() {
            var TabLevel = SetttingUI.TabLevel.Zero;
            var AddedContent = Main.MCEContext.AddedContent;
            UI.Div(0, 15);
            using (UI.VerticalScope()) {
                UI.Toggle("New Settings Off By Default".bold(), ref AddedContent.NewSettingsOffByDefault);
                UI.Space(25);

                SetttingUI.SettingGroup("AlternativeCapstones", TabLevel, AddedContent.AlternativeCapstones);
            }
        }
    }
}
