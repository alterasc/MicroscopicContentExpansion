using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;


namespace MicroscopicContentExpansion.NewContent.Archetypes.KnightoftheSepulcherFeatures;
internal class TouchoftheCrypt
{

    public static BlueprintFeatureReference AddTouchoftheCrypt()
    {
        var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

        var negativeEnergyAffinity = BlueprintTools.GetBlueprint<BlueprintFeature>("d5ee498e19722854198439629c1841a5");

        return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "KnightoftheSepulcherTouchoftheCrypt", bp =>
        {
            bp.SetName(MCEContext, "Touch of the Crypt");
            bp.SetDescription(MCEContext, "At 5th level, a knight of the sepulcher gains a +2 bonus on saving throws against " +
                "mind-affecting effects, death effects, and poison. He is harmed by positive energy effects and healed by negative" +
                " energy effects as though he were undead, although negative energy effects that don’t heal undead (such as " +
                "enervation) affect him normally. The knight of the sepulcher has a 25% chance of ignoring critical hits and the " +
                "bonus damage from sneak attacks as though he were wearing armor of light fortification.");
            bp.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[] {
                    negativeEnergyAffinity.ToReference<BlueprintUnitFactReference>()
                };
            });
            bp.AddComponent<AddFortification>(c =>
            {
                c.Bonus = 25;
            });
            bp.AddComponent<SavingThrowContextBonusAgainstDescriptor>(c =>
            {
                c.SpellDescriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Death | SpellDescriptor.Poison;
                c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
                c.Value = new ContextValue()
                {
                    ValueType = ContextValueType.Rank
                };
            });
            bp.AddComponent<ContextRankConfig>(c =>
            {
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Progression = ContextRankProgression.Custom;
                c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClassRef };
                c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 10,
                        ProgressionValue = 2
                    },
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 100,
                        ProgressionValue = 4
                    }
                };
            });

            bp.IsClassFeature = true;
        }).ToReference<BlueprintFeatureReference>();
    }
}
