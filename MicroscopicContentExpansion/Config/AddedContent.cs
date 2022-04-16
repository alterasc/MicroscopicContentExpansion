using TabletopTweaks.Core.Config;

namespace MicroscopicContentExpansion.Config {
    public class AddedContent : IUpdatableSettings {
        public bool NewSettingsOffByDefault = false;
        public SettingGroup AlternativeCapstones = new();
        public SettingGroup NewClasses = new();

        public void Init() {
        }

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as AddedContent;
            NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;
            AlternativeCapstones.LoadSettingGroup(loadedSettings.AlternativeCapstones, NewSettingsOffByDefault);
            AlternativeCapstones.LoadSettingGroup(loadedSettings.NewClasses, NewSettingsOffByDefault);
        }
    }
}
