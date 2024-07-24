using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Utilities.SpellTools;

namespace MicroscopicContentExpansion.NewContent.Spells;
internal class BladeofDarkTriumph
{
    public static BlueprintAbilityReference AddBladeofDarkTriumph()
    {
        var icon = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("688d42200cbb2334c8e27191c123d18f").Icon;

        var ghostTouchEnchant = BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("47857e1a5a3ec1a46adf6491b1423b4f");

        var buff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "BladeofDarkTriumphBuff", bp =>
        {
            bp.SetName(MCEContext, "Blade of Dark Triumph");
            bp.SetDescription(MCEContext, "You strengthen the bond between your fiendish boon weapon and its unholy spirit. The weapon gains the ghost touch property.");
            bp.m_Icon = icon;
            bp.IsClassFeature = true;
            bp.AddComponent<BuffEnchantAnyWeapon>(c =>
            {
                c.m_EnchantmentBlueprint = ghostTouchEnchant;
                c.Slot = Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand;
            });
            bp.Stacking = StackingType.Replace;
            bp.Frequency = DurationRate.Minutes;
            bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
            bp.FxOnStart = new Kingmaker.ResourceLinks.PrefabLink();
            bp.FxOnRemove = new Kingmaker.ResourceLinks.PrefabLink();
        });

        var spell = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "BladeofDarkTriumph", bp =>
        {
            bp.SetName(MCEContext, "Blade of Dark Triumph");
            bp.SetDescription(MCEContext, "You strengthen the bond between your fiendish boon weapon and its unholy spirit. The weapon gains the ghost touch property.");
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
                        m_IsExtendable = true,
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
            bp.AddComponent<AbilityCasterHasFacts>(c =>
            {
                c.m_Facts = [
                   MCEContext.GetModBlueprintReference<BlueprintUnitFactReference>("AntipaladinWeaponBondProgression"),
                   MCEContext.GetModBlueprintReference<BlueprintUnitFactReference>("TyrantWeaponBondProgression")
                ];
                c.NeedsAll = false;
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

        AddBladeofBrightVictory();

        return spell;
    }

    public static void AddBladeofBrightVictory()
    {
        var icon = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("688d42200cbb2334c8e27191c123d18f").Icon;

        var ghostTouchEnchant = BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("47857e1a5a3ec1a46adf6491b1423b4f");

        var buff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "BladeofBrightVictoryBuff", bp =>
        {
            bp.SetName(MCEContext, "Blade of Bright Victory");
            bp.SetDescription(MCEContext, "You strengthen the bond between your divine bond weapon and its celestial spirit. The weapon gains the ghost touch property.");
            bp.m_Icon = icon;
            bp.IsClassFeature = true;
            bp.AddComponent<BuffEnchantAnyWeapon>(c =>
            {
                c.m_EnchantmentBlueprint = ghostTouchEnchant;
                c.Slot = Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand;
            });
            bp.Stacking = StackingType.Replace;
            bp.Frequency = DurationRate.Minutes;
            bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
            bp.FxOnStart = new Kingmaker.ResourceLinks.PrefabLink();
            bp.FxOnRemove = new Kingmaker.ResourceLinks.PrefabLink();
        });

        var spell = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "BladeofBrightVictory", bp =>
        {
            bp.SetName(MCEContext, "Blade of Bright Victory");
            bp.SetDescription(MCEContext, "You strengthen the bond between your divine bond weapon and its celestial spirit. The weapon gains the ghost touch property.");
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
                        m_IsExtendable = true,
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
            bp.AddComponent<AbilityCasterHasFacts>(c =>
            {
                c.m_Facts = [
                   BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("e08a817f475c8794aa56fdd904f43a57")
                ];
                c.NeedsAll = true;
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

        if (!MCEContext.AddedContent.Spells.IsEnabled("BladeofBrightVictory")) { return; }

        SpellTools.AddToSpellList(spell, SpellList.PaladinSpellList, 3);
    }
}