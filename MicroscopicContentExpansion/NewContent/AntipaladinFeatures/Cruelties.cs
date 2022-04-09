using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Base.Main;

namespace MicroscopicContentExpansion.Base.NewContent.AntipaladinFeatures {
    internal class Cruelties {

        private const string DESCRIPTION = "At 3rd level, and every three levels thereafter, an antipaladin" +
            " can select one cruelty. Each cruelty adds an effect to the antipaladin’s touch of corruption ability. " +
            "Whenever the antipaladin uses touch of corruption to deal damage to one target, the target also receives" +
            " the additional effect from one of the cruelties possessed by the antipaladin. This choice is made when" +
            " the touch is used. The target receives a Fortitude save to avoid this cruelty. If the save is successful," +
            " the target takes the damage as normal, but not the effects of the cruelty. The DC of this save is equal to" +
            " 10 + 1/2 the antipaladin’s level + the antipaladin’s Charisma modifier";

        private static void AddFatiquedCruelty(out BlueprintFeature CrueltyFeature, out BlueprintAbility CrueltyAbility) {
            var FatigueIcon = BlueprintTools.GetBlueprint<BlueprintBuff>("e6f2fc5d73d88064583cb828801212f4");
            var FatiguedBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e6f2fc5d73d88064583cb828801212f4");
            CrueltyFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltyFatiquedFeature", bp => {
                bp.SetName(MCEContext, "Cruelty - Fatiqued");
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_Icon = FatigueIcon.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true; ;
            });

            var Action = new ContextActionSavingThrow() {
                m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0],
                Type = SavingThrowType.Fortitude,
                HasCustomDC = false,
                CustomDC = new ContextValue(),
                Actions = Helpers.CreateActionList(new ContextActionConditionalSaved() {
                    Succeed = new ActionList(),
                    Failed = Helpers.CreateActionList(
                            new ContextActionApplyBuff() {
                                m_Buff = FatiguedBuff,
                                Permanent = true,
                                DurationValue = new ContextDurationValue() {
                                    m_IsExtendable = true,
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue()
                                },
                                IsFromSpell = true,
                            }),
                })
            };

            CrueltyAbility = TouchofCorruption.CreateTouchOfCorruption(
                "AntipaladinTouchOfCorruptionFatiqued",
                "Touch of Corruption - Fatiqued",
                "Applies Touch of Corruption with Fatiqued cruelty.\nOn failed saving throw the target is fatigued.",
                FatigueIcon.Icon,
                Action,
                CrueltyFeature.ToReference<BlueprintUnitFactReference>()
            );
        }
        public static void AddCruelties() {

            AddFatiquedCruelty(out var FatiquedCrueltyFeaure, out var FatiquedCrueltyAbility);

            var BaseAbility = BlueprintTools.GetModBlueprint<BlueprintAbility>(MCEContext, "AntipaladinTouchOfCorruptionBase");
            var variants = BaseAbility.GetComponent<AbilityVariants>();
            variants.m_Variants = variants.m_Variants.AppendToArray(FatiquedCrueltyAbility.ToReference<BlueprintAbilityReference>());

            var CrueltySelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(MCEContext, "AntipaladinCrueltySelection", bp => {
                bp.SetName(MCEContext, "Cruelty");
                bp.SetDescription(MCEContext, "At 3rd level, and every three levels thereafter, an antipaladin can select one cruelty." +
                    " Each cruelty adds an effect to the antipaladin’s touch of corruption ability. Whenever the antipaladin uses " +
                    "touch of corruption to deal damage to one target, the target also receives the additional effect from one of the" +
                    " cruelties possessed by the antipaladin. This choice is made when the touch is used. The target receives a " +
                    "Fortitude save to avoid this cruelty. If the save is successful, the target takes the damage as normal, but not" +
                    " the effects of the cruelty. The DC of this save is equal to 10 + 1/2 the antipaladin’s level + the antipaladin’s" +
                    " Charisma modifier.");
                bp.m_AllFeatures = new BlueprintFeatureReference[] {
                                FatiquedCrueltyFeaure.ToReference<BlueprintFeatureReference>()
                            };
                bp.Mode = SelectionMode.Default;
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.IsClassFeature = true;
            });
        }
    }
}
