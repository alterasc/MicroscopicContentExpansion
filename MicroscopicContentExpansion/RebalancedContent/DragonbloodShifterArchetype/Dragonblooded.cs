using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
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
        AddBite();
        AddWings();
        ApplyGoldDragonBonuses();
        FixMythicGDSpellMetamagic();
    }

    /// <summary>
    /// Fixes some missing metamagic from GD mythic spells
    /// </summary>
    private static void FixMythicGDSpellMetamagic()
    {
        if (MCEContext.Homebrew.DragonbloodShifter.IsDisabled("FixGDSpellMetamagicTags"))
        {
            return;
        }

        // lvl 8
        var dragonMight = GetBP<BlueprintAbility>("bfc6aa5be6bc41f68ca78aef37913e9f");
        dragonMight.AvailableMetamagic |= Metamagic.Extend | Metamagic.Heighten | Metamagic.CompletelyNormal;

        var summonDragonI = GetBP<BlueprintAbility>("1e42ecaa79454e85b274edc73e130a03");
        summonDragonI.AvailableMetamagic |= Metamagic.Extend | Metamagic.CompletelyNormal | Metamagic.Quicken | Metamagic.Reach | Metamagic.Heighten | Metamagic.Empower | Metamagic.Maximize;

        var dragonSmite = GetBP<BlueprintAbility>("a508fd48695440cd8216526a859ecb53");
        dragonSmite.AvailableMetamagic |= Metamagic.Heighten | Metamagic.CompletelyNormal | Metamagic.Quicken;

        // lvl 9
        var dragonWrath = GetBP<BlueprintAbility>("59d08b909d684b91a137766ab22f4b1a");
        dragonWrath.AvailableMetamagic |= Metamagic.Heighten | Metamagic.CompletelyNormal | Metamagic.Quicken | Metamagic.Empower | Metamagic.Maximize;

        var dragonPride = GetBP<BlueprintAbility>("f7bc6e97e7d44ed8ba5c4d9f76a5a3d3");
        dragonPride.AvailableMetamagic |= Metamagic.Heighten | Metamagic.CompletelyNormal | Metamagic.Quicken;

        var summonDragonII = GetBP<BlueprintAbility>("51b498f1cacd42e08ed6852f53261f11");
        summonDragonII.AvailableMetamagic |= Metamagic.Extend | Metamagic.CompletelyNormal | Metamagic.Quicken | Metamagic.Reach | Metamagic.Heighten;

        var thousandBites = GetBP<BlueprintAbility>("d35b16edbd5c436286e34cf7bcbdb645");
        thousandBites.AvailableMetamagic |= Metamagic.Extend | Metamagic.Heighten | Metamagic.CompletelyNormal;

        // lvl 10
        var summonDragonIII = GetBP<BlueprintAbility>("cb127670411c41298a4aa4d0a165a20b");
        summonDragonIII.AvailableMetamagic |= Metamagic.Extend | Metamagic.CompletelyNormal | Metamagic.Quicken | Metamagic.Reach;

        var dragonUltimateApsu = GetBP<BlueprintAbility>("cff9e3bf5ccf40c489023bf368c2c802");
        dragonUltimateApsu.AvailableMetamagic |= Metamagic.CompletelyNormal | Metamagic.Quicken | Metamagic.Empower | Metamagic.Maximize | Metamagic.Persistent;

        var dragonUltimateDahak = GetBP<BlueprintAbility>("5b1984f4af00412eb0c0efb0ebb90189");
        dragonUltimateDahak.AvailableMetamagic |= Metamagic.CompletelyNormal | Metamagic.Quicken | Metamagic.Empower | Metamagic.Maximize | Metamagic.Persistent;
    }

    /// <summary>
    /// Apply GD bonuses to Dragonblood20 forms:
    /// Make Thousand Bites spell affect Gold and Black dragon forms of lvl 20 shifter.
    /// </summary>
    private static void ApplyGoldDragonBonuses()
    {
        if (MCEContext.Homebrew.DragonbloodShifter.IsDisabled("ApplyGDBonuses"))
        {
            return;
        }
        var thousandBitesBuff = GetBP<BlueprintBuff>("61bfcdf05852443c8f4577c34bf2b6ef");
        thousandBitesBuff.AddComponent<BuffExtraEffects>(c =>
        {
            c.m_CheckedBuff = GetBPRef<BlueprintBuffReference>("833873205d9b46e99217d02cd04a20d4"); //ShifterDragonFormGoldBuff20
            c.m_ExtraEffectBuff = GetBPRef<BlueprintBuffReference>("11a5d86ee17e4594a7ffb8cc4a6f05cd"); //ThousandBitesBuffEffect
        });
        thousandBitesBuff.AddComponent<BuffExtraEffects>(c =>
        {
            c.m_CheckedBuff = GetBPRef<BlueprintBuffReference>("2288af142a164f8799c4af47a1d59964"); //ShifterDragonFormBlackBuff20
            c.m_ExtraEffectBuff = GetBPRef<BlueprintBuffReference>("11a5d86ee17e4594a7ffb8cc4a6f05cd"); //ThousandBitesBuffEffect
        });
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
            var airborne = GetBPRef<BlueprintUnitFactReference>("70cffb448c132fa409e49156d013b175");
            foreach (var buffId in dragonFormBuffs9.Concat(dragonFormBuffs14).Concat(dragonFormBuffs20))
            {
                var buff = GetBP<BlueprintBuff>(buffId);
                buff.AddComponent<AddFacts>(x =>
                {
                    x.m_Facts = [airborne];
                });
            }
        }

        if (MCEContext.Homebrew.DragonbloodShifter.IsEnabled("IncreaseFormStatBonuses"))
        {
            var airborne = GetBPRef<BlueprintUnitFactReference>("70cffb448c132fa409e49156d013b175");
            foreach (var buffId in dragonFormBuffs14)
            {
                var buff = GetBP<BlueprintBuff>(buffId);
                var polymorphComponent = buff.GetComponent<Polymorph>();
                polymorphComponent.StrengthBonus += 2;
                polymorphComponent.ConstitutionBonus += 2;
            }
            foreach (var buffId in dragonFormBuffs20)
            {
                var buff = GetBP<BlueprintBuff>(buffId);
                var polymorphComponent = buff.GetComponent<Polymorph>();
                polymorphComponent.StrengthBonus += 4;
                polymorphComponent.ConstitutionBonus += 4;
            }
        }

        var dragonBiteType = GetBP<BlueprintWeaponType>("12a8a3a89e62d6b4fbc09ecdc187a828");
        var dragonBiteTypeRef = dragonBiteType.ToReference<BlueprintWeaponTypeReference>();
        var biteIcon = GetBP<BlueprintItemWeapon>("f3ff6972c32f22e4ba4c85c3982a03cf").m_Icon;

        var bite9 = GetBP<BlueprintItemWeapon>("61bc14eca5f8c1040900215000cfc218");

        var dragonBite1d8 = Helpers.CreateBlueprint<BlueprintItemWeapon>(MCEContext, "BiteDragon1d8", a =>
        {
            a.m_DisplayNameText = dragonBiteType.m_DefaultNameText;
            a.m_DescriptionText = bite9.m_DescriptionText;
            a.m_FlavorText = bite9.m_FlavorText;
            a.m_Enchantments = bite9.m_Enchantments;
            a.m_Icon = biteIcon;
            a.m_Size = Kingmaker.Enums.Size.Medium;
            a.m_Type = dragonBiteTypeRef;
            a.m_OverrideDamageDice = true;
            a.m_DamageDice = new DiceFormula(1, DiceType.D8);
            a.m_VisualParameters = bite9.m_VisualParameters;
            a.m_EquipmentEntity = bite9.m_EquipmentEntity;
        }).ToReference<BlueprintItemWeaponReference>();

        var dragonBite2d6 = Helpers.CreateBlueprint<BlueprintItemWeapon>(MCEContext, "BiteDragonLarge2d6", a =>
        {
            a.m_DisplayNameText = dragonBiteType.m_DefaultNameText;
            a.m_DescriptionText = bite9.m_DescriptionText;
            a.m_FlavorText = bite9.m_FlavorText;
            a.m_Enchantments = bite9.m_Enchantments;
            a.m_Icon = biteIcon;
            a.m_Size = Kingmaker.Enums.Size.Large;
            a.m_Type = dragonBiteTypeRef;
            a.m_OverrideDamageDice = true;
            a.m_DamageDice = new DiceFormula(2, DiceType.D6);
            a.m_VisualParameters = bite9.m_VisualParameters;
            a.m_EquipmentEntity = bite9.m_EquipmentEntity;
        }).ToReference<BlueprintItemWeaponReference>();

        var dragonBite2d8 = Helpers.CreateBlueprint<BlueprintItemWeapon>(MCEContext, "BiteDragonHuge2d8", a =>
        {
            a.m_DisplayNameText = dragonBiteType.m_DefaultNameText;
            a.m_DescriptionText = bite9.m_DescriptionText;
            a.m_FlavorText = bite9.m_FlavorText;
            a.m_Enchantments = bite9.m_Enchantments;
            a.m_Icon = biteIcon;
            a.m_Size = Kingmaker.Enums.Size.Huge;
            a.m_Type = dragonBiteTypeRef;
            a.m_OverrideDamageDice = true;
            a.m_DamageDice = new DiceFormula(2, DiceType.D8);
            a.m_VisualParameters = bite9.m_VisualParameters;
            a.m_EquipmentEntity = bite9.m_EquipmentEntity;
        }).ToReference<BlueprintItemWeaponReference>();

        if (MCEContext.Homebrew.DragonbloodShifter.IsEnabled("IncreaseFormBiteStrBonus"))
        {
            foreach (var buffId in dragonFormBuffs9)
            {
                var buff = GetBP<BlueprintBuff>(buffId);
                var polymorphComponent = buff.GetComponent<Polymorph>();
                if (polymorphComponent.m_AdditionalLimbs[0].Get().Category == Kingmaker.Enums.WeaponCategory.Bite)
                {
                    polymorphComponent.m_AdditionalLimbs[0] = dragonBite1d8;
                }
            }
            foreach (var buffId in dragonFormBuffs14)
            {
                var buff = GetBP<BlueprintBuff>(buffId);
                var polymorphComponent = buff.GetComponent<Polymorph>();
                if (polymorphComponent.m_AdditionalLimbs[0].Get().Category == Kingmaker.Enums.WeaponCategory.Bite)
                {
                    polymorphComponent.m_AdditionalLimbs[0] = dragonBite2d6;
                }
            }
            foreach (var buffId in dragonFormBuffs20)
            {
                var buff = GetBP<BlueprintBuff>(buffId);
                var polymorphComponent = buff.GetComponent<Polymorph>();
                if (polymorphComponent.m_AdditionalLimbs[0].Get().Category == Kingmaker.Enums.WeaponCategory.Bite)
                {
                    polymorphComponent.m_AdditionalLimbs[0] = dragonBite2d8;
                }
            }
        }
    }

    /// <summary>
    /// Make aspects last forever
    /// </summary>
    private static void EverlastingAspect()
    {

        if (MCEContext.Homebrew.DragonbloodShifter.IsDisabled("EverlastingAspect")) { return; }

        var dragonbloodShifterBrassAbility = GetBP<BlueprintActivatableAbility>("2d5f55dd02354faf8c9cfb8e968ada28");
        var dragonbloodShifterBronzeAbility = GetBP<BlueprintActivatableAbility>("f85adf95d187448fa6cba324a487e94b");
        var dragonbloodShifterCopperAbility = GetBP<BlueprintActivatableAbility>("5d26af2e3ca9492aa4fd224ec0083e3b");
        var dragonbloodShifterGoldAbility = GetBP<BlueprintActivatableAbility>("3cc1f78b09804dce9f4e575060161117");
        var dragonbloodShifterSilverAbility = GetBP<BlueprintActivatableAbility>("34444997005e40adadb144b0cfcfe086");
        var dragonbloodShifterBlackAbility = GetBP<BlueprintActivatableAbility>("950d6040a7f04b17b60000827fae66e1");
        var dragonbloodShifterBlueAbility = GetBP<BlueprintActivatableAbility>("1bc23b75b28749f1a17e675604392a78");
        var dragonbloodShifterGreenAbility = GetBP<BlueprintActivatableAbility>("a0e28069d62b43ea827a529aa42b781f");
        var dragonbloodShifterRedAbility = GetBP<BlueprintActivatableAbility>("9b6437dff6f945ce831c734fc5717775");
        var dragonbloodShifterWhiteAbility = GetBP<BlueprintActivatableAbility>("6e98c227c6d84ca89f3183c4d83dac8e");
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
        var dragonAspectSelection = GetBPRef<BlueprintUnitFactReference>("c2872505b99c43b8b146ed89ffeb9af5");
        var extendedAspects = GetBP<BlueprintFeature>("88e0ca9a742b436ea48ce4845d178c8a");
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

        var shifterDragonFormFeature = GetBP<BlueprintFeature>("d8e9d249a426400bb47fefa6d0158049");
        var shifterDragonFormFeatureImproved = GetBP<BlueprintFeature>("5cae07b7e9474d3eb382baa703e82ca8");
        var shifterDragonFormFeatureFinal = GetBP<BlueprintFeature>("c676ba5eb744492583d989244c81f127");
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

    /// <summary>
    /// Add bite at lvl 4
    /// </summary>
    private static void AddBite()
    {
        var icon = GetBP<BlueprintFeature>("7e7e4cd3d93984b439799048e6657237").m_Icon;

        var ddBite = GetBP<BlueprintItemWeapon>("c66afbc07845e4245bf62021b7278a43");

        var shifterBite = Helpers.CreateBlueprint<BlueprintItemWeapon>(MCEContext, "DragonbloodShifterBiteWeapon", a =>
        {
            a.m_DisplayNameText = ddBite.m_DisplayNameText;
            a.m_DescriptionText = ddBite.m_DescriptionText;
            a.m_FlavorText = ddBite.m_FlavorText;
            a.m_Icon = ddBite.m_Icon;
            a.m_Size = Kingmaker.Enums.Size.Medium;
            a.m_Type = ddBite.m_Type;
            a.m_EquipmentEntity = ddBite.m_EquipmentEntity;
            a.m_VisualParameters = ddBite.m_VisualParameters;
            a.m_Enchantments = [GetBPRef<BlueprintWeaponEnchantmentReference>("ae2be9fefbd5438f821f0113db8fd572")];
        });

        var bite = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DragonbloodShifterBite", a =>
        {
            a.SetName(MCEContext, "Dragon Bite");
            a.SetDescription(MCEContext, "At 4th level dragonblood shifter gains a bite {g|Encyclopedia:Attack}attack{/g}. This is a primary {g|Encyclopedia:NaturalAttack}natural attack{/g} that deals {g|Encyclopedia:Dice}1d6{/g} points of {g|Encyclopedia:Damage}damage{/g} (1d4 if the dragonblood shifter is Small), plus 1.5 times the dragonblood shifter's {g|Encyclopedia:Strength}Strength{/g} modifier. It counts as cold iron, silver, and magic for the purpose of overcoming {g|Encyclopedia:Damage_Reduction}damage reduction{/g}.");
            a.m_Icon = icon;
            a.AddComponent<AddAdditionalLimb>(c =>
            {
                c.m_Weapon = shifterBite.ToReference<BlueprintItemWeaponReference>();
            });
            a.AddComponent<AddOutgoingPhysicalDamageProperty>(c =>
            {
                c.AddMagic = true;
                c.AddMaterial = true;
                c.Material = Kingmaker.Enums.Damage.PhysicalDamageMaterial.ColdIron & Kingmaker.Enums.Damage.PhysicalDamageMaterial.Silver;
                c.CheckWeaponType = true;
                c.m_WeaponType = ddBite.m_Type;
            });
        });

        if (MCEContext.Homebrew.DragonbloodShifter.IsDisabled("AddBiteAttack")) { return; }

        var archetype = GetBP<BlueprintArchetype>("2d5b06e413a9408cbd5bb999b5a4cc4a");
        var lvl4Entry = archetype.AddFeatures.FirstOrDefault(x => x.Level == 4);
        if (lvl4Entry != null)
        {
            lvl4Entry.m_Features.Add(bite.ToReference<BlueprintFeatureBaseReference>());
        }
        else
        {
            lvl4Entry = Helpers.CreateLevelEntry(4, bite);
            archetype.AddFeatures = archetype.AddFeatures.AppendToArray(lvl4Entry);
        }
    }

    /// <summary>
    /// Adds wings matching last activated aspect at lvl 5
    /// </summary>
    private static void AddWings()
    {
        var ddWingsAbility = GetBP<BlueprintActivatableAbility>("a800d71694dc7634b9481c1cbf5b355f");

        var buff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "DragonbloodShifterWingsBuff", bp =>
        {
            bp.m_Flags = BlueprintBuff.Flags.StayOnDeath | BlueprintBuff.Flags.HiddenInUi;
            bp.AddComponent<DragonbloodWingController>(c =>
            {
                c.MatchingWingsDictionary = new Dictionary<BlueprintGuid, BlueprintBuffReference> {
                    {BlueprintGuid.Parse("950d6040a7f04b17b60000827fae66e1"), GetBPRef<BlueprintBuffReference>("ddfe6e85e1eed7a40aa911280373c228") },
                    {BlueprintGuid.Parse("1bc23b75b28749f1a17e675604392a78"), GetBPRef<BlueprintBuffReference>("800cde038f9e6304d95365edc60ab0a4") },
                    {BlueprintGuid.Parse("a0e28069d62b43ea827a529aa42b781f"), GetBPRef<BlueprintBuffReference>("a4ccc396e60a00f44907e95bc8bf463f") },
                    {BlueprintGuid.Parse("9b6437dff6f945ce831c734fc5717775"), GetBPRef<BlueprintBuffReference>("08ae1c01155a2184db869e9ebedc758d") },
                    {BlueprintGuid.Parse("6e98c227c6d84ca89f3183c4d83dac8e"), GetBPRef<BlueprintBuffReference>("381a168acd79cd54baf87a17ca861d9b") },
                    {BlueprintGuid.Parse("2d5f55dd02354faf8c9cfb8e968ada28"), GetBPRef<BlueprintBuffReference>("7f5acae38fc1e0f4c9325d8a4f4f81fc") },
                    {BlueprintGuid.Parse("f85adf95d187448fa6cba324a487e94b"), GetBPRef<BlueprintBuffReference>("482ee5d001527204bb86e34240e2ce65") },
                    {BlueprintGuid.Parse("5d26af2e3ca9492aa4fd224ec0083e3b"), GetBPRef<BlueprintBuffReference>("a25d6fc69cba80548832afc6c4787379") },
                    {BlueprintGuid.Parse("3cc1f78b09804dce9f4e575060161117"), GetBPRef<BlueprintBuffReference>("984064a3dd0f25444ad143b8a33d7d92") },
                    {BlueprintGuid.Parse("34444997005e40adadb144b0cfcfe086"), GetBPRef<BlueprintBuffReference>("5a791c1b0bacee3459d7f5137fa0bd5f") },
                };
            });
        });

        var ability = Helpers.CreateBlueprint<BlueprintActivatableAbility>(MCEContext, "DragonbloodShifterWingsAbility", bp =>
        {
            bp.m_DisplayName = ddWingsAbility.m_DisplayName;
            bp.m_Description = ddWingsAbility.m_Description;
            bp.m_Buff = buff.ToReference<BlueprintBuffReference>();
            bp.AddComponent<RestrictionHasFact>(c =>
            {
                c.m_Feature = GetBPRef<BlueprintUnitFactReference>("e4979934bdb39d842b28bee614606823");
                c.Not = true;
            });
            bp.Group = ActivatableAbilityGroup.Wings;
            bp.IsOnByDefault = true;
            bp.DeactivateImmediately = true;
            bp.ActivationType = AbilityActivationType.Immediately;
            bp.m_ActivateWithUnitCommand = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
            bp.m_ActivateOnUnitAction = AbilityActivateOnUnitActionType.Attack;
            bp.ResourceAssetIds = ddWingsAbility.ResourceAssetIds;
            bp.m_Icon = ddWingsAbility.m_Icon;
        });

        var feature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DragonbloodShifterWingsFeature", bp =>
        {
            bp.m_DisplayName = ddWingsAbility.m_DisplayName;
            bp.m_Description = ddWingsAbility.m_Description;
            bp.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = [ability.ToReference<BlueprintUnitFactReference>()];
            });
            bp.m_Icon = ddWingsAbility.m_Icon;
        });

        if (MCEContext.Homebrew.DragonbloodShifter.IsDisabled("AddWings")) { return; }

        var archetype = GetBP<BlueprintArchetype>("2d5b06e413a9408cbd5bb999b5a4cc4a");
        var lvl4Entry = archetype.AddFeatures.FirstOrDefault(x => x.Level == 5);
        if (lvl4Entry != null)
        {
            lvl4Entry.m_Features.Add(feature.ToReference<BlueprintFeatureBaseReference>());
        }
        else
        {
            lvl4Entry = Helpers.CreateLevelEntry(5, feature);
            archetype.AddFeatures = archetype.AddFeatures.AppendToArray(lvl4Entry);
        }
    }
}
