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
using MicroscopicContentExpansion.NewComponents;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Feats;
internal class FeintingFlurry
{
    internal static void Add()
    {
        var feintingFlurryIcon = AssetLoader.LoadInternal(MCEContext, folder: "", file: "FeintingFlurry.png");

        var feint = GetBP<BlueprintFeature>("c610310d31414edabcedf0c8a6fe32c4");
        if (feint == null) return;

        var feintAbility = GetBPRef<BlueprintAbilityReference>("1bb6f0b196aa457ba80bdb312dc64952");

        var monkFlurry = GetBP<BlueprintFeature>("fd99770e6bd240a4aab70f7af103e56a");
        var qmFlurry = GetBP<BlueprintFeature>("44b0f313ec56481eb447019fbe714330");
        var soheiFlurry = GetBP<BlueprintFeature>("cd4381b73b6709146bbcc0a528a6f471");
        var zaFlurry = GetBP<BlueprintFeature>("3e470edc8a733b641bcbbbb5b9527ff6");

        var dlc6Reward = GetBPRef<BlueprintDlcRewardReference>("b94f823171a84e30ad7a1b892433ab5d");

        var description = Helpers.CreateString(MCEContext, "FeintingFlurry.Description", "While using flurry of blows to make {g|Encyclopedia:MeleeAttack}melee attacks{/g}, you can forgo your melee attack to make a {g|Encyclopedia:Persuasion}Persuasion{/g} (bluff) {g|Encyclopedia:Check}check{/g} to feint an opponent.\r\nSpecial: At 7th level a zen archer can feint while using Flurry of Blows with a bow.", Locale.enGB, shouldProcess: true);

        var buff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "FeintingFlurryBuff", a =>
        {
            a.SetName(MCEContext, "Feinting Flurry");
            a.m_Description = description;
            a.m_Icon = feintingFlurryIcon;
            a.AddComponent<ReduceAttacksCount>(c =>
            {
                c.ReduceCount = 1;
                c.OnlyFromPrimaryHand = true;
                c.Condition = new();
            });
            a.AddComponent<FeintingFlurryTrigger>(c =>
            {
                c.flurryReference = GetBPRef<BlueprintUnitFactReference>("332362f3bd39ebe46a740a36960fdcb4");
                c.monkClass = GetBPRef<BlueprintCharacterClassReference>("e8f21e5b58e0569468e420ebea456124");
                c.feint = GetBPRef<BlueprintUnitFactReference>("c610310d31414edabcedf0c8a6fe32c4");
                c.rangedFeint = GetBPRef<BlueprintUnitFactReference>("a2e947d6be234abba7c3ac0bd5dc9b1d");
                c.zenArcherArchetype = GetBPRef<BlueprintArchetypeReference>("2b1a58a7917084f49b097e86271df21c");
                c.action = new()
                {
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

        var ability = Helpers.CreateBlueprint<BlueprintActivatableAbility>(MCEContext, "FeintingFlurryActivatableAbility", a =>
        {
            a.SetName(MCEContext, "Feinting Flurry");
            a.m_Description = description;
            a.m_Icon = feintingFlurryIcon;
            a.DeactivateImmediately = true;
            a.ActivationType = AbilityActivationType.WithUnitCommand;
            a.m_ActivateWithUnitCommand = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
            a.m_ActivateOnUnitAction = AbilityActivateOnUnitActionType.Attack;
            a.m_Buff = buff.ToReference<BlueprintBuffReference>();
        });

        var feintingFlurry = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "FeintingFlurry", a =>
        {
            a.SetName(MCEContext, "Feinting Flurry");
            a.m_Description = description;
            a.m_Icon = feintingFlurryIcon;
            a.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = [ability.ToReference<BlueprintUnitFactReference>()];
            });
            a.AddPrerequisiteFeature(feint);
            a.AddPrerequisite<PrerequisiteStatValue>(c =>
            {
                c.Stat = StatType.Dexterity;
                c.Value = 15;
            });
            a.AddPrerequisite<PrerequisiteStatValue>(c =>
            {
                c.Stat = StatType.Intelligence;
                c.Value = 13;
            });
            a.AddPrerequisiteFeaturesFromList(1, monkFlurry, qmFlurry, soheiFlurry, zaFlurry);
            a.AddComponent<DlcCondition>(c =>
            {
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
