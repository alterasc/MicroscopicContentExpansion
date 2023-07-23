using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using MicroscopicContentExpansion.NewComponents;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.Blueprints.BlueprintAbilityResource;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Archetypes.IronTyrantFeatures {

    internal class IronTyrantArmorBond {
        const string NAME = "Fiendish Bond";

        const string ARMOR_BOND_DESCRIPTION = @"At 5th level, instead of forming a fiendish bond with his weapon or a servant, an iron tyrant can form a bond with his armor. As a standard action, an iron tyrant can enhance his armor by calling upon a fiendish spirit’s aid. This bond lasts for 1 minute per antipaladin level. When called, the spirit causes the armor to shed unholy light like a torch.
At 5th level, the spirit grants the armor a +1 enhancement bonus. For every 3 antipaladin levels beyond 5th, the armor gains another +1 enhancement bonus, to a maximum of +6 at 20th level.
These bonuses stack with existing armor enhancement bonuses to a maximum of +5, or they can be used to add any of the following armor special abilities: fortification (heavy, light, or medium), spell resistance (13, 15, 17, or 19).
Adding these special abilities consumes an amount of bonus equal to the special ability’s base price modifier. These special abilities are added to any special abilities the armor already has, but duplicate abilities do not stack.
If the armor is not magical, at least a +1 enhancement bonus must be added before any other special abilities can be added. The bonus and special abilities granted by the spirit are determined when the spirit is called and cannot be changed until the spirit is called again. The fiendish spirit imparts no bonuses if the armor is worn by anyone other than the iron tyrant, but it resumes giving bonuses if the iron tyrant dons the armor again. An iron tyrant can use this ability once per day at 5th level, and one additional time per day for every 4 levels beyond 5th, to a total of four times per day at 17th level.";


        public static BlueprintProgression AddFiendishBond() {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var icon = BlueprintTools.GetBlueprint<BlueprintFeature>("35e2d9525c240ce4c8ae47dd387b6e53").Icon;

            var fiendishBondResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(MCEContext, "IronTyrantArmorBondResource", bp => {
                bp.m_Icon = icon;
                bp.m_MaxAmount = new Amount() {
                    BaseValue = 1,
                };
            });

            var armorBondAdditionalUse = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "IronTyrantArmorBondAdditionalUse", bp => {
                bp.SetName(MCEContext, $"{NAME} - Additional Use");
                bp.SetDescription(MCEContext, ARMOR_BOND_DESCRIPTION);
                bp.Ranks = 3;
                bp.m_Icon = icon;
                bp.IsClassFeature = true;
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = fiendishBondResource.ToReference<BlueprintAbilityResourceReference>();
                });
            });

            var bondDurationBuff = Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, "IronTyrantArmorBondDurationBuff", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, ARMOR_BOND_DESCRIPTION);
                bp.m_Icon = icon;
                bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
                bp.Stacking = StackingType.Replace;
                bp.Frequency = DurationRate.Rounds;
            });

            var paladinArmorBondSwitchAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("7ff088ab58c69854b82ea95c2b0e35b4");

            var weaponBondSwitchAbility = Helpers.CreateBlueprint<BlueprintAbility>(MCEContext, "IronTyrantArmorBondSwitchAbility", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, ARMOR_BOND_DESCRIPTION);
                bp.NeedEquipWeapons = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Extend | Metamagic.Heighten;
                bp.CanTargetEnemies = false;
                bp.CanTargetFriends = false;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.m_Icon = icon;
                bp.LocalizedDuration = Helpers.CreateString(MCEContext, $"{bp.name}.Duration", "1 minute per antipaladin level");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionArmorEnchantPoolMCE() {
                            Group = ActivatableAbilityGroup.DivineWeaponProperty,
                            EnchantPool = EnchantPoolType.SacredArmorPool,
                            m_DefaultEnchantments = new BlueprintItemEnchantmentReference[] {
                                BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("1d9b60d57afb45c4f9bb0a3c21bb3b98"),
                                BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("d45bfd838c541bb40bde7b0bf0e1b684"),
                                BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("51c51d841e9f16046a169729c13c4d4f"),
                                BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("a23bcee56c9fcf64d863dafedb369387"),
                                BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("15d7d6cbbf56bd744b37bbf9225ea83b")
                            },
                            DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Minutes,
                                DiceType = Kingmaker.RuleSystem.DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = new ContextValue() {
                                    ValueType = ContextValueType.Rank
                                },
                                m_IsExtendable = true
                            }
                        },
                        new ContextActionApplyBuff() {
                            m_Buff = bondDurationBuff.ToReference<BlueprintBuffReference>(),
                            DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Minutes,
                                DiceType = Kingmaker.RuleSystem.DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = new ContextValue() {
                                    ValueType = ContextValueType.Rank
                                },
                                m_IsExtendable = true
                            }
                        }
                    ); ;
                });
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Stat = StatType.Unknown;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_Max = 20;
                    c.m_Class = new BlueprintCharacterClassReference[] { AntipaladinClassRef };
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = fiendishBondResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                });
                bp.AddComponent<AbilityCasterAlignment>(c => {
                    c.Alignment = Kingmaker.UnitLogic.Alignments.AlignmentMaskType.Evil;
                });
            });

            var lightFortificationBuff = CreateArmorBondBuff("LightFortification"
                , BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("1e69e9029c627914eb06608dad707b36"));

            var armorBondFortification25 = CreateArmorBondChoice("LightFortification",
                BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("54915668d929ad14c8b68e867211789d").Icon,
                lightFortificationBuff.ToReference<BlueprintBuffReference>(), 1);


            var armorBond = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "IronTyrantArmorBondFeature", bp => {
                bp.SetName(MCEContext, "Fiendish Bond (+1)");
                bp.SetDescription(MCEContext, ARMOR_BOND_DESCRIPTION);
                bp.m_Icon = icon;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        weaponBondSwitchAbility.ToReference<BlueprintUnitFactReference>(),
                        armorBondFortification25.ToReference<BlueprintUnitFactReference>()
                    };
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = fiendishBondResource.ToReference<BlueprintAbilityResourceReference>();

                });
            });

            var spellResistance13Buff = CreateArmorBondBuff("SpellResistance13"
                , BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("4bc20fd0e137e1645a18f030b961ef3d"));

            var armorBondspellResistance13 = CreateArmorBondChoice("SpellResistance13",
                BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("dcec122f4835ae344869db303328b580").Icon,
                spellResistance13Buff.ToReference<BlueprintBuffReference>(), 2);


            var armorBond2 = CreateArmorBondFeaturePlusX(2, icon, armorBondspellResistance13.ToReference<BlueprintUnitFactReference>());

            var spellResistance15Buff = CreateArmorBondBuff("SpellResistance15"
                , BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("ad0f81f6377180d4292a2316efb950f2"));
            var armorBondspellResistance15 = CreateArmorBondChoice("SpellResistance15",
                BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("4ecd141667ce43f43825c16b852d1b44").Icon,
                spellResistance15Buff.ToReference<BlueprintBuffReference>(), 3);

            var mediumFortificationBuff = CreateArmorBondBuff("MediumFortification"
                , BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("62ec0b22425fb424c82fd52d7f4c02a5"));
            var mediumFortification = CreateArmorBondChoice("MediumFortification",
                BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("409487ddf11386240b23f0a17ec4de3f").Icon,
                mediumFortificationBuff.ToReference<BlueprintBuffReference>(), 3);

            var armorBond3 = CreateArmorBondFeaturePlusX(3, icon,
                armorBondspellResistance15.ToReference<BlueprintUnitFactReference>(),
                mediumFortification.ToReference<BlueprintUnitFactReference>()
                );

            var spellResistance17Buff = CreateArmorBondBuff("SpellResistance17"
                , BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("49fe9e1969afd874181ed7613120c250"));
            var armorBondspellResistance17 = CreateArmorBondChoice("SpellResistance17",
                BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("46393997456d0b747998886b5c30b9cb").Icon,
                spellResistance17Buff.ToReference<BlueprintBuffReference>(), 4);

            var armorBond4 = CreateArmorBondFeaturePlusX(4, icon, armorBondspellResistance17.ToReference<BlueprintUnitFactReference>());

            var spellResistance19Buff = CreateArmorBondBuff("SpellResistance19"
                , BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("583938eaafc820f49ad94eca1e5a98ca"));
            var armorBondspellResistance19 = CreateArmorBondChoice("SpellResistance19",
                BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("ccf91cd189f900146a62cc0a1a62fbd6").Icon,
                spellResistance19Buff.ToReference<BlueprintBuffReference>(), 5);

            var heavyFortificationBuff = CreateArmorBondBuff("HeavyFortification"
                , BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("9b1538c732e06544bbd955fee570a2be"));
            var heavyFortification = CreateArmorBondChoice("HeavyFortification",
                BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("f42a30cdd14b57241abd1022d5ab1721").Icon,
                mediumFortificationBuff.ToReference<BlueprintBuffReference>(), 5);


            var armorBond5 = CreateArmorBondFeaturePlusX(5, icon,
                armorBondspellResistance19.ToReference<BlueprintUnitFactReference>(),
                heavyFortification.ToReference<BlueprintUnitFactReference>());

            var armorBond6 = CreateArmorBondFeaturePlusX(6, icon);

            return Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, "IronTyrantFiendishBondProgression", bp => {
                bp.SetName(MCEContext, "Fiendish Bond");
                bp.SetDescription(MCEContext, ARMOR_BOND_DESCRIPTION);
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel{
                        m_Class = AntipaladinClassRef
                    }
                };
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(5, armorBond),
                    Helpers.CreateLevelEntry(8, armorBond2),
                    Helpers.CreateLevelEntry(9, armorBondAdditionalUse),
                    Helpers.CreateLevelEntry(11, armorBond3),
                    Helpers.CreateLevelEntry(13, armorBondAdditionalUse),
                    Helpers.CreateLevelEntry(14, armorBond4),
                    Helpers.CreateLevelEntry(17, armorBond5, armorBondAdditionalUse),
                    Helpers.CreateLevelEntry(20, armorBond6),
                };
            });

        }

        private static BlueprintActivatableAbility CreateArmorBondChoice(string name, UnityEngine.Sprite icon,
            BlueprintBuffReference bondBuff, int weight = 1) {
            return Helpers.CreateBlueprint<BlueprintActivatableAbility>(MCEContext, $"IronTyrantArmorBond{name}Choice", bp => {
                bp.SetName(MCEContext, $"Fiendish Bond - {name}");
                bp.SetDescription(MCEContext, ARMOR_BOND_DESCRIPTION);
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

        private static BlueprintBuff CreateArmorBondBuff(string name, BlueprintItemEnchantmentReference enchant) {
            return Helpers.CreateBlueprint<BlueprintBuff>(MCEContext, $"IronTyrantArmorBond{name}Buff", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.Stacking = StackingType.Stack;
                bp.Frequency = DurationRate.Rounds;
                bp.AddComponent<AddBondProperty>(c => {
                    c.EnchantPool = EnchantPoolType.SacredArmorPool;
                    c.m_Enchant = enchant;
                });
            });
        }

        private static BlueprintFeature CreateArmorBondFeaturePlusX(int bonus, UnityEngine.Sprite icon, params BlueprintUnitFactReference[] facts) {
            return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, $"IronTyrantArmorBondPlus{bonus}", bp => {
                bp.SetName(MCEContext, $"Fiendish Bond (+{bonus})");
                bp.SetDescription(MCEContext, ARMOR_BOND_DESCRIPTION);
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
