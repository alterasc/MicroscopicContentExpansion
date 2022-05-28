using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using MicroscopicContentExpansion.NewComponents;
using MicroscopicContentExpansion.Utils;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Feats {
    class StartossStyleChain {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                MCEContext.Logger.LogHeader("Adding Startoss Style chain feats");

                AddStartossChain();
            }

            private static void AddStartossChain() {
                var startossStyleIcon = AssetLoader.LoadInternal(MCEContext, folder: "", file: "StartossStyle.png");
                var startossCometIcon = AssetLoader.LoadInternal(MCEContext, folder: "", file: "StartossComet.png");

                var startossCometAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "StartossCometAbility", bp => {
                    bp.SetName(MCEContext, "Startoss Comet");
                    bp.SetDescription(MCEContext, "As a standard action, you can make a single ranged thrown weapon attack at your " +
                        "full attack bonus with the chosen weapon. If you hit, you deal damage normally and can make a second attack " +
                        "(at your full attack bonus) against a target within one range increment of the first. You determine cover for " +
                        "this attack from the first target’s space instead of your space.You can make only one additional attack per round " +
                        "with this feat.If you have Vital Strike, Improved Vital Strike, or Greater Vital Strike, you can add the additional " +
                        "damage from those feats to the initial ranged attack(but not the second attack).");
                    bp.m_Icon = startossStyleIcon;
                    bp.AddComponent<AbilityCustomStartossComet>(c => {
                        c.VitalStrikeMod = 2;
                        c.m_MythicBlueprint = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("e07bcb271ecefec44be314e1c807c798");
                        c.m_RowdyFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("6ce0dd0cd1ef43eda9e62cdf483e05c3");
                    });
                    bp.Type = AbilityType.Physical;
                    bp.Range = AbilityRange.Weapon;
                    bp.CanTargetEnemies = true;
                    bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special;
                    bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                    bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                    bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                });

                var startossCometFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "StartossCometFeature", bp => {
                    bp.SetName(MCEContext, "Startoss Comet");
                    bp.SetDescription(MCEContext, "As a standard action, you can make a single ranged thrown weapon attack at your " +
                        "full attack bonus with the chosen weapon. If you hit, you deal damage normally and can make a second attack " +
                        "(at your full attack bonus) against a target within one range increment of the first. You determine cover for " +
                        "this attack from the first target’s space instead of your space.You can make only one additional attack per round " +
                        "with this feat.If you have Vital Strike, Improved Vital Strike, or Greater Vital Strike, you can add the additional " +
                        "damage from those feats to the initial ranged attack(but not the second attack).");
                    bp.m_Icon = startossStyleIcon;
                    bp.IsClassFeature = true;
                    bp.Groups = new FeatureGroup[] {
                        FeatureGroup.CombatFeat,
                        FeatureGroup.Feat
                    };
                    bp.AddComponent<FeatureTagsComponent>(c => {
                        c.FeatureTags = FeatureTag.Attack | FeatureTag.Melee;
                    });
                    bp.AddComponent<AddFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] { startossCometAbility.ToReference<BlueprintUnitFactReference>() };
                    });
                });
                FeatTools.AddAsFeat(startossCometFeature);

            }
        }
    }
}
