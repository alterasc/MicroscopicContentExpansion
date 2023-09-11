using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using MicroscopicContentExpansion.NewComponents;
using MicroscopicContentExpansion.Utils;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures {
    internal class AuraofDespair {
        private const string NAME = "Aura of Despair";
        private const string DESCRIPTION = "At 8th level, enemies within 10 feet of an antipaladin take a –2 penalty on all saving throws." +
            " This penalty does not stack with the penalty from aura of cowardice.\nThis ability functions only while the antipaladin is" +
            " conscious, not if he is unconscious or dead.";

        public static void AddAuraOfDespairFeature() {

            var CrushingDespairIcon = BlueprintTools.GetBlueprint<BlueprintAbility>("4baf4109145de4345861fe0f2209d903").Icon;
            var AuraOfDespairEffectBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfDespairEffectBuff", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = CrushingDespairIcon;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.Penalty;
                    c.Value = -2;
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveFortitude;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.Penalty;
                    c.Value = -2;
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveReflex;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.Penalty;
                    c.Value = -2;
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveWill;
                });
            });

            var AuraOfDespairArea = AuraUtils.CreateUnconditionalHostileAuraEffect(
                modContext: MCEContext,
                bpName: "AntipaladinAuraOfDespairArea",
                size: 13,
                buff: AuraOfDespairEffectBuff.ToReference<BlueprintBuffReference>()
            );

            var AuraOfDespairWidenArea = AuraUtils.CreateUnconditionalHostileAuraEffect(
                modContext: MCEContext,
                bpName: "AntipaladinAuraOfDespairWidenArea",
                size: 22,
                buff: AuraOfDespairEffectBuff.ToReference<BlueprintBuffReference>()
            );

            var AuraOfDespairBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfDespairBuff", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = AuraOfDespairArea.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });

            var AuraOfDespairWidenBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfDespairWidenBuff", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = AuraOfDespairWidenArea.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });

            var AuraOfDespairFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAuraOfDespairFeature", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = CrushingDespairIcon;
                bp.AddComponent<AuraFeatureComponentWithWiden>(c => {
                    c.DefaultBuff = AuraOfDespairBuff.ToReference<BlueprintBuffReference>();
                    c.WidenFact = MCEContext.GetModBlueprintReference<BlueprintUnitFactReference>("WidenAurasBuff");
                    c.WidenBuff = AuraOfDespairWidenBuff.ToReference<BlueprintBuffReference>();
                });
            });
        }
    }
}
