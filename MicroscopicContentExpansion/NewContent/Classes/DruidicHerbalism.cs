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
using MicroscopicContentExpansion.NewComponents;
using System;
using System.Linq;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Classes;
internal class DruidicHerbalism
{
    internal static void Add()
    {
        var druidBondSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("3830f3630a33eba49b60f511b4c8f2a8");
        var featureInternalName = "DruidicHerbalism";
        var description = "A druid who chooses to learn druidic herbalism can create herbal concoctions that function like potions." +
            "\n\nAt 1st level druid receives Brew Potions feat, and can make any potion without spending ingredients." +
            "\n\nAt 7th level druid can craft herbal concoctions in no time. She can create a special concoction of any spell that she can cast, but to do so, she must expend a spell slot of the same level. These special concoctions do not cost her anything to create and function like extracts created by an alchemist with the infusion discovery.";

        var brewPotions = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("c0f8c4e513eb493408b8070a1de93fc0");

        var freePotions = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DruidicConcoctions", a =>
        {
            a.SetName(MCEContext, "Druidic Concoctions");
            a.SetDescription(MCEContext, "Druid can brew any potion without spending ingredients.");
            a.AddComponent<HarmonyPatchActivator>(c =>
            {
                c.PatchType = typeof(DruidicHerbalismPatches);
            });
        });

        var infusion = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DruidicHerbalismInfusion", a =>
        {
            a.SetName(MCEContext, "Druidic Herbalism - Infusion");
            a.SetDescription(MCEContext, "At 7th level druid can craft herbal concoctions in no time. She can create a special concoction of any spell that she can cast, but to do so, she must expend a spell slot of the same level. These special concoctions do not cost her anything to create and function like extracts created by an alchemist with the infusion discovery.");
        });

        var druidicHerbalismProgression = Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, featureInternalName, a =>
        {
            a.SetName(MCEContext, "Druidic Herbalism");
            a.SetDescription(MCEContext, description);
            a.m_Classes = [
                new BlueprintProgression.ClassWithLevel() {
                    m_Class = ClassTools.ClassReferences.DruidClass
                }
            ];
            a.Groups = [
                FeatureGroup.DruidBond
            ];
            a.LevelEntries = [
                Helpers.CreateLevelEntry(1, brewPotions, freePotions),
                Helpers.CreateLevelEntry(7, infusion),
            ];
        });

        if (MCEContext.AddedContent.NewClasses.IsDisabled("DruidicHerbalism")) return;

        druidBondSelection.m_AllFeatures = druidBondSelection.m_AllFeatures.AddToArray(druidicHerbalismProgression.ToReference<BlueprintFeatureReference>()).ToArray();
    }
}

[HarmonyPatch]
internal class DruidicHerbalismPatches
{
    private static bool Enabled;

    private static BlueprintGuid DruidClassGuid;
    private static BlueprintFeatureReference DruidicConcoctions;
    private static BlueprintFeatureReference DruidicHerbalismInfusion;

    [HarmonyPrepare]
    internal static void Init()
    {
        if (Enabled) { return; }

        DruidClassGuid = new(new Guid("610d836f3a3a9ed42a4349b62f002e96"));
        DruidicConcoctions = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("1758c2df-11f7-449f-a4fc-2762f9b68ae5");
        DruidicHerbalismInfusion = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("5ddf6390-09f3-44ea-8cac-63b3f44d6e10");
        Enabled = true;
        MCEContext.Logger.Log($"Finished DruidicHerbalismPatches.Init");
    }

    [HarmonyPatch(typeof(CraftRoot), nameof(CraftRoot.CreateCraftInfo))]
    [HarmonyPostfix]
    internal static void CraftRoot_CreateCraftInfo_Patch(CraftRoot __instance, ref CraftAvailInfo __result, UnitEntityData crafter, BlueprintItemEquipmentUsable item, Spellbook spellbook, BlueprintAbility abillity, CraftRequirements[] list, CraftInfoComponent craftComp, MetamagicData metamagic, int metamagicBorder, int metamagicBorderColor)
    {
        if (item.Type == UsableItemType.Potion && crafter.HasFact(DruidicConcoctions))
        {
            CraftItemInfo craftItemInfo = new CraftItemInfo
            {
                Item = __result.Info.Item,
                CastingModifierBonus = __result.Info.CastingModifierBonus,
                SpellLevel = __result.Info.SpellLevel,
                IsArcane = __result.Info.IsArcane,
                CasterLevel = __result.Info.CasterLevel,
                CraftCost = [],
                Metamagic = __result.Info.Metamagic,
                MetamagicBorder = __result.Info.MetamagicBorder,
                MetamagicBorderColor = __result.Info.MetamagicBorderColor
            };
            __result = new CraftAvailInfo
            {
                Info = craftItemInfo,
                Requirements = new CraftRequirements() { RequiredFeature = __result.Requirements.RequiredFeature },
                IsHaveFeature = __result.IsHaveFeature,
                IsHaveItem = __result.IsHaveItem,
                IsHaveResources = true,
                MaxCasterLevel = spellbook.EffectiveCasterLevel
            };
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.AlchemistInfusion), MethodType.Getter)]
    internal static void AbilityData_AlchemistInfusion_Patch(AbilityData __instance, ref bool __result)
    {
        if (__result) return;

        if (__instance.Caster != null && __instance?.Spellbook?.Blueprint?.m_CharacterClass != null
            && __instance.Spellbook.Blueprint.m_CharacterClass.Guid == DruidClassGuid
            && __instance.Caster.HasFact(DruidicHerbalismInfusion))
        {
            __result = true;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.TargetAnchor), MethodType.Getter)]
    internal static void AbilityData_TargetAnchor_Patch(AbilityData __instance, ref AbilityTargetAnchor __result)
    {
        if (__result == AbilityTargetAnchor.Unit) return;

        if (__instance.Range == AbilityRange.Personal && __result == AbilityTargetAnchor.Owner
            && __instance.Caster != null && __instance?.Spellbook?.Blueprint?.m_CharacterClass != null
            && __instance.Spellbook.Blueprint.m_CharacterClass.Guid == DruidClassGuid
            && __instance.Caster.HasFact(DruidicHerbalismInfusion))
        {
            __result = AbilityTargetAnchor.Unit;
        }
    }
}
