using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;
using static MicroscopicContentExpansion.Utils.ActionFlow;

namespace MicroscopicContentExpansion.NewContent.Antipaladin {
    internal class SmiteGood {

        public static void AddSmiteGood() {

            var FiendishSmiteGoodBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("a9035e49d6d79a64eaec321f2cb629a8");

            var SmiteGoodIcon = BlueprintTools.GetBlueprint<BlueprintBuff>("114af78efc58e5a4c86bb12ee1d907cc").Icon;

            var SmiteGoodBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinSmiteGoodBuff", bp => {
                bp.SetName(MCEContext, "Smite Good");
                bp.SetDescription(MCEContext, "Once per day, an antipaladin can call " +
                    "out to the dark powers to crush the forces of good. As a swift action, the antipaladin chooses one target within sight to " +
                    "smite. If this target is good, the antipaladin adds his Charisma bonus (if any) on his attack rolls and adds his antipaladin" +
                    " level on all damage rolls made against the target of his smite. If the target of smite good is an outsider with the good" +
                    " subtype, a good-aligned dragon, or a good creature with levels of cleric or paladin, the bonus to damage on the first" +
                    " successful attack increases to 2 points of damage per level the antipaladin possesses. Regardless of the target, smite good" +
                    " attacks automatically bypass any DR the creature might possess.In addition, while smite good is in effect, the antipaladin" +
                    " gains a deflection bonus equal to his Charisma modifier(if any) to his AC against attacks made by the target of the smite." +
                    " If the antipaladin targets a creature that is not good, the smite is wasted with no effect.");
                bp.m_Icon = SmiteGoodIcon;
                bp.IsClassFeature = true;
                bp.Stacking = StackingType.Stack;
                bp.FxOnStart = FiendishSmiteGoodBuff.FxOnStart;
                bp.FxOnRemove = FiendishSmiteGoodBuff.FxOnRemove;
                bp.AddComponent<AttackBonusAgainstTarget>(c => {
                    c.Descriptor = (ModifierDescriptor)TabletopTweaks.Core.MechanicsChanges.AdditionalModifierDescriptors.Untyped.Charisma;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Shared,
                        ValueShared = AbilitySharedValue.StatBonus
                    };
                    c.CheckCaster = true;
                });
                bp.AddComponent<DamageBonusAgainstTarget>(c => {
                    c.CheckCaster = true;
                    c.ApplyToSpellDamage = true;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Shared,
                        ValueShared = AbilitySharedValue.DamageBonus
                    };
                });
                bp.AddComponent<ACBonusAgainstTarget>(c => {
                    c.CheckCaster = true;
                    c.Descriptor = ModifierDescriptor.Deflection;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Shared,
                        ValueShared = AbilitySharedValue.StatBonus
                    };


                });
                bp.AddComponent<RemoveBuffIfCasterIsMissing>(c => {
                    c.RemoveOnCasterDeath = true;
                });
                bp.AddComponent<IgnoreTargetDR>(c => {
                    c.CheckCaster = true;
                });
                bp.AddComponent<UniqueBuff>();


            });
            var AntipaladinClassRef = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(MCEContext, "AntipaladinClass");

            var SmiteGoodResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(MCEContext, "AntipaladinSmiteGoodResource", bp => {
                bp.m_Min = 1;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 1,
                    IncreasedByStat = false,
                    IncreasedByLevel = false,
                };
                bp.m_Max = 10;
            });

            AntipaladinFeatures.TipoftheSpear.AddTipoftheSpear();

            var TipoftheSpear = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(MCEContext, "AntipaladinTipoftheSpear");

            var FiendishSmiteGoodAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("478cf0e6c5f3a4142835faeae3bd3e04");
            var SmiteGoodAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "AntipaladinSmiteGoodAbility", bp => {
                bp.SetName(MCEContext, "Smite Good");
                bp.SetDescription(MCEContext, "Once per day, an antipaladin can call " +
                    "out to the dark powers to crush the forces of good. As a swift action, the antipaladin chooses one target within sight to " +
                    "smite. If this target is good, the antipaladin adds his Charisma bonus (if any) on his attack rolls and adds his antipaladin" +
                    " level on all damage rolls made against the target of his smite. If the target of smite good is an outsider with the good" +
                    " subtype, a good-aligned dragon, or a good creature with levels of cleric or paladin, the bonus to damage on the first" +
                    " successful attack increases to 2 points of damage per level the antipaladin possesses. Regardless of the target, smite good" +
                    " attacks automatically bypass any DR the creature might possess.In addition, while smite good is in effect, the antipaladin" +
                    " gains a deflection bonus equal to his Charisma modifier(if any) to his AC against attacks made by the target of the smite." +
                    " If the antipaladin targets a creature that is not good, the smite is wasted with no effect. The smite good effect remains" +
                    " until the target of the smite is dead or the next time the antipaladin rests and regains his uses of this ability. " +
                    "At 4th level, and at every three levels thereafter, the antipaladin may smite good one additional time per day, " +
                    "as indicated on Table: Antipaladin, to a maximum of seven times per day at 19th level.");
                bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "Until the target of Smite Good is dead");
                bp.LocalizedSavingThrow = Helpers.CreateString(MCEContext, $"{bp.name}.SavingThrow", "None");
                bp.m_Icon = SmiteGoodIcon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Medium;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = false;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Point;
                bp.ActionType = UnitCommand.CommandType.Swift;
                bp.AvailableMetamagic = Metamagic.Heighten | Metamagic.Reach;
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = DoSingle<Conditional>(cond => {
                        cond.ConditionsChecker = IfAll(
                            new OrAndLogic() {
                                ConditionsChecker = IfAny(
                                    new ContextConditionAlignment() {
                                        CheckCaster = false,
                                        Alignment = AlignmentComponent.Good
                                    },
                                    new ContextConditionCasterHasFact() {
                                        m_Fact = TipoftheSpear
                                    }
                                )
                            },
                            new ContextConditionHasBuffFromCaster() {
                                m_Buff = SmiteGoodBuff.ToReference<BlueprintBuffReference>(),
                                Not = true
                            }
                        );
                        cond.IfTrue = DoSingle<ContextActionApplyBuff>(apb => {
                            apb.m_Buff = SmiteGoodBuff.ToReference<BlueprintBuffReference>();
                            apb.Permanent = true;
                            apb.DurationValue = new ContextDurationValue() {
                                m_IsExtendable = true,
                                DiceCountValue = new ContextValue(),
                                BonusValue = new ContextValue()
                            };
                        });
                        cond.IfFalse = DoNothing();
                    });
                });
                bp.AddComponent<ContextCalculateSharedValue>(c => {
                    c.ValueType = AbilitySharedValue.StatBonus;
                    c.Value = new ContextDiceValue() {
                        DiceCountValue = new ContextValue(),
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
                        DiceCountValue = new ContextValue(),
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageBonus,
                            ValueShared = AbilitySharedValue.DamageBonus,
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
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = SmiteGoodResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>();
                    c.ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference>();
                });

                bp.AddComponent<AbilityCasterAlignment>(c => {
                    c.Alignment = Kingmaker.UnitLogic.Alignments.AlignmentMaskType.Evil;
                });
            });


            var SmiteGoodFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinSmiteGoodFeature", bp => {
                bp.SetName(MCEContext, "Smite Good");
                bp.SetDescription(MCEContext, "Once per day, an antipaladin can call " +
                    "out to the dark powers to crush the forces of good. As a swift action, the antipaladin chooses one target within sight to " +
                    "smite. If this target is good, the antipaladin adds his Charisma bonus (if any) on his attack rolls and adds his antipaladin" +
                    " level on all damage rolls made against the target of his smite. If the target of smite good is an outsider with the good" +
                    " subtype, a good-aligned dragon, or a good creature with levels of cleric or paladin, the bonus to damage on the first" +
                    " successful attack increases to 2 points of damage per level the antipaladin possesses. Regardless of the target, smite good" +
                    " attacks automatically bypass any DR the creature might possess.In addition, while smite good is in effect, the antipaladin" +
                    " gains a deflection bonus equal to his Charisma modifier(if any) to his AC against attacks made by the target of the smite." +
                    " If the antipaladin targets a creature that is not good, the smite is wasted with no effect. The smite good effect remains" +
                    " until the target of the smite is dead or the next time the antipaladin rests and regains his uses of this ability. " +
                    "At 4th level, and at every three levels thereafter, the antipaladin may smite good one additional time per day, " +
                    "as indicated on Table: Antipaladin, to a maximum of seven times per day at 19th level.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = SmiteGoodIcon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;

                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = SmiteGoodResource.ToReference<BlueprintAbilityResourceReference>();
                });
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        SmiteGoodAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
            });
            var AntipaladinSmiteGoodAdditionalUse = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinSmiteGoodAdditionalUse", bp => {
                bp.SetName(MCEContext, "Smite Good - Additional Use");
                bp.SetDescription(MCEContext, "At 4th level, and at every three levels thereafter, the antipaladin may smite good one additional time" +
                    " per day, to a maximum of seven times per day at 19th level.");
                bp.m_Icon = SmiteGoodIcon;
                bp.Ranks = 10;
                bp.IsClassFeature = true;
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = SmiteGoodResource.ToReference<BlueprintAbilityResourceReference>();
                    c.Value = 1;
                });
            });

        }
    }
}
