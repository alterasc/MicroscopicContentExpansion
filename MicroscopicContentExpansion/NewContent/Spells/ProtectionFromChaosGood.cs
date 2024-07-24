using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Spells;
internal class ProtectionFromChaosGood
{
    public static BlueprintAbilityReference AddProtectionFromChaosGood()
    {
        var protectionFromChaos = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("1eaf1020e82028d4db55e6e464269e00");
        var protectionFromGood = BlueprintTools.GetBlueprint<BlueprintAbility>("2ac7637daeb2aa143a3bae860095b63e");

        return Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "ProtectionFromChaosGood", bp =>
        {
            bp.SetName(MCEContext, "Protection from Chaos/Good");
            bp.SetDescription(MCEContext, "The subject gains a +2 deflection {g|Encyclopedia:Bonus}bonus{/g}" +
                " to {g|Encyclopedia:Armor_Class}AC{/g} and a +2 resistance bonus on {g|Encyclopedia:Saving_Throw" +
                "}saves{/g}. Both these bonuses apply against {g|Encyclopedia:Attack}attacks{/g} made or effects" +
                " created by creatures with the corresponding {g|Encyclopedia:Alignment}alignment{/g}.");
            bp.m_Icon = protectionFromGood.Icon;
            bp.CanTargetFriends = true;
            bp.EffectOnAlly = AbilityEffectOnUnit.None;
            bp.EffectOnEnemy = AbilityEffectOnUnit.None;
            bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch;
            bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
            bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Extend | Metamagic.Heighten | Metamagic.Reach | Metamagic.CompletelyNormal;
            bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute/level");
            bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
            bp.AddComponent<SpellComponent>(c =>
            {
                c.School = SpellSchool.Abjuration;
            });
            bp.AddComponent<AbilityVariants>(c =>
            {
                c.m_Variants = [
                    protectionFromChaos,
                    protectionFromGood.ToReference<BlueprintAbilityReference>()
                ];
            });
        }).ToReference<BlueprintAbilityReference>();
    }

    public static BlueprintAbilityReference AddProtectionFromChaosGoodCommunal()
    {
        var protectionFromChaosCommunal = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("0ec75ec95d9e39d47a23610123ba1bad");
        var protectionFromGoodCommunal = BlueprintTools.GetBlueprint<BlueprintAbility>("5bfd4cce1557d5744914f8f6d85959a4");

        return Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "ProtectionFromChaosGoodCommunal", bp =>
        {
            bp.SetName(MCEContext, "Protection from Chaos/Good - Communal");
            bp.SetDescription(MCEContext, "All allies within 30 feet gain a +2 deflection {g|Encyclopedia:Bonus}bonus{/g} " +
                "to {g|Encyclopedia:Armor_Class}AC{/g} and a +2 resistance bonus on {g|Encyclopedia:Saving_Throw}saves{/g}." +
                " Both these bonuses apply against {g|Encyclopedia:Attack}attacks{/g} made or effects created by creatures" +
                " with corresponding {g|Encyclopedia:Alignment}alignments{/g}.");
            bp.m_Icon = protectionFromGoodCommunal.Icon;
            bp.CanTargetFriends = true;
            bp.EffectOnAlly = AbilityEffectOnUnit.None;
            bp.EffectOnEnemy = AbilityEffectOnUnit.None;
            bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch;
            bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
            bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Extend | Metamagic.Heighten | Metamagic.Reach | Metamagic.CompletelyNormal;
            bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute");
            bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
            bp.AddComponent<SpellComponent>(c =>
            {
                c.School = SpellSchool.Abjuration;
            });
            bp.AddComponent<AbilityVariants>(c =>
            {
                c.m_Variants = [
                    protectionFromChaosCommunal,
                    protectionFromGoodCommunal.ToReference<BlueprintAbilityReference>()
                ];
            });
            bp.AddComponent<AbilityTargetsAround>(c =>
            {
                c.m_Radius = 30.Feet();
                c.m_TargetType = TargetType.Ally;
            });
            bp.AddComponent<CraftInfoComponent>(c =>
            {
                c.SpellType = CraftSpellType.Other;
                c.SavingThrow = CraftSavingThrow.None;
                c.AOEType = CraftAOE.AOE;
            });
        }).ToReference<BlueprintAbilityReference>();
    }
}
