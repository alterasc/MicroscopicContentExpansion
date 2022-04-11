using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Base.Main;

namespace MicroscopicContentExpansion.Base.NewContent.AntipaladinFeatures {
    internal class Cruelties {

        private const string DESCRIPTION = "At 3rd level, and every three levels thereafter, an antipaladin" +
            " can select one cruelty. Each cruelty adds an effect to the antipaladin’s touch of corruption ability. " +
            "Whenever the antipaladin uses touch of corruption to deal damage to one target, the target also receives" +
            " the additional effect from one of the cruelties possessed by the antipaladin. This choice is made when" +
            " the touch is used. The target receives a Fortitude save to avoid this cruelty. If the save is successful," +
            " the target takes the damage as normal, but not the effects of the cruelty. The DC of this save is equal to" +
            " 10 + 1/2 the antipaladin’s level + the antipaladin’s Charisma modifier";

        private static void AddFatiquedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var FatiguedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("e6f2fc5d73d88064583cb828801212f4");
            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyFatiquedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Fatiqued");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is fatigued.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = FatiguedBuff.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
            });

            var Action = new ContextActionSavingThrow() {
                m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0],
                Type = SavingThrowType.Fortitude,
                HasCustomDC = false,
                CustomDC = new ContextValue(),
                Actions = Helpers.CreateActionList(new ContextActionConditionalSaved() {
                    Succeed = new ActionList(),
                    Failed = Helpers.CreateActionList(
                            new ContextActionApplyBuff() {
                                m_Buff = FatiguedBuff.ToReference<BlueprintBuffReference>(),
                                Permanent = true,
                                DurationValue = new ContextDurationValue() {
                                    m_IsExtendable = true,
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue()
                                },
                                IsFromSpell = true,
                            }),
                })
            };
            var Descriptor = Helpers.Create<SpellDescriptorComponent>(c => {
                c.Descriptor = SpellDescriptor.Fatigue;
            });

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionFatiqued",
                "Touch of Corruption - Fatiqued",
                "Applies Touch of Corruption with Fatiqued cruelty.\nOn failed saving throw the target is fatigued.",
                FatiguedBuff.Icon,
                Action,
                null,
                Descriptor,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        private static void AddShakenCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(MCEContext, "AntipaladinClass");

            var ShakenBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("25ec6cb6ab1845c48a95f9c20b034220");

            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyShakenFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Shaken");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is shaken for 1 round per level of the antipaladin.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = ShakenBuff.Icon;
                bp.Ranks = 1;

                bp.IsClassFeature = true;
            });

            var Action = new ContextActionSavingThrow() {
                m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0],
                Type = SavingThrowType.Fortitude,
                HasCustomDC = false,
                CustomDC = new ContextValue(),
                Actions = Helpers.CreateActionList(new ContextActionConditionalSaved() {
                    Succeed = new ActionList(),
                    Failed = Helpers.CreateActionList(
                            new ContextActionApplyBuff() {
                                m_Buff = ShakenBuff.ToReference<BlueprintBuffReference>(),
                                DurationValue = new ContextDurationValue() {
                                    m_IsExtendable = true,
                                    DiceCountValue = 0,
                                    BonusValue = new ContextValue() {
                                        ValueType = ContextValueType.Rank,
                                        ValueRank = AbilityRankType.DamageDice,
                                    }
                                },
                                IsFromSpell = true,
                            }),
                })
            };

            var ContextVarConfig = Helpers.Create<ContextRankConfig>(c => {
                c.m_Type = AbilityRankType.DamageDice;
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClassRef };
                c.m_Progression = ContextRankProgression.AsIs;
                c.m_Max = 20;
            });

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionShaken",
                "Touch of Corruption - Shaken",
                "Applies Touch of Corruption with Shaken cruelty.\nOn failed saving throw the " +
                "target is shaken for 1 round per level of the antipaladin.",
                ShakenBuff.Icon,
                Action,
                ContextVarConfig,
                null,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        private static void AddSickenedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(MCEContext, "AntipaladinClass");

            var SickenedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4e42460798665fd4cb9173ffa7ada323");

            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltySickenedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Sickened");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is sickened for 1 round per level of the antipaladin.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = SickenedBuff.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
            });

            var Action = new ContextActionSavingThrow() {
                m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0],
                Type = SavingThrowType.Fortitude,
                HasCustomDC = false,
                CustomDC = new ContextValue(),
                Actions = Helpers.CreateActionList(new ContextActionConditionalSaved() {
                    Succeed = new ActionList(),
                    Failed = Helpers.CreateActionList(
                            new ContextActionApplyBuff() {
                                m_Buff = SickenedBuff.ToReference<BlueprintBuffReference>(),
                                DurationValue = new ContextDurationValue() {
                                    m_IsExtendable = true,
                                    DiceCountValue = 0,
                                    BonusValue = new ContextValue() {
                                        ValueType = ContextValueType.Rank,
                                        ValueRank = AbilityRankType.DamageDice,
                                    }
                                },
                                IsFromSpell = true,
                            }),
                })
            };

            var ContextVarConfig = Helpers.Create<ContextRankConfig>(c => {
                c.m_Type = AbilityRankType.DamageDice;
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClassRef };
                c.m_Progression = ContextRankProgression.AsIs;
                c.m_Max = 20;
            });

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionSickened",
                "Touch of Corruption - Sickened",
                "Applies Touch of Corruption with Sickened cruelty.\nOn failed saving throw the " +
                "target is sickened for 1 round per level of the antipaladin.",
                SickenedBuff.Icon,
                Action,
                ContextVarConfig,
                null,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        public static void AddCruelties() {

            AddFatiquedCruelty(out var FatiquedCrueltyFeaure, out var FatiquedCrueltyAbility);
            AddShakenCruelty(out var ShakenCrueltyFeaure, out var ShakenCrueltyAbility);
            AddSickenedCruelty(out var SickenedCrueltyFeaure, out var SickenedCrueltyAbility);

            var BaseAbility = BlueprintTools.GetModBlueprint<BlueprintAbility>(MCEContext, "AntipaladinTouchOfCorruptionBase");
            var variants = BaseAbility.GetComponent<AbilityVariants>();
            variants.m_Variants = variants.m_Variants.AppendToArray(
                new BlueprintAbilityReference[] {
                    FatiquedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    ShakenCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    SickenedCrueltyAbility.ToReference<BlueprintAbilityReference>()
                }
            );

            var CrueltySelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(MCEContext, "AntipaladinCrueltySelection", bp => {
                bp.SetName(MCEContext, "Cruelty");
                bp.SetDescription(MCEContext, "At 3rd level, and every three levels thereafter, an antipaladin can select one cruelty." +
                    " Each cruelty adds an effect to the antipaladin’s touch of corruption ability. Whenever the antipaladin uses " +
                    "touch of corruption to deal damage to one target, the target also receives the additional effect from one of the" +
                    " cruelties possessed by the antipaladin. This choice is made when the touch is used. The target receives a " +
                    "Fortitude save to avoid this cruelty. If the save is successful, the target takes the damage as normal, but not" +
                    " the effects of the cruelty. The DC of this save is equal to 10 + 1/2 the antipaladin’s level + the antipaladin’s" +
                    " Charisma modifier.");
                bp.m_AllFeatures = new BlueprintFeatureReference[] {
                                FatiquedCrueltyFeaure.ToReference<BlueprintFeatureReference>(),
                                ShakenCrueltyFeaure.ToReference<BlueprintFeatureReference>(),
                                SickenedCrueltyFeaure.ToReference<BlueprintFeatureReference>()
                            };
                bp.Mode = SelectionMode.Default;
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.IsClassFeature = true;
            });
        }
    }
}
