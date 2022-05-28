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
                var snakeStyleIcon = AssetLoader.LoadInternal(MCEContext, folder: "", file: "Snake.png");

                var startossCometAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "StartossCometAbility", bp => {
                    bp.SetName(MCEContext, "Startoss Comet");
                    bp.SetDescription(MCEContext, "Startoss Comet");
                    bp.m_Icon = snakeStyleIcon;
                    bp.AddComponent<AbilityCustomStartossComet>(c => {
                        c.VitalStrikeMod = 1;
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
                    bp.SetDescription(MCEContext, "Startoss Comet");
                    bp.m_Icon = snakeStyleIcon;
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
