using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using MicroscopicContentExpansion.Utils;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures {
    internal class AuraofSin {
        private const string NAME = "Aura of Sin";
        private const string DESCRIPTION = "At 14th level, an antipaladin’s weapons are treated as evil-aligned for the purposes" +
            " of overcoming damage reduction. Any attack made against an enemy within 10 feet of him is treated as evil-aligned" +
            " for the purposes of overcoming damage reduction. This ability functions only while the antipaladin is conscious," +
            " not if he is unconscious or dead.";

        public static void AddAuraOfSinFeature() {
            var AOSIcon = BlueprintTools.GetBlueprint<BlueprintAbility>("8bc64d869456b004b9db255cdd1ea734").Icon;

            var AuraOfSinEffectBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfSinEffectBuff", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = AOSIcon;
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Frequency = DurationRate.Rounds;
                bp.Stacking = StackingType.Replace;
                bp.AddComponent<AddIncomingDamageWeaponProperty>(c => {
                    c.Material = Kingmaker.Enums.Damage.PhysicalDamageMaterial.Adamantite;
                    c.AddAlignment = true;
                    c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Evil;
                    c.Reality = Kingmaker.Enums.Damage.DamageRealityType.Ghost;
                });
                bp.FxOnRemove = new PrefabLink();
                bp.FxOnStart = new PrefabLink();
            });

            var AuraOfSinArea = Helpers.CreateBlueprint<BlueprintAbilityAreaEffect>(MCEContext, "AntipaladinAuraOfSinArea", bp => {
                bp.AggroEnemies = true;
                bp.Shape = AreaEffectShape.Cylinder;
                bp.Size = 13.Feet();
                bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
                bp.AddComponent(AuraUtils.CreateUnconditionalAuraEffect(AuraOfSinEffectBuff.ToReference<BlueprintBuffReference>()));
            });

            var AuraOfSinBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfSinBuff", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = AOSIcon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = AuraOfSinArea.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });

            var AuraOfSinFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAuraOfSinFeature", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = AOSIcon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AuraFeatureComponent>(c => {
                    c.m_Buff = AuraOfSinBuff.ToReference<BlueprintBuffReference>();
                });

                bp.AddComponent<AddOutgoingPhysicalDamageProperty>(c => {
                    c.Material = Kingmaker.Enums.Damage.PhysicalDamageMaterial.Adamantite;
                    c.AddAlignment = true;
                    c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Evil;
                    c.Reality = Kingmaker.Enums.Damage.DamageRealityType.Ghost;
                });
            });
        }
    }
}
