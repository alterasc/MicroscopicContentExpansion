using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Modding;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace MicroscopicContentExpansion.NewContent.Feats;

internal class CrusadersFlurry
{

    internal const CustomMechanicsFeature CrusadersFlurryMechanicsFeature = (CustomMechanicsFeature)1047;

    internal static void Add()
    {
        MCEContext.Logger.LogHeader("Adding Crusader's Flurry");

        var cflurryUnlock = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "CrusadersFlurryUnlock", bp =>
        {

            var hasHomeBrewArchetypes = OwlcatModificationsManager.Instance.AppliedModifications.Any(x => x.Manifest.UniqueName == "HomebrewArchetypes");
            bp.AddComponent<AddCustomMechanicsFeature>(c =>
            {
                c.Feature = CrusadersFlurryMechanicsFeature;
            });
            bp.SetName(MCEContext, "Crusader's Flurry");
            bp.SetDescription(MCEContext, "You can use your deity’s favored weapon as if it were a monk weapon.");
            bp.IsClassFeature = true;
            bp.Groups = [
                    FeatureGroup.Feat
                ];
            bp.AddPrerequisiteFeature(GetBPRef<BlueprintFeatureReference>("1e1f627d26ad36f43bbd26cc2bf8ac7e"));

            List<BlueprintFeatureReference> flurryPrereqs = new List<BlueprintFeatureReference>() {
                GetBPRef<BlueprintFeatureReference>("fd99770e6bd240a4aab70f7af103e56a"),
                GetBPRef<BlueprintFeatureReference>("cd4381b73b6709146bbcc0a528a6f471")
            };
            if (hasHomeBrewArchetypes)
            {
                flurryPrereqs.Add(GetBPRef<BlueprintFeatureReference>("a86e13b03d0d50e4a91a8d8bf9d7d2b1")); //SacredFistFlurryUnlock
            }
            bp.AddComponent<PrerequisiteFeaturesFromList>(c =>
            {
                c.m_Features = flurryPrereqs.ToArray();
            });

            bp.AddComponent<PrerequisiteFeaturesFromList>(c =>
            {
                c.m_Features = [
                    GetBPRef<BlueprintFeatureReference>("d332c1748445e8f4f9e92763123e31bd"), //ChannelEnergySelection
                    GetBPRef<BlueprintFeatureReference>("a9ab1bbc79ecb174d9a04699986ce8d5"), //ChannelEnergyHospitalerFeature
                    GetBPRef<BlueprintFeatureReference>("7d49d7f590dc9a948b3bd1c8b7979854"), //ChannelEnergyEmpyrealFeature
                    GetBPRef<BlueprintFeatureReference>("cb6d55dda5ab906459d18a435994a760"), //ChannelEnergyPaladinFeature
                    GetBPRef<BlueprintFeatureReference>("b8ec9dccc0e7ef74fb4072b0679c2aec"), //ShamanLifeSpiritChannelEnergyFeature
                    GetBPRef<BlueprintFeatureReference>("4bf9a9afadca5304e89bf52f2ac2d236"), //OracleRevelationChannelFeature
                    GetBPRef<BlueprintFeatureReference>("bd588bc544d2f8547a02bb82ad9f466a"), //WarpriestChannelEnergyFeature
                    GetBPRef<BlueprintFeatureReference>("e02c8a7336a542f4baffa116b6506950"), //WarpriestChannelNegativeFeature
                    GetBPRef<BlueprintFeatureReference>("b40316f05d4772e4894688e6743602bd"), //HexChannelerChannelFeature
                    GetBPRef<BlueprintFeatureReference>("a79013ff4bcd4864cb669622a29ddafb"), //ChannelEnergyFeature
                    GetBPRef<BlueprintFeatureReference>("295dff380fb8ed743bd5c76a30a49a46"), //LichChannelNegativeFeature
                    GetBPRef<BlueprintFeatureReference>("927707dce06627d4f880c90b5575125f"), //NecromancySchoolBaseFeature
                    GetBPRef<BlueprintFeatureReference>("06d824227f664c5fbb0e88901339ca91") //AntipaladinChannelNegativeEnergyFeature
                ];
            });
        });

        if (MCEContext.AddedContent.Feats.IsDisabled("CrusadersFlurry")) { return; }

        FeatTools.AddAsFeat(cflurryUnlock);
    }

}
