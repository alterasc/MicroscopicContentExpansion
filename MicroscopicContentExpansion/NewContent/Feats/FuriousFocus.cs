using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using MicroscopicContentExpansion.NewComponents;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Feats {
    class FuriousFocus {
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                MCEContext.Logger.LogHeader("Adding furious focus");


                var furiousFocus = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "FuriousFocusFeature", bp => {
                    bp.SetName(MCEContext, "Furious Focus");
                    bp.SetDescription(MCEContext, "When you are wielding a two-handed weapon or a one-handed weapon with two hands," +
                        " and using the Power Attack feat, you do not suffer Power Attack’s penalty on melee attack rolls on the" +
                        " first attack you make each turn. You still suffer the penalty on any additional attacks, including attacks of opportunity.");
                    bp.Ranks = 1;
                    bp.IsClassFeature = true;
                    bp.Groups = new FeatureGroup[] {
                        FeatureGroup.CombatFeat,
                        FeatureGroup.Feat
                    };
                    bp.AddComponent<WeaponParametersFuriousFocus>();
                    bp.AddComponent<FeatureTagsComponent>(c => {
                        c.FeatureTags = FeatureTag.Attack | FeatureTag.Melee;
                    });
                    bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                        c.Stat = StatType.Strength;
                        c.Value = 13;
                    });
                    bp.AddPrerequisiteFeature(
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("9972f33f977fc724c838e59641b2fca5")
                    );
                });
                FeatTools.AddAsFeat(furiousFocus);
                var FighterFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("41c8486641f7d6d4283ca9dae4147a9f");
                FighterFeatSelection.AddFeatures(furiousFocus);

            }
        }
    }
}
