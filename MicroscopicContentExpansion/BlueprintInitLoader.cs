using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using MicroscopicContentExpansion.NewContent.Classes;
using MicroscopicContentExpansion.NewContent.Feats;
using MicroscopicContentExpansion.NewContent.Spells;
using MicroscopicContentExpansion.RebalancedContent.DragonbloodShifterArchetype;
using MicroscopicContentExpansion.RebalancedContent.MythicArmor;

namespace MicroscopicContentExpansion;
[HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
internal class BlueprintInitLoader
{
    static bool Initialized;

    [HarmonyPostfix]
    static void AddModBlueprints()
    {
        if (Initialized) return;
        Initialized = true;
        AntipaladinAdder.AddAntipaladin();
        CrusadersFlurry.Add();
        SnakeStyleChain.AddSnakeStyle();
        StartossStyleChain.AddStartossChain();
        DimenshionalDervish.AddDimenshionalSavantFeatChain();
        KiLeech.AddKiLeech();
        DruidicHerbalism.Add();
        CrusaderLegionBlessing.Add();
        MonkStyleStrikeFlyingKick.AddFlyingKick();
        MonkTimelessBody.AddTimelessBody();
        MonkFlawlessMind.AddFlawlessMind();
        MonkDiamondResilience.AddDiamondResilience();
        FeintingFlurry.Add();

        Dragonblooded.Change();
        MythicArmorFeatTweaks.Change();
    }
}