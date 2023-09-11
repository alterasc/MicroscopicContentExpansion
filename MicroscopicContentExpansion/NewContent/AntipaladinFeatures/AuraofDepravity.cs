using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using MicroscopicContentExpansion.NewComponents;
using MicroscopicContentExpansion.Utils;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures {
    internal class AuraofDepravity {
        private const string NAME = "Aura of Depravity";
        private const string DESCRIPTION = "At 17th level, an antipaladin gains DR 5/good. Each enemy within 10 feet takes a –4 penalty" +
            " on saving throws against compulsion effects. This ability functions only while the antipaladin is conscious, not if he is" +
            " unconscious or dead.";

        public static void AddAuraOfDepravityFeature() {
            var AOCIcon = BlueprintTools.GetBlueprint<BlueprintFeature>("d673c30720e8e7c4bb0903dc3c9ab649").Icon;

            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var AuraOfDepravityEffectBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfDepravityEffectBuff", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = AOCIcon;
                bp.AddComponent<SavingThrowBonusAgainstDescriptor>(c => {
                    c.SpellDescriptor = SpellDescriptor.Compulsion;
                    c.ModifierDescriptor = ModifierDescriptor.Penalty;
                    c.Value = -4;
                    c.Bonus = 0;
                });
            });

            var AuraOfDepravityArea = AuraUtils.CreateUnconditionalHostileAuraEffect(
                modContext: MCEContext,
                bpName: "AntipaladinAuraOfDepravityArea",
                size: 13,
                buff: AuraOfDepravityEffectBuff.ToReference<BlueprintBuffReference>()
            );

            var AuraOfDepravityWidenArea = AuraUtils.CreateUnconditionalHostileAuraEffect(
                modContext: MCEContext,
                bpName: "AntipaladinAuraOfDepravityWidenArea",
                size: 22,
                buff: AuraOfDepravityEffectBuff.ToReference<BlueprintBuffReference>()
            );

            var AuraOfDepravityBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfDepravityBuff", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = AuraOfDepravityArea.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });

            var AuraOfDepravityWidenBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfDepravityWidenBuff", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = AuraOfDepravityWidenArea.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });

            var AuraOfDepravityFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAuraOfDepravityFeature", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = AOCIcon;
                bp.AddComponent<AuraFeatureComponentWithWiden>(c => {
                    c.DefaultBuff = AuraOfDepravityBuff.ToReference<BlueprintBuffReference>();
                    c.WidenFact = MCEContext.GetModBlueprintReference<BlueprintUnitFactReference>("WidenAurasBuff");
                    c.WidenBuff = AuraOfDepravityWidenBuff.ToReference<BlueprintBuffReference>();
                });
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.BypassedByAlignment = true;
                    c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Good;
                    c.Value = 5;
                });
            });
        }
    }
}
