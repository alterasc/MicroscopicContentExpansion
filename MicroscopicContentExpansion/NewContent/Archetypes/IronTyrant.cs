using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Base.Main;

namespace MicroscopicContentExpansion.Base.NewContent.Archetypes {
    internal class IronTyrant {
        private const string NAME = "Iron Tyrant";
        private const string DESCRIPTION = "Covered from head to toe in blackened armor decorated with grotesque shapes and bristling with spikes, iron tyrants make an unmistakable impression on the battlefield. These antipaladins’ armor is an outward symbol of their inner power, and they are rarely seen without it. Iron tyrants seek the strength to rule over domains as unfettered despots, and depend on their armor as protection against those they have not yet cowed.";

        public static void AddIronTyrant() {
            var AntipaladinClass = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(MCEContext, "AntipaladinClass");
            var TouchOfCorruptionFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinTouchOfCorruptionFeature");
            var CrueltySelection = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltySelection");
            var ChannelNegativeEnergy = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinChannelNegativeEnergyFeature");

            var ArmorShieldCombatFeatSelection = AddArmorShieldCombatFeatSelection();
            var IronFist = AddIronFist();

            var IronTyrant = Helpers.CreateBlueprint<BlueprintArchetype>(MCEContext, "IronTyrantArchetype", bp => {
                bp.LocalizedName = Helpers.CreateString(MCEContext, $"IronTyrantArchetype.Name", NAME);
                bp.LocalizedDescription = Helpers.CreateString(MCEContext, $"IronTyrantArchetype.Description", DESCRIPTION);
                bp.LocalizedDescriptionShort = Helpers.CreateString(MCEContext, $"IronTyrantArchetype.Description", DESCRIPTION);
                bp.RemoveFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(2, TouchOfCorruptionFeature),
                    Helpers.CreateLevelEntry(3, CrueltySelection),
                    Helpers.CreateLevelEntry(4, ChannelNegativeEnergy),
                    Helpers.CreateLevelEntry(6, CrueltySelection),
                    Helpers.CreateLevelEntry(9, CrueltySelection),
                    Helpers.CreateLevelEntry(12, CrueltySelection),
                    Helpers.CreateLevelEntry(15, CrueltySelection),
                    Helpers.CreateLevelEntry(18, CrueltySelection)

                };
                bp.AddFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(2, IronFist),
                    Helpers.CreateLevelEntry(3, ArmorShieldCombatFeatSelection),
                    Helpers.CreateLevelEntry(6, ArmorShieldCombatFeatSelection),
                    Helpers.CreateLevelEntry(9, ArmorShieldCombatFeatSelection),
                    Helpers.CreateLevelEntry(12, ArmorShieldCombatFeatSelection),
                    Helpers.CreateLevelEntry(15, ArmorShieldCombatFeatSelection),
                    Helpers.CreateLevelEntry(18, ArmorShieldCombatFeatSelection)
                };
            });

            var Archetypes = AntipaladinClass.m_Archetypes.AppendToArray(IronTyrant.ToReference<BlueprintArchetypeReference>());
            AntipaladinClass.m_Archetypes = Archetypes;
        }

        private static BlueprintFeatureSelection AddArmorShieldCombatFeatSelection() {
            var BonusFeatSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(MCEContext, "IronTyrantBonusFeatSelection", bp => {
                bp.SetName(MCEContext, "Bonus Feat");
                bp.SetDescription(MCEContext, "At 3rd level and every 3 antipaladin levels thereafter, an iron tyrant gains a bonus feat" +
                    " in addition to those gained from normal advancement. This feat must be a combat feat that relates to the iron" +
                    " tyrant’s armor or shield, such as Shield Focus, or one of the Armor Mastery feats.");
                bp.m_AllFeatures = new BlueprintFeatureReference[0];
                bp.m_Features = new BlueprintFeatureReference[0];
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Defense;
                });
            });

            var PossibleFeats = new BlueprintFeatureReference[] {
                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("6105f450bb2acbd458d277e71e19d835"), //TowerShieldProficiency
                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("453f5181a5ed3a445abfa3bcd3f4ac0c"), //ArcaneArmorMaster
                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("1a0298abacb6e0f45b7e28553e99e76c"), //ArcaneArmorTraining
                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("76d4885a395976547a13c5d6bf95b482"), //ArmorFocusSelection
                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("0b442a7b4aa598d4e912a4ecee0500ff"), //BashingFinish
                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("121811173a614534e8720d7550aae253"), //ShieldBash
                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("ac57069b6bf8c904086171683992a92a"), //ShieldFocus
                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("afd05ca5363036c44817c071189b67e1"), //ShieldFocusGreater
                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("dbec636d84482944f87435bd31522fcc"), //ShieldMaster
                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("0b707584fc2ea724aa72c396c2230dc7"), //ShieldedCaster
                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("8976de442862f82488a4b138a0a89907")  //ShieldWall                
            };
            BonusFeatSelection.AddFeatures(PossibleFeats);
            //Adding feats from TTT-Base if they exists
            var ShieldMasterySelection = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("ef38e0fe-68f1-4c88-a9de-acc421455d14");
            if (!ShieldMasterySelection.IsEmpty()) {
                BonusFeatSelection.AddFeatures(ShieldMasterySelection);
            }
            var ArmorMasterySelection = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("4432c1ac-66c7-4a86-9ffb-6fa21d69232f");
            if (!ArmorMasterySelection.IsEmpty()) {
                BonusFeatSelection.AddFeatures(ArmorMasterySelection);
            }
            return BonusFeatSelection;
        }

        private static BlueprintFeature AddIronFist() {
            var IronFist = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "IronTyrantIronFist", bp => {
                bp.SetName(MCEContext, "Iron Fist");
                bp.SetDescription(MCEContext, "At 2nd level, an iron tyrant gains Improved Unarmed Strike as a bonus feat.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    var IUS = BlueprintTools.GetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167").ToReference<BlueprintUnitFactReference>();
                    c.m_Facts = new BlueprintUnitFactReference[] { IUS };
                });
            });

            return IronFist;
        }
    }
}
