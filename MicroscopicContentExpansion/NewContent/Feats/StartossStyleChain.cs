using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using MicroscopicContentExpansion.NewComponents;
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

                var startossStyleBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "StartossStyleBuff", bp => {
                    bp.SetName(MCEContext, "Startoss Style");
                    bp.SetDescription(MCEContext, "Choose one weapon from the thrown fighter weapon group. While using this style " +
                        "and the chosen weapon, you gain a bonus on damage rolls made with the weapon equal to 2 + 2 per style feat " +
                        "you possess that lists Startoss Style as a prerequisite (maximum +6 damage).You cannot use this ability if you " +
                        "are carrying a weapon or a shield in your off hand(except for a buckler).Special: In addition to the chosen weapon, " +
                        "a character with this feat and the weapon training(thrown) class feature can use Startoss Style with any thrown weapons " +
                        "that she wields in one hand.");
                    bp.m_Icon = startossStyleIcon;
                });

                var startossStyleAbility = Helpers.CreateBlueprint<BlueprintActivatableAbility>(MCEContext, "StartossStyleToggleAbility", bp => {
                    bp.SetName(MCEContext, "Startoss Style");
                    bp.SetDescription(MCEContext, "Choose one weapon from the thrown fighter weapon group. While using this style " +
                        "and the chosen weapon, you gain a bonus on damage rolls made with the weapon equal to 2 + 2 per style feat " +
                        "you possess that lists Startoss Style as a prerequisite (maximum +6 damage).You cannot use this ability if you " +
                        "are carrying a weapon or a shield in your off hand(except for a buckler).Special: In addition to the chosen weapon, " +
                        "a character with this feat and the weapon training(thrown) class feature can use Startoss Style with any thrown weapons " +
                        "that she wields in one hand.");
                    bp.m_Icon = startossStyleIcon;
                    bp.ActivationType = AbilityActivationType.WithUnitCommand;
                    bp.m_ActivateWithUnitCommand = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift;
                    bp.IsOnByDefault = true;
                    bp.Group = ActivatableAbilityGroup.CombatStyle;
                    bp.DeactivateImmediately = true;
                    bp.DoNotTurnOffOnRest = true;
                    bp.m_Buff = startossStyleBuff.ToReference<BlueprintBuffReference>();
                });

                var startossStyleFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "StartossStyleFeature", bp => {
                    bp.SetName(MCEContext, "Startoss Style");
                    bp.SetDescription(MCEContext, "Choose one weapon from the thrown fighter weapon group. While using this style " +
                        "and the chosen weapon, you gain a bonus on damage rolls made with the weapon equal to 2 + 2 per style feat " +
                        "you possess that lists Startoss Style as a prerequisite (maximum +6 damage).You cannot use this ability if you " +
                        "are carrying a weapon or a shield in your off hand(except for a buckler).Special: In addition to the chosen weapon, " +
                        "a character with this feat and the weapon training(thrown) class feature can use Startoss Style with any thrown weapons " +
                        "that she wields in one hand.");
                    bp.m_Icon = startossStyleIcon;
                    bp.IsClassFeature = true;
                    bp.Groups = new FeatureGroup[] {
                        FeatureGroup.CombatFeat,
                        FeatureGroup.Feat
                    };
                    bp.AddComponent<FeatureTagsComponent>(c => {
                        c.FeatureTags = FeatureTag.Attack | FeatureTag.Ranged;
                    });
                    bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                        c.Stat = StatType.Dexterity;
                        c.Value = 13;
                    });
                    bp.AddPrerequisiteFeature(BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("0da0c194d6e1d43419eb8d990b28e0ab"));
                    bp.AddPrerequisiteFeature(BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("1e1f627d26ad36f43bbd26cc2bf8ac7e"));
                    bp.AddComponent<AddFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] { startossStyleAbility.ToReference<BlueprintUnitFactReference>() };
                    });
                });
                FeatTools.AddAsFeat(startossStyleFeature);

                var startossCometAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "StartossCometAbility", bp => {
                    bp.SetName(MCEContext, "Startoss Comet");
                    bp.SetDescription(MCEContext, "As a standard action, you can make a single ranged thrown weapon attack at your " +
                        "full attack bonus with the chosen weapon. If you hit, you deal damage normally and can make a second attack " +
                        "(at your full attack bonus) against a target within one range increment of the first. You determine cover for " +
                        "this attack from the first target’s space instead of your space.You can make only one additional attack per round " +
                        "with this feat.If you have Vital Strike, Improved Vital Strike, or Greater Vital Strike, you can add the additional " +
                        "damage from those feats to the initial ranged attack(but not the second attack).");
                    bp.m_Icon = startossCometIcon;
                    bp.Type = AbilityType.Physical;
                    bp.Range = AbilityRange.Weapon;
                    bp.CanTargetEnemies = true;
                    bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special;
                    bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                    bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                    bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                    bp.AddComponent<AbilityCasterHasChosenWeaponFromGroup>(c => {
                        c.WeaponGroup = Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Thrown;
                        c.ChosenWeaponFeature = BlueprintTools.GetBlueprintReference<BlueprintParametrizedFeatureReference>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
                        c.WeaponGroupReference = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("0bbf10151dd1d8d4c8653d245e425453");
                    });
                });

                var startossCometFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "StartossCometFeature", bp => {
                    bp.SetName(MCEContext, "Startoss Comet");
                    bp.SetDescription(MCEContext, "As a standard action, you can make a single ranged thrown weapon attack at your " +
                        "full attack bonus with the chosen weapon. If you hit, you deal damage normally and can make a second attack " +
                        "(at your full attack bonus) against a target within one range increment of the first. You determine cover for " +
                        "this attack from the first target’s space instead of your space.You can make only one additional attack per round " +
                        "with this feat.If you have Vital Strike, Improved Vital Strike, or Greater Vital Strike, you can add the additional " +
                        "damage from those feats to the initial ranged attack(but not the second attack).");
                    bp.m_Icon = startossCometIcon;
                    bp.IsClassFeature = true;
                    bp.Groups = new FeatureGroup[] {
                        FeatureGroup.CombatFeat,
                        FeatureGroup.Feat
                    };
                    bp.AddComponent<FeatureTagsComponent>(c => {
                        c.FeatureTags = FeatureTag.Attack | FeatureTag.Ranged;
                    });
                    bp.AddComponent<AddFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] { startossCometAbility.ToReference<BlueprintUnitFactReference>() };
                    });
                    bp.AddPrerequisiteFeature(startossStyleFeature);
                });
                FeatTools.AddAsFeat(startossCometFeature);

                var startossShowerFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "StartossShowerFeature", bp => {
                    bp.SetName(MCEContext, "Startoss Shower");
                    bp.SetDescription(MCEContext, "When you hit an opponent while using the Startoss Comet feat, you can continue " +
                        "to make attacks against foes that are within one range increment of all previous opponents. You determine " +
                        "cover for each attack from the most recently hit foe’s space instead of your space, and you cannot attack an " +
                        "individual foe more than once during this attack action.You can make a maximum number of attacks equal to 1 + 1 " +
                        "per 5 points of base attack bonus you possess.If you have Vital Strike, Improved Vital Strike, or Greater Vital " +
                        "Strike, you can add the additional damage from those feats to the initial ranged attack(but not any subsequent attacks).");
                    bp.IsClassFeature = true;
                    bp.Groups = new FeatureGroup[] {
                        FeatureGroup.CombatFeat,
                        FeatureGroup.Feat
                    };
                    bp.AddComponent<FeatureTagsComponent>(c => {
                        c.FeatureTags = FeatureTag.Attack | FeatureTag.Ranged;
                    });
                    bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                        c.Stat = StatType.Dexterity;
                        c.Value = 13;
                    });
                    bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                        c.Stat = StatType.BaseAttackBonus;
                        c.Value = 4;
                    });
                    bp.AddPrerequisiteFeature(startossCometFeature);
                });
                FeatTools.AddAsFeat(startossShowerFeature);

                startossStyleBuff.AddComponent<StartossStyleComponent>(c => {
                    c.StartossComet = startossCometFeature.ToReference<BlueprintFeatureReference>();
                    c.StartossShower = startossShowerFeature.ToReference<BlueprintFeatureReference>();
                    c.WeaponGroup = Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Thrown;
                    c.WeaponGroupReference = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("0bbf10151dd1d8d4c8653d245e425453");
                    c.ChosenWeaponFeature = BlueprintTools.GetBlueprintReference<BlueprintParametrizedFeatureReference>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
                });

                startossCometAbility.AddComponent<AbilityCustomStartossComet>(c => {
                    c.m_MythicBlueprint = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("e07bcb271ecefec44be314e1c807c798");
                    c.m_RowdyFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("6ce0dd0cd1ef43eda9e62cdf483e05c3");
                    c.m_VitalStrike = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("14a1fc1356df9f146900e1e42142fc9d");
                    c.m_VitalStrikeImproved = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("52913092cd018da47845f36e6fbe240f");
                    c.m_VitalStrikeGreater = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("e2d1fa11f6b095e4fb2fd1dcf5e36eb3");
                    c.m_StartossShower = startossShowerFeature.ToReference<BlueprintFeatureReference>();
                });
            }
        }
    }
}
