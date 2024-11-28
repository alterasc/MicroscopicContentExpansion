using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Spells;
public class WidenAuras
{

    public static BlueprintAbilityReference AddWidenAuras()
    {
        var icon = GetBP<BlueprintFeature>("b73bc1b252994e6582a644dd6a7f31dc").Icon;

        const string widenAurasName = "Widen Auras";
        const string widenAurasDesc = "The range of your antipaladin auras doubles.";

        var buff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "WidenAurasBuff", bp =>
        {
            bp.SetName(MCEContext, widenAurasName);
            bp.SetDescription(MCEContext, widenAurasDesc);
            bp.m_Icon = icon;
            bp.Frequency = DurationRate.Minutes;
            bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
        });

        var spell = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "WidenAurasAbility", bp =>
        {
            bp.SetName(MCEContext, widenAurasName);
            bp.SetDescription(MCEContext, widenAurasDesc);
            bp.m_Icon = icon;
            bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Extend | Metamagic.Heighten | Metamagic.CompletelyNormal;
            bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute/level");
            bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
            bp.AddComponent<SpellComponent>(c =>
            {
                c.School = SpellSchool.Transmutation;
            });
            bp.AddComponent<AbilityEffectRunAction>(c =>
            {
                c.AddAction<ContextActionApplyBuff>(b =>
                {
                    b.m_Buff = buff.ToReference<BlueprintBuffReference>();
                    b.DurationValue = new ContextDurationValue()
                    {
                        Rate = DurationRate.Minutes,
                        DiceCountValue = 0,
                        BonusValue = new ContextValue()
                        {
                            ValueType = ContextValueType.Rank
                        }
                    };
                    b.AsChild = false;
                    b.IsFromSpell = true;
                });
            });
            bp.Type = AbilityType.Spell;
            bp.Range = AbilityRange.Personal;
            bp.CanTargetFriends = true;
            bp.CanTargetSelf = true;
            bp.EffectOnAlly = AbilityEffectOnUnit.None;
            bp.EffectOnEnemy = AbilityEffectOnUnit.None;
            bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
            bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
        }).ToReference<BlueprintAbilityReference>();

        return spell;
    }
}