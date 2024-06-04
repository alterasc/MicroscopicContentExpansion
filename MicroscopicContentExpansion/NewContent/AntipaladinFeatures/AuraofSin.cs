using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using MicroscopicContentExpansion.NewComponents;
using MicroscopicContentExpansion.Utils;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures;
internal class AuraofSin
{
    private const string NAME = "Aura of Sin";
    private const string DESCRIPTION = "At 14th level, an antipaladin’s weapons are treated as evil-aligned for the purposes" +
        " of overcoming damage reduction. Any attack made against an enemy within 10 feet of him is treated as evil-aligned" +
        " for the purposes of overcoming damage reduction. This ability functions only while the antipaladin is conscious," +
        " not if he is unconscious or dead.";

    public static void AddAuraOfSinFeature()
    {
        var AOSIcon = BlueprintTools.GetBlueprint<BlueprintAbility>("8bc64d869456b004b9db255cdd1ea734").Icon;

        var AuraOfSinEffectBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfSinEffectBuff", bp =>
        {
            bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            bp.AddComponent<AddIncomingDamageWeaponProperty>(c =>
            {
                c.AddAlignment = true;
                c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Evil;
            });
        });

        var AuraOfSinArea = AuraUtils.CreateUnconditionalHostileAuraEffect(
            modContext: MCEContext,
            bpName: "AntipaladinAuraOfSinArea",
            size: 13,
            buff: AuraOfSinEffectBuff.ToReference<BlueprintBuffReference>()
        );

        var AuraOfSinWidenArea = AuraUtils.CreateUnconditionalHostileAuraEffect(
            modContext: MCEContext,
            bpName: "AntipaladinAuraOfSinWidenArea",
            size: 22,
            buff: AuraOfSinEffectBuff.ToReference<BlueprintBuffReference>()
        );

        var AuraOfSinBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfSinBuff", bp =>
        {
            bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            bp.AddComponent<AddAreaEffect>(c =>
            {
                c.m_AreaEffect = AuraOfSinArea.ToReference<BlueprintAbilityAreaEffectReference>();
            });
        });

        var AuraOfSinWidenBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfSinWidenBuff", bp =>
        {
            bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            bp.AddComponent<AddAreaEffect>(c =>
            {
                c.m_AreaEffect = AuraOfSinWidenArea.ToReference<BlueprintAbilityAreaEffectReference>();
            });
        });

        var AuraOfSinFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAuraOfSinFeature", bp =>
        {
            bp.SetName(MCEContext, NAME);
            bp.SetDescription(MCEContext, DESCRIPTION);
            bp.m_Icon = AOSIcon;
            bp.AddComponent<AuraFeatureComponentWithWiden>(c =>
            {
                c.DefaultBuff = AuraOfSinBuff.ToReference<BlueprintBuffReference>();
                c.WidenFact = MCEContext.GetModBlueprintReference<BlueprintUnitFactReference>("WidenAurasBuff");
                c.WidenBuff = AuraOfSinWidenBuff.ToReference<BlueprintBuffReference>();
            });
            bp.AddComponent<AddOutgoingPhysicalDamageProperty>(c =>
            {
                c.AddAlignment = true;
                c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Evil;
            });
        });
    }
}
