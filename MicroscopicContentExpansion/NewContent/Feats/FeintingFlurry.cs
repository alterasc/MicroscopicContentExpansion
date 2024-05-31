using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.DLC;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization.Shared;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Feats;
internal class FeintingFlurry {
    internal static void Add() {

        var feintingFlurryIcon = AssetLoader.LoadInternal(MCEContext, folder: "", file: "FeintingFlurry.png");

        var feint = BlueprintTools.GetBlueprint<BlueprintFeature>("c610310d31414edabcedf0c8a6fe32c4");
        if (feint == null) return;

        var feintAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("1bb6f0b196aa457ba80bdb312dc64952");

        var monkFlurry = BlueprintTools.GetBlueprint<BlueprintFeature>("fd99770e6bd240a4aab70f7af103e56a");
        var qmFlurry = BlueprintTools.GetBlueprint<BlueprintFeature>("44b0f313ec56481eb447019fbe714330");
        var soheiFlurry = BlueprintTools.GetBlueprint<BlueprintFeature>("cd4381b73b6709146bbcc0a528a6f471");

        var dlc6Reward = BlueprintTools.GetBlueprintReference<BlueprintDlcRewardReference>("b94f823171a84e30ad7a1b892433ab5d");

        var description = Helpers.CreateString(MCEContext, "FeintingFlurry.Description", "While using flurry of blows to make {g|Encyclopedia:MeleeAttack}melee attacks{/g}, you can forgo your melee attack to make a {g|Encyclopedia:Persuasion}Persuasion{/g} (bluff) {g|Encyclopedia:Check}check{/g} to feint an opponent.", Locale.enGB, shouldProcess: true);

        var buff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "FeintingFlurryBuff", a => {
            a.SetName(MCEContext, "Feinting Flurry");
            a.m_Description = description;
            a.m_Icon = feintingFlurryIcon;
            a.AddComponent<ReduceAttacksCount>(c => {
                c.ReduceCount = 1;
                c.OnlyFromPrimaryHand = true;
                c.Condition = new();
            });
            a.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                c.TriggerBeforeAttack = true;
                c.OnlyHit = true;
                c.OnMiss = false;
                c.OnlyOnFullAttack = true;
                c.OnlyOnFirstAttack = true;
                c.OnlyOnFirstHit = false;
                c.CriticalHit = false;
                c.OnlyNatural20 = false;
                c.OnAttackOfOpportunity = false;
                c.NotCriticalHit = false;
                c.OnlySneakAttack = false;
                c.NotSneakAttack = false;
                c.CheckWeaponBlueprint = false;
                c.CheckWeaponCategory = false;
                c.CheckWeaponGroup = false;
                c.CheckWeaponRangeType = true;
                c.RangeType = Kingmaker.Enums.WeaponRangeType.Melee;
                c.CheckPhysicalDamageForm = false;
                c.ReduceHPToZero = false;
                c.DamageMoreTargetMaxHP = false;
                c.CheckDistance = false;
                c.AllNaturalAndUnarmed = false;
                c.DuelistWeapon = false;
                c.NotExtraAttack = false;
                c.OnCharge = false;
                c.IgnoreAutoHit = false;
                c.ActionsOnInitiator = false;
                c.Action = new() {
                    Actions = [
                        new ContextActionCastSpell() {
                            m_Spell = feintAbility,
                            OverrideSpellbook = false,
                            OverrideDC = false,
                            DC = 0,
                            OverrideSpellLevel = false,
                            SpellLevel = 0,
                            CastByTarget = false,
                            LogIfCanNotTarget = false,
                            MarkAsChild = false
                        }
                    ]
                };
            });
        });

        var ability = Helpers.CreateBlueprint<BlueprintActivatableAbility>(MCEContext, "FeintingFlurryActivatableAbility", a => {
            a.SetName(MCEContext, "Feinting Flurry");
            a.m_Description = description;
            a.m_Icon = feintingFlurryIcon;
            a.DeactivateImmediately = true;
            a.ActivationType = AbilityActivationType.WithUnitCommand;
            a.m_ActivateWithUnitCommand = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
            a.m_ActivateOnUnitAction = AbilityActivateOnUnitActionType.Attack;
            a.m_Buff = buff.ToReference<BlueprintBuffReference>();
        });

        var feintingFlurry = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "FeintingFlurry", a => {
            a.SetName(MCEContext, "Feinting Flurry");
            a.m_Description = description;
            a.m_Icon = feintingFlurryIcon;
            a.AddComponent<AddFacts>(c => {
                c.m_Facts = [ability.ToReference<BlueprintUnitFactReference>()];
            });
            a.AddPrerequisiteFeature(feint);
            a.AddPrerequisite<PrerequisiteStatValue>(c => {
                c.Stat = StatType.Dexterity;
                c.Value = 15;
            });
            a.AddPrerequisite<PrerequisiteStatValue>(c => {
                c.Stat = StatType.Intelligence;
                c.Value = 13;
            });
            a.AddPrerequisiteFeaturesFromList(1, monkFlurry, qmFlurry, soheiFlurry);
            a.AddComponent<DlcCondition>(c => {
                c.m_DlcReward = dlc6Reward;
            });
            a.Groups = [
                FeatureGroup.CombatFeat,
                FeatureGroup.Feat
            ];
        });

        if (MCEContext.AddedContent.Feats.IsDisabled("FeintingFlurry")) { return; }
        FeatTools.AddAsFeat(feintingFlurry);
    }
}
