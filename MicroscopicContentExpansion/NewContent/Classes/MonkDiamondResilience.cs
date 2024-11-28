using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements.DamageResistance;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Classes;
internal class MonkDiamondResilience
{
    internal static void AddDiamondResilience()
    {
        var monkClassRef = GetBPRef<BlueprintCharacterClassReference>("e8f21e5b58e0569468e420ebea456124");

        var damageReductionIcon = GetBP<BlueprintFeature>("cffb5cddefab30140ac133699d52a8f8").m_Icon;

        var diamondResilience = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "MonkDiamondResilience", a =>
        {
            a.SetName(MCEContext, "Ki Power: Diamond Resilience");
            a.SetDescription(MCEContext, "A monk gains DR 2/—. At 16th level, the damage reduction increases to 4/—. At 19th level, it increases to DR 6/—.");
            a.m_Icon = damageReductionIcon;
            a.AddComponent<SpecificBuffImmunity>(c =>
            {
                c.m_Buff = GetBPRef<BlueprintBuffReference>("a1aecb0c003a49b9ae385035875f1b92"); // DLC3_HasteIslandStacks
            });
            a.AddPrerequisite<PrerequisiteClassLevel>(c =>
            {
                c.m_CharacterClass = monkClassRef;
                c.Level = 14;
            });
            a.AddComponent<TTAddDamageResistancePhysical>(c =>
            {
                c.Value = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                {
                    ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                };
                c.Pool = new();
                c.BypassedByMaterial = false;
                c.BypassedByForm = false;
                c.BypassedByMagic = false;
                c.BypassedByAlignment = false;
                c.BypassedByReality = false;
                c.BypassedByWeaponType = false;
                c.BypassedByMeleeWeapon = false;
                c.BypassedByEpic = false;
                c.SourceIsClassFeature = true;
            });
            a.AddComponent<ContextRankConfig>(c =>
            {
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Progression = ContextRankProgression.Custom;
                c.m_Class = [monkClassRef];
                c.m_CustomProgression = [
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 15,
                        ProgressionValue = 2
                    },
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 18,
                        ProgressionValue = 4
                    },
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 100,
                        ProgressionValue = 6
                    }
                ];
            });

        });
        if (MCEContext.AddedContent.Feats.IsEnabled("MonkDiamondResilience"))
        {
            var diamondResilienceRef = diamondResilience.ToReference<BlueprintFeatureReference>();
            var monkKiPowerSelection = GetBP<BlueprintFeatureSelection>("3049386713ff04245a38b32483362551");
            var scaledFistKiPowerSelection = GetBP<BlueprintFeatureSelection>("4694f6ac27eaed34abb7d09ab67b4541");

            monkKiPowerSelection.m_AllFeatures = monkKiPowerSelection.m_AllFeatures.AppendToArray(diamondResilienceRef);
            scaledFistKiPowerSelection.m_AllFeatures = scaledFistKiPowerSelection.m_AllFeatures.AppendToArray(diamondResilienceRef);
        }
    }
}
