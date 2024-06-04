using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.RebalancedContent.DragonbloodShifterArchetype;
internal class Dragonblooded
{
    internal static void Change()
    {
        EverlastingAspect();
        AllowMultipleAspectsAtOnce();
        FormBuffs();
    }

    /// <summary>
    /// Adding Airborne property to dragonblooded dragon forms
    /// </summary>
    private static void FormBuffs()
    {
        List<string> dragonFormBuffs9 = [
            "662bdcd3eef541fb91d88b9ee79d0d37", // ShifterDragonFormBlackBuff9
            "abaa7d56f843410e97c61ff2c87d39c6", // ShifterDragonFormBlueBuff9
            "205e7bae0d7c428b8f2a451f7934219a", // ShifterDragonFormBrassBuff9
            "cd72e19154f143269e48caff753eab63", // ShifterDragonFormBronzeBuff9
            "3f5625345d0c481abec69c0241d50019", // ShifterDragonFormCopperBuff9
            "f5ac253cbee44744a7399f17765160d5", // ShifterDragonFormGoldBuff9
            "ab25d91564a04b3fa0ae84d52b6407d5", // ShifterDragonFormGreenBuff9
            "a1f0de0190ce40e19d97c6967a9693c3", // ShifterDragonFormRedBuff9
            "3c4bf82676d345dca2718cac680f5906", // ShifterDragonFormSilverBuff9
            "8b82ee0ca203452a952a25c0f867b2fe", // ShifterDragonFormWhiteBuff9
        ];

        List<string> dragonFormBuffs14 = [
            "8fb6bf56c9174d5e8cf24069e6b0c965", // ShifterDragonFormBlackBuff14
            "b9c75c14fe6d48e0962e1ce9f42d4c9e", // ShifterDragonFormBlueBuff14
            "445d70781c2848dc9c63d80718a6c26f", // ShifterDragonFormBrassBuff14
            "0ff1819f465140068e02aaf87c17ec2c", // ShifterDragonFormBronzeBuff14
            "e9736d47de3643009a5514668a48ffe0", // ShifterDragonFormCopperBuff14
            "5a679cd137d64c629995c626616dbb17", // ShifterDragonFormGoldBuff14
            "3d887a79a7384149bd38b4d9d97c44b5", // ShifterDragonFormGreenBuff14
            "8242311f5c3c4cad90e67ef79cf5a6c2", // ShifterDragonFormRedBuff14
            "2de04456ce2d4e79804f899498ab31cc", // ShifterDragonFormSilverBuff14
            "b9b1fbf0ec224ccfac3dc5451d00a26a", // ShifterDragonFormWhiteBuff14
        ];

        List<string> dragonFormBuffs20 = [
            "2288af142a164f8799c4af47a1d59964", // ShifterDragonFormBlackBuff20
            "3a046d0a7bec4740b55df562950ef8ef", // ShifterDragonFormBlueBuff20
            "6ccdc7596ec744a3aaf5c1ea87079277", // ShifterDragonFormBrassBuff20
            "3aa815d2677a4ed791abe20cb7e7a2e6", // ShifterDragonFormBronzeBuff20
            "42887e88445b48a6bcc90f293c8b6967", // ShifterDragonFormCopperBuff20
            "833873205d9b46e99217d02cd04a20d4", // ShifterDragonFormGoldBuff20
            "988155b6f04a49dabc1b6cb674b71f04", // ShifterDragonFormGreenBuff20
            "2ef361195bc048839608bac3950b1f23", // ShifterDragonFormRedBuff20
            "3be4d85d65a94960b4242522d0965633", // ShifterDragonFormSilverBuff20
            "9d6996a50f6a4de289a44293420f75be", // ShifterDragonFormWhiteBuff20
        ];

        if (MCEContext.Homebrew.DragonbloodShifter.IsEnabled("AddAirborneToForms"))
        {
            var airborne = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("70cffb448c132fa409e49156d013b175");
            foreach (var buffId in dragonFormBuffs9.Concat(dragonFormBuffs14).Concat(dragonFormBuffs20))
            {
                var buff = BlueprintTools.GetBlueprint<BlueprintBuff>(buffId);
                buff.AddComponent<AddFacts>(x =>
                {
                    x.m_Facts = [airborne];
                });
            }
        }

        if (MCEContext.Homebrew.DragonbloodShifter.IsEnabled("IncreaseFormStatBonuses"))
        {
            var airborne = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("70cffb448c132fa409e49156d013b175");
            foreach (var buffId in dragonFormBuffs14)
            {
                var buff = BlueprintTools.GetBlueprint<BlueprintBuff>(buffId);
                var polymorphComponent = buff.GetComponent<Polymorph>();
                polymorphComponent.StrengthBonus += 2;
                polymorphComponent.ConstitutionBonus += 2;
            }
            foreach (var buffId in dragonFormBuffs20)
            {
                var buff = BlueprintTools.GetBlueprint<BlueprintBuff>(buffId);
                var polymorphComponent = buff.GetComponent<Polymorph>();
                polymorphComponent.StrengthBonus += 4;
                polymorphComponent.ConstitutionBonus += 4;
            }
        }
    }

    /// <summary>
    /// Make aspects last forever
    /// </summary>
    private static void EverlastingAspect()
    {

        if (MCEContext.Homebrew.DragonbloodShifter.IsDisabled("EverlastingAspect")) { return; }

        var dragonbloodShifterBrassAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("2d5f55dd02354faf8c9cfb8e968ada28");
        var dragonbloodShifterBronzeAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("f85adf95d187448fa6cba324a487e94b");
        var dragonbloodShifterCopperAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("5d26af2e3ca9492aa4fd224ec0083e3b");
        var dragonbloodShifterGoldAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("3cc1f78b09804dce9f4e575060161117");
        var dragonbloodShifterSilverAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("34444997005e40adadb144b0cfcfe086");
        var dragonbloodShifterBlackAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("950d6040a7f04b17b60000827fae66e1");
        var dragonbloodShifterBlueAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("1bc23b75b28749f1a17e675604392a78");
        var dragonbloodShifterGreenAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("a0e28069d62b43ea827a529aa42b781f");
        var dragonbloodShifterRedAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("9b6437dff6f945ce831c734fc5717775");
        var dragonbloodShifterWhiteAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("6e98c227c6d84ca89f3183c4d83dac8e");
        List<BlueprintActivatableAbility> allAbilities =
        [
            dragonbloodShifterBrassAbility,
            dragonbloodShifterBronzeAbility,
            dragonbloodShifterCopperAbility,
            dragonbloodShifterGoldAbility,
            dragonbloodShifterSilverAbility,
            dragonbloodShifterBlackAbility,
            dragonbloodShifterBlueAbility,
            dragonbloodShifterGreenAbility,
            dragonbloodShifterRedAbility,
            dragonbloodShifterWhiteAbility
        ];
        foreach (var ability in allAbilities)
        {
            ability.RemoveComponents<ActivatableAbilityResourceLogic>();
        }
        var dragonAspectSelection = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("c2872505b99c43b8b146ed89ffeb9af5");
        var extendedAspects = BlueprintTools.GetBlueprint<BlueprintFeature>("88e0ca9a742b436ea48ce4845d178c8a");
        extendedAspects.RemoveComponents<RecommendationHasFeature>(x =>
        {
            return dragonAspectSelection.Equals(x.m_Feature);
        });

        var prereqCondition = extendedAspects.GetComponent<PrerequisiteCondition>();
        if (prereqCondition?.Condition is OrAndLogic orAndLogic)
        {
            var conds = orAndLogic.ConditionsChecker.Conditions;
            orAndLogic.ConditionsChecker.Conditions = conds.Where(x =>
            {
                if (x is ContextConditionHasFact hasFact)
                {
                    if (dragonAspectSelection.Equals(hasFact.m_Fact))
                    {
                        return false;
                    }
                }
                return true;
            }).ToArray();
        }
    }

    /// <summary>
    /// Multiple draconic aspects as you level
    /// </summary>
    private static void AllowMultipleAspectsAtOnce()
    {

        if (MCEContext.Homebrew.DragonbloodShifter.IsDisabled("MultipleAspectsAtOnce")) { return; }

        var shifterDragonFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d8e9d249a426400bb47fefa6d0158049");
        var shifterDragonFormFeatureImproved = BlueprintTools.GetBlueprint<BlueprintFeature>("5cae07b7e9474d3eb382baa703e82ca8");
        var shifterDragonFormFeatureFinal = BlueprintTools.GetBlueprint<BlueprintFeature>("c676ba5eb744492583d989244c81f127");
        shifterDragonFormFeature.AddComponent<IncreaseActivatableAbilityGroupSize>(c =>
        {
            c.Group = ActivatableAbilityGroup.ShifterAspect;
        });
        shifterDragonFormFeatureImproved.AddComponent<IncreaseActivatableAbilityGroupSize>(c =>
        {
            c.Group = ActivatableAbilityGroup.ShifterAspect;
        });
        shifterDragonFormFeatureFinal.AddComponent<IncreaseActivatableAbilityGroupSize>(c =>
        {
            c.Group = ActivatableAbilityGroup.ShifterAspect;
        });
    }
}
