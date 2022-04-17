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
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using MicroscopicContentExpansion.Utils;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures {
    internal class AuraofCowardice {
        private const string NAME = "Aura of Cowardice";
        private const string DESCRIPTION = "At 3rd level, an antipaladin radiates a palpably daunting aura that causes all enemies" +
                    " within 10 feet to take a –4 penalty on saving throws against fear effects. Creatures that are normally immune to" +
                    " fear lose that immunity while within 10 feet of an antipaladin with this ability. This ability functions only" +
                    " while the antipaladin remains conscious, not if he is unconscious or dead.";

        public static void AddAuraOfCowardiceFeature() {
            var AOCIcon = BlueprintTools.GetBlueprint<BlueprintAbility>("d2aeac47450c76347aebbc02e4f463e0").Icon;
            var AntipaladinClassRef = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(MCEContext, "AntipaladinClass");

            var AuraOfCowardiceEffectBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfCowardiceEffectBuff", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = AOCIcon;

                bp.AddComponent<SavingThrowContextBonusAgainstDescriptor>(c => {
                    c.SpellDescriptor = SpellDescriptor.Fear;
                    c.ModifierDescriptor = ModifierDescriptor.Penalty;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClassRef };
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 7,
                            ProgressionValue = -4
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 100,
                            ProgressionValue = -2
                        }
                    };
                });

                bp.AddComponent<AbilityAreaEffectRunAction>(c => {
                    c.Round = Helpers.CreateActionList(
                        new ContextActionRemoveBuffsByDescriptor() {
                            SpellDescriptor = SpellDescriptor.FearImmunity
                        });
                });
                bp.Frequency = DurationRate.Rounds;
                bp.IsClassFeature = true;
                bp.FxOnRemove = new PrefabLink();
                bp.FxOnStart = new PrefabLink();
            });


            var AuraOfCowardiceArea = Helpers.CreateBlueprint<BlueprintAbilityAreaEffect>(MCEContext, "AntipaladinAuraOfCowardiceArea", bp => {
                bp.AggroEnemies = true;
                bp.AffectEnemies = true;
                bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
                bp.Shape = AreaEffectShape.Cylinder;
                bp.Size = 13.Feet();
                bp.Fx = new PrefabLink();
                bp.AddComponent(AuraUtils.CreateUnconditionalAuraEffect(AuraOfCowardiceEffectBuff.ToReference<BlueprintBuffReference>()));
            });

            var AuraOfCowardiceBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "AntipaladinAuraOfCowardiceBuff", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = AOCIcon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = AuraOfCowardiceArea.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });

            var AuraOfCowardiceFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAuraOfCowardiceFeature", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_Icon = AOCIcon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AuraFeatureComponent>(c => {
                    c.m_Buff = AuraOfCowardiceBuff.ToReference<BlueprintBuffReference>();
                });
            });
        }
    }
}
