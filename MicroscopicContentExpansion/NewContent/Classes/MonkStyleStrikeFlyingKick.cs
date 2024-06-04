using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.TurnBasedModifiers;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Enums;
using Kingmaker.Localization.Shared;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using MicroscopicContentExpansion.NewComponents;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Classes;
internal class MonkStyleStrikeFlyingKick
{
    internal static void AddFlyingKick()
    {

        var monkClassRef = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("e8f21e5b58e0569468e420ebea456124");

        var flyingKickIcon = AssetLoader.LoadInternal(MCEContext, folder: "", file: "FlyingKick.png");

        var fastMovementFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("73ab5b41fa465ee429ad6658368c6629");
        var monkFlurryOfBlowstUnlock = BlueprintTools.GetBlueprint<BlueprintFeature>("fd99770e6bd240a4aab70f7af103e56a");

        var flurryOfBlowsRef = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("332362f3bd39ebe46a740a36960fdcb4");

        var mountedBuffRef = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("b2d13e8f3bb0f1d4c891d71b4d983cf7");

        var flyingKickAnimationBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "FlyingKickAnimationBuff", a =>
        {
            a.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            a.AddComponent<SpecialAnimationMonkKick>();
        });

        var flyingKickFlurrySuppressionBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "FlyingKickFlurrySuppressionBuff", a =>
        {
            a.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            a.AddComponent<ReduceAttacksCount>(c =>
            {
                c.ReduceCount = 1;
                c.OnlyFromPrimaryHand = true;
                c.Condition = new();
            });
        });
        List<BlueprintUnitFactReference> abilities = [];
        var abilityName = Helpers.CreateString(MCEContext, $"FlyingKickAbility.Name", "Flying Kick");
        var abilityDescription = Helpers.CreateString(MCEContext, "FlyingKickAbility.Description", "The monk leaps through the air to strike a foe with a kick. Before the attack, the monk can move a distance equal to his fast movement bonus. This movement is made as part of the monk’s flurry of blows attack. This movement provokes an attack of opportunity as normal. Monk has to be unarmed to perform this style strike.", Locale.enGB, shouldProcess: true);
        for (int i = 1; i <= 6; i++)
        {
            var flyingKickAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, $"FlyingKickAbility{i}", a =>
            {
                a.m_DisplayName = abilityName;
                a.m_Description = abilityDescription;
                a.Type = AbilityType.Physical;
                a.CanTargetEnemies = true;
                a.CanTargetFriends = false;
                a.CanTargetPoint = false;
                a.CanTargetSelf = false;
                a.ShouldTurnToTarget = true;
                a.EffectOnAlly = AbilityEffectOnUnit.None;
                a.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                a.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                a.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                a.Range = AbilityRange.Custom;
                a.CustomRange = (i * 10).Feet();
                a.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                a.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                a.m_Icon = flyingKickIcon;
                a.AddComponent<HideDCFromTooltip>();
                a.AddComponent<AbilityIsFullRoundInTurnBased>(c => c.FullRoundIfTurnBased = true);
                a.AddComponent<AbilityRequirementCanMove>();
                a.AddComponent<AbilityRequirementHasItemInHands>(c =>
                {
                    c.m_Type = AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon;
                    c.m_ExcludeLimbs = true;
                });
                a.AddComponent<AbilityCasterHasNoFacts>(c =>
                {
                    c.m_Facts = [mountedBuffRef];
                });
                a.AddComponent<AbilityCustomFlyingKick>(c =>
                {
                    c.m_AddBuffWhileRunning = flyingKickAnimationBuff.ToReference<BlueprintBuffReference>();
                    c.m_FlurrySuppressionBuff = flyingKickFlurrySuppressionBuff.ToReference<BlueprintBuffReference>();
                });
                a.AddComponent<AbilityCasterHasFacts>(c =>
                {
                    c.m_Facts = [flurryOfBlowsRef];
                });
                a.AddComponent<AbilityCasterMainWeaponCheck>(c =>
                {
                    c.Category = [WeaponCategory.UnarmedStrike];
                });
            });
            abilities.Add(flyingKickAbility.ToReference<BlueprintUnitFactReference>());
        }

        var flyingKickStyleStrikeBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "FlyingKickStyleStrikeBuff", a =>
        {
            a.m_DisplayName = abilityName;
            a.m_Description = abilityDescription;
            a.m_Flags = BlueprintBuff.Flags.StayOnDeath & BlueprintBuff.Flags.HiddenInUi;
            a.AddComponent<AddFeatureDependingOnClassLevel>(c =>
            {
                c.m_Class = monkClassRef;
                c.m_AdditionalClasses = [];
                c.m_Archetypes = [];
                c.featureArray = abilities.ToArray();
            });
        });

        var flyingKickActivatableAbility = Helpers.CreateBlueprint<BlueprintActivatableAbility>(MCEContext, "FlyingKickActivatableAbility", a =>
        {
            a.SetName(MCEContext, "Flying Kick - Activate Style Strike");
            a.SetDescription(MCEContext, "The monk leaps through the air to strike a foe with a kick. Before the attack, the monk can move a distance equal to his fast movement bonus. This movement is made as part of the monk’s flurry of blows attack. This movement provokes an attack of opportunity as normal. Monk has to be unarmed to perform this style strike.\r\n\r\nActivate this style strike to receive flying kick ability.");
            a.Group = ActivatableAbilityGroup.StyleStrike;
            a.ActivationType = AbilityActivationType.Immediately;
            a.m_ActivateWithUnitCommand = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
            a.m_Icon = flyingKickIcon;
            a.m_Buff = flyingKickStyleStrikeBuff.ToReference<BlueprintBuffReference>();
        });

        var flyingKickFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "FlyingKickFeature", a =>
        {
            a.m_DisplayName = abilityName;
            a.m_Description = abilityDescription;
            a.AddPrerequisiteFeature(fastMovementFeature);
            a.AddPrerequisiteFeature(monkFlurryOfBlowstUnlock);
            a.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = [flyingKickActivatableAbility.ToReference<BlueprintUnitFactReference>()];
            });
            a.m_Icon = flyingKickIcon;
        });
        if (MCEContext.AddedContent.Feats.IsEnabled("FlyingKickStyleStrike"))
        {
            var styleStrikeSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("7bc6a93f6e48eff49be5b0cde83c9450");
            styleStrikeSelection.m_AllFeatures = styleStrikeSelection.m_AllFeatures.AppendToArray(flyingKickFeature.ToReference<BlueprintFeatureReference>());
        }
    }
}