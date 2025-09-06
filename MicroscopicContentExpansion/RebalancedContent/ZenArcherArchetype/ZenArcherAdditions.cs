using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.RebalancedContent.ZenArcherArchetype;
internal static class ZenArcherAdditions
{
    internal static void Create()
    {
        var cunningShot = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "ZenArcherCunningArcher", a =>
        {
            a.SetName(MCEContext, "Cunning Archer");
            a.SetDescription(MCEContext, "Zen Archer can use his {g|Encyclopedia:Wisdom}Wisdom{/g} score in place of his {g|Encyclopedia:Intelligence}Intelligence{/g} score when qualifying for combat {g|Encyclopedia:Feat}feats{/g}. This ability counts as having Combat Expertise for the purpose of feat prerequisites");
            a.AddComponent<ReplaceStatForPrerequisites>(c =>
            {
                c.OldStat = Kingmaker.EntitySystem.Stats.StatType.Intelligence;
                c.Policy = ReplaceStatForPrerequisites.StatReplacementPolicy.NewStat;
                c.NewStat = Kingmaker.EntitySystem.Stats.StatType.Wisdom;
            });
        });
        if (MCEContext.Homebrew.ZenArcher.IsEnabled("CunningArcher"))
        {
            var featureRef = cunningShot.ToReference<BlueprintFeatureReference>();
            ApplyForAll<BlueprintFeature>(
                [
                    "c610310d31414edabcedf0c8a6fe32c4", // Feint
                    "32429740d6a5470aaaa02f20d61e43d3", // Final Feint
                    "39425a13df904e3cb0e3f6debfe65cab", // Slayer's Feint
                    "52c6b07a68940af41b270b3710682dc7", // Greater Dirty Trick
                    "ed699d64870044b43bb5a7fbe3f29494", // Dirty Trick
                    "63d8e3a9ab4d72e4081a7862d7246a79", // Greater Disarm
                    "25bc9c439ac44fd44ac3b1e58890916f", // Disarm
                    "4cc71ae82bdd85b40b3cfe6697bb7949", // Greater Trip
                    "0f15c6f70d8fb2b49aa6cc24239cc5fa", // Trip
                ],
                bp =>
                {
                    bp.AddComponent<PrerequisiteFeature>(
                        c =>
                        {
                            c.Group = Prerequisite.GroupType.Any;
                            c.m_Feature = featureRef;
                        }
                        );
                }
                );
            var combatExpertise = GetBPRef<BlueprintFeatureReference>("4c44724ffa8844f4d9bedb5bb27d144a");
            var slayersFeint = GetBP<BlueprintFeature>("39425a13df904e3cb0e3f6debfe65cab");
            slayersFeint.GetComponents<PrerequisiteFeature>()
                .Where(c => combatExpertise.Equals(c.m_Feature))
                .ForEach(c => c.Group = Prerequisite.GroupType.Any);
            var zaArchetype = GetBP<BlueprintArchetype>("2b1a58a7917084f49b097e86271df21c");
            var lvl4Feature = zaArchetype.AddFeatures.FirstOrDefault(x => x.Level == 4);
            if (lvl4Feature == null)
            {
                lvl4Feature = new LevelEntry() { Level = 4, m_Features = [] };
                zaArchetype.AddFeatures = zaArchetype.AddFeatures.AppendToArray(lvl4Feature);
            }
            lvl4Feature.m_Features.Add(cunningShot.ToReference<BlueprintFeatureBaseReference>());
        }
        if (MCEContext.Homebrew.ZenArcher.IsEnabled("BonusFeintFeats"))
        {
            var feintRef = GetBPRef<BlueprintFeatureReference>("c610310d31414edabcedf0c8a6fe32c4");
            var rangedFeintRef = GetBPRef<BlueprintFeatureReference>("a2e947d6be234abba7c3ac0bd5dc9b1d");
            var feintingFlurry = MCEContext.GetModBlueprintReference<BlueprintFeatureReference>("FeintingFlurry");
            BlueprintFeatureReference[] arr = [feintRef, rangedFeintRef, feintingFlurry];
            var za6BonusFeat = GetBP<BlueprintFeatureSelection>("2a1eec5b782182f4cafbd20fcd069692");
            var za10BonusFeat = GetBP<BlueprintFeatureSelection>("15e623d9dba4e314aa0649e4f61e132b");

            za6BonusFeat.AddFeatures(arr);
            za10BonusFeat.AddFeatures(arr);
        }
        if (MCEContext.Homebrew.ZenArcher.IsEnabled("BonusSnapShotFeats"))
        {
            var improvedSnapShotRef = GetBPRef<BlueprintFeatureReference>("c3453e7e215c1f149b938be27ac754c6");
            var greaterSnapShotRef = GetBPRef<BlueprintFeatureReference>("67b09c86234cecc4c8309f22f7d33973");
            BlueprintFeatureReference[] arr = [improvedSnapShotRef, greaterSnapShotRef];

            var za10BonusFeat = GetBP<BlueprintFeatureSelection>("15e623d9dba4e314aa0649e4f61e132b");

            za10BonusFeat.AddFeatures(arr);
        }
    }
}