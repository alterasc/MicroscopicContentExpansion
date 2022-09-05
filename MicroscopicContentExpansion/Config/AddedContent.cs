using TabletopTweaks.Core.Config;

namespace MicroscopicContentExpansion.Config {
    public class AddedContent : IUpdatableSettings {
        public bool NewSettingsOffByDefault = false;
        public SettingGroup Spells = new();
        public SettingGroup NewClasses = new();

        public void Init() {
        }

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as AddedContent;
            NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;
            Spells.LoadSettingGroup(loadedSettings.Spells, NewSettingsOffByDefault);
            NewClasses.LoadSettingGroup(loadedSettings.NewClasses, NewSettingsOffByDefault);
        }
    }
}
