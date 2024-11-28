using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.RebalancedContent.MythicArmor;
internal static class MythicArmorFeatTweaks
{
    internal static void Change()
    {
        AddEnhancementToOffense();
    }

    private static void AddEnhancementToOffense()
    {
        if (MCEContext.Homebrew.MythicArmorFeats.IsDisabled("ArmorEnhancementBonusToBypassDR"))
        {
            return;
        }
        var heavyOffenseBuff = GetBP<BlueprintBuff>("5ec6fdb6fa4741798e3264c09e91c949");
        var comp = heavyOffenseBuff.GetComponents<AdditionalDiceOnAttack>().ToList();
        if (comp.Count > 0)
        {
            var diceComp = comp[0];
            heavyOffenseBuff.AddComponent<ArmorACAsDamage>(c =>
            {
                c.Value = diceComp.Value;
                c.RangeType = Kingmaker.Enums.WeaponRangeType.Melee;
            });
            heavyOffenseBuff.RemoveComponents<AdditionalDiceOnAttack>();
        }

        var mediumOffenseBuff = GetBP<BlueprintBuff>("8ddbd82754ac44c1990c212a3ed1f1c8");
        var compM = mediumOffenseBuff.GetComponents<AdditionalDiceOnAttack>().ToList();
        if (compM.Count > 0)
        {
            var diceComp = compM[0];
            mediumOffenseBuff.AddComponent<ArmorACAsDamage>(c =>
            {
                c.Value = diceComp.Value;
                c.RangeType = Kingmaker.Enums.WeaponRangeType.Melee;
            });
            mediumOffenseBuff.RemoveComponents<AdditionalDiceOnAttack>();
        }
    }
}
