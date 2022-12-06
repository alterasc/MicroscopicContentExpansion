using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using MicroscopicContentExpansion.NewComponents;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Feats {
    internal class CrusadersFlurry {

        internal static void Add() {
            MCEContext.Logger.LogHeader("Adding Crusader's Flurry");

            var deitySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("59e7a76987fe3b547b9cce045f4db3e4");

            var cflurryUnlock = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "CrusadersFlurryUnlock", bp => {
                bp.AddComponent<CrusadersFlurryUnlock>(c => {
                    c._deitySelection = BlueprintTools.GetBlueprintReference<BlueprintFeatureSelectionReference>("59e7a76987fe3b547b9cce045f4db3e4");
                    c._soheiArchetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("fad7c56737ed12e42aacc330acc86428");
                    c._flurryFact1 = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("332362f3bd39ebe46a740a36960fdcb4");
                    c._flurryFact11 = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("de25523acc24b1448aa90f74d6512a08");
                    c._flurryFact20 = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("98319382db05-42ef91e0523392d49757");
                    c._weaponFocus = BlueprintTools.GetBlueprintReference<BlueprintParametrizedFeatureReference>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
                    c._monkFlurry11 = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("a34b8a9fcc9024b42bacfd5e6b614bfa");
                    c._soheiFlurry11 = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("dfc54683a9b7b2d4294ad1fd2acd5877");
                    c._oldMaster = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("0e6eea0e813f44489835b8940264b7af");
                });
                bp.SetName(MCEContext, "Crusader's Flurry");
                bp.SetDescription(MCEContext, "You can use your deity’s favored weapon as if it were a monk weapon.");
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] {
                        FeatureGroup.Feat
                    };
                bp.AddPrerequisiteFeature(BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("1e1f627d26ad36f43bbd26cc2bf8ac7e"));

                bp.AddComponent<PrerequisiteFeaturesFromList>(c => {
                    c.m_Features = new BlueprintFeatureReference[] {
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("fd99770e6bd240a4aab70f7af103e56a"),
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("cd4381b73b6709146bbcc0a528a6f471")
                    };
                });

                bp.AddComponent<PrerequisiteFeaturesFromList>(c => {
                    c.m_Features = new BlueprintFeatureReference[] {
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("d332c1748445e8f4f9e92763123e31bd"),
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("a9ab1bbc79ecb174d9a04699986ce8d5"),
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("7d49d7f590dc9a948b3bd1c8b7979854"),
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("cb6d55dda5ab906459d18a435994a760"),
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("b8ec9dccc0e7ef74fb4072b0679c2aec"),
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("4bf9a9afadca5304e89bf52f2ac2d236"),
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("bd588bc544d2f8547a02bb82ad9f466a"),
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("b40316f05d4772e4894688e6743602bd"),
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("a79013ff4bcd4864cb669622a29ddafb"),
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("295dff380fb8ed743bd5c76a30a49a46"),
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("927707dce06627d4f880c90b5575125f"),
                        BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("06d824227f664c5fbb0e88901339ca91")
                    };
                });
            });

            if (MCEContext.AddedContent.Feats.IsDisabled("CrusadersFlurry")) { return; }

            FeatTools.AddAsFeat(cflurryUnlock);
        }

    }
}
