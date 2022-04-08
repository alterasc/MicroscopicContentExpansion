using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Base.Main;

namespace MicroscopicContentExpansion.Base.NewContent.AntipaladinFeatures {
    internal class TouchofCorruption {
        public static void AddTouchofCorruption() {
            var AbsoluteDeathAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("7d721be6d74f07f4d952ee8d6f8f44a0");
            var AntipaladinClassRef = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(MCEContext, "AntipaladinClass");
            var BestowCurseSpell = BlueprintTools.GetBlueprint<BlueprintAbility>("989ab5c44240907489aba0a8568d0603");

            var TouchOfCorruptionResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(MCEContext, "AntipaladinTouchOfCorruptionResource", bp => {
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 1,
                    m_Class = new BlueprintCharacterClassReference[1] { AntipaladinClassRef },
                    m_ClassDiv = new BlueprintCharacterClassReference[1] { AntipaladinClassRef },
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                    IncreasedByLevelStartPlusDivStep = true,
                    StartingLevel = 2,
                    LevelStep = 2,
                    PerStepIncrease = 1,
                    StartingIncrease = 0,
                    IncreasedByStat = true,
                    ResourceBonusStat = StatType.Charisma
                };
                bp.m_Max = 20;
            });

            var NegativeEnergyAffinity = BlueprintTools.GetBlueprint<BlueprintFeature>("d5ee498e19722854198439629c1841a5");
            var TouchItem = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("bb337517547de1a4189518d404ec49d4");

            var TouchOfCorruptionAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "AntipaladinTouchOfCorruptionAbility", bp => {
                bp.SetName(MCEContext, "Touch of Corruption");
                bp.SetDescription(MCEContext, "Beginning at 2nd level, an antipaladin surrounds his hand with a fiendish flame," +
                    " causing terrible wounds to open on those he touches. Each day he can use this ability a number of times" +
                    " equal to 1/2 his antipaladin level + his Charisma modifier. As a touch attack, an antipaladin can cause" +
                    " 1d6 points of damage for every two antipaladin levels he possesses. Using this ability is a standard action" +
                    " that does not provoke attacks of opportunity.\nAlternatively, an antipaladin can use this power to heal" +
                    " undead creatures, restoring 1d6 hit points for every two levels the antipaladin possesses.This ability is" +
                    " modified by any feat, spell, or effect that specifically works with the lay on hands paladin class feature." +
                    " For example, the Extra Lay On Hands feat grants an antipaladin 2 additional uses of the touch of corruption" +
                    " class feature.");
                bp.m_Icon = BestowCurseSpell.Icon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Touch;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "Instant");
                bp.LocalizedSavingThrow = Helpers.CreateString(MCEContext, $"{bp.name}.SavingThrow", "None");
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = TouchOfCorruptionResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>();
                    c.ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference>();
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = Kingmaker.Enums.AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClassRef };
                    c.m_Progression = ContextRankProgression.Div2;
                });
                bp.AddComponent<AbilityDeliverTouch>(c => {
                    c.m_TouchWeapon = TouchItem.ToReference<BlueprintItemWeaponReference>();

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
                                          Value = 0,
                                          ValueRank = Kingmaker.Enums.AbilityRankType.Default,
                                          ValueType = ContextValueType.Rank
                                      },
                                      BonusValue = new ContextValue()
                                  }
                              }),
                          IfFalse = Helpers.CreateActionList(
                                                new ContextActionDealDamage() {
                                                    DamageType = new DamageTypeDescription() {
                                                        Type = DamageType.Energy,
                                                        Common = new DamageTypeDescription.CommomData(),
                                                        Physical = new DamageTypeDescription.PhysicalData(),
                                                        Energy = Kingmaker.Enums.Damage.DamageEnergyType.NegativeEnergy,
                                                    },
                                                    Duration = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        DiceCountValue = new ContextValue(),
                                                        BonusValue = new ContextValue(),
                                                    },
                                                    Value = new ContextDiceValue() {
                                                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                                                        DiceCountValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank,
                                                        },
                                                        BonusValue = new ContextValue(),
                                                    }
                                                })

                      });
                });

                bp.AddComponent<AbilityEffectMiss>();


                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = AbsoluteDeathAbility.GetComponent<AbilitySpawnFx>().PrefabLink;
                    c.Anchor = AbsoluteDeathAbility.GetComponent<AbilitySpawnFx>().Anchor;
                    c.PositionAnchor = AbsoluteDeathAbility.GetComponent<AbilitySpawnFx>().PositionAnchor;
                    c.OrientationAnchor = AbsoluteDeathAbility.GetComponent<AbilitySpawnFx>().OrientationAnchor;
                });

                bp.AddComponent<AbilityCasterAlignment>(c => {
                    c.Alignment = Kingmaker.UnitLogic.Alignments.AlignmentMaskType.Evil;
                });

            });

            var TouchOfCorruptionBase = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "AntipaladinTouchOfCorruptionBase", bp => {
                bp.SetName(MCEContext, "Touch of Corruption");
                bp.SetDescription(MCEContext, "Beginning at 2nd level, an antipaladin surrounds his hand with a fiendish flame," +
                    " causing terrible wounds to open on those he touches. Each day he can use this ability a number of times" +
                    " equal to 1/2 his antipaladin level + his Charisma modifier. As a touch attack, an antipaladin can cause" +
                    " 1d6 points of damage for every two antipaladin levels he possesses. Using this ability is a standard action" +
                    " that does not provoke attacks of opportunity.\nAlternatively, an antipaladin can use this power to heal" +
                    " undead creatures, restoring 1d6 hit points for every two levels the antipaladin possesses.This ability is" +
                    " modified by any feat, spell, or effect that specifically works with the lay on hands paladin class feature." +
                    " For example, the Extra Lay On Hands feat grants an antipaladin 2 additional uses of the touch of corruption" +
                    " class feature.");
                bp.m_Icon = BestowCurseSpell.Icon;
                bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "Instant");
                bp.LocalizedSavingThrow = Helpers.CreateString(MCEContext, $"{bp.name}.SavingThrow", "None");
                bp.AddComponent<AbilityVariants>(c => {
                    c.m_Variants = new BlueprintAbilityReference[] { TouchOfCorruptionAbility.ToReference<BlueprintAbilityReference>() };
                });
            }
            );

            var TouchOfCorruptionFact = Helpers.CreateBlueprint<BlueprintUnitFact>(MCEContext, "AntipaladinTouchOfCorruptionFact", bp => {
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = TouchOfCorruptionResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
            });

            var TouchOfCorruptionFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinTouchOfCorruptionFeature", bp => {
                bp.SetName(MCEContext, "Touch of Corruption");
                bp.SetDescription(MCEContext, "Beginning at 2nd level, an antipaladin surrounds his hand with a fiendish flame," +
                    " causing terrible wounds to open on those he touches. Each day he can use this ability a number of times" +
                    " equal to 1/2 his antipaladin level + his Charisma modifier. As a touch attack, an antipaladin can cause" +
                    " 1d6 points of damage for every two antipaladin levels he possesses. Using this ability is a standard action" +
                    " that does not provoke attacks of opportunity.\nAlternatively, an antipaladin can use this power to heal" +
                    " undead creatures, restoring 1d6 hit points for every two levels the antipaladin possesses.This ability is" +
                    " modified by any feat, spell, or effect that specifically works with the lay on hands paladin class feature." +
                    " For example, the Extra Lay On Hands feat grants an antipaladin 2 additional uses of the touch of corruption" +
                    " class feature.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = BestowCurseSpell.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        TouchOfCorruptionBase.ToReference<BlueprintUnitFactReference>(),
                        TouchOfCorruptionFact.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });

            var TouchOfCorruptionUse = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinTouchOfCorruptionAdditionalUse", bp => {
                bp.SetName(MCEContext, "Touch of Corruption - Additional Use");
                bp.SetDescription(MCEContext, "Beginning at 2nd level, an antipaladin surrounds his hand with a fiendish flame," +
                    " causing terrible wounds to open on those he touches. Each day he can use this ability a number of times" +
                    " equal to 1/2 his antipaladin level + his Charisma modifier. As a touch attack, an antipaladin can cause" +
                    " 1d6 points of damage for every two antipaladin levels he possesses. Using this ability is a standard action" +
                    " that does not provoke attacks of opportunity.\nAlternatively, an antipaladin can use this power to heal" +
                    " undead creatures, restoring 1d6 hit points for every two levels the antipaladin possesses.This ability is" +
                    " modified by any feat, spell, or effect that specifically works with the lay on hands paladin class feature." +
                    " For example, the Extra Lay On Hands feat grants an antipaladin 2 additional uses of the touch of corruption" +
                    " class feature.");
                bp.Ranks = 10;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = TouchOfCorruptionResource.ToReference<BlueprintAbilityResourceReference>();
                    c.Value = 1;
                });
            });
            var ExtraLayOnHands = BlueprintTools.GetBlueprint<BlueprintFeature>("a2b2f20dfb4d3ed40b9198e22be82030");
            ExtraLayOnHands.AddComponent<IncreaseResourceAmount>(c => {
                c.m_Resource = TouchOfCorruptionResource.ToReference<BlueprintAbilityResourceReference>();
                c.Value = 2;
            });
        }
    }
}
