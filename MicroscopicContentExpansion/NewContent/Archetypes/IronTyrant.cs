using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using MicroscopicContentExpansion.NewContent.Archetypes.IronTyrantFeatures;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Archetypes;
internal class IronTyrant
{
    private const string NAME = "Iron Tyrant";
    private const string DESCRIPTION = "Covered from head to toe in blackened armor decorated with grotesque shapes and bristling with spikes, iron tyrants make an unmistakable impression on the battlefield. These antipaladins’ armor is an outward symbol of their inner power, and they are rarely seen without it. Iron tyrants seek the strength to rule over domains as unfettered despots, and depend on their armor as protection against those they have not yet cowed.";

    public static void AddIronTyrant()
    {
        var AntipaladinClass = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(MCEContext, "AntipaladinClass");
        var touchOfCorruptionFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinTouchOfCorruptionFeature");
        var crueltySelection = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltySelection");
        var channelNegativeEnergy = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinChannelNegativeEnergyFeature");
        var fiendishBoon = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinFiendishBoonSelection");

        var armorShieldCombatFeatSelection = AddArmorShieldCombatFeatSelection();
        var ironFist = AddIronFist();
        var ironTyrantFiendishBond = IronTyrantArmorBond.AddFiendishBond();
        var unstoppable = AddUnstoppable();

        var IronTyrant = Helpers.CreateBlueprint<BlueprintArchetype>(MCEContext, "IronTyrantArchetype", bp =>
        {
            bp.LocalizedName = Helpers.CreateString(MCEContext, $"{bp.name}.Name", NAME);
            bp.LocalizedDescription = Helpers.CreateString(MCEContext, $"{bp.name}.Description", DESCRIPTION);
            bp.LocalizedDescriptionShort = Helpers.CreateString(MCEContext, $"{bp.name}.Description", DESCRIPTION);
            bp.RemoveFeatures = [
                Helpers.CreateLevelEntry(2, touchOfCorruptionFeature),
                Helpers.CreateLevelEntry(3, crueltySelection),
                Helpers.CreateLevelEntry(4, channelNegativeEnergy),
                Helpers.CreateLevelEntry(5, fiendishBoon),
                Helpers.CreateLevelEntry(6, crueltySelection),
                Helpers.CreateLevelEntry(9, crueltySelection),
                Helpers.CreateLevelEntry(12, crueltySelection),
                Helpers.CreateLevelEntry(15, crueltySelection),
                Helpers.CreateLevelEntry(18, crueltySelection)

            ];
            bp.AddFeatures = [
                Helpers.CreateLevelEntry(2, ironFist),
                Helpers.CreateLevelEntry(3, armorShieldCombatFeatSelection),
                Helpers.CreateLevelEntry(4, unstoppable),
                Helpers.CreateLevelEntry(5, ironTyrantFiendishBond),
                Helpers.CreateLevelEntry(6, armorShieldCombatFeatSelection),
                Helpers.CreateLevelEntry(9, armorShieldCombatFeatSelection),
                Helpers.CreateLevelEntry(12, armorShieldCombatFeatSelection),
                Helpers.CreateLevelEntry(15, armorShieldCombatFeatSelection),
                Helpers.CreateLevelEntry(18, armorShieldCombatFeatSelection)
            ];
        });

        var Archetypes = AntipaladinClass.m_Archetypes.AppendToArray(IronTyrant.ToReference<BlueprintArchetypeReference>());
        AntipaladinClass.m_Archetypes = Archetypes;
    }

    private static BlueprintFeature AddUnstoppable()
    {
        var difficultTerrainImmunity = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DifficultTerrainImmunityFeature", bp =>
        {
            bp.SetName(MCEContext, "Difficult terrain immunity");
            bp.HideInUI = true;
            bp.HideInCharacterSheetAndLevelUp = true;
            bp.IsClassFeature = true;
            bp.AddComponent<AddConditionImmunity>(c =>
            {
                c.Condition = Kingmaker.UnitLogic.UnitCondition.DifficultTerrain;
            });
        });

        return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "IronTyrantUnstoppable", bp =>
        {
            bp.SetName(MCEContext, "Unstoppable");
            bp.SetDescription(MCEContext, "At 4th level, when wearing armor, an iron tyrant is immune to effects of difficult terrain.");
            bp.IsClassFeature = true;
            bp.AddComponent<HasArmorFeatureUnlock>(c =>
            {
                c.m_NewFact = difficultTerrainImmunity.ToReference<BlueprintUnitFactReference>();
            });
        });
    }
    private static BlueprintFeatureSelection AddArmorShieldCombatFeatSelection()
    {
        var BonusFeatSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(MCEContext, "IronTyrantBonusFeatSelection", bp =>
        {
            bp.SetName(MCEContext, "Bonus Feat");
            bp.SetDescription(MCEContext, "At 3rd level and every 3 antipaladin levels thereafter, an iron tyrant gains a bonus feat" +
                " in addition to those gained from normal advancement. This feat must be a combat feat that relates to the iron" +
                " tyrant’s armor or shield, such as Shield Focus, or one of the Armor Mastery feats.");
            bp.m_AllFeatures = [];
            bp.m_Features = [];
            bp.IsClassFeature = true;
            bp.Groups = [FeatureGroup.Feat, FeatureGroup.CombatFeat];
            bp.AddComponent<FeatureTagsComponent>(c =>
            {
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
        if (!ShieldMasterySelection.IsEmpty())
        {
            BonusFeatSelection.AddFeatures(ShieldMasterySelection);
        }
        var ArmorMasterySelection = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("4432c1ac-66c7-4a86-9ffb-6fa21d69232f");
        if (!ArmorMasterySelection.IsEmpty())
        {
            BonusFeatSelection.AddFeatures(ArmorMasterySelection);
        }
        return BonusFeatSelection;
    }

    private static BlueprintFeature AddIronFist()
    {
        var IronFist = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "IronTyrantIronFist", bp =>
        {
            bp.SetName(MCEContext, "Iron Fist");
            bp.SetDescription(MCEContext, "At 2nd level, an iron tyrant gains Improved Unarmed Strike as a bonus feat.");
            bp.IsClassFeature = true;
            bp.AddComponent<AddFacts>(c =>
            {
                var IUS = BlueprintTools.GetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167").ToReference<BlueprintUnitFactReference>();
                c.m_Facts = [IUS];
            });
        });

        return IronFist;
    }
}
