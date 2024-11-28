using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Modding;
using Kingmaker.Utility;
using MicroscopicContentExpansion.NewComponents;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Feats;
internal class CrusadersFlurry
{

    internal static void Add()
    {
        MCEContext.Logger.LogHeader("Adding Crusader's Flurry");

        var cflurryUnlock = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "CrusadersFlurryUnlock", bp =>
        {

            var hasHomeBrewArchetypes = OwlcatModificationsManager.Instance.AppliedModifications.Any(x => x.Manifest.UniqueName == "HomebrewArchetypes");
            List<BlueprintFeatureReference> flurry2ndFacts = [
                GetBPRef<BlueprintFeatureReference>("a34b8a9fcc9024b42bacfd5e6b614bfa"),
                GetBPRef<BlueprintFeatureReference>("dfc54683a9b7b2d4294ad1fd2acd5877")
            ];
            if (hasHomeBrewArchetypes)
            {
                flurry2ndFacts.Add(GetBPRef<BlueprintFeatureReference>("885ad943c0c3f0445aef3813f869921f")); //SacredFistFlurry15Unlock
            }
            bp.AddComponent<CrusadersFlurryUnlock>(c =>
            {
                c._deitySelection = GetBPRef<BlueprintFeatureSelectionReference>("59e7a76987fe3b547b9cce045f4db3e4");
                c._soheiArchetype = GetBPRef<BlueprintArchetypeReference>("fad7c56737ed12e42aacc330acc86428");
                c._flurryFact1 = GetBPRef<BlueprintUnitFactReference>("332362f3bd39ebe46a740a36960fdcb4");
                c._flurryFact11 = GetBPRef<BlueprintUnitFactReference>("de25523acc24b1448aa90f74d6512a08");
                c._flurryFact20 = GetBPRef<BlueprintUnitFactReference>("98319382db0542ef91e0523392d49757");
                c._weaponFocus = GetBPRef<BlueprintParametrizedFeatureReference>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
                c._flurry2ndfacts = flurry2ndFacts.ToArray();
                c._oldMaster = GetBPRef<BlueprintFeatureReference>("0e6eea0e813f44489835b8940264b7af");
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
