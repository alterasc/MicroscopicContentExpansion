using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using MicroscopicContentExpansion.NewComponents;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Feats;
class SnakeStyleChain
{

    internal static void AddSnakeStyle()
    {
        MCEContext.Logger.LogHeader("Adding Snake Style chain feats");

        var snakeStyleIcon = AssetLoader.LoadInternal(MCEContext, folder: "", file: "Snake.png");

        var snakeStyleBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "SnakeStyleBuff", bp =>
        {
            bp.SetName(MCEContext, "Snake Style");
            bp.SetDescription(MCEContext, "You gain a +2 dodge bonus to AC and you can deal piercing damage with your unarmed strikes.");
            bp.m_Icon = snakeStyleIcon;
            bp.AddComponent<AddStatBonus>(c =>
            {
                c.Stat = StatType.AC;
                c.Descriptor = ModifierDescriptor.Dodge;
                c.Value = 2;
            });
            bp.FxOnStart = new Kingmaker.ResourceLinks.PrefabLink();
            bp.FxOnRemove = new Kingmaker.ResourceLinks.PrefabLink();
        });

        var snakeStyleAbility = Helpers.CreateBlueprint<BlueprintActivatableAbility>(MCEContext, "SnakeStyleToggleAbility", bp =>
        {
            bp.SetName(MCEContext, "Snake Style");
            bp.SetDescription(MCEContext, "You gain a +2 dodge bonus to AC and you can deal piercing damage with your unarmed strikes.");
            bp.m_Icon = snakeStyleIcon;
            bp.ActivationType = AbilityActivationType.Immediately;
            bp.m_ActivateWithUnitCommand = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift;
            bp.m_ActivateOnUnitAction = AbilityActivateOnUnitActionType.Attack;
            bp.IsOnByDefault = true;
            bp.Group = ActivatableAbilityGroup.CombatStyle;
            bp.m_Buff = snakeStyleBuff.ToReference<BlueprintBuffReference>();
        });

        var snakeStyleFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "SnakeStyleFeature", bp =>
        {
            bp.SetName(MCEContext, "Snake Style");
            bp.SetDescription(MCEContext, "You gain a +2 dodge bonus to AC and you can deal piercing damage with your unarmed strikes.");
            bp.m_Icon = snakeStyleIcon;
            bp.IsClassFeature = true;
            bp.Groups = new FeatureGroup[] {
                    FeatureGroup.CombatFeat,
                    FeatureGroup.Feat
                };
            bp.AddComponent<FeatureTagsComponent>(c =>
            {
                c.FeatureTags = FeatureTag.Attack | FeatureTag.Melee;
            });
            bp.AddPrerequisite<PrerequisiteStatValue>(c =>
            {
                c.Stat = StatType.SkillAthletics;
                c.Value = 1;
            });
            bp.AddPrerequisite<PrerequisiteStatValue>(c =>
            {
                c.Stat = StatType.SkillPerception;
                c.Value = 3;
            });
            var improvedUnarmedStrike = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("7812ad3672a4b9a4fb894ea402095167");
            bp.AddPrerequisiteFeature(improvedUnarmedStrike);
            bp.AddComponent<AddOutgoingPhysicalDamageProperty>(c =>
            {
                c.m_WeaponType = BlueprintTools.GetBlueprintReference<BlueprintWeaponTypeReference>("fcca8e6b85d19b14786ba1ab553e23ad");
                c.CheckWeaponType = true;
                c.AddForm = true;
                c.Form = Kingmaker.Enums.Damage.PhysicalDamageForm.Piercing;
            });
            bp.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = new BlueprintUnitFactReference[] { snakeStyleAbility.ToReference<BlueprintUnitFactReference>() };
            });
        });

        var snakeSidewind = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "SnakeSidewindBuff", bp =>
        {
            bp.SetName(MCEContext, "Snake Sidewind");
            bp.SetDescription(MCEContext, "");
            bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            bp.AddComponent<CriticalConfirmationUnarmed>(c =>
            {
                c.Bonus = 4;
            });
        });

        var snakeSidewindFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "SnakeSidewindFeature", bp =>
        {
            bp.SetName(MCEContext, "Snake Sidewind");
            bp.SetDescription(MCEContext, "You gain a +4 bonus to CMD against trip combat maneuvers and on Athletics checks and saving throws against ground effects. While using the Snake Style feat, you receive +4 bonus on attack roll made to confirm critical hits with unarmed weapons.");
            bp.IsClassFeature = true;
            bp.Groups = new FeatureGroup[] {
                    FeatureGroup.CombatFeat,
                    FeatureGroup.Feat
                };
            bp.AddComponent<FeatureTagsComponent>(c =>
            {
                c.FeatureTags = FeatureTag.Attack | FeatureTag.Melee;
            });
            bp.AddPrerequisiteFeature(snakeStyleFeature);
            bp.AddPrerequisite<PrerequisiteStatValue>(c =>
            {
                c.Stat = StatType.SkillAthletics;
                c.Value = 3;
            });
            bp.AddPrerequisite<PrerequisiteStatValue>(c =>
            {
                c.Stat = StatType.SkillPerception;
                c.Value = 6;
            });
            bp.AddComponent<CMDBonusAgainstManeuvers>(c =>
            {
                c.Maneuvers = new CombatManeuver[] { CombatManeuver.Trip };
                c.Value = 4;
                c.Descriptor = ModifierDescriptor.UntypedStackable;
            });
            bp.AddComponent<SavingThrowBonusAgainstDescriptor>(c =>
            {
                c.SpellDescriptor = SpellDescriptor.Ground;
                c.Value = 4;
                c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
            });
            bp.AddComponent<AddStatBonus>(c =>
            {
                c.Stat = StatType.SkillAthletics;
                c.Value = 4;
                c.Descriptor = ModifierDescriptor.UntypedStackable;
            });
        });

        var snakeFangFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "SnakeFangFeature", bp =>
        {
            bp.SetName(MCEContext, "Snake Fang");
            bp.SetDescription(MCEContext, "While using the Snake Style feat, when an opponent’s attack misses you, you can make an unarmed strike against that opponent as an attack of opportunity.");
            bp.IsClassFeature = true;
            bp.Groups = new FeatureGroup[] {
                    FeatureGroup.CombatFeat,
                    FeatureGroup.Feat
                };
            bp.AddComponent<FeatureTagsComponent>(c =>
            {
                c.FeatureTags = FeatureTag.Attack | FeatureTag.Melee;
            });
            bp.AddPrerequisiteFeature(snakeSidewindFeature);
            var combatReflexes = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("0f8939ae6f220984e8fb568abbdfba95");
            bp.AddPrerequisiteFeature(combatReflexes);
            bp.AddPrerequisite<PrerequisiteStatValue>(c =>
            {
                c.Stat = StatType.SkillAthletics;
                c.Value = 6;
            });
            bp.AddPrerequisite<PrerequisiteStatValue>(c =>
            {
                c.Stat = StatType.SkillPerception;
                c.Value = 9;
            });
        });


        snakeStyleBuff.AddConditionalBuff(snakeSidewindFeature, snakeSidewind);

        snakeStyleBuff.AddComponent<SnakeFangOnMissHandler>(c =>
        {
            c.m_Fact = snakeFangFeature.ToReference<BlueprintUnitFactReference>();
        });

        if (MCEContext.AddedContent.Feats.IsDisabled("SnakeStyleFeatChain")) { return; }

        FeatTools.AddAsFeat(snakeStyleFeature);
        FeatTools.AddAsFeat(snakeSidewindFeature);
        FeatTools.AddAsFeat(snakeFangFeature);
    }

}
