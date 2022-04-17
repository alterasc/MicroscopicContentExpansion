using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Classes {
    class RadianceFix {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (MCEContext.Fixes.Miscellaneous.IsDisabled("Radiance_6_Fix")) { return; }
                MCEContext.Logger.LogHeader("Patching Radiance");

                FixRadianceEnchantments();

            }
            static void FixRadianceEnchantments() {
                var radiance6Bad = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("bff8a4bb7f24a2c499db0781b5750133");
                var radianceBadBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b894f848bf557df47aacb00f2463c8f9");
                var baneLivingEnch = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("e1d6f5e3cd3855b43a0cb42f6c747e1c");
                radianceBadBuff.AddComponent<BuffEnchantSpecificWeaponWorn>(c => {
                    c.m_EnchantmentBlueprint = baneLivingEnch.ToReference<BlueprintItemEnchantmentReference>();
                    c.m_WeaponBlueprint = radiance6Bad;
                });

                var radiance6Good = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("cf5c1a507825f184dacbc3abe14b9db1");
                var radianceGoodBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f10cba2c41612614ea28b5fc2743bc4c");
                var holyEnch = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("28a9964d81fedae44bae3ca45710c140");
                radianceGoodBuff.AddComponent<BuffEnchantSpecificWeaponWorn>(c => {
                    c.m_EnchantmentBlueprint = holyEnch.ToReference<BlueprintItemEnchantmentReference>();
                    c.m_WeaponBlueprint = radiance6Good;
                });
            }
        }
    }
}
