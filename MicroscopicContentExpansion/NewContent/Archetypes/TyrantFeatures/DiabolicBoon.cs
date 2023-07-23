using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Archetypes.TyrantFeatures {
    internal class DiabolicBoon {
        const string NAME = "Diabolic Boon";

        const string DESCRIPTION = "Upon reaching 5th level, a tyrant receives a boon from his dark patrons. This boon can " +
            "take one of two forms. Once the form is chosen, it cannot be changed.\r\nThe first type of bond allows " +
            "the tyrant to enhance his weapon as a standard action by calling upon the aid of a fiendish spirit for 1 minute " +
            "per tyrant level. When called, the spirit causes the weapon to shed unholy light as a torch. At 5th level, this " +
            "spirit grants the weapon a +1 enhancement bonus. For every three levels beyond 5th, the weapon gains another +1 " +
            "enhancement bonus, to a maximum of +6 at 20th level.\r\nAdding these properties consumes an amount of bonus equal" +
            " to the property’s cost (sorted and listed below).\r\nThese bonuses can be added to the weapon, stacking with " +
            "existing weapon bonuses to a maximum of +5, or they can be used to add any of the following weapon properties:\r\n+1: " +
            "flaming, keen, vicious\r\n+2: axiomatic, flaming burst, unholy\r\n+3: speed\r\n+5: vorpal\r\nThese bonuses " +
            "are added to any properties the weapon already has, but duplicate abilities do not stack. If the weapon is not magical," +
            " at least a +1 enhancement bonus must be added before any other properties can be added. The bonus and properties " +
            "granted by the spirit are determined when the spirit is called and cannot be changed until the spirit is called again." +
            " The fiendish spirit imparts no bonuses if the weapon is held by anyone other than the tyrant but resumes giving " +
            "bonuses if returned to the tyrant. These bonuses apply to only one end of a double weapon. A tyrant can use this " +
            "ability once per day at 5th level, and one additional time per day for every four levels beyond 5th, to a total of" +
            " four times per day at 17th level.\r\nThe second type of bond allows an tyrant to gain the service of a fiendish " +
            "animal. This functions as druid's animal companion. Servant immediately gains fiendish template. At 15th level, " +
            "a tyrant’s servant gains spell resistance equal to the tyrant’s level + 11.";

        const string WEAPON_BOND_DESCRIPTION = "Upon reaching 5th level, a tyrant forms a divine bond with his weapon. " +
                            "As a standard action, he can call upon the aid of a fiendish " +
                            "spirit for 1 minute per tyrant level.\nAt 5th level, this spirit grants the weapon a +1 enhancement " +
                            "bonus. For every three levels beyond 5th, the weapon gains another +1 " +
                            "enhancement bonus, to a maximum of +6 at 20th level. These bonuses can be added to the weapon, stacking " +
                            "with existing weapon bonuses to a maximum of +5.\nAlternatively, they can be used to add any of the " +
                            "following weapon properties: " +
                            "flaming, keen, vicious, axiomatic, flaming burst, unholy, speed, and vorpal." +
                            " Adding these properties consumes an amount of bonus equal to the property's cost. These bonuses are added" +
                            " to any properties the weapon already has, but duplicate abilities do not stack.\nA tyrant can use this" +
                            " ability once per day at 5th level, and one additional time per day for every four levels beyond 5th, to" +
                            " a total of four times per day at 17th level.";

        public static BlueprintFeatureSelection AddDiabolicBoon() {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var AntipaladinServantSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(MCEContext, "AntipaladinServantSelection");

            var tyrantDiabolicBoonWeapon = AddDiabolicBondWeapon();

            return Helpers.CreateBlueprint<BlueprintFeatureSelection>(MCEContext, "TyrantDiabolicBoonSelection", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.m_DescriptionShort = bp.m_Description;
                bp.m_AllFeatures = new BlueprintFeatureReference[] {
                    AntipaladinServantSelection.ToReference<BlueprintFeatureReference>(),
                    tyrantDiabolicBoonWeapon.ToReference<BlueprintFeatureReference>()
                };
                bp.Mode = SelectionMode.Default;
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.IsClassFeature = true;
            });
        }

        private static BlueprintProgression AddDiabolicBondWeapon() {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");
            var icon = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("a68cd0fbf5d21ef4f8b9375ec0ac53b9").Icon;
            var weaponBondResource = BlueprintTools.GetModBlueprint<BlueprintAbilityResource>(MCEContext, "AntipaladinWeaponBondResource");
            var weaponBondAdditionalUse = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinFiendishBondAdditionalUse");
            var weaponBondDurationBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(MCEContext, "AntipaladinWeaponBondDurationBuff");
            var paladinWeaponBondSwitchAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("7ff088ab58c69854b82ea95c2b0e35b4");
            var weaponBondSwitchAbility = BlueprintTools.GetModBlueprint<BlueprintAbility>(MCEContext, "AntipaladinWeaponBondSwitchAbility");


            var weaponBond = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinWeaponBondFeature");


            var weaponBondAxiomaticBuff = CreateWeaponBondBuff("Axiomatic"
                , BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("0ca43051edefcad4b9b2240aa36dc8d4"));
            var weaponBondAxiomatic = CreateWeaponBondChoice("Axiomatic",
                BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("8ed07b0cc56223c46953348f849f3309").Icon,
                weaponBondAxiomaticBuff.ToReference<BlueprintBuffReference>(), 2);
            var weaponBondFlamingBurst = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(MCEContext, "AntipaladinWeaponBondFlamingBurstChoice");
            var weaponBondUnholy = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(MCEContext, "AntipaladinWeaponBondUnholyChoice");


            var weaponBond2 = CreateWeaponBondFeaturePlusX(2, icon,
                weaponBondAxiomatic.ToReference<BlueprintUnitFactReference>(),
                weaponBondFlamingBurst.ToReference<BlueprintUnitFactReference>(),
                weaponBondUnholy.ToReference<BlueprintUnitFactReference>()
                );


            var weaponBond3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinWeaponBondPlus3");
            var weaponBond4 = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinWeaponBondPlus4");
            var weaponBond5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinWeaponBondPlus5");
            var weaponBond6 = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinWeaponBondPlus6");



            return Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, "TyrantWeaponBondProgression", bp => {
                bp.SetName(MCEContext, "Diabolic Bond");
                bp.SetDescription(MCEContext, WEAPON_BOND_DESCRIPTION);
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel{
                        m_Class = AntipaladinClassRef
                    }
                };
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(5, weaponBond),
                    Helpers.CreateLevelEntry(8, weaponBond2),
                    Helpers.CreateLevelEntry(9, weaponBondAdditionalUse),
                    Helpers.CreateLevelEntry(11, weaponBond3),
                    Helpers.CreateLevelEntry(13, weaponBondAdditionalUse),
                    Helpers.CreateLevelEntry(14, weaponBond4),
                    Helpers.CreateLevelEntry(17, weaponBond5, weaponBondAdditionalUse),
                    Helpers.CreateLevelEntry(20, weaponBond6),
                };
            });
        }

        private static BlueprintActivatableAbility CreateWeaponBondChoice(string name, UnityEngine.Sprite icon,
            BlueprintBuffReference bondBuff, int weight = 1) {
            return Helpers.CreateBlueprint<BlueprintActivatableAbility>(MCEContext, $"TyrantWeaponBond{name}Choice", bp => {
                bp.SetName(MCEContext, $"Fiendish Weapon Bond - {name}");
                bp.SetDescription(MCEContext, WEAPON_BOND_DESCRIPTION);
                bp.m_Icon = icon;
                bp.DeactivateImmediately = true;
                bp.Group = ActivatableAbilityGroup.DivineWeaponProperty;
                bp.ActivationType = AbilityActivationType.Immediately;
                bp.m_ActivateWithUnitCommand = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                bp.m_ActivateOnUnitAction = AbilityActivateOnUnitActionType.Attack;
                bp.WeightInGroup = weight;
                bp.m_Buff = bondBuff;
            });
        }

        private static BlueprintBuff CreateWeaponBondBuff(string name, BlueprintItemEnchantmentReference enchant) {
            return Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, $"TyrantWeaponBond{name}Buff", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.Stacking = StackingType.Stack;
                bp.Frequency = DurationRate.Rounds;
                bp.AddComponent<AddBondProperty>(c => {
                    c.EnchantPool = EnchantPoolType.DivineWeaponBond;
                    c.m_Enchant = enchant;
                });
            });
        }

        private static BlueprintFeature CreateWeaponBondFeaturePlusX(int bonus, UnityEngine.Sprite icon, params BlueprintUnitFactReference[] facts) {
            return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, $"TyrantWeaponBondPlus{bonus}", bp => {
                bp.SetName(MCEContext, $"Fiendish Weapon Bond (+{bonus})");
                bp.SetDescription(MCEContext, WEAPON_BOND_DESCRIPTION);
                bp.m_Icon = icon;
                if (facts != null && facts.Length > 0) {
                    bp.AddComponent<AddFacts>(c => {
                        c.m_Facts = facts;
                    });
                }
                bp.AddComponent<IncreaseActivatableAbilityGroupSize>(c => {
                    c.Group = ActivatableAbilityGroup.DivineWeaponProperty;
                });
            });
        }

    }
}
