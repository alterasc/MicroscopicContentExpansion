using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
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
    internal class AuraofDepravity {
        private const string NAME = "Aura of Depravity";
        private const string DESCRIPTION = "At 17th level, an antipaladin gains DR 5/good. Each enemy within 10 feet takes a –4 penalty" +
            " on saving throws against compulsion effects. This ability functions only while the antipaladin is conscious, not if he is" +
            " unconscious or dead.";

        public static void AddAuraOfDepravityFeature() {
            var AOCIcon = BlueprintTools.GetBlueprint<BlueprintFeature>("d673c30720e8e7c4bb0903dc3c9ab649").Icon;

            var AntipaladinClassRef = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(MCEContext, "AntipaladinClass");

            var AuraOfDepravityEffectBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfDepravityEffectBuff", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = AOCIcon;
                bp.AddComponent<SavingThrowBonusAgainstDescriptor>(c => {
                    c.SpellDescriptor = SpellDescriptor.Compulsion;
                    c.ModifierDescriptor = ModifierDescriptor.Penalty;
                    c.Value = -4;
                    c.Bonus = new ContextValue {
                        ValueType = ContextValueType.Simple,
                        Value = 0,
                        ValueRank = AbilityRankType.Default,
                        Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.None
                    };
                });
                bp.Frequency = DurationRate.Rounds;
                bp.IsClassFeature = true;
            });


            var AuraOfDepravityArea = Helpers.CreateBlueprint<BlueprintAbilityAreaEffect>(MCEContext, "AntipaladinAuraOfDepravityArea", bp => {
                bp.AggroEnemies = true;
                bp.AffectEnemies = true;
                bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
                bp.Shape = AreaEffectShape.Cylinder;
                bp.Size = new Feet() { m_Value = 13 };
                bp.Fx = new PrefabLink();
                bp.AddComponent<AbilityAreaEffectBuff>(c => {
                    c.m_Buff = AuraOfDepravityEffectBuff.ToReference<BlueprintBuffReference>();
                    c.Condition = ActionFlow.IfSingle<ContextConditionIsEnemy>();
                });
            });

            var AuraOfDepravityBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfDepravityBuff", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = AOCIcon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = AuraOfDepravityArea.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });

            var AuraOfDepravityFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAuraOfDepravityFeature", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = AOCIcon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AuraFeatureComponent>(c => {
                    c.m_Buff = AuraOfDepravityBuff.ToReference<BlueprintBuffReference>();
                });
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Material = Kingmaker.Enums.Damage.PhysicalDamageMaterial.Adamantite;
                    c.BypassedByAlignment = true;
                    c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Good;
                    c.Reality = Kingmaker.Enums.Damage.DamageRealityType.Ghost;
                    c.Value = 5;
                    c.Pool = 12;
                });
            });
        }
    }
}
