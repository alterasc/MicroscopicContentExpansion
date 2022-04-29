using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
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
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Spells {
    internal class DeadlyJuggernaut {
        public static BlueprintAbilityReference AddDeadlyJuggernaut() {
            var icon = BlueprintTools.GetBlueprint<BlueprintFeature>("8ec618121de114845981933a3d5c4b02").Icon;

            var statBonusBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "DeadlyJuggernautStatBonusBuff", bp => {
                bp.SetName(MCEContext, "Deadly Juggernaut");
                bp.SetDescription(MCEContext, "With every enemy life you take, you become increasingly dangerous and difficult to stop." +
                    " During the duration of the spell, you gain a cumulative +1 luck bonus on melee attack rolls, melee weapon damage " +
                    "rolls, Strength checks, and Strength-based skill checks as well as DR 2/— each time you reduce a qualifying opponent" +
                    " to 0 or few hit points (maximum +5 bonus and DR 10/—) with a melee attack. A qualifying opponent has a number of" +
                    " Hit Dice equal to or greater than your Hit Dice –4.");
                bp.m_Icon = icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Shared,
                        ValueShared = AbilitySharedValue.Damage,
                    };
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalDamage;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Shared,
                        ValueShared = AbilitySharedValue.Damage,
                    };
                });
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Value = new ContextValue() {
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

            var buff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "DeadlyJuggernautBuff", bp => {
                bp.SetName(MCEContext, "Deadly Juggernaut");
                bp.SetDescription(MCEContext, "With every enemy life you take, you become increasingly dangerous and difficult to stop." +
                    " During the duration of the spell, you gain a cumulative +1 luck bonus on melee attack rolls, melee weapon damage " +
                    "rolls, Strength checks, and Strength-based skill checks as well as DR 2/— each time you reduce a qualifying opponent" +
                    " to 0 or few hit points (maximum +5 bonus and DR 10/—) with a melee attack. A qualifying opponent has a number of" +
                    " Hit Dice equal to or greater than your Hit Dice –4.");
                bp.m_Icon = icon;
                bp.IsClassFeature = true;
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Progression = ContextRankProgression.BonusValue;
                    c.m_StepLevel = -4;
                });
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.WaitForAttackResolve = true;
                    c.OnlyHit = true;
                    c.CheckWeaponRangeType = true;
                    c.RangeType = WeaponRangeType.Melee;
                    c.ReduceHPToZero = true;
                    c.Action = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = ActionFlow.IfAll(
                                new ContextConditionCompare() {
                                    m_Type = ContextConditionCompare.Type.GreaterOrEqual,
                                    CheckValue = new ContextValue() {
                                        ValueType = ContextValueType.TargetProperty,
                                        Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.Level
                                    },
                                    TargetValue = new ContextValue() {
                                        ValueType = ContextValueType.Rank
                                    }
                                },
                                new ContextConditionCompare() {
                                    m_Type = ContextConditionCompare.Type.Less,
                                    CheckValue = new ContextValue() {
                                        ValueType = ContextValueType.Shared,
                                        ValueShared = AbilitySharedValue.Damage
                                    },
                                    TargetValue = 5
                                }
                            ),
                            IfTrue = Helpers.CreateActionList(
                                new ContextActionChangeSharedValue() {
                                    SharedValue = AbilitySharedValue.Damage,
                                    Type = SharedValueChangeType.Add,
                                    AddValue = 1
                                },
                                new ContextActionChangeSharedValue() {
                                    SharedValue = AbilitySharedValue.Heal,
                                    Type = SharedValueChangeType.Add,
                                    AddValue = 2
                                },
                                new ContextActionRemoveBuff() {
                                    m_Buff = statBonusBuff.ToReference<BlueprintBuffReference>(),
                                    ToCaster = true,
                                    OnlyFromCaster = true
                                },
                                new ContextActionApplyBuff() {
                                    m_Buff = statBonusBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue(),
                                    SameDuration = true,
                                    IsFromSpell = true,
                                    ToCaster = true
                                }
                            ),
                            IfFalse = ActionFlow.DoNothing()
                        }
                    );
                });
                bp.Stacking = StackingType.Replace;
                bp.Frequency = DurationRate.Minutes;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.FxOnStart = new Kingmaker.ResourceLinks.PrefabLink();
                bp.FxOnRemove = new Kingmaker.ResourceLinks.PrefabLink();
            });

            return Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DeadlyJuggernaut", bp => {
                bp.SetName(MCEContext, "Deadly Juggernaut");
                bp.SetDescription(MCEContext, "With every enemy life you take, you become increasingly dangerous and difficult to stop." +
                    " During the duration of the spell, you gain a cumulative +1 luck bonus on melee attack rolls, melee weapon damage " +
                    "rolls, Strength checks, and Strength-based skill checks as well as DR 2/— each time you reduce a qualifying opponent" +
                    " to 0 or few hit points (maximum +5 bonus and DR 10/—) with a melee attack. A qualifying opponent has a number of" +
                    " Hit Dice equal to or greater than your Hit Dice –4.");
                bp.m_Icon = icon;
                bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Extend | Metamagic.Heighten | Metamagic.CompletelyNormal;
                bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute/level");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Necromancy;
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.AddAction<ContextActionApplyBuff>(b => {
                        b.m_Buff = buff.ToReference<BlueprintBuffReference>();
                        b.DurationValue = new ContextDurationValue() {
                            Rate = DurationRate.Minutes,
                            m_IsExtendable = true,
                            DiceCountValue = 0,
                            BonusValue = new ContextValue() {
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
        }


    }
}
