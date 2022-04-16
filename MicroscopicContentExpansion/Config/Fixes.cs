using TabletopTweaks.Core.Config;

namespace MicroscopicContentExpansion.Config {
    public class Fixes : IUpdatableSettings {
        public bool NewSettingsOffByDefault = false;
        public SettingGroup Miscellaneous = new();

        public void Init() {
        }

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as Fixes;
            NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;
            Miscellaneous.LoadSettingGroup(loadedSettings.Miscellaneous, NewSettingsOffByDefault);
        }
    }
}
