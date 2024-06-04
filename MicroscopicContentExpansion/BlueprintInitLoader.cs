using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using MicroscopicContentExpansion.NewContent.Antipaladin;
using MicroscopicContentExpansion.NewContent.Classes;
using MicroscopicContentExpansion.NewContent.Feats;
using MicroscopicContentExpansion.NewContent.Spells;
using MicroscopicContentExpansion.RebalancedContent.DragonbloodShifterArchetype;

namespace MicroscopicContentExpansion {
    internal class BlueprintInitLoader {

        [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
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
            }
        }
    }
}