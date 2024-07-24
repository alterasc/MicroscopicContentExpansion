using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using MicroscopicContentExpansion.Utils;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Spells;
internal class KiLeech
{
    internal static void AddKiLeech()
    {
        var icon = BlueprintTools.GetBlueprint<BlueprintAbility>("1cde0691195feae45bab5b83ea3f221e").Icon;

        const string KiLeechName = "Ki Leech";
        const string KiLeechDescription = "You place your spirit in a receptive state so when you confirm a critical hit against a living enemy or reduce a living enemy to 0 or fewer hit points, you can steal some of that creature’s ki. This replenishes 1 point of ki as long as you have at least 1 ki point in your ki pool. This does not allow you to exceed your ki pool’s maximum. This ability does not stack with similar abilities (such as the steal ki ability of the hungry ghost monk). This spell has no effect if you do not have a ki pool.";

        var kiBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "KiLeechBuff", bp =>
        {
            bp.SetName(MCEContext, KiLeechName);
            bp.SetDescription(MCEContext, KiLeechDescription);
            bp.m_Icon = icon;
            bp.IsClassFeature = true;
            var actions = ActionFlow.DoSingle<Conditional>(ac =>
            {
                ac.ConditionsChecker = ActionFlow.IfAll( //not undead, not construct
                    new ContextConditionHasFact()
                    {
                        m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("734a29b693e9ec346ba2951b27987e33"),
                        Not = true,
                    },
                    new ContextConditionHasFact()
                    {
                        m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("fd389783027d63343b4a5634bd81645f"),
                        Not = true
                    }
                );
                ac.IfTrue = ActionFlow.DoSingle<ContextActionOnContextCaster>(bc =>
                {
                    bc.Actions = ActionFlow.DoSingle<ContextRestoreResource>(cc =>
                    {
                        cc.m_Resource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("9d9c90a9a1f52d04799294bf91c80a82");
                        cc.Value = 1;
                    });
                });
                ac.IfFalse = ActionFlow.DoNothing();
            });
            bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
            {
                c.OnlyHit = true;
                c.CriticalHit = true;
                c.Action = actions;
            });
            bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
            {
                c.OnlyHit = true;
                c.NotCriticalHit = true;
                c.ReduceHPToZero = true;
                c.Action = actions;
            });
            bp.Stacking = StackingType.Replace;
            bp.Frequency = DurationRate.Minutes;
            bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
            bp.FxOnStart = new Kingmaker.ResourceLinks.PrefabLink();
            bp.FxOnRemove = new Kingmaker.ResourceLinks.PrefabLink();
        });

        var sfBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "ScaledFistLeechBuff", bp =>
        {
            bp.SetName(MCEContext, KiLeechName);
            bp.SetDescription(MCEContext, KiLeechDescription);
            bp.m_Icon = icon;
            bp.IsClassFeature = true;
            var actions = ActionFlow.DoSingle<Conditional>(ac =>
            {
                ac.ConditionsChecker = ActionFlow.IfAll( //not undead, not construct
                    new ContextConditionHasFact()
                    {
                        m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("734a29b693e9ec346ba2951b27987e33"),
                        Not = true,
                    },
                    new ContextConditionHasFact()
                    {
                        m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("fd389783027d63343b4a5634bd81645f"),
                        Not = true
                    }
                );
                ac.IfTrue = ActionFlow.DoSingle<ContextActionOnContextCaster>(bc =>
                {
                    bc.Actions = ActionFlow.DoSingle<ContextRestoreResource>(cc =>
                    {
                        cc.m_Resource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("7d002c1025fbfe2458f1509bf7a89ce1");
                        cc.Value = 1;
                    });
                });
                ac.IfFalse = ActionFlow.DoNothing();
            });
            bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
            {
                c.OnlyHit = true;
                c.CriticalHit = true;
                c.Action = actions;
            });
            bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
            {
                c.OnlyHit = true;
                c.NotCriticalHit = true;
                c.ReduceHPToZero = true;
                c.Action = actions;
            });
            bp.Stacking = StackingType.Replace;
            bp.Frequency = DurationRate.Minutes;
            bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
            bp.FxOnStart = new Kingmaker.ResourceLinks.PrefabLink();
            bp.FxOnRemove = new Kingmaker.ResourceLinks.PrefabLink();
        });

        var monkClassRef = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("e8f21e5b58e0569468e420ebea456124");


        var ability = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "KiLeechAbility", bp =>
        {
            bp.SetName(MCEContext, KiLeechName);
            bp.SetDescription(MCEContext, KiLeechDescription);
            bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute/level");
            bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
            bp.m_Icon = icon;
            bp.CanTargetSelf = true;
            bp.Range = AbilityRange.Personal;
            bp.AddComponent<AbilityEffectRunAction>(c =>
            {
                c.Actions = ActionFlow.DoSingle<ContextActionApplyBuff>(a =>
                {
                    a.m_Buff = kiBuff.ToReference<BlueprintBuffReference>();
                    a.Permanent = false;
                    a.DurationValue = new ContextDurationValue()
                    {
                        Rate = DurationRate.Minutes,
                        DiceCountValue = 0,
                        BonusValue = new ContextValue()
                        {
                            ValueType = ContextValueType.Rank
                        }
                    };
                    a.AsChild = true;
                });
            });
            bp.AddComponent<SpellComponent>(c =>
            {
                c.School = SpellSchool.Necromancy;
            });
            bp.AddContextRankConfig(c =>
            {
                c.m_Type = AbilityRankType.Default;
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Progression = ContextRankProgression.AsIs;
                c.m_Class = [monkClassRef];
            });
        });

        var feature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "KiLeechFeature", bp =>
        {
            bp.SetName(MCEContext, KiLeechName);
            bp.SetDescription(MCEContext, KiLeechDescription);
            bp.IsClassFeature = true;
            bp.AddPrerequisite<PrerequisiteClassLevel>(c =>
            {
                c.m_CharacterClass = monkClassRef;
                c.Level = 10;
            });
            bp.m_Icon = icon;
            bp.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = [ability.ToReference<BlueprintUnitFactReference>()];
            });
        });

        var sfAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "ScaledFistLeechAbility", bp =>
        {
            bp.SetName(MCEContext, KiLeechName);
            bp.SetDescription(MCEContext, KiLeechDescription);
            bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute/level");
            bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
            bp.m_Icon = icon;
            bp.CanTargetSelf = true;
            bp.Range = AbilityRange.Personal;
            bp.AddComponent<AbilityEffectRunAction>(c =>
            {
                c.Actions = ActionFlow.DoSingle<ContextActionApplyBuff>(a =>
                {
                    a.m_Buff = sfBuff.ToReference<BlueprintBuffReference>();
                    a.Permanent = false;
                    a.DurationValue = new ContextDurationValue()
                    {
                        Rate = DurationRate.Minutes,
                        DiceCountValue = 0,
                        BonusValue = new ContextValue()
                        {
                            ValueType = ContextValueType.Rank
                        }
                    };
                    a.AsChild = true;
                });
            });
            bp.AddComponent<SpellComponent>(c =>
            {
                c.School = SpellSchool.Necromancy;
            });
            bp.AddContextRankConfig(c =>
            {
                c.m_Type = AbilityRankType.Default;
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Progression = ContextRankProgression.AsIs;
                c.m_Class = [monkClassRef];
            });
        });

        var sfFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "ScaledFistLeechFeature", bp =>
        {
            bp.SetName(MCEContext, KiLeechName);
            bp.SetDescription(MCEContext, KiLeechDescription);
            bp.IsClassFeature = true;
            bp.AddPrerequisite<PrerequisiteClassLevel>(c =>
            {
                c.m_CharacterClass = monkClassRef;
                c.Level = 10;
            });
            bp.m_Icon = icon;
            bp.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = [sfAbility.ToReference<BlueprintUnitFactReference>()];
            });
        });

        var monkKiPowerSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("3049386713ff04245a38b32483362551");
        var sfKiPowerSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("4694f6ac27eaed34abb7d09ab67b4541");

        monkKiPowerSelection.AddFeatures(feature);
        sfKiPowerSelection.AddFeatures(sfFeature);
    }
}
