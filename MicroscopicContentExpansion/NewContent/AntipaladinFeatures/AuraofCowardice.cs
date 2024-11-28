using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using MicroscopicContentExpansion.NewComponents;
using MicroscopicContentExpansion.Utils;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures;
internal class AuraofCowardice
{
    private const string NAME = "Aura of Cowardice";
    private const string DESCRIPTION = "At 3rd level, an antipaladin radiates a palpably daunting aura that causes all enemies" +
                " within 10 feet to take a –4 penalty on saving throws against fear effects. Creatures that are normally immune to" +
                " fear lose that immunity while within 10 feet of an antipaladin with this ability. This ability functions only" +
                " while the antipaladin remains conscious, not if he is unconscious or dead.";

    public static void AddAuraOfCowardiceFeature()
    {
        var AOCIcon = GetBP<BlueprintAbility>("d2aeac47450c76347aebbc02e4f463e0").Icon;
        var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

        var AuraOfCowardiceEffectBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfCowardiceEffectBuff", bp =>
        {
            bp.SetName(MCEContext, NAME);
            bp.SetDescription(MCEContext, DESCRIPTION);
            bp.m_Icon = AOCIcon;

            bp.AddComponent<SavingThrowContextBonusAgainstDescriptor>(c =>
            {
                c.SpellDescriptor = SpellDescriptor.Fear;
                c.ModifierDescriptor = ModifierDescriptor.Penalty;
                c.Value = new ContextValue()
                {
                    ValueType = ContextValueType.Rank
                };
            });

            bp.AddComponent<ContextRankConfig>(c =>
            {
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Progression = ContextRankProgression.Custom;
                c.m_Class = [AntipaladinClassRef];
                c.m_CustomProgression = [
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 7,
                        ProgressionValue = -4
                    },
                    new ContextRankConfig.CustomProgressionItem() {
                        BaseValue = 100,
                        ProgressionValue = -2
                    }
                ];
            });

            bp.AddComponent<SpellDescriptorImmunityIgnore>(c =>
            {
                c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened;
            });

            bp.AddComponent<BuffDescriptorImmunityIgnore>(c =>
            {
                c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened;
            });
        });


        var AuraOfCowardiceArea = AuraUtils.CreateUnconditionalHostileAuraEffect(
            modContext: MCEContext,
            bpName: "AntipaladinAuraOfCowardiceArea",
            size: 13,
            buff: AuraOfCowardiceEffectBuff.ToReference<BlueprintBuffReference>()
        );

        var AuraOfCowardiceWidenArea = AuraUtils.CreateUnconditionalHostileAuraEffect(
            modContext: MCEContext,
            bpName: "AntipaladinAuraOfCowardiceWidenArea",
            size: 22,
            buff: AuraOfCowardiceEffectBuff.ToReference<BlueprintBuffReference>()
        );

        var AuraOfCowardiceBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfCowardiceBuff", bp =>
        {
            bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            bp.AddComponent<AddAreaEffect>(c =>
            {
                c.m_AreaEffect = AuraOfCowardiceArea.ToReference<BlueprintAbilityAreaEffectReference>();
            });
        });

        var AuraOfCowardiceWidenBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfCowardiceWidenBuff", bp =>
        {
            bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            bp.AddComponent<AddAreaEffect>(c =>
            {
                c.m_AreaEffect = AuraOfCowardiceWidenArea.ToReference<BlueprintAbilityAreaEffectReference>();
            });
        });

        var AuraOfCowardiceFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAuraOfCowardiceFeature", bp =>
        {
            bp.SetName(MCEContext, NAME);
            bp.SetDescription(MCEContext, DESCRIPTION);
            bp.m_Icon = AOCIcon;
            bp.AddComponent<AuraFeatureComponentWithWiden>(c =>
            {
                c.DefaultBuff = AuraOfCowardiceBuff.ToReference<BlueprintBuffReference>();
                c.WidenFact = MCEContext.GetModBlueprintReference<BlueprintUnitFactReference>("WidenAurasBuff");
                c.WidenBuff = AuraOfCowardiceWidenBuff.ToReference<BlueprintBuffReference>();
            });
        });
    }
}
