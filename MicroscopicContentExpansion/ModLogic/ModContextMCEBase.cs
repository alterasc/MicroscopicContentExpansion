﻿using Kingmaker.Blueprints;
using MicroscopicContentExpansion.Config;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.Utilities;
using static UnityModManagerNet.UnityModManager;

namespace MicroscopicContentExpansion.ModLogic;
public class ModContextMCEBase : ModContextBase
{
    public AddedContent AddedContent;
    public Homebrew Homebrew;

    public ModContextMCEBase(ModEntry ModEntry) : base(ModEntry)
    {
#if DEBUG
        Debug = true;
#endif
    }
    public override void LoadAllSettings()
    {
        LoadSettings("AddedContent.json", "MicroscopicContentExpansion.Config", ref AddedContent);
        LoadSettings("Homebrew.json", "MicroscopicContentExpansion.Config", ref Homebrew);
        LoadBlueprints("MicroscopicContentExpansion.Config", this);
        LoadLocalization("MicroscopicContentExpansion.Localization");
    }

    public override void AfterBlueprintCachePatches()
    {
        base.AfterBlueprintCachePatches();
    }

    public override void SaveAllSettings()
    {
        base.SaveAllSettings();
        SaveSettings("AddedContent.json", AddedContent);
        SaveSettings("Homebrew.json", Homebrew);
    }

    public T GetModBlueprintReference<T>(string name) where T : BlueprintReferenceBase
        => BlueprintTools.GetModBlueprintReference<T>(this, name);
}
