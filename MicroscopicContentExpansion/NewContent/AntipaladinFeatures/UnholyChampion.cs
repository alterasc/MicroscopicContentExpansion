using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures {
    internal class UnholyChampion {
        private const string NAME = "Unholy Champion";
        private const string DESCRIPTION = "At 20th level, an antipaladin becomes a conduit for the might of the dark " +
            "powers. His DR increases to 10/good. In addition, whenever he channels negative energy or uses touch of " +
            "corruption to damage a creature, he deals the maximum possible amount.";

        public static void AddUnholyChampion() {

            var UnholyChampion = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinUnholyChampion", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Material = Kingmaker.Enums.Damage.PhysicalDamageMaterial.Adamantite;
                    c.BypassedByAlignment = true;
                    c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Good;
                    c.Reality = Kingmaker.Enums.Damage.DamageRealityType.Ghost;
                    c.Value = 10;
                    c.Pool = 12;
                });
                bp.AddComponent<AutoMetamagic>(c => {
                    c.m_AllowedAbilities = AutoMetamagic.AllowedType.Any;
                    c.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Maximize;
                    c.Abilities = new List<BlueprintAbilityReference> {
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinChannelEnergyHarm"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinChannelEnergyHeal"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionUnmodified"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionBlinded"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionCursed"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionDazed"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionDiseased"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionExhausted"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionFatiqued"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionFrightened"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionNauseated"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionParalyzed"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionPoisoned"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionShaken"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionSickened"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionStaggered"),
                        BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(MCEContext, "AntipaladinTouchOfCorruptionStunned")
                    };
                });
            });

            var TipoftheSpear = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(MCEContext, "AntipaladinTipoftheSpear");

            Helpers.CreateBlueprint<BlueprintFeatureSelection>(MCEContext, "AntipaladinCapstone", bp => {
                bp.SetName(MCEContext, "Antipaladin Capstone");
                bp.SetDescription(MCEContext, "At 20th level, antipaladin gains a powerful class feature");
                bp.m_AllFeatures = new BlueprintFeatureReference[] {
                    UnholyChampion.ToReference<BlueprintFeatureReference>(),
                    TipoftheSpear
                };
                bp.Mode = SelectionMode.Default;
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.IsClassFeature = true;
            });
        }
    }
}
