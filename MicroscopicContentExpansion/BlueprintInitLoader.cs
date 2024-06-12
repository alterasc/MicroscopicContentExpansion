using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using MicroscopicContentExpansion.NewContent.Classes;
using MicroscopicContentExpansion.NewContent.Feats;
using MicroscopicContentExpansion.NewContent.Spells;
using MicroscopicContentExpansion.RebalancedContent.DragonbloodShifterArchetype;
using MicroscopicContentExpansion.RebalancedContent.MythicArmor;

namespace MicroscopicContentExpansion;
internal class BlueprintInitLoader
{

    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    static class BlueprintsCache_Init_Patch
    {
        static bool Initialized;

        static void Postfix()
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
            MythicAmorFeatTweaks.Change();

            //if (MCEContext.Homebrew.MythicArmorFeats.IsEnabled("MithralDoesNotLowerCategory"))
            //{
            //    var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
            //    var method = new HarmonyMethod(typeof(UnitArmorPatch).GetMethod(nameof(UnitArmorPatch.Patch), flags | System.Reflection.BindingFlags.Static));
            //    HarmonyInstance.Patch(typeof(UnitArmor).GetMethod(nameof(UnitArmor.CheckCondition), flags), prefix: method);
            //}
        }
    }
}