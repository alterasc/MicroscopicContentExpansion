using HarmonyLib;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using System.Linq;

namespace MicroscopicContentExpansion.RebalancedContent.MythicArmor;

internal static class UnitArmorPatch
{
    internal static bool Patch(UnitArmor __instance, ref bool __result)
    {
        MCEContext.Logger.Log($"Prefix start");
        if (__instance.Unit == null || !__instance.Unit.TryGetValue(out UnitEntityData unitEntityData))
        {
            __result = false;
            return false;
        }
        ItemEntityArmor armor = RestrictionsHelper.CheckHasArmor(unitEntityData) ? unitEntityData.Body.Armor.Armor : null;
        MCEContext.Logger.Log($"Prefix armor type: {armor.Blueprint.ProficiencyGroup}");
        ItemEntityShield shield = (unitEntityData.Body.PrimaryHand != null && unitEntityData.Body.PrimaryHand.HasShield) ? unitEntityData.Body.PrimaryHand.Shield : null;
        ItemEntityShield shield2 = (unitEntityData.Body.SecondaryHand != null && unitEntityData.Body.SecondaryHand.HasShield) ? unitEntityData.Body.SecondaryHand.Shield : null;
        __result = (__instance.IncludeArmorCategories == null
                    || __instance.IncludeArmorCategories.Length == 0
                    || (armor != null && __instance.IncludeArmorCategories.Contains(armor.Blueprint.ProficiencyGroup)) || (shield != null && __instance.IncludeArmorCategories.Contains(shield.ArmorComponent.Blueprint.ProficiencyGroup)) || (shield2 != null && __instance.IncludeArmorCategories.Contains(shield2.ArmorComponent.Blueprint.ProficiencyGroup)))
            && (__instance.ExcludeArmorCategories == null
            || __instance.ExcludeArmorCategories.Length == 0
            || ((armor == null
                 || !__instance.ExcludeArmorCategories.Contains(armor.Blueprint.ProficiencyGroup))
                 && (shield == null || !__instance.ExcludeArmorCategories.Contains(shield.ArmorComponent.Blueprint.ProficiencyGroup)) && (shield2 == null || !__instance.ExcludeArmorCategories.Contains(shield2.ArmorComponent.Blueprint.ProficiencyGroup))));
        MCEContext.Logger.Log($"Prefix ran: {__result}");
        return false;
    }
}
