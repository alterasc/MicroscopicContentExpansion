using TabletopTweaks.Core.Config;

namespace MicroscopicContentExpansion.Config;
public class Homebrew : IUpdatableSettings
{
    public bool NewSettingsOffByDefault = false;
    public SettingGroup DragonbloodShifter = new();
    public SettingGroup MythicArmorFeats = new();

    public void Init()
    {
    }

    public void OverrideSettings(IUpdatableSettings userSettings)
    {
        var loadedSettings = userSettings as Homebrew;
        NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;
        DragonbloodShifter.LoadSettingGroup(loadedSettings.DragonbloodShifter, NewSettingsOffByDefault);
        MythicArmorFeats.LoadSettingGroup(loadedSettings.MythicArmorFeats, NewSettingsOffByDefault);
    }
}
