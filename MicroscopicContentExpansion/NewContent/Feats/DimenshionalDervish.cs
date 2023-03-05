using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.TurnBasedModifiers;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using MicroscopicContentExpansion.NewComponents;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Feats {
    internal class DimenshionalDervish {

        private const string DimensionDoorGUID = "4a648b57935a59547b7a2ee86fb4f26a";
        private const string KiPowerResource = "9d9c90a9a1f52d04799294bf91c80a82";
        private const string SFPowerResource = "7d002c1025fbfe2458f1509bf7a89ce1";
        private const string KiAbundantStep = "008466f45b3e2e64793b30f3d16e41c0";
        private const string SFAbundantStep = "b56f18c437dc324438f0d956fb34a8cd";
        internal static void AddDimenshionalSavantFeatChain() {
            MCEContext.Logger.LogHeader("Adding Dimenshional Savant chain feats");
            AddDimensionalDervish();
        }

        private static void AddDimensionalDervish() {
            var flickeringStep = AddFlickeringStep();
            var dimensionalAgility = AddDimensionalAgility(flickeringStep);
            var dimensionalAssault = AddDimensionalAssault(dimensionalAgility, flickeringStep);
            var icon = BlueprintTools.GetBlueprint<BlueprintAbility>("4c349361d720e844e846ad8c19959b1e").m_Icon;

            const string dervishName = "Dimenshional Dervish";
            const string dervishDescription = "You can take a full-attack action, activating abundant step or casting dimension door as a swift action. If you do, you can teleport up to twice your speed (up to the maximum distance allowed by the spell or ability), dividing this teleportation into increments you use before your first attack, between each attack, and after your last attack. You must teleport at least 5 feet each time you teleport.";
            const string kiDervishName = "Ki Power: " + dervishName;
            const string fsDervishName = "Flickering Step (" + dervishName + ")";

            var dimensionalDervishAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DimensionalDervishAbility", bp => {
                bp.SetName(MCEContext, dervishName);
                bp.SetDescription(MCEContext, dervishDescription);
                bp.m_Icon = icon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.DoubleMove;
                bp.CanTargetEnemies = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_IsFullRoundAction = false;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent<AbilityIsFullRoundInTurnBased>(c => {
                    c.FullRoundIfTurnBased = true;
                });
                bp.AddComponent<AbilityCasterHasSwiftAction>();
                bp.AddComponent<AbilityCustomDimensionalDervish>(c => {
                });
                bp.AddComponent<AbilityShowIfCasterHasFact>(c => {
                    c.m_UnitFact = MCEContext.GetModBlueprintReference<BlueprintUnitFactReference>("DimensionalDervishFeature");
                });
                bp.AddComponent<AbilityCasterHasFacts>(c => {
                    c.NeedsAll = true;
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        MCEContext.GetModBlueprintReference<BlueprintUnitFactReference>("DimensionalDervishFeature"),
                        dimensionalAgility.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });

            var dimensionalDervishKiAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DimensionalDervishKiAbility", bp => {
                bp.SetName(MCEContext, kiDervishName);
                bp.SetDescription(MCEContext, dervishDescription);
                bp.m_Icon = icon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.DoubleMove;
                bp.CanTargetEnemies = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_IsFullRoundAction = false;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent<AbilityIsFullRoundInTurnBased>(c => {
                    c.FullRoundIfTurnBased = true;
                });
                bp.AddComponent<AbilityCasterHasSwiftAction>();
                bp.AddComponent<AbilityCustomDimensionalDervish>(c => {
                });
                bp.AddComponent<AbilityShowIfCasterHasFact>(c => {
                    c.m_UnitFact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(KiAbundantStep);
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_IsSpendResource = true;
                    c.Amount = 2;
                    c.m_RequiredResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>(KiPowerResource);
                });
                bp.AddComponent<AbilityCasterHasFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { dimensionalAgility.ToReference<BlueprintUnitFactReference>() };
                });
            });

            var dimensionalDervishScaledFistAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DimensionalDervishScaledFistAbility", bp => {
                bp.SetName(MCEContext, kiDervishName);
                bp.SetDescription(MCEContext, dervishDescription);
                bp.m_Icon = icon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.DoubleMove;
                bp.CanTargetEnemies = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_IsFullRoundAction = false;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent<AbilityIsFullRoundInTurnBased>(c => {
                    c.FullRoundIfTurnBased = true;
                });
                bp.AddComponent<AbilityCasterHasSwiftAction>();
                bp.AddComponent<AbilityCustomDimensionalDervish>(c => {
                });
                bp.AddComponent<AbilityShowIfCasterHasFact>(c => {
                    c.m_UnitFact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(SFAbundantStep);
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_IsSpendResource = true;
                    c.Amount = 2;
                    c.m_RequiredResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>(SFPowerResource);
                });
                bp.AddComponent<AbilityCasterHasFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { dimensionalAgility.ToReference<BlueprintUnitFactReference>() };
                });
            });

            var dimensionalDervishFlickeringStepAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DimensionalDervishFlickeringStepAbility", bp => {
                bp.SetName(MCEContext, fsDervishName);
                bp.SetDescription(MCEContext, dervishDescription);
                bp.m_Icon = icon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.DoubleMove;
                bp.CanTargetEnemies = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_IsFullRoundAction = false;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent<AbilityIsFullRoundInTurnBased>(c => {
                    c.FullRoundIfTurnBased = true;
                });
                bp.AddComponent<AbilityCasterHasSwiftAction>();
                bp.AddComponent<AbilityCustomDimensionalDervish>(c => {
                });
                bp.AddComponent<AbilityShowIfCasterHasFact>(c => {
                    c.m_UnitFact = flickeringStep.ToReference<BlueprintUnitFactReference>();
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.m_RequiredResource = MCEContext.GetModBlueprintReference<BlueprintAbilityResourceReference>("FlickeringStepResource");
                });
                bp.AddComponent<AbilityCasterHasFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { dimensionalAgility.ToReference<BlueprintUnitFactReference>() };
                });
            });

            var dimensionalDervishFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DimensionalDervishFeature", bp => {
                bp.SetName(MCEContext, dervishName);
                bp.SetDescription(MCEContext, dervishDescription);
                bp.m_Icon = icon;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] {
                    FeatureGroup.CombatFeat,
                    FeatureGroup.Feat
                };
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack;
                });
                bp.AddPrerequisiteFeature(dimensionalAgility);
                bp.AddPrerequisiteFeature(dimensionalAssault);
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.BaseAttackBonus;
                    c.Value = 6;
                });
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        dimensionalDervishKiAbility.ToReference<BlueprintUnitFactReference>(),
                        dimensionalDervishScaledFistAbility.ToReference<BlueprintUnitFactReference>(),
                        dimensionalDervishFlickeringStepAbility.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });

            var dimensionDoor = BlueprintTools.GetBlueprint<BlueprintAbility>(DimensionDoorGUID);
            var av = dimensionDoor.GetComponent<AbilityVariants>();
            av.m_Variants = av.m_Variants.AppendToArray(dimensionalDervishAbility.ToReference<BlueprintAbilityReference>());

            //add blocks
            var MidnightFane_DimensionLock_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("4b0cd08a3cea2844dba9889c1d34d667");
            var DLC1_DimensionLock_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("6e339c2bc7ea488c9b655b029984405d");
            var IvoryLabyrinth_DimensionLock_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("c5a1c11666ede1f41ad003d70c628b98");
            var Darkness_DimensionLock_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("8e59c88d7fe846299a641f62675827c5");
            var arr = new BlueprintBuff[] { MidnightFane_DimensionLock_Buff, DLC1_DimensionLock_Buff, IvoryLabyrinth_DimensionLock_Buff, Darkness_DimensionLock_Buff };
            var spellsArr = new BlueprintAbilityReference[] {
                flickeringStep.ToReference<BlueprintAbilityReference>(),
                MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("DimensionalAssaultAbility"),
                MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("DimensionalAssaultKiAbility"),
                MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("DimensionalAssaultScaledFistAbility"),
                MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("DimensionalAssaultFlickeringStepAbility"),
                dimensionalDervishAbility.ToReference<BlueprintAbilityReference>(),
                dimensionalDervishKiAbility.ToReference<BlueprintAbilityReference>(),
                dimensionalDervishScaledFistAbility.ToReference<BlueprintAbilityReference>(),
                dimensionalDervishFlickeringStepAbility.ToReference<BlueprintAbilityReference>()
            };
            foreach (var item in arr) {
                var c = item.GetComponent<ForbidSpecificSpellsCast>();
                c.m_Spells = c.m_Spells.AppendToArray(spellsArr);
            }

            if (MCEContext.AddedContent.Feats.IsEnabled("DimensionalDervishFeatChain")) {
                FeatTools.AddAsFeat(dimensionalDervishFeature);
            }
        }

        private static BlueprintFeature AddDimensionalAgility(BlueprintFeature flickeringStep) {
            const string agilityName = "Dimensional Agility";
            const string agilityDescription = "After using abundant step or casting dimension door, you can take any actions you still have remaining on your turn.";

            var dimensionDoor = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(DimensionDoorGUID);
            var kiAbundantStep = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("008466f45b3e2e64793b30f3d16e41c0");
            var sfAbundantStep = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("b56f18c437dc324438f0d956fb34a8cd");
            var dimensionalAgilityFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DimensionalAgilityFeature", bp => {
                bp.SetName(MCEContext, agilityName);
                bp.SetDescription(MCEContext, agilityDescription);
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] {
                    FeatureGroup.CombatFeat,
                    FeatureGroup.Feat
                };
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack;
                });
                bp.AddPrerequisite<PrerequisiteSpellKnown>(c => {
                    c.m_Spell = dimensionDoor;
                    c.Group = Prerequisite.GroupType.Any;
                });
                bp.AddPrerequisite<PrerequisiteFeaturesFromList>(c => {
                    c.m_Features = new BlueprintFeatureReference[] { kiAbundantStep, sfAbundantStep, flickeringStep.ToReference<BlueprintFeatureReference>() };
                    c.Group = Prerequisite.GroupType.Any;
                });
            });

            if (MCEContext.AddedContent.Feats.IsEnabled("DimensionalDervishFeatChain")) {
                FeatTools.AddAsFeat(dimensionalAgilityFeature);
            }
            return dimensionalAgilityFeature;
        }

        private static BlueprintFeature AddDimensionalAssault(BlueprintFeature dimensionalAgility, BlueprintFeature flickeringStep) {
            const string assaultName = "Dimensional Assault";
            const string kiAssaultName = "Ki Power: " + assaultName;
            const string fsAssaultName = "Flickering Step (" + assaultName + ")";
            const string assaultDescription = "As a full-round action, you use abundant step or cast dimension door as a special charge. Doing so allows you to teleport up to double your current speed (up to the maximum distance allowed by the spell or ability) and to make the attack normally allowed on a charge.";

            var icon = BlueprintTools.GetBlueprint<BlueprintAbility>("4c349361d720e844e846ad8c19959b1e").m_Icon;

            var dimensionalAssaultAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DimensionalAssaultAbility", bp => {
                bp.SetName(MCEContext, assaultName);
                bp.SetDescription(MCEContext, assaultDescription);
                bp.m_Icon = icon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.DoubleMove;
                bp.CanTargetEnemies = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_IsFullRoundAction = false;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent<AbilityIsFullRoundInTurnBased>(c => {
                    c.FullRoundIfTurnBased = true;
                });
                bp.AddComponent<AbilityCustomDimensionalAssault>();
                bp.AddComponent<AbilityShowIfCasterHasFact>(c => {
                    c.m_UnitFact = MCEContext.GetModBlueprintReference<BlueprintUnitFactReference>("DimensionalAssaultFeature");
                });
                bp.AddComponent<AbilityCasterHasFacts>(c => {
                    c.NeedsAll = true;
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        MCEContext.GetModBlueprintReference<BlueprintUnitFactReference>("DimensionalAssaultFeature"),
                        dimensionalAgility.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });

            var dimensionalAssaultKiAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DimensionalAssaultKiAbility", bp => {
                bp.SetName(MCEContext, kiAssaultName);
                bp.SetDescription(MCEContext, assaultDescription);
                bp.m_Icon = icon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.DoubleMove;
                bp.CanTargetEnemies = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_IsFullRoundAction = false;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent<AbilityIsFullRoundInTurnBased>(c => {
                    c.FullRoundIfTurnBased = true;
                });
                bp.AddComponent<AbilityCustomDimensionalAssault>();
                bp.AddComponent<AbilityShowIfCasterHasFact>(c => {
                    c.m_UnitFact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(KiAbundantStep);
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_IsSpendResource = true;
                    c.Amount = 2;
                    c.m_RequiredResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>(KiPowerResource);
                });
                bp.AddComponent<AbilityCasterHasFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { dimensionalAgility.ToReference<BlueprintUnitFactReference>() };
                });
            });

            var dimensionalAssaultScaledFistAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DimensionalAssaultScaledFistAbility", bp => {
                bp.SetName(MCEContext, fsAssaultName);
                bp.SetDescription(MCEContext, assaultDescription);
                bp.m_Icon = icon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.DoubleMove;
                bp.CanTargetEnemies = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_IsFullRoundAction = false;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent<AbilityIsFullRoundInTurnBased>(c => {
                    c.FullRoundIfTurnBased = true;
                });
                bp.AddComponent<AbilityCustomDimensionalAssault>();
                bp.AddComponent<AbilityShowIfCasterHasFact>(c => {
                    c.m_UnitFact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(SFAbundantStep);
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_IsSpendResource = true;
                    c.Amount = 2;
                    c.m_RequiredResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>(SFPowerResource);
                });
                bp.AddComponent<AbilityCasterHasFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { dimensionalAgility.ToReference<BlueprintUnitFactReference>() };
                });
            });

            var dimensionalAssaultFlickeringStepAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "DimensionalAssaultFlickeringStepAbility", bp => {
                bp.SetName(MCEContext, kiAssaultName);
                bp.SetDescription(MCEContext, assaultDescription);
                bp.m_Icon = icon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.DoubleMove;
                bp.CanTargetEnemies = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_IsFullRoundAction = false;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent<AbilityIsFullRoundInTurnBased>(c => {
                    c.FullRoundIfTurnBased = true;
                });
                bp.AddComponent<AbilityCustomDimensionalAssault>();
                bp.AddComponent<AbilityShowIfCasterHasFact>(c => {
                    c.m_UnitFact = flickeringStep.ToReference<BlueprintUnitFactReference>();
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.m_RequiredResource = MCEContext.GetModBlueprintReference<BlueprintAbilityResourceReference>("FlickeringStepResource");
                });
                bp.AddComponent<AbilityCasterHasFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { dimensionalAgility.ToReference<BlueprintUnitFactReference>() };
                });
            });

            var dimensionalAssaultFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DimensionalAssaultFeature", bp => {
                bp.SetName(MCEContext, assaultName);
                bp.SetDescription(MCEContext, assaultDescription);
                bp.m_Icon = icon;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] {
                    FeatureGroup.CombatFeat,
                    FeatureGroup.Feat
                };
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack;
                });
                bp.AddPrerequisiteFeature(dimensionalAgility);
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        dimensionalAssaultKiAbility.ToReference<BlueprintUnitFactReference>(),
                        dimensionalAssaultScaledFistAbility.ToReference<BlueprintUnitFactReference>(),
                        dimensionalAssaultFlickeringStepAbility.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });

            var dimensionDoor = BlueprintTools.GetBlueprint<BlueprintAbility>(DimensionDoorGUID);
            var av = dimensionDoor.GetComponent<AbilityVariants>();
            av.m_Variants = av.m_Variants.AppendToArray(dimensionalAssaultAbility.ToReference<BlueprintAbilityReference>());

            if (MCEContext.AddedContent.Feats.IsEnabled("DimensionalDervishFeatChain")) {
                FeatTools.AddAsFeat(dimensionalAssaultFeature);
            }

            return dimensionalAssaultFeature;
        }

        private static BlueprintFeature AddFlickeringStep() {
            var icon = BlueprintTools.GetBlueprint<BlueprintAbility>(DimensionDoorGUID).m_Icon;

            const string name = "Flickering Step";
            const string description = "You can use dimension door as a spell-like ability.\nYou can use this feat’s benefit once per day, plus an additional time per day for every 5 character levels.";

            var kiAbundantStepAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("336a841704b7e2341b51f89fc9491f54");

            var resource = Helpers.CreateBlueprint<BlueprintAbilityResource>(MCEContext, "FlickeringStepResource", bp => {
                bp.m_MaxAmount = new() {
                    BaseValue = 1,
                    IncreasedByLevelStartPlusDivStep = true,
                    LevelStep = 5,
                    StartingLevel = 0,
                    StartingIncrease = 0,
                    LevelIncrease = 1,
                    OtherClassesModifier = 1,
                    PerStepIncrease = 1,
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0]
                };
            });

            var flickeringStepAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "FlickeringStepAbility", bp => {
                bp.SetName(MCEContext, name);
                bp.SetDescription(MCEContext, description);
                bp.m_Icon = icon;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Long;
                bp.CanTargetPoint = true;
                bp.CanTargetSelf = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move;
                bp.m_IsFullRoundAction = false;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent(kiAbundantStepAbility.GetComponent<AbilityCustomDimensionDoor>());
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
                });
            });

            var flickeringStepFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "FlickeringStepFeature", bp => {
                bp.SetName(MCEContext, name);
                bp.SetDescription(MCEContext, description);
                bp.m_Icon = icon;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] {
                    FeatureGroup.Feat
                };
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack;
                });
                bp.AddComponent<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillKnowledgeArcana;
                    c.Value = 9;
                });
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { flickeringStepAbility.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = resource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
            });

            if (MCEContext.AddedContent.Feats.IsEnabled("DimensionalDervishFeatChain")) {
                FeatTools.AddAsFeat(flickeringStepFeature);
            }
            return flickeringStepFeature;
        }
    }
}
