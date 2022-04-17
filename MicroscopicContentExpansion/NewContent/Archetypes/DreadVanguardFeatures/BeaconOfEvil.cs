using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UI.UnitSettings.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using MicroscopicContentExpansion.Utils;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;
using static MicroscopicContentExpansion.Utils.ActionFlow;


namespace MicroscopicContentExpansion.NewContent.Archetypes.DreadVanguardFeatures {
    internal class BeaconOfEvil {
        const string NAME = "Beacon of Evil";

        const string DESCRIPTION = @"A dread vanguard unleashes the powers of his vile masters to strengthen both himself and his allies.
At 4th level and every 4 level thereafter, a dread vanguard gains one additional use of his touch of corruption ability per day. As a standard action, he can spend a use of his touch of corruption ability to manifest the darkness in his soul as an area of flickering shadows with a 30-foot radius centered on him. These shadows don’t affect visibility. The antipaladin and all allies in the area gain a +1 morale bonus to AC and on attack rolls, damage rolls, and saving throws against fear, and also ignore the first 5 points of hardness when attacking unattended inanimate objects. This lasts for 1 minute, as long as the dread vanguard is conscious.
At 8th level, the aura grants fast healing 3 to the dread vanguard as well as to his allies while they remain within it. Additionally, while this aura is active, the antipaladin can use his touch of corruption ability against any targets within its radius by making a ranged touch attack.
At 12th level, when he activates this ability, a dread vanguard can choose to increase the radius of one antipaladin aura he possesses to 30 feet. Also, the morale bonus granted to AC and on attack rolls, damage rolls, and saving throws against fear increases to +2.
At 16th level, the fast healing granted by this ability increases to 5. Additionally, the antipaladin’s weapons and those of his allies within the aura’s radius are considered evil for the purpose of overcoming damage reduction.
At 20th level, the beacon of evil’s radius increases to 60 feet, and the morale bonus granted to AC and on attack rolls, damage rolls, and saving throws against fear increases to +4. Lastly, attacks made by the dread vanguard and his allies within the aura’s radius are infused with pure unholy power, and deal an additional 1d6 points of damage.";

        public static BlueprintFeatureReference AddBeaconOfEvil() {
            var AntipaladinClass = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(MCEContext, "AntipaladinClass");
            var TouchOfCorruptionResource = BlueprintTools.GetModBlueprintReference<BlueprintAbilityResourceReference>(MCEContext, "AntipaladinTouchOfCorruptionResource");

            var icon = BlueprintTools.GetBlueprint<BlueprintAbility>("a02cf51787df937489ef5d4cf5970335").Icon;

            var beaconOfEvilAreaEffectBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "DreadVanguardBeaconOfEvilAreaEffectBuff", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = icon;
                bp.Stacking = StackingType.Replace;
                bp.IsClassFeature = true;
                bp.Frequency = DurationRate.Rounds;

                bp.AddComponent<SavingThrowContextBonusAgainstDescriptor>(c => {
                    c.SpellDescriptor = SpellDescriptor.Fear;
                    c.ModifierDescriptor = ModifierDescriptor.Morale;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice,
                    };
                });

                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Morale;
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.AC;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice,
                    };
                });


                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Morale;
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice,
                    };
                });

                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Morale;
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalDamage;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice,
                    };
                });

                bp.AddComponent<AddEffectContextFastHealing>(c => {
                    c.Bonus = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageBonus,
                    };
                });

                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_Type = AbilityRankType.DamageDice;
                    c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClass };
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 11,
                            ProgressionValue = 1
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 19,
                            ProgressionValue = 2
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 100,
                            ProgressionValue = 4
                        }
                    };
                });

                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_Type = AbilityRankType.DamageBonus;
                    c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClass };
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 7,
                            ProgressionValue = 0
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 15,
                            ProgressionValue = 3
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 100,
                            ProgressionValue = 5
                        }
                    };
                });

                bp.IsClassFeature = true;
            });

            var beaconOfEvilAreaEffect = Helpers.CreateBlueprint<BlueprintAbilityAreaEffect>(MCEContext, "DreadVanguardBeaconOfEvilAreaEffect", bp => {
                bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
                bp.Shape = AreaEffectShape.Cylinder;
                bp.Size = 30.Feet();
                bp.Fx = new PrefabLink();
                bp.AddComponent<AbilityAreaEffectBuff>(c => {
                    c.Condition = ActionFlow.IfSingle<ContextConditionIsAlly>();
                    c.m_Buff = beaconOfEvilAreaEffectBuff.ToReference<BlueprintBuffReference>();
                });
            });

            var beaconOfEvilBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "DreadVanguardBeaconOfEvilBuff", bp => {
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.Frequency = DurationRate.Rounds;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = beaconOfEvilAreaEffect.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });

            var beaconOfEvilAreaEffect20 = Helpers.CreateBlueprint<BlueprintAbilityAreaEffect>(MCEContext, "DreadVanguardBeaconOfEvilAreaEffect", bp => {
                bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
                bp.Shape = AreaEffectShape.Cylinder;
                bp.Size = 60.Feet();
                bp.Fx = new PrefabLink();
                bp.AddComponent<AbilityAreaEffectBuff>(c => {
                    c.Condition = ActionFlow.IfSingle<ContextConditionIsAlly>();
                    c.m_Buff = beaconOfEvilAreaEffectBuff.ToReference<BlueprintBuffReference>();
                });
            });

            var beaconOfEvilBuff20 = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "DreadVanguardBeaconOfEvilBuff", bp => {
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.Frequency = DurationRate.Rounds;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = beaconOfEvilAreaEffect20.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });

            var beacon20 = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "DreadVanguardBeaconOfEvilFeature20");

            var beaconOfEvilAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DreadVanguardBeaconOfEvilAbility", bp => {
                bp.SetName(MCEContext, $"{NAME}");
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = icon;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = TouchOfCorruptionResource;
                    c.m_IsSpendResource = true;
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = DoSingle<Conditional>(ac => {
                        ac.ConditionsChecker = IfSingle<ContextConditionCasterHasFact>(cond => {
                            cond.m_Fact = beacon20.ToReference<BlueprintUnitFactReference>();
                        });
                        ac.IfTrue = DoSingle<ContextActionApplyBuff>(cc => {
                            cc.m_Buff = beaconOfEvilBuff20.ToReference<BlueprintBuffReference>();
                            cc.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Minutes,
                                BonusValue = new ContextValue() {
                                    Value = 1
                                }
                            };
                        });
                        ac.IfFalse = DoSingle<ContextActionApplyBuff>(cc => {
                            cc.m_Buff = beaconOfEvilBuff.ToReference<BlueprintBuffReference>();
                            cc.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Minutes,
                                BonusValue = new ContextValue() {
                                    Value = 1
                                }
                            };
                        });
                    });
                });
            });

            return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DreadVanguardBeaconOfEvilFeature", bp => {
                bp.SetName(MCEContext, $"{NAME}");
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.IsClassFeature = true;
                bp.m_Icon = icon;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { beaconOfEvilAbility.ToReference<BlueprintUnitFactReference>() };
                });
            }).ToReference<BlueprintFeatureReference>();
        }
    }
}
