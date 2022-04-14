using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
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
                bp.Frequency = DurationRate.Rounds;
                bp.IsClassFeature = true;
            });


            var AuraOfDespairArea = Helpers.CreateBlueprint<BlueprintAbilityAreaEffect>(MCEContext, "AntipaladinAuraOfDespairArea", bp => {
                bp.AggroEnemies = true;
                bp.AffectEnemies = true;
                bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
                bp.Shape = AreaEffectShape.Cylinder;
                bp.Size = new Feet() { m_Value = 13 };
                bp.Fx = new PrefabLink();
                bp.AddComponent<AbilityAreaEffectBuff>(c => {
                    c.m_Buff = AuraOfDespairEffectBuff.ToReference<BlueprintBuffReference>();
                    c.Condition = ActionFlow.IfSingle<ContextConditionIsEnemy>();
                });
            });

            var AuraOfDespairBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfDespairBuff", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = CrushingDespairIcon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = AuraOfDespairArea.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });

            var AuraOfDespairFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAuraOfDespairFeature", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = CrushingDespairIcon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AuraFeatureComponent>(c => {
                    c.m_Buff = AuraOfDespairBuff.ToReference<BlueprintBuffReference>();
                });
            });
        }
    }
}
