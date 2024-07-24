using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.AnimalCompanions;
public class NightmareAnimalCompanion
{

    public static BlueprintFeature Add()
    {

        var horseUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("fb8300e8298c08d4a9f50dfa1203e98d");
        var baseNightmareUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("c8072d74e64caf244ac6b784e6838e12");

        var hoofFireDamage = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "NightmareHoofFireDamageFeature", bp =>
        {
            bp.SetName(MCEContext, "Nightmare - Hoof Fire damage");
            bp.SetDescription(MCEContext, "Nightmare hoofs deal additional 1d4 fire damage");
            bp.AddComponent<AdditionalDiceOnAttack>(c =>
            {
                c.AttackType = AdditionalDiceOnAttack.WeaponOptions.AllAttacks;
                c.OnHit = true;
                c.m_WeaponType = BlueprintTools.GetBlueprintReference<BlueprintWeaponTypeReference>("ad298a0ee3ca1ba419d0c973d1a905f2");
                c.Value = new ContextDiceValue()
                {
                    DiceType = Kingmaker.RuleSystem.DiceType.D4,
                    DiceCountValue = 1,
                    BonusValue = 0
                };
                c.InitiatorConditions = new();
                c.TargetConditions = new();
                c.DamageType = new DamageTypeDescription()
                {
                    Type = DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Fire
                };
            });
        });

        var removeAnimalTypeFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "NightmareRemoveAnimalType", bp =>
        {
            bp.HideInUI = true;
            bp.HideInCharacterSheetAndLevelUp = true;
            bp.AddComponent<RemoveFeatureOnApply>(c =>
            {
                c.m_Feature = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("a95311b3dc996964cbaa30ff9965aaf6");
            });
        });

        var removalOnClassLvlvFeature = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "NightmareRemoveAnimalTypeOnClassLvlFeature", bp =>
        {
            bp.HideInUI = true;
            bp.HideInCharacterSheetAndLevelUp = true;
            bp.AddComponent<AddFeatureOnClassLevel>(c =>
            {
                c.m_Class = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("26b10d4340839004f960f9816f6109fe");
                c.Level = 2;
                c.m_Feature = removeAnimalTypeFeature.ToReference<BlueprintFeatureReference>();
            });
        });

        var nightmareACUnit = Helpers.CreateBlueprint<BlueprintUnit>(MCEContext, "AnimalCompanionUnitNightmare", bp =>
        {
            bp.AddComponents(horseUnit.GetComponents<AddClassLevels>());
            bp.AddComponent<AllowDyingCondition>();
            bp.AddComponent<AddResurrectOnRest>();
            bp.AddComponent<LockEquipmentSlot>(c =>
            {
                c.m_SlotType = LockEquipmentSlot.SlotType.MainHand;
            });
            bp.AddComponent<LockEquipmentSlot>(c =>
            {
                c.m_SlotType = LockEquipmentSlot.SlotType.OffHand;
            });
            bp.AddComponent(horseUnit.GetComponent<CMDBonusAgainstManeuvers>());
            bp.AddComponent<AddFacts>(c =>
            {
                c.m_Facts = [BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("9c57e9674b4a4a2b9920f9fec47f7e6a")];
            });
            bp.m_Type = baseNightmareUnit.m_Type;
            bp.LocalizedName = baseNightmareUnit.LocalizedName;
            bp.Size = Kingmaker.Enums.Size.Large;
            bp.Color = baseNightmareUnit.Color;
            bp.Alignment = baseNightmareUnit.Alignment;
            bp.m_Portrait = horseUnit.m_Portrait;
            bp.Prefab = baseNightmareUnit.Prefab;
            bp.Visual = baseNightmareUnit.Visual;
            bp.m_Faction = horseUnit.m_Faction;
            bp.FactionOverrides = horseUnit.FactionOverrides;
            bp.m_Brain = horseUnit.m_Brain;
            bp.Body = new BlueprintUnit.UnitBody()
            {
                m_EmptyHandWeapon = horseUnit.Body.m_EmptyHandWeapon,
                m_PrimaryHand = horseUnit.Body.m_PrimaryHand,
                m_PrimaryHandAlternative1 = horseUnit.Body.m_PrimaryHandAlternative1,
                m_PrimaryHandAlternative2 = horseUnit.Body.m_PrimaryHandAlternative2,
                m_PrimaryHandAlternative3 = horseUnit.Body.m_PrimaryHandAlternative3,
                m_AdditionalSecondaryLimbs = baseNightmareUnit.Body.m_AdditionalSecondaryLimbs
            };
            bp.Strength = baseNightmareUnit.Strength;
            bp.Dexterity = baseNightmareUnit.Dexterity;
            bp.Constitution = baseNightmareUnit.Constitution;
            bp.Intelligence = baseNightmareUnit.Intelligence;
            bp.Wisdom = baseNightmareUnit.Wisdom;
            bp.Charisma = baseNightmareUnit.Charisma;
            bp.Speed = baseNightmareUnit.Speed;
            bp.Skills = horseUnit.Skills;
            bp.MaxHP = horseUnit.MaxHP;
            bp.m_AddFacts = [
                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("13c87ac5985cc85498ef9d1ac8b78923"),
                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("c33f2d68d93ceee488aa4004347dffca"),
                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("b9342e2a6dc5165489ba3412c50ca3d1"),
                hoofFireDamage.ToReference<BlueprintUnitFactReference>(),
                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("5279fc8380dd9ba419b4471018ffadd1"),
                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("136fa0343d5b4b348bdaa05d83408db3"),
                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("e2986f96fa1cd3b4f8d9dfd8a9907731"),
                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("75bb2b3c41c99e041b4743fdb16a4289"),
                removalOnClassLvlvFeature.ToReference<BlueprintUnitFactReference>()
            ];
        });

        var rankFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("1670990255e4fe948a863bafd5dbda5d");

        var animalCompanionFeatureHorse = BlueprintTools.GetBlueprint<BlueprintFeature>("9dc58b5901677c942854019d1dd98374");

        var animalCompanionFeatureNightmare = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AnimalCompanionFeatureNightmare", bp =>
        {
            bp.AddComponent<AddPet>(c =>
            {
                c.Type = Kingmaker.Enums.PetType.AnimalCompanion;
                c.ProgressionType = Kingmaker.Enums.PetProgressionType.AnimalCompanion;
                c.m_Pet = nightmareACUnit.ToReference<BlueprintUnitReference>();
                c.m_LevelRank = rankFeature;
                c.UpgradeLevel = 99;
                c.m_LevelContextValue = 0;
            });
            bp.AddPrerequisite<PrerequisitePet>(c =>
            {
                c.Type = Kingmaker.Enums.PetType.AnimalCompanion;
                c.NoCompanion = true;
                c.Group = Prerequisite.GroupType.Any;
            });
            bp.SetName(MCEContext, "Fiendish Companion - Nightmare");

            var description = """
            {g|Encyclopedia:Size}Size{/g}: Large
            {g|Encyclopedia:Speed}Speed{/g}: 50 ft.
            {g|Encyclopedia:Armor_Class}AC{/g}: +8 natural armor
            {g|Encyclopedia:Attack}Attack{/g}: bite ({g|Encyclopedia:Dice}1d4{/g}), 2 hooves (1d6)
            {g|Encyclopedia:Ability_Scores}Ability scores{/g}: {g|Encyclopedia:Strength}Str{/g} 18, {g|Encyclopedia:Dexterity}Dex{/g} 15, {g|Encyclopedia:Constitution}Con{/g} 16, {g|Encyclopedia:Intelligence}Int{/g} 13, {g|Encyclopedia:Wisdom}Wis{/g} 13, {g|Encyclopedia:Charisma}Cha{/g} 12
            Special qualities: Hoof attacks deal additional 1d4 fire damage.
            Nightmares are not animals, but evil outsiders from {g|Abaddon}Abaddon{/g} and thus are not affected by abilities targeting specifically animals.
            """;
            bp.SetDescription(MCEContext, description);
            bp.Groups = [FeatureGroup.AnimalCompanion];
            bp.ReapplyOnLevelUp = true;
            bp.m_Icon = animalCompanionFeatureHorse.m_Icon;
        });

        return animalCompanionFeatureNightmare;
    }
}
