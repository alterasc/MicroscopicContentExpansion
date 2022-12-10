using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using MicroscopicContentExpansion.NewContent.Antipaladin;
using MicroscopicContentExpansion.NewContent.Feats;

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
            }
        }
    }
}