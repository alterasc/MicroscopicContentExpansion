using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures;
internal class UnholyChampion
{
    private const string NAME = "Unholy Champion";
    private const string DESCRIPTION = "At 20th level, an antipaladin becomes a conduit for the might of the dark " +
        "powers. His DR increases to 10/good. In addition, whenever he channels negative energy or uses touch of " +
        "corruption to damage a creature, he deals the maximum possible amount.";

    public static void AddUnholyChampion()
    {
        var UnholyChampion = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinUnholyChampion", bp =>
        {
            bp.SetName(MCEContext, NAME);
            bp.SetDescription(MCEContext, DESCRIPTION);
            bp.Ranks = 1;
            bp.IsClassFeature = true;
            bp.AddComponent<AddDamageResistancePhysical>(c =>
            {
                c.Material = Kingmaker.Enums.Damage.PhysicalDamageMaterial.Adamantite;
                c.BypassedByAlignment = true;
                c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Good;
                c.Reality = Kingmaker.Enums.Damage.DamageRealityType.Ghost;
                c.Value = 10;
                c.Pool = 12;
            });
            bp.AddComponent<AutoMetamagic>(c =>
            {
                c.m_AllowedAbilities = AutoMetamagic.AllowedType.Any;
                c.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Maximize;
                c.Abilities = [
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinChannelEnergyHarm"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinChannelEnergyHeal"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionUnmodified"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionBlinded"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionCursed"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionDazed"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionDiseased"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionExhausted"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionFatigued"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionFrightened"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionNauseated"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionParalyzed"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionPoisoned"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionShaken"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionSickened"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionStaggered"),
                    MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("AntipaladinTouchOfCorruptionStunned")
                ];
            });
        });

        var perfectBodyFlawlessMind = GetBPRef<BlueprintFeatureReference>("cb61beef-4f35-4992-a09e-821d170a6582");

        var markOfVengeance = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(MCEContext, "AntipaladinAuraOfVengeanceFeature");

        Helpers.CreateBlueprint<BlueprintFeatureSelection>(MCEContext, "AntipaladinCapstone", bp =>
        {
            bp.SetName(MCEContext, "Antipaladin Capstone");
            bp.SetDescription(MCEContext, "At 20th level, antipaladin gains a powerful class feature");
            bp.m_AllFeatures = [
                UnholyChampion.ToReference<BlueprintFeatureReference>(),
                perfectBodyFlawlessMind,
                markOfVengeance
            ];
            bp.Mode = SelectionMode.Default;
            bp.Groups = [FeatureGroup.None];
            bp.IsClassFeature = true;
        });
    }
}
