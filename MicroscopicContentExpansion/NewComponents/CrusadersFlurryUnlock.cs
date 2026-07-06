using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using MicroscopicContentExpansion.NewContent.Feats;
using System.Reflection.Emit;
using TabletopTweaks.Core.NewUnitParts;

namespace MicroscopicContentExpansion.NewComponents;

[HarmonyPatch(typeof(MonkNoArmorAndMonkWeaponFeatureUnlock), nameof(MonkNoArmorAndMonkWeaponFeatureUnlock.CheckEligibility))]
internal static class CrusaderMonkWeaponUnlockPatch
{
    private static bool Enabled = false;
    private static BlueprintFeatureSelectionReference _deitySelection;
    private static BlueprintFeatureReference _weaponFocus;

    [HarmonyPrepare]
    internal static void Init()
    {
        if (Enabled) { return; }

        _deitySelection = GetBPRef<BlueprintFeatureSelectionReference>("59e7a76987fe3b547b9cce045f4db3e4");
        _weaponFocus = GetBPRef<BlueprintFeatureReference>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
        Enabled = true;
        MCEContext.Logger.Log($"Finished CrusaderMonkWeaponUnlockPatch.Init");
    }

    [HarmonyBefore("DarkCodex")]
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> IsMonkOverride(IEnumerable<CodeInstruction> original)
    {
        var callToIsMonk = typeof(BlueprintItemWeapon).GetProperty(nameof(BlueprintItemWeapon.IsMonk)).GetMethod;

        foreach (var instr in original)
        {
            yield return instr;
            if (instr.Calls(callToIsMonk))
            {
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return CodeInstruction.Call(typeof(CrusaderMonkWeaponUnlockPatch), nameof(IsCrusaderWeapon));
            }
        }
    }


    internal static bool IsCrusaderWeapon(bool previousResult, MonkNoArmorAndMonkWeaponFeatureUnlock component)
    {
        if (previousResult)
            return previousResult;
        var owner = component.Owner;
        var unitPart = owner.Get<UnitPartCustomMechanicsFeatures>();
        if (unitPart is null || !unitPart.GetMechanicsFeature(CrusadersFlurry.CrusadersFlurryMechanicsFeature))
            return false;

        if (owner.Body.PrimaryHand.MaybeWeapon == null)
            return false;
        var primaryWeapon = owner.Body.PrimaryHand.Weapon.Blueprint;
        if (primaryWeapon.m_Type.Get().m_AttackType != Kingmaker.RuleSystem.AttackType.Melee)
            return false;
        if (owner.GetFeature((BlueprintFeature)_weaponFocus, (FeatureParam)primaryWeapon.Category) == null)
            return false;

        if (MCEContext.AddedContent.Feats.IsEnabled("CrusadersFlurryNoGodCheck"))
            return true;

        owner.Progression.Selections.TryGetValue(_deitySelection?.Get(), out var selection);
        if (selection == null)
            return false;
        if (!selection.m_SelectionsByLevel.TryGetValue(1, out var selectedAtLvl1)) return false;
        if (selectedAtLvl1.Count == 0) return false;
        var selectedDeity = selectedAtLvl1.First();
        var comp = selectedDeity.GetComponent<AddStartingEquipment>();
        if (comp == null || comp.m_BasicItems == null || comp.m_BasicItems.Count() == 0)
            return false;
        var favoredWeaponType = ((BlueprintItemWeapon)comp.m_BasicItems[0].Get()).m_Type;
        if (primaryWeapon.m_Type.Equals(favoredWeaponType))
        {
            return true;
        }

        return false;
    }
}