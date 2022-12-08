using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using MicroscopicContentExpansion.Utils;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures {
    internal class Cruelties {

        private const string DESCRIPTION = "At 3rd level, and every three levels thereafter, an antipaladin" +
            " can select one cruelty. Each cruelty adds an effect to the antipaladin’s touch of corruption ability. " +
            "Whenever the antipaladin uses touch of corruption to deal damage to one target, the target also receives" +
            " the additional effect from one of the cruelties possessed by the antipaladin. This choice is made when" +
            " the touch is used. The target receives a Fortitude save to avoid this cruelty. If the save is successful," +
            " the target takes the damage as normal, but not the effects of the cruelty. The DC of this save is equal to" +
            " 10 + 1/2 the antipaladin’s level + the antipaladin’s Charisma modifier";

        private static void BuildCruelty(
            BlueprintCharacterClassReference antipaladinClassRef,
            out BlueprintFeature crueltyFeature,
            out BlueprintAbility crueltyAbility,
            string name,
            string description,
            UnityEngine.Sprite icon,
            int prerequsiteLevel,
            BlueprintFeatureReference[] prerequisiteFeatures,
            GameAction[] actions,
            ContextRankConfig config,
            SpellDescriptor spellDescriptor
            ) {

            crueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCruelty" + name + "Feature", bp => {
                bp.SetName(MCEContext, "Cruelty - " + name);
                bp.SetDescription(MCEContext, DESCRIPTION + "\n" + description);
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                if (prerequsiteLevel > 0) {
                    bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                        c.m_CharacterClass = antipaladinClassRef;
                        c.Level = prerequsiteLevel;
                    });
                }
                if (prerequisiteFeatures != null && prerequisiteFeatures.Length > 0) {
                    foreach (var feature in prerequisiteFeatures) {
                        bp.AddPrerequisite<PrerequisiteFeature>(c => {
                            c.m_Feature = feature;
                        });
                    }
                }
            });
            SpellDescriptorComponent Descriptor = null;
            if (spellDescriptor != SpellDescriptor.None) {
                Descriptor = Helpers.Create<SpellDescriptorComponent>(c => {
                    c.Descriptor = spellDescriptor;
                });
            }

            var Action = new ContextActionSavingThrow() {
                m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0],
                Type = SavingThrowType.Fortitude,
                HasCustomDC = false,
                CustomDC = new ContextValue(),
                Actions = ActionFlow.DoSingle<ContextActionConditionalSaved>(act => {
                    act.Succeed = ActionFlow.DoNothing();
                    act.Failed = Helpers.CreateActionList(actions);
                })
            };

            crueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruption" + name,
                "Touch of Corruption - " + name,
                "Applies Touch of Corruption with " + name + " cruelty.\n" + description,
                icon,
                Action,
                config,
                Descriptor,
                crueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        private static void AddFatiquedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");
            var FatiguedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("e6f2fc5d73d88064583cb828801212f4");

            var actions = new ContextActionApplyBuff() {
                m_Buff = FatiguedBuff.ToReference<BlueprintBuffReference>(),
                Permanent = true,
                DurationValue = new ContextDurationValue(),
                IsFromSpell = true,
            };

            BuildCruelty(
                AntipaladinClassRef,
                out var InnerCrueltyFeature,
                out var InnerCrueltyAbility,
                "Fatiqued",
                "The target is fatigued.",
                FatiguedBuff.Icon,
                0,
                new BlueprintFeatureReference[0],
                new GameAction[] { actions },
                null,
                SpellDescriptor.Fatigue
            );

            CrueltyAbility = InnerCrueltyAbility;
            CrueltyFeature = InnerCrueltyFeature;

        }

        private static void AddShakenCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");
            var ShakenBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("25ec6cb6ab1845c48a95f9c20b034220");

            var actions = new ContextActionApplyBuff() {
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
            };

            var contextVarConfig = Helpers.Create<ContextRankConfig>(c => {
                c.m_Type = AbilityRankType.DamageDice;
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClassRef };
                c.m_Progression = ContextRankProgression.AsIs;
                c.m_Max = 20;
            });

            BuildCruelty(
                AntipaladinClassRef,
                out var InnerCrueltyFeature,
                out var InnerCrueltyAbility,
                "Shaken",
                "The target is shaken for 1 round per level of the antipaladin.",
                ShakenBuff.Icon,
                0,
                new BlueprintFeatureReference[0],
                new GameAction[] { actions },
                contextVarConfig,
                SpellDescriptor.None
            );

            CrueltyAbility = InnerCrueltyAbility;
            CrueltyFeature = InnerCrueltyFeature;
        }

        private static void AddSickenedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var SickenedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4e42460798665fd4cb9173ffa7ada323");

            var actions = new ContextActionApplyBuff() {
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
            };

            var contextVarConfig = Helpers.Create<ContextRankConfig>(c => {
                c.m_Type = AbilityRankType.DamageDice;
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClassRef };
                c.m_Progression = ContextRankProgression.AsIs;
                c.m_Max = 20;
            });

            BuildCruelty(
                AntipaladinClassRef,
                out var InnerCrueltyFeature,
                out var InnerCrueltyAbility,
                "Sickened",
                "The target is sickened for 1 round per level of the antipaladin",
                SickenedBuff.Icon,
                0,
                new BlueprintFeatureReference[0],
                new GameAction[] { actions },
                contextVarConfig,
                SpellDescriptor.None
            );

            CrueltyAbility = InnerCrueltyAbility;
            CrueltyFeature = InnerCrueltyFeature;
        }

        private static void AddDazedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var DazedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("d2e35b870e4ac574d9873b36402487e5");

            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyDazedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Dazed");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is dazed for 1 round.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = DazedBuff.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = AntipaladinClassRef;
                    c.Level = 6;
                });
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
                                m_Buff = DazedBuff.ToReference<BlueprintBuffReference>(),
                                UseDurationSeconds = true,
                                DurationSeconds = 6,
                                DurationValue = new ContextDurationValue(),
                                IsFromSpell = true,
                            }),
                })
            };

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionDazed",
                "Touch of Corruption - Dazed",
                "Applies Touch of Corruption with Sickened cruelty.\nOn failed saving throw the " +
                "target is dazed for 1 round.",
                DazedBuff.Icon,
                Action,
                null,
                null,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        private static void AddDiseasedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var DiseasedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("103aac6bc1cfd454492cee1fd680db05");

            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyDiseasedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Diseased");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target contracts a disease, as if the antipaladin" +
                    " had cast contagion, using his antipaladin level as his caster level.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = DiseasedBuff.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = AntipaladinClassRef;
                    c.Level = 6;
                });
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
                                m_Buff = DiseasedBuff.ToReference<BlueprintBuffReference>(),
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

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionDiseased",
                "Touch of Corruption - Diseased",
                "Applies Touch of Corruption with Diseased cruelty.\nOn failed saving throw the " +
                "target contracts a disease, as if the antipaladin had cast Contagion.",
                DiseasedBuff.Icon,
                Action,
                null,
                null,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        private static void AddStaggeredCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var StaggeredBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("df3950af5a783bd4d91ab73eb8fa0fd3");

            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyStaggeredFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Staggered");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is staggered for 1 round per two levels of the antipaladin.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = StaggeredBuff.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = AntipaladinClassRef;
                    c.Level = 6;
                });
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
                                m_Buff = StaggeredBuff.ToReference<BlueprintBuffReference>(),
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
                c.m_Progression = ContextRankProgression.Div2;
                c.m_Max = 20;
            });

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionStaggered",
                "Touch of Corruption - Staggered",
                "Applies Touch of Corruption with Staggered cruelty.\nOn failed saving throw the " +
                "target is staggered for 1 round per two levels of the antipaladin.",
                StaggeredBuff.Icon,
                Action,
                ContextVarConfig,
                null,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        private static void AddExhaustedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");
            var ExhaustedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("46d1b9cc3d0fd36469a471b047d773a2");
            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyExhaustedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Exhausted");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is exhausted.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = ExhaustedBuff.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = AntipaladinClassRef;
                    c.Level = 9;
                });
                bp.AddPrerequisite<PrerequisiteFeature>(c => {
                    c.m_Feature = MCEContext.GetModBlueprintReference<BlueprintFeatureReference>("AntipaladinCrueltyFatiquedFeature");
                });
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
                                m_Buff = ExhaustedBuff.ToReference<BlueprintBuffReference>(),
                                Permanent = true,
                                DurationValue = new ContextDurationValue() {
                                    m_IsExtendable = true,
                                    DiceCountValue = 0,
                                    BonusValue = 0
                                },
                                IsFromSpell = true,
                            }),
                })
            };
            var Descriptor = Helpers.Create<SpellDescriptorComponent>(c => {
                c.Descriptor = SpellDescriptor.Exhausted;
            });

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionExhausted",
                "Touch of Corruption - Exhausted",
                "Applies Touch of Corruption with Exhausted cruelty.\nOn failed saving throw the target is exhausted.",
                ExhaustedBuff.Icon,
                Action,
                null,
                Descriptor,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        private static void AddFrightenedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var FrightenedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f08a7239aa961f34c8301518e71d4cdf");

            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyFrightenedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Frightened");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is frightened for 1 round per two levels of the antipaladin.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = FrightenedBuff.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = AntipaladinClassRef;
                    c.Level = 9;
                });
                bp.AddPrerequisite<PrerequisiteFeature>(c => {
                    c.m_Feature = MCEContext.GetModBlueprintReference<BlueprintFeatureReference>("AntipaladinCrueltyShakenFeature");
                });
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
                                m_Buff = FrightenedBuff.ToReference<BlueprintBuffReference>(),
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
            var Descriptor = Helpers.Create<SpellDescriptorComponent>(c => {
                c.Descriptor = SpellDescriptor.Fear;
            });

            var ContextVarConfig = Helpers.Create<ContextRankConfig>(c => {
                c.m_Type = AbilityRankType.DamageDice;
                c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClassRef };
                c.m_Progression = ContextRankProgression.Div2;
                c.m_Max = 20;
            });

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionFrightened",
                "Touch of Corruption - Frightened",
                "Applies Touch of Corruption with frightened cruelty.\nOn failed saving throw the " +
                "target is frightened for 1 round per two levels of the antipaladin.",
                FrightenedBuff.Icon,
                Action,
                ContextVarConfig,
                Descriptor,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        private static void AddNauseatedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var NauseatedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("956331dba5125ef48afe41875a00ca0e");

            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyNauseatedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Nauseated");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is nauseated for 1 round per three levels of the antipaladin.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = NauseatedBuff.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = AntipaladinClassRef;
                    c.Level = 9;
                });
                bp.AddPrerequisite<PrerequisiteFeature>(c => {
                    c.m_Feature = MCEContext.GetModBlueprintReference<BlueprintFeatureReference>("AntipaladinCrueltySickenedFeature");
                });
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
                                m_Buff = NauseatedBuff.ToReference<BlueprintBuffReference>(),
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
                c.m_Progression = ContextRankProgression.DivStep;
                c.m_StepLevel = 3;
                c.m_Max = 20;
            });

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionNauseated",
                "Touch of Corruption - Nauseated",
                "Applies Touch of Corruption with Nauseated cruelty.\nOn failed saving throw the " +
                "target is nauseated for 1 round per three levels of the antipaladin.",
                NauseatedBuff.Icon,
                Action,
                ContextVarConfig,
                null,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        private static void AddPoisonedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var PoisonedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("ba1ae42c58e228c4da28328ea6b4ae34");

            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyPoisonedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Poisoned");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is poisoned, as if the antipaladin had cast poison, using the antipaladin’s level as the caster level.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = PoisonedBuff.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = AntipaladinClassRef;
                    c.Level = 9;
                });
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
                                m_Buff = PoisonedBuff.ToReference<BlueprintBuffReference>(),
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
                "AntipaladinTouchOfCorruptionPoisoned",
                "Touch of Corruption - Poisoned",
                "Applies Touch of Corruption with Poisoned cruelty.\nOn failed saving throw the " +
                "target is poisoned, as if the antipaladin had cast poison.",
                PoisonedBuff.Icon,
                Action,
                ContextVarConfig,
                null,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }


        private static void AddBlindedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var BlindedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("187f88d96a0ef464280706b63635f2af");

            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyBlindedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Nauseated");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is blinded for 1 round per level of the antipaladin.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = BlindedBuff.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = AntipaladinClassRef;
                    c.Level = 12;
                });
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
                                m_Buff = BlindedBuff.ToReference<BlueprintBuffReference>(),
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
                "AntipaladinTouchOfCorruptionBlinded",
                "Touch of Corruption - Blinded",
                "Applies Touch of Corruption with Blinded cruelty.\nOn failed saving throw the " +
                "target is blinded for 1 round per three levels of the antipaladin.",
                BlindedBuff.Icon,
                Action,
                ContextVarConfig,
                null,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        private static void AddStunnedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var StunnedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("09d39b38bb7c6014394b6daced9bacd3");

            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyStunnedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Stunned");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is stunned for 1 round per three levels of the antipaladin.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = StunnedBuff.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = AntipaladinClassRef;
                    c.Level = 12;
                });
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
                                m_Buff = StunnedBuff.ToReference<BlueprintBuffReference>(),
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
                c.m_Progression = ContextRankProgression.DivStep;
                c.m_StepLevel = 4;
                c.m_Max = 20;
            });

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionStunned",
                "Touch of Corruption - Stunned",
                "Applies Touch of Corruption with Stunned cruelty.\nOn failed saving throw the " +
                "target is stunned for 1 round per four levels of the antipaladin.",
                StunnedBuff.Icon,
                Action,
                ContextVarConfig,
                null,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        private static void AddParalyzedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var Paralyzed = BlueprintTools.GetBlueprint<BlueprintBuff>("af1e2d232ebbb334aaf25e2a46a92591");

            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyParalyzedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Paralyzed");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is paralyzed for 1 round.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = Paralyzed.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = AntipaladinClassRef;
                    c.Level = 12;
                });
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
                                m_Buff = Paralyzed.ToReference<BlueprintBuffReference>(),
                                UseDurationSeconds = true,
                                DurationSeconds = 6,
                                DurationValue = new ContextDurationValue(),
                                IsFromSpell = true,
                            }),
                })
            };

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionParalyzed",
                "Touch of Corruption - Paralyzed",
                "Applies Touch of Corruption with Paralyzed cruelty.\nOn failed saving throw the " +
                "target is paralyzed for 1 round.",
                Paralyzed.Icon,
                Action,
                null,
                null,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        private static void AddCursedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var Cursed = BlueprintTools.GetBlueprint<BlueprintBuff>("caae9592917719a41b601b678a8e6ddf");

            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyCursedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Cursed");
                bp.SetDescription(MCEContext, DESCRIPTION + "\nThe target is cursed.");
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = Cursed.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = AntipaladinClassRef;
                    c.Level = 9;
                });
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
                                m_Buff = Cursed.ToReference<BlueprintBuffReference>(),
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

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionCursed",
                "Touch of Corruption - Cursed",
                "Applies Touch of Corruption with Cursed cruelty.\nOn failed saving throw the " +
                "target is cursed, as if the antipaladin had cast bestow curse.",
                Cursed.Icon,
                Action,
                null,
                null,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }

        public static void AddCruelties() {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            AddFatiquedCruelty(out var FatiquedCrueltyFeature, out var FatiquedCrueltyAbility);
            AddShakenCruelty(out var ShakenCrueltyFeature, out var ShakenCrueltyAbility);
            AddSickenedCruelty(out var SickenedCrueltyFeature, out var SickenedCrueltyAbility);
            AddDazedCruelty(out var DazedCrueltyFeature, out var DazedCrueltyAbility);
            AddDiseasedCruelty(out var DiseasedCrueltyFeature, out var DiseasedCrueltyAbility);
            AddStaggeredCruelty(out var StaggeredCrueltyFeature, out var StaggeredCrueltyAbility);
            AddCursedCruelty(out var CursedCrueltyFeature, out var CursedCrueltyAbility);
            AddExhaustedCruelty(out var ExhaustedCrueltyFeature, out var ExhaustedCrueltyAbility);
            AddFrightenedCruelty(out var FrightenedCrueltyFeature, out var FrightenedCrueltyAbility);
            AddNauseatedCruelty(out var NauseatedCrueltyFeature, out var NauseatedCrueltyAbility);
            AddPoisonedCruelty(out var PoisonedCrueltyFeature, out var PoisonedCrueltyAbility);
            AddBlindedCruelty(out var BlindedCrueltyFeature, out var BlindedCrueltyAbility);
            AddStunnedCruelty(out var StunnedCrueltyFeature, out var StunnedCrueltyAbility);
            AddParalyzedCruelty(out var ParalyzedCrueltyFeature, out var ParalyzedCrueltyAbility);

            var BaseAbility = BlueprintTools.GetModBlueprint<BlueprintAbility>(MCEContext, "AntipaladinTouchOfCorruptionBase");
            var variants = BaseAbility.GetComponent<AbilityVariants>();
            variants.m_Variants = variants.m_Variants.AppendToArray(
                new BlueprintAbilityReference[] {
                    FatiquedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    ShakenCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    SickenedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    DazedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    DiseasedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    StaggeredCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    CursedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    ExhaustedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    FrightenedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    NauseatedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    PoisonedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    BlindedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    StunnedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
                    ParalyzedCrueltyAbility.ToReference<BlueprintAbilityReference>(),
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
                                FatiquedCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                ShakenCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                SickenedCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                DazedCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                DiseasedCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                StaggeredCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                CursedCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                ExhaustedCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                FrightenedCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                NauseatedCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                PoisonedCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                BlindedCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                StunnedCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                                ParalyzedCrueltyFeature.ToReference<BlueprintFeatureReference>(),
                            };
                bp.Mode = SelectionMode.Default;
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.IsClassFeature = true;
            });
        }
    }
}
