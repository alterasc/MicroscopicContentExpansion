using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Base.Main;

namespace MicroscopicContentExpansion.Base.NewContent.AntipaladinFeatures {
    internal class ChannelNegativeEnergy {
        private const string NAME = "Channel Negative Energy";
        private const string DESCRIPTION = "When an antipaladin reaches 4th level, he gains the supernatural ability to" +
            " channel negative energy like a cleric. Using this ability consumes two uses of his touch of corruption ability." +
            " An antipaladin uses his level as his effective cleric level when channeling negative energy." +
            " This is a Charisma-based ability.";

        public static void AddChannelNegativeEnergy() {

            BlueprintUnitFact ChannelEnergyFact = BlueprintTools.GetBlueprint<BlueprintUnitFact>("93f062bc0bf70e84ebae436e325e30e8");
            BlueprintAbility ChannelNegativeEnergy = BlueprintTools.GetBlueprint<BlueprintAbility>("89df18039ef22174b81052e2e419c728");
            BlueprintAbilityResource ChannelEnergyResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("5e2bba3e07c37be42909a12945c27de7");
            BlueprintUnitProperty MythicChannelProperty = BlueprintTools.GetBlueprint<BlueprintUnitProperty>("152e61de154108d489ff34b98066c25c");
            BlueprintFeature SelectiveChannel = BlueprintTools.GetBlueprint<BlueprintFeature>("fd30c69417b434d47b6b03b9c1f568ff");
            BlueprintFeature ExtraChannel = BlueprintTools.GetBlueprint<BlueprintFeature>("cd9f19775bd9d3343a31a065e93f0c47");

            var TouchOfCorruptionResource = BlueprintTools.GetModBlueprint<BlueprintAbilityResource>(MCEContext, "AntipaladinTouchOfCorruptionResource");
            var AntipaladinClass = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(MCEContext, "AntipaladinClass");
            var NegativeEnergyAffinity = BlueprintTools.GetBlueprint<BlueprintFeature>("d5ee498e19722854198439629c1841a5");

            var ChannelNegativeEnergyAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "AntipaladinChannelNegativeEnergyAbility", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = ChannelNegativeEnergy.Icon;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AvailableMetamagic = Metamagic.Empower | Metamagic.Maximize | Metamagic.Heighten | Metamagic.Quicken;
                bp.Range = AbilityRange.Personal;
                bp.Type = AbilityType.Special;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.ResourceAssetIds = ChannelNegativeEnergy.ResourceAssetIds;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = TouchOfCorruptionResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 2;
                    c.ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>();
                    c.ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference>();
                });
                bp.AddComponent<AbilityTargetsAround>(c => {
                    c.m_Radius = 30.Feet();
                    c.m_TargetType = TargetType.Any;
                    c.m_Condition = new ConditionsChecker() {
                        Conditions = new Condition[0]
                    };
                });
                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = ChannelNegativeEnergy.GetComponent<AbilitySpawnFx>().PrefabLink;
                    c.PositionAnchor = AbilitySpawnFxAnchor.None;
                    c.OrientationAnchor = AbilitySpawnFxAnchor.None;
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = new SpellDescriptorWrapper(SpellDescriptor.ChannelNegativeHarm);
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageDice;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 1;
                    c.m_StepLevel = 2;
                    c.m_Min = 1;
                    c.m_UseMin = true;
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    c.m_CustomProperty = MythicChannelProperty.ToReference<BlueprintUnitPropertyReference>();
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_Min = 0;
                    c.m_UseMin = true;
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                  new ContextConditionHasFact() {
                                      m_Fact = NegativeEnergyAffinity.ToReference<BlueprintUnitFactReference>()
                                  }
                              }
                            },
                            IfTrue = Helpers.CreateActionList(
                              new ContextActionHealTarget() {
                                  Value = new ContextDiceValue() {
                                      DiceType = Kingmaker.RuleSystem.DiceType.D6,
                                      DiceCountValue = new ContextValue() {
                                          ValueType = ContextValueType.Rank
                                      },
                                      BonusValue = new ContextValue()
                                  }
                              }),
                            IfFalse = Helpers.CreateActionList(),
                        },

                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                    new ContextConditionCasterHasFact() {
                                        m_Fact = SelectiveChannel.ToReference<BlueprintUnitFactReference>()
                                    }
                                }
                            },
                            IfTrue = Helpers.CreateActionList(
                                new Conditional() {
                                    ConditionsChecker = new ConditionsChecker() {
                                        Conditions = new Condition[] {
                                            new ContextConditionIsEnemy()
                                        }
                                    },
                                    IfTrue = Helpers.CreateActionList(
                                        new ContextActionSavingThrow() {
                                            m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0],
                                            Type = SavingThrowType.Will,
                                            CustomDC = new ContextValue(),
                                            Actions = Helpers.CreateActionList(
                                                new ContextActionDealDamage() {
                                                    DamageType = new DamageTypeDescription() {
                                                        Type = DamageType.Direct,
                                                        Common = new DamageTypeDescription.CommomData(),
                                                        Physical = new DamageTypeDescription.PhysicalData(),
                                                    },
                                                    Duration = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        DiceCountValue = new ContextValue(),
                                                        BonusValue = new ContextValue()
                                                    },
                                                    Value = new ContextDiceValue() {
                                                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                                                        DiceCountValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank,
                                                            ValueRank = AbilityRankType.DamageDice
                                                        },
                                                        BonusValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank,
                                                            ValueRank = AbilityRankType.DamageBonus
                                                        }
                                                    },
                                                    IsAoE = true,
                                                    HalfIfSaved = true
                                                }
                                            )
                                        }
                                    ),
                                    IfFalse = Helpers.CreateActionList()
                                }
                            ),
                            IfFalse = Helpers.CreateActionList(
                                new Conditional() {
                                    ConditionsChecker = new ConditionsChecker() {
                                        Conditions = new Condition[] {
                                            new ContextConditionIsCaster(){
                                                Not = true
                                            }
                                        }
                                    },
                                    IfTrue = Helpers.CreateActionList(
                                        new ContextActionSavingThrow() {
                                            m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0],
                                            Type = SavingThrowType.Will,
                                            CustomDC = new ContextValue(),
                                            Actions = Helpers.CreateActionList(
                                                new ContextActionDealDamage() {
                                                    DamageType = new DamageTypeDescription() {
                                                        Type = DamageType.Direct,
                                                        Common = new DamageTypeDescription.CommomData(),
                                                        Physical = new DamageTypeDescription.PhysicalData(),
                                                    },
                                                    Duration = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        DiceCountValue = new ContextValue(),
                                                        BonusValue = new ContextValue()
                                                    },
                                                    Value = new ContextDiceValue() {
                                                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                                                        DiceCountValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank,
                                                            ValueRank = AbilityRankType.DamageDice
                                                        },
                                                        BonusValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank,
                                                            ValueRank = AbilityRankType.DamageBonus
                                                        }
                                                    },
                                                    IsAoE = true,
                                                    HalfIfSaved = true
                                                }
                                            )
                                        }
                                    ),
                                    IfFalse = Helpers.CreateActionList()
                                }
                            )
                        }
                    );
                });
            });
            var ChannelNegativeEnergyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinChannelNegativeEnergyFeature", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = ChannelNegativeEnergy.Icon;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        ChannelEnergyFact.ToReference<BlueprintUnitFactReference>(),
                        ChannelNegativeEnergyAbility.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });
            SelectiveChannel.AddPrerequisiteFeature(ChannelNegativeEnergyFeature, Prerequisite.GroupType.Any);
            ExtraChannel.AddPrerequisiteFeature(ChannelNegativeEnergyFeature, Prerequisite.GroupType.Any);
        }
    }
}
