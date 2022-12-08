using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using MicroscopicContentExpansion.Utils;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;
using static TabletopTweaks.Core.MechanicsChanges.AdditionalModifierDescriptors;

namespace MicroscopicContentExpansion.NewContent.Antipaladin {
    internal class AuraofVengeance {

        private const string NAME = "Mark of Vengeance";
        private const string DESCRIPTION = "At 11th level, an antipaladin can expend two uses of his smite good ability to grant" +
            " the ability to smite good to all allies within 10 feet, using his bonuses. Allies must use this smite good ability" +
            " by the start of the antipaladin’s next turn and the bonuses last for 1 minute. Using this ability is a free action." +
            " Good creatures gain no benefit from this ability.";

        public static void AddAuraofVengeance() {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var TipoftheSpear = MCEContext.GetModBlueprintReference<BlueprintUnitFactReference>("AntipaladinTipoftheSpear");

            var FingerOfDeathIcon = BlueprintTools.GetBlueprint<BlueprintAbility>("6f1dcf6cfa92d1948a740195707c0dbe").Icon;

            var SmiteGoodResource = BlueprintTools.GetModBlueprint<BlueprintAbilityResource>(MCEContext, "AntipaladinSmiteGoodResource");
            var FiendishSmiteGoodBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("a9035e49d6d79a64eaec321f2cb629a8");

            var AuraOfVengeanceBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraofVengeanceBuff", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.IsClassFeature = true;
                bp.Stacking = StackingType.Stack;
                bp.m_Icon = FingerOfDeathIcon;
                bp.FxOnStart = FiendishSmiteGoodBuff.FxOnStart;
                bp.FxOnRemove = FiendishSmiteGoodBuff.FxOnRemove;
                bp.AddComponent<AttackBonusAgainstTarget>(c => {
                    c.Descriptor = (ModifierDescriptor)Untyped.Charisma;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Shared,
                        ValueShared = AbilitySharedValue.StatBonus
                    };
                    c.CheckCasterFriend = true;
                });
                bp.AddComponent<DamageBonusAgainstTarget>(c => {
                    c.CheckCasterFriend = true;
                    c.ApplyToSpellDamage = true;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Shared,
                        ValueShared = AbilitySharedValue.DamageBonus
                    };
                });
                bp.AddComponent<ACBonusAgainstTarget>(c => {
                    c.Descriptor = ModifierDescriptor.Deflection;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Shared,
                        ValueShared = AbilitySharedValue.StatBonus
                    };
                    c.CheckCasterFriend = true;

                });
                bp.AddComponent<IgnoreTargetDR>(c => {
                    c.CheckCasterFriend = true;
                });
                bp.AddComponent<UniqueBuff>();
            });

            var FiendishSmiteGoodAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("478cf0e6c5f3a4142835faeae3bd3e04");

            var AuraofVengeanceAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "AntipaladinAuraofVengeanceAbility", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute");
                bp.LocalizedSavingThrow = Helpers.CreateString(MCEContext, $"{bp.name}.SavingThrow", "None");
                bp.m_Icon = FingerOfDeathIcon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Medium;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = false;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Point;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.AvailableMetamagic = Metamagic.Heighten | Metamagic.Reach;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = SmiteGoodResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 2;
                    c.ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>();
                    c.ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference>();
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = ActionFlow.DoSingle<Conditional>(cond => {
                        cond.ConditionsChecker = ActionFlow.IfAll(
                                        new OrAndLogic() {
                                            ConditionsChecker = ActionFlow.IfAny(
                                                    new ContextConditionAlignment() {
                                                        CheckCaster = false,
                                                        Alignment = AlignmentComponent.Good
                                                    },
                                                    new ContextConditionCasterHasFact() {
                                                        m_Fact = TipoftheSpear
                                                    }
                                                )
                                        },
                                        new ContextConditionHasBuff() {
                                            m_Buff = AuraOfVengeanceBuff.ToReference<BlueprintBuffReference>(),
                                            Not = true
                                        }
                                    );
                        cond.IfTrue = ActionFlow.DoSingle<ContextActionApplyBuff>(bf => {
                            bf.m_Buff = AuraOfVengeanceBuff.ToReference<BlueprintBuffReference>();
                            bf.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Minutes,
                                DiceType = Kingmaker.RuleSystem.DiceType.Zero,
                                m_IsExtendable = true,
                                DiceCountValue = 0,
                                BonusValue = 1
                            };
                        });
                        cond.IfFalse = ActionFlow.DoNothing();
                    });

                });
                bp.AddComponent<ContextCalculateSharedValue>(c => {
                    c.ValueType = AbilitySharedValue.StatBonus;
                    c.Value = new ContextDiceValue() {
                        DiceCountValue = 0,
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Rank,
                            ValueShared = AbilitySharedValue.StatBonus
                        },
                    };
                    c.Modifier = 1;
                });
                bp.AddComponent<ContextCalculateSharedValue>(c => {
                    c.ValueType = AbilitySharedValue.DamageBonus;
                    c.Value = new ContextDiceValue() {
                        DiceCountValue = 0,
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageBonus
                        },
                    };
                    c.Modifier = 1;
                });
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.DamageBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClassRef };
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_Max = 20;
                });
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.StatBonus;
                    c.m_Stat = StatType.Charisma;
                    c.m_Max = 20;
                });

                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = FiendishSmiteGoodAbility.GetComponent<AbilitySpawnFx>().PrefabLink;
                    c.Anchor = FiendishSmiteGoodAbility.GetComponent<AbilitySpawnFx>().Anchor;
                    c.PositionAnchor = FiendishSmiteGoodAbility.GetComponent<AbilitySpawnFx>().PositionAnchor;
                    c.OrientationAnchor = FiendishSmiteGoodAbility.GetComponent<AbilitySpawnFx>().OrientationAnchor;
                });

                bp.AddComponent<AbilityCasterAlignment>(c => {
                    c.Alignment = Kingmaker.UnitLogic.Alignments.AlignmentMaskType.Evil;
                });
            });

            var AuraOfAbsolutionFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAuraOfVengeanceFeature", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.m_Icon = FingerOfDeathIcon;

                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                            AuraofVengeanceAbility.ToReference<BlueprintUnitFactReference>(),
                        };
                });

            });
        }
    }
}
