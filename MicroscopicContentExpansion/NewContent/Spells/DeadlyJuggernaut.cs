using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using MicroscopicContentExpansion.Utils;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Core.Utilities.SpellTools;

namespace MicroscopicContentExpansion.NewContent.Spells;
internal class DeadlyJuggernaut
{
    public static BlueprintAbilityReference AddDeadlyJuggernaut()
    {
        var icon = BlueprintTools.GetBlueprint<BlueprintFeature>("8ec618121de114845981933a3d5c4b02").Icon;

        const string deadlyJuggDesc = "With every enemy life you take, you become increasingly dangerous and difficult to stop." +
                " During the duration of the spell, you gain a cumulative +1 luck bonus on melee attack rolls, melee weapon damage " +
                "rolls, Strength checks, and Strength-based skill checks as well as DR 2/— each time you reduce a qualifying opponent" +
                " to 0 or few hit points (maximum +5 bonus and DR 10/—) with a melee attack. A qualifying opponent has a number of" +
                " Hit Dice equal to or greater than your Hit Dice –4.";
        var statBonusBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "DeadlyJuggernautStatBonusBuff", bp =>
        {
            bp.SetName(MCEContext, "Deadly Juggernaut");
            bp.SetDescription(MCEContext, deadlyJuggDesc);
            bp.m_Icon = icon;
            bp.IsClassFeature = true;
            bp.AddComponent<AttackTypeAttackBonus>(c =>
            {
                c.Descriptor = ModifierDescriptor.Luck;
                c.Type = WeaponRangeType.Melee;
                c.AttackBonus = 1;
                c.Value = new ContextValue()
                {
                    ValueType = ContextValueType.Shared,
                    ValueShared = AbilitySharedValue.Damage,
                };
            });
            bp.AddComponent<WeaponAttackTypeDamageBonus>(c =>
            {
                c.Descriptor = ModifierDescriptor.Luck;
                c.Type = WeaponRangeType.Melee;
                c.AttackBonus = 1;
                c.Value = new ContextValue()
                {
                    ValueType = ContextValueType.Shared,
                    ValueShared = AbilitySharedValue.Damage,
                };
            });
            bp.AddComponent<AddDamageResistancePhysical>(c =>
            {
                c.Value = new ContextValue()
                {
                    ValueType = ContextValueType.Shared,
                    ValueShared = AbilitySharedValue.Heal,
                };
            });
            bp.Stacking = StackingType.Replace;
            bp.Frequency = DurationRate.Minutes;
            bp.m_Flags = BlueprintBuff.Flags.IsFromSpell | BlueprintBuff.Flags.HiddenInUi;
            bp.FxOnStart = new Kingmaker.ResourceLinks.PrefabLink();
            bp.FxOnRemove = new Kingmaker.ResourceLinks.PrefabLink();
        });

        var buff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "DeadlyJuggernautBuff", bp =>
        {
            bp.SetName(MCEContext, "Deadly Juggernaut");
            bp.SetDescription(MCEContext, deadlyJuggDesc);
            bp.m_Icon = icon;
            bp.IsClassFeature = true;
            bp.AddContextRankConfig(c =>
            {
                c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                c.m_Progression = ContextRankProgression.BonusValue;
                c.m_StepLevel = -4;
            });
            bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
            {
                c.OnlyHit = true;
                c.CheckWeaponRangeType = true;
                c.RangeType = WeaponRangeType.Melee;
                c.ReduceHPToZero = true;
                c.Action = ActionFlow.DoSingle<Conditional>(ac =>
                {
                    ac.ConditionsChecker = ActionFlow.IfAll(
                        new ContextConditionCompare()
                        {
                            m_Type = ContextConditionCompare.Type.GreaterOrEqual,
                            CheckValue = new ContextValue()
                            {
                                ValueType = ContextValueType.TargetProperty,
                                Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.Level
                            },
                            TargetValue = new ContextValue()
                            {
                                ValueType = ContextValueType.Rank
                            }
                        },
                        new ContextConditionCompare()
                        {
                            m_Type = ContextConditionCompare.Type.Less,
                            CheckValue = new ContextValue()
                            {
                                ValueType = ContextValueType.Shared,
                                ValueShared = AbilitySharedValue.Damage
                            },
                            TargetValue = 5
                        }
                    );
                    ac.IfTrue = Helpers.CreateActionList(
                        new ContextActionChangeSharedValue()
                        {
                            SharedValue = AbilitySharedValue.Damage,
                            Type = SharedValueChangeType.Add,
                            AddValue = 1
                        },
                        new ContextActionChangeSharedValue()
                        {
                            SharedValue = AbilitySharedValue.Heal,
                            Type = SharedValueChangeType.Add,
                            AddValue = 2
                        },
                        new ContextActionRemoveBuff()
                        {
                            m_Buff = statBonusBuff.ToReference<BlueprintBuffReference>(),
                            ToCaster = true,
                            OnlyFromCaster = true
                        },
                        new ContextActionApplyBuff()
                        {
                            m_Buff = statBonusBuff.ToReference<BlueprintBuffReference>(),
                            DurationValue = new ContextDurationValue(),
                            SameDuration = true,
                            IsFromSpell = true,
                            ToCaster = true
                        }
                    );
                    ac.IfFalse = ActionFlow.DoNothing();
                });
            });
            bp.Stacking = StackingType.Replace;
            bp.Frequency = DurationRate.Minutes;
            bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
            bp.FxOnStart = new Kingmaker.ResourceLinks.PrefabLink();
            bp.FxOnRemove = new Kingmaker.ResourceLinks.PrefabLink();
        });

        AddKiDeadlyJuggernaut(icon, buff);

        var spell = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DeadlyJuggernaut", bp =>
        {
            bp.SetName(MCEContext, "Deadly Juggernaut");
            bp.SetDescription(MCEContext, deadlyJuggDesc);
            bp.m_Icon = icon;
            bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Extend | Metamagic.Heighten | Metamagic.CompletelyNormal;
            bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute/level");
            bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
            bp.AddComponent<SpellComponent>(c =>
            {
                c.School = SpellSchool.Necromancy;
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
            bp.Type = AbilityType.Spell;
            bp.Range = AbilityRange.Personal;
            bp.CanTargetFriends = true;
            bp.CanTargetSelf = true;
            bp.EffectOnAlly = AbilityEffectOnUnit.None;
            bp.EffectOnEnemy = AbilityEffectOnUnit.None;
            bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
            bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
        }).ToReference<BlueprintAbilityReference>();

        if (MCEContext.AddedContent.Spells.IsEnabled("DeadlyJuggernaut"))
        {
            SpellTools.AddToSpellList(spell, SpellList.ClericSpellList, 3);
            SpellTools.AddToSpellList(spell, SpellList.InquisitorSpellList, 3);
            SpellTools.AddToSpellList(spell, SpellList.PaladinSpellList, 3);
            SpellTools.AddToSpellList(spell, SpellList.WarpriestSpelllist, 3);
        }

        return spell;
    }

    private static void AddKiDeadlyJuggernaut(UnityEngine.Sprite icon, BlueprintBuff buff)
    {
        var monkClassRef = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("e8f21e5b58e0569468e420ebea456124");

        const string kiDeadlyJuggernautDescription = "A monk with this ki power can spend 2 points from his ki pool as a standard action to grant himself Deadly Juggernaut buff: \nWith every enemy life you take, you become increasingly dangerous and difficult to stop." +
                            " During the duration of the spell, you gain a cumulative +1 luck bonus on melee attack rolls, melee weapon damage " +
                            "rolls, Strength checks, and Strength-based skill checks as well as DR 2/— each time you reduce a qualifying opponent" +
                            " to 0 or few hit points (maximum +5 bonus and DR 10/—) with a melee attack. A qualifying opponent has a number of" +
                            " Hit Dice equal to or greater than your Hit Dice –4.";

        var ability = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "KiDeadlyJuggernautAbility", bp =>
        {
            bp.SetName(MCEContext, "Ki Power: Deadly Juggernaut");
            bp.SetDescription(MCEContext, kiDeadlyJuggernautDescription);
            bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute/level");
            bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
            bp.m_Icon = icon;
            bp.CanTargetSelf = true;
            bp.Range = AbilityRange.Personal;
            bp.AddComponent<AbilityEffectRunAction>(c =>
            {
                c.Actions = ActionFlow.DoSingle<ContextActionApplyBuff>(a =>
                {
                    a.m_Buff = buff.ToReference<BlueprintBuffReference>();
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
            bp.AddComponent<AbilityResourceLogic>(c =>
            {
                c.m_RequiredResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("9d9c90a9a1f52d04799294bf91c80a82");
                c.m_IsSpendResource = true;
                c.Amount = 2;
            });
            bp.AddContextRankConfig(c =>
            {
                c.m_Type = AbilityRankType.Default;
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Progression = ContextRankProgression.AsIs;
                c.m_Class = new BlueprintCharacterClassReference[] { monkClassRef };
            });
        });

        var feature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "KiDeadlyJuggernautFeature", bp =>
        {
            bp.SetName(MCEContext, "Ki Power: Deadly Juggernaut");
            bp.SetDescription(MCEContext, kiDeadlyJuggernautDescription);
            bp.IsClassFeature = true;
            bp.AddPrerequisite<PrerequisiteClassLevel>(c =>
            {
                c.m_CharacterClass = monkClassRef;
                c.Level = 8;
            });
            bp.m_Icon = icon;
            bp.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[] { ability.ToReference<BlueprintUnitFactReference>() };
            });
        });

        var sfAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "ScaledFistKiDeadlyJuggernautAbility", bp =>
        {
            bp.SetName(MCEContext, "Ki Power: Deadly Juggernaut");
            bp.SetDescription(MCEContext, kiDeadlyJuggernautDescription);
            bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute/level");
            bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
            bp.m_Icon = icon;
            bp.CanTargetSelf = true;
            bp.Range = AbilityRange.Personal;
            bp.AddComponent<AbilityEffectRunAction>(c =>
            {
                c.Actions = ActionFlow.DoSingle<ContextActionApplyBuff>(a =>
                {
                    a.m_Buff = buff.ToReference<BlueprintBuffReference>();
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
            bp.AddComponent<AbilityResourceLogic>(c =>
            {
                c.m_RequiredResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("7d002c1025fbfe2458f1509bf7a89ce1");
                c.m_IsSpendResource = true;
                c.Amount = 2;
            });
            bp.AddContextRankConfig(c =>
            {
                c.m_Type = AbilityRankType.Default;
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Progression = ContextRankProgression.AsIs;
                c.m_Class = new BlueprintCharacterClassReference[] { monkClassRef };
            });
        });

        var sfFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "ScaledFistKiDeadlyJuggernautFeature", bp =>
        {
            bp.SetName(MCEContext, "Ki Power: Deadly Juggernaut");
            bp.SetDescription(MCEContext, kiDeadlyJuggernautDescription);
            bp.IsClassFeature = true;
            bp.AddPrerequisite<PrerequisiteClassLevel>(c =>
            {
                c.m_CharacterClass = monkClassRef;
                c.Level = 8;
            });
            bp.m_Icon = icon;
            bp.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[] { sfAbility.ToReference<BlueprintUnitFactReference>() };
            });
        });

        var drunkenKiAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DrunkenKiDeadlyJuggernautAbility", bp =>
        {
            bp.SetName(MCEContext, "Ki Power: Deadly Juggernaut");
            bp.SetDescription(MCEContext, kiDeadlyJuggernautDescription);
            bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute/level");
            bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
            bp.m_Icon = icon;
            bp.CanTargetSelf = true;
            bp.Range = AbilityRange.Personal;
            bp.AddComponent<AbilityEffectRunAction>(c =>
            {
                c.Actions = ActionFlow.DoSingle<ContextActionApplyBuff>(a =>
                {
                    a.m_Buff = buff.ToReference<BlueprintBuffReference>();
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
            bp.AddComponent<AbilityResourceLogic>(c =>
            {
                c.m_RequiredResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("fd01f3f969a04febab7877a17aebb812");
                c.m_IsSpendResource = true;
                c.Amount = 2;
            });
            bp.AddContextRankConfig(c =>
            {
                c.m_Type = AbilityRankType.Default;
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Progression = ContextRankProgression.AsIs;
                c.m_Class = new BlueprintCharacterClassReference[] { monkClassRef };
            });
        });

        var drunkenKiFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DrunkenKiDeadlyJuggernautFeature", bp =>
        {
            bp.SetName(MCEContext, "Ki Power: Deadly Juggernaut");
            bp.SetDescription(MCEContext, kiDeadlyJuggernautDescription);
            bp.IsClassFeature = true;
            bp.AddPrerequisite<PrerequisiteClassLevel>(c =>
            {
                c.m_CharacterClass = monkClassRef;
                c.Level = 8;
            });
            bp.m_Icon = icon;
            bp.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = [sfAbility.ToReference<BlueprintUnitFactReference>()];
            });
        });

        var monkKiPowerSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("3049386713ff04245a38b32483362551");
        var sfKiPowerSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("4694f6ac27eaed34abb7d09ab67b4541");
        var drunkenKiPowerSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("97da13a43026460f8d4d54e1c69af202");

        if (MCEContext.AddedContent.Spells.IsEnabled("DeadlyJuggernaut"))
        {
            monkKiPowerSelection.AddFeatures(feature);
            sfKiPowerSelection.AddFeatures(sfFeature);
            drunkenKiPowerSelection.AddFeatures(drunkenKiFeature);
        }
    }

}