using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Craft;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.Blueprints.Classes.BlueprintProgression;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Classes {
    internal class DruidicHerbalism {
        internal static void Add() {
            var druidBondSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("3830f3630a33eba49b60f511b4c8f2a8");
            var featureInternalName = "DruidicHerbalism";
            var description = "A druid who chooses to learn druidic herbalism can create herbal concoctions that function like potions." +
                "\n\nAt 1st level druid receives Brew Potions feat, and can make any potion without spending ingredients." +
                "\n\nAt 7th level druid can craft herbal concoctions in no time. She can create a special concoction of any spell that she can cast, but to do so, she must expend a spell slot of the same level. These special concoctions do not cost her anything to create and function like extracts created by an alchemist with the infusion discovery.";

            var brewPotions = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("c0f8c4e513eb493408b8070a1de93fc0");

            var freePotions = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DruidicConcoctions", a => {
                a.SetName(MCEContext, "Druidic Concoctions");
                a.SetDescription(MCEContext, "Druid can brew any potion without spending ingredients.");
            });

            var infusion = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DruidicHerbalismInfusion", a => {
                a.SetName(MCEContext, "Druidic Herbalism - Infusion");
                a.SetDescription(MCEContext, "At 7th level druid can craft herbal concoctions in no time. She can create a special concoction of any spell that she can cast, but to do so, she must expend a spell slot of the same level. These special concoctions do not cost her anything to create and function like extracts created by an alchemist with the infusion discovery.");
            });

            var druidicHerbalismProgression = Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, featureInternalName, a => {
                a.SetName(MCEContext, "Druidic Herbalism");
                a.SetDescription(MCEContext, description);
                a.m_Classes = new ClassWithLevel[] {
                    new ClassWithLevel() {
                        m_Class = ClassTools.ClassReferences.DruidClass
                    }
                };
                a.Groups = new FeatureGroup[] {
                    FeatureGroup.DruidBond
                };
                a.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, brewPotions, freePotions),
                    Helpers.CreateLevelEntry(7, infusion),
                };
            });

            if (MCEContext.AddedContent.NewClasses.IsDisabled("DruidicHerbalism")) return;

            druidBondSelection.m_AllFeatures = druidBondSelection.m_AllFeatures.AddToArray(druidicHerbalismProgression.ToReference<BlueprintFeatureReference>()).ToArray();
        }
    }

    [HarmonyPatch(typeof(CraftRoot), nameof(CraftRoot.CreateCraftInfo))]
    internal class CraftRoot_CreateCraftInfo_Patch {
        [HarmonyPostfix]
        internal static void Postfix(CraftRoot __instance, ref CraftAvailInfo __result, UnitEntityData crafter, BlueprintItemEquipmentUsable item, Spellbook spellbook, BlueprintAbility abillity, CraftRequirements[] list, CraftInfoComponent craftComp, MetamagicData metamagic, int metamagicBorder, int metamagicBorderColor) {
            if (MCEContext.AddedContent.NewClasses.IsDisabled("DruidicHerbalism")) return;

            try {
                if (item.Type == UsableItemType.Potion
                && crafter.HasFact(BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("1758c2df-11f7-449f-a4fc-2762f9b68ae5"))) {
                    CraftItemInfo craftItemInfo = new CraftItemInfo {
                        Item = __result.Info.Item,
                        CastingModifierBonus = __result.Info.CastingModifierBonus,
                        SpellLevel = __result.Info.SpellLevel,
                        IsArcane = __result.Info.IsArcane,
                        CasterLevel = __result.Info.CasterLevel,
                        CraftCost = new List<BlueprintIngredient.Reference>(),
                        Metamagic = __result.Info.Metamagic,
                        MetamagicBorder = __result.Info.MetamagicBorder,
                        MetamagicBorderColor = __result.Info.MetamagicBorderColor
                    };
                    __result = new CraftAvailInfo {
                        Info = craftItemInfo,
                        Requirements = new CraftRequirements() { RequiredFeature = __result.Requirements.RequiredFeature },
                        IsHaveFeature = __result.IsHaveFeature,
                        IsHaveItem = __result.IsHaveItem,
                        IsHaveResources = true,
                        MaxCasterLevel = spellbook.EffectiveCasterLevel
                    };
                }
            } catch (Exception e) {
                MCEContext.Logger.LogError(e, "DruidicHerbalism patch CraftRoot.CreateCraftInfo error");
            }
        }
    }

    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.AlchemistInfusion), MethodType.Getter)]
    internal class AbilityData_AlchemistInfusion_Patch {
        [HarmonyPostfix]
        internal static void Postfix(AbilityData __instance, ref bool __result) {
            if (__result) return;

            if (MCEContext.AddedContent.NewClasses.IsDisabled("DruidicHerbalism")) return;

            try {
                if (__instance.Caster != null && __instance?.Spellbook?.Blueprint?.m_CharacterClass != null
                    && __instance.Spellbook.Blueprint.m_CharacterClass.Guid.m_Guid == Guid.Parse("610d836f3a3a9ed42a4349b62f002e96")
                    && __instance.Caster.HasFact(BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("5ddf6390-09f3-44ea-8cac-63b3f44d6e10"))) {
                    __result = true;
                }
            } catch (Exception e) {
                MCEContext.Logger.LogError(e, "DruidicHerbalism patch AbilityData.AlchemistInfusion error");
            }
        }
    }

    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.TargetAnchor), MethodType.Getter)]
    internal class AbilityData_TargetAnchor_Patch {
        [HarmonyPostfix]
        internal static void Postfix(AbilityData __instance, ref AbilityTargetAnchor __result) {
            if (__result == AbilityTargetAnchor.Unit) return;

            if (MCEContext.AddedContent.NewClasses.IsDisabled("DruidicHerbalism")) return;

            try {
                if (__instance.Range == AbilityRange.Personal && __result == AbilityTargetAnchor.Owner
                    && __instance.Caster != null && __instance?.Spellbook?.Blueprint?.m_CharacterClass != null
                    && __instance.Spellbook.Blueprint.m_CharacterClass.Guid.m_Guid == Guid.Parse("610d836f3a3a9ed42a4349b62f002e96")
                    && __instance.Caster.HasFact(BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("5ddf6390-09f3-44ea-8cac-63b3f44d6e10"))) {
                    __result = AbilityTargetAnchor.Unit;
                }
            } catch (Exception e) {
                MCEContext.Logger.LogError(e, "DruidicHerbalism patch AbilityData.TargetAnchor error");
            }

        }
    }
}
