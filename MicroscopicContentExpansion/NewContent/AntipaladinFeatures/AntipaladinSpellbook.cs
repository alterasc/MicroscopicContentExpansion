using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using MicroscopicContentExpansion.NewContent.Spells;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.Blueprints.Classes.BlueprintProgression;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures {
    internal class AntipaladinSpellbook {

        public static void AddAntipaladinSpellbook() {

            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var PaladinSpellLevelsRef = BlueprintTools.GetBlueprintReference<BlueprintSpellsTableReference>("9aed4803e424ae8429c392d8fbfb88ff");
            var PaladinSpellListRef = BlueprintTools.GetBlueprintReference<BlueprintSpellListReference>("9f5be2f7ea64fe04eb40878347b147bc");

            var protectionFromLawGood = ProtectionFromLawGood.AddProtectionFromLawGood();
            var protectionFromLawGoodCommunal = ProtectionFromLawGood.AddProtectionFromLawGoodCommunal();

            var deadlyJuggernaut = DeadlyJuggernaut.AddDeadlyJuggernaut();
            var bladeofDarkTriumph = BladeofDarkTriumph.AddBladeofDarkTriumph();

            var AntipaladinSpellList = Helpers.CreateBlueprint<BlueprintSpellList>(MCEContext, "AntipaladinSpelllist", bp => {
                bp.SpellsByLevel = new SpellLevelList[] {
                    new SpellLevelList(0){},
                    new SpellLevelList(1) {
                        m_Spells = new List<BlueprintAbilityReference>() {
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("8bc64d869456b004b9db255cdd1ea734"), //Bane
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("b7731c2b4fa1c9844a092329177be4c3"), //Boneshaker
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("bd81a3931aa285a4f9844585b5d97e51"), //Cause Fear
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("feb70aab86cc17f4bb64432c83737ac2"), //Command
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("e5cb4c4459e437e49a4cd73fde6b9063"), //Inflict Light Wounds
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("d7fdd79f0f6b6a2418298e936bb68e40"), //Magic Weapon
                            protectionFromLawGood,
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("8fd74eddd9b6c224693d9ab241f25e84")  //Summon Monster I
                        }
                    },
                    new SpellLevelList(2) {
                        m_Spells = new List<BlueprintAbilityReference>() {
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("4c3d08935262b6544ae97599b3a9556d"), //Bull Strength
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("446f7bf201dc1934f96ac0a26e324803"), //Eagle’s Splendor
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("c7104f7526c4c524f91474614054547e"), //Hold Person
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("89940cde01689fb46946b2f8cd7b66b7"), //Invisibility
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("c9198d9dfd2515d4ba98335b57bb66c7"), //Litany of Eloquence
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("16f7754287811724abe1e0ead88f74ca"), //Litany of Entanglement
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("dee3074b2fbfb064b80b973f9b56319e"), //Pernicious Poison
                            protectionFromLawGoodCommunal,
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("08cb5f4c3b2695e44971bf5c45205df0"), //Scare
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("1724061e89c667045a6891179ee2e8e7")  //Summon Monster II                            
                        }
                    },
                    new SpellLevelList(3) {
                        m_Spells = new List<BlueprintAbilityReference>() {
                            deadlyJuggernaut,
                            bladeofDarkTriumph,
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("4b76d32feb089ad4499c3a1ce8e1ac27"), //Animate Dead
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("989ab5c44240907489aba0a8568d0603"), //Bestow Curse
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("48e2744846ed04b4580be1a3343a5d3d"), //Contagion
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("92681f181b507b34ea87018e8f7a528a"), //Dispel Magic
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("14d749ecacca90a42b6bf1c3f580bb0c"), //Inflict Moderate Wounds
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("0f92caa35619f234298d95a4b6dda90d"), //Magic Weapon, Greater
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("903092f6488f9ce45a80943923576ab3"), //Displacement (Shield of Darkness)
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("5d61dde0020bbf54ba1521f7ca0229dc")  //Summon Monster III
                        }
                    },
                    new SpellLevelList(4) {
                        m_Spells = new List<BlueprintAbilityReference>() {
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("d2aeac47450c76347aebbc02e4f463e0"), //Fear
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("b0b8a04a3d74e03489862b03f4e467a6"), //Inflict Serious Wounds
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("ecaa0def35b38f949bd1976a6c9539e0"), //Invisibility, Greater
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("435e73bcff18f304293484f9511b4672"), //Litany of Madness
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("d797007a142a6c0409a74b064065a15e"), //Poison
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("b56521d58f996cd4299dab3f38d5fe31"), //Profane Nimbus
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("9047cb1797639924487ec0ad566a3fea"), //Resounding Blow
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("a6e59e74cba46a44093babf6aec250fc"), //Slay Living
                            BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("7ed74a3ec8c458d4fb50b192fd7be6ef")  //Summon Monster IV                            
                        }
                    }
                };
            });

            var antipaladinSpellbook = Helpers.CreateBlueprint<BlueprintSpellbook>(MCEContext, "AntipaladinSpellbook", bp => {
                bp.Name = Helpers.CreateString(MCEContext, $"{bp.name}.Name", "Antipaladin");
                bp.CastingAttribute = Kingmaker.EntitySystem.Stats.StatType.Charisma;
                bp.AllSpellsKnown = true;
                bp.CantripsType = CantripsType.Cantrips;
                bp.HasSpecialSpellList = false;
                bp.m_SpellsPerDay = PaladinSpellLevelsRef;
                bp.m_SpellSlots = null;
                bp.m_SpellList = AntipaladinSpellList.ToReference<BlueprintSpellListReference>();
                bp.m_CharacterClass = AntipaladinClassRef;
                bp.IsArcane = false;
                bp.Spontaneous = false;
                bp.CasterLevelModifier = -3;

            });
            SpellTools.Spellbook.AllSpellbooks.Add(antipaladinSpellbook);
        }

        internal static void AddPrestigeProgression() {
            var antipaladinSpellbook = BlueprintTools.GetBlueprintReference<BlueprintSpellbookReference>("c164072a-cbd2-4c6a-8c4e-271bbb7a6903");
            var antipaladinClassRef = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("8939eff2-5a0a-4b77-ad1a-b6be4c760a6c");
            var tyrantArchetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("6086bfab-e337-4850-ba16-3d23eb15ef15");
            var prestigeProgressionDescription = BlueprintTools.GetBlueprint<BlueprintProgression>("c7dd4b32faf088444b5f83033a1228ea").m_Description;

            #region Mystic Theurge support            
            {
                var mtClassRef = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("0920ea7e4fd7a404282e3d8b0ac41838");
                var mtDivineSpellbookSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("7cd057944ce7896479717778330a4933");

                var mtAntipaladinLevelUp = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "MysticTheurgeAntipaladinLevelUp", bp => {
                    bp.AddComponent<AddSpellbookLevel>(c => {
                        c.m_Spellbook = antipaladinSpellbook;
                    });
                    bp.IsClassFeature = true;
                    bp.Ranks = 10;
                    bp.ReapplyOnLevelUp = false;
                    bp.HideInUI = true;
                });

                var mysticTheurgeAntipaladinProgression = Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, "MysticTheurgeAntipaladinProgression", bp => {
                    bp.m_DisplayName = Helpers.CreateString(MCEContext, $"{bp.name}.DisplayName", "Antipaladin");
                    bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                        c.m_CharacterClass = antipaladinClassRef;
                        c.RequiredSpellLevel = 2;
                    });
                    bp.AddPrerequisite<PrerequisiteNoArchetype>(c => {
                        c.m_CharacterClass = antipaladinClassRef;
                        c.m_Archetype = tyrantArchetype;
                    });
                    bp.AddComponent<MysticTheurgeSpellbook>(c => {
                        c.m_CharacterClass = antipaladinClassRef;
                        c.m_MysticTheurge = mtClassRef;
                    });
                    bp.m_Classes = new ClassWithLevel[] {
                        new ClassWithLevel(){
                            m_Class = mtClassRef,
                            AdditionalLevel = 0
                        }
                    };
                    bp.HideInUI = true;
                    bp.HideInCharacterSheetAndLevelUp = false;
                    bp.HideNotAvailibleInUI = true;
                    bp.Ranks = 1;
                    bp.ReapplyOnLevelUp = false;
                    bp.IsClassFeature = true;
                    bp.ForAllOtherClasses = false;
                    bp.m_Description = prestigeProgressionDescription;
                    bp.LevelEntries = Enumerable.Range(1, 10).Select(i => Helpers.CreateLevelEntry(i, mtAntipaladinLevelUp)).ToArray();
                });

                mtDivineSpellbookSelection.m_AllFeatures = mtDivineSpellbookSelection.m_AllFeatures.AppendToArray(mysticTheurgeAntipaladinProgression.ToReference<BlueprintFeatureReference>());
            }
            #endregion

            #region Hellknight Signifer support
            {
                var hksClassRef = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("ee6425d6392101843af35f756ce7fefd");
                var hksSpellbookSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("68782aa7a302b6d43a42a71c6e9b5277");

                var hksAntipaladinLevelUp = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "HellknightSigniferAntipaladinLevelUp", bp => {
                    bp.AddComponent<AddSpellbookLevel>(c => {
                        c.m_Spellbook = antipaladinSpellbook;
                    });
                    bp.IsClassFeature = true;
                    bp.Ranks = 10;
                    bp.ReapplyOnLevelUp = false;
                    bp.HideInUI = true;
                });
                var hellknightSigniferAntipaladinProgression = Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, "HellknightSigniferAntipaladinProgression", bp => {
                    bp.m_DisplayName = Helpers.CreateString(MCEContext, $"{bp.name}.DisplayName", "Antipaladin");
                    bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                        c.m_CharacterClass = antipaladinClassRef;
                        c.RequiredSpellLevel = 3;
                    });
                    bp.AddPrerequisite<PrerequisiteNoArchetype>(c => {
                        c.m_CharacterClass = antipaladinClassRef;
                        c.m_Archetype = tyrantArchetype;
                    });
                    bp.m_Classes = new ClassWithLevel[] {
                        new ClassWithLevel(){
                            m_Class = hksClassRef,
                            AdditionalLevel = 0
                        }
                    };
                    bp.HideInUI = true;
                    bp.HideInCharacterSheetAndLevelUp = false;
                    bp.HideNotAvailibleInUI = true;
                    bp.Ranks = 1;
                    bp.ReapplyOnLevelUp = false;
                    bp.IsClassFeature = true;
                    bp.ForAllOtherClasses = false;
                    bp.m_Description = prestigeProgressionDescription;
                    bp.LevelEntries = Enumerable.Range(1, 10).Select(i => Helpers.CreateLevelEntry(i, hksAntipaladinLevelUp)).ToArray();
                });
                hksSpellbookSelection.m_AllFeatures = hksSpellbookSelection.m_AllFeatures.AppendToArray(hellknightSigniferAntipaladinProgression.ToReference<BlueprintFeatureReference>());
            }
            #endregion

            #region Loremaster support
            {
                var loremasterClassRef = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("4a7c05adfbaf05446a6bf664d28fb103");
                var loremasterSpellbookSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("7a28ab4dfc010834eabc770152997e87");

                var hksAntipaladinLevelUp = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "LoremasterAntipaladinLevelUp", bp => {
                    bp.AddComponent<AddSpellbookLevel>(c => {
                        c.m_Spellbook = antipaladinSpellbook;
                    });
                    bp.IsClassFeature = true;
                    bp.Ranks = 10;
                    bp.ReapplyOnLevelUp = false;
                    bp.HideInUI = true;
                });
                var loremasterAntipaladinProgression = Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, "LoremasterAntipaladinProgression", bp => {
                    bp.m_DisplayName = Helpers.CreateString(MCEContext, $"{bp.name}.DisplayName", "Antipaladin");
                    bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                        c.m_CharacterClass = antipaladinClassRef;
                        c.RequiredSpellLevel = 3;
                    });
                    bp.AddPrerequisite<PrerequisiteNoArchetype>(c => {
                        c.m_CharacterClass = antipaladinClassRef;
                        c.m_Archetype = tyrantArchetype;
                    });
                    bp.m_Classes = new ClassWithLevel[] {
                        new ClassWithLevel(){
                            m_Class = loremasterClassRef,
                            AdditionalLevel = 0
                        }
                    };
                    bp.HideInUI = true;
                    bp.HideInCharacterSheetAndLevelUp = false;
                    bp.HideNotAvailibleInUI = true;
                    bp.Ranks = 1;
                    bp.ReapplyOnLevelUp = false;
                    bp.IsClassFeature = true;
                    bp.ForAllOtherClasses = false;
                    bp.m_Description = prestigeProgressionDescription;
                    bp.LevelEntries = Enumerable.Range(1, 10).Select(i => Helpers.CreateLevelEntry(i, hksAntipaladinLevelUp)).ToArray();
                });
                loremasterSpellbookSelection.m_AllFeatures = loremasterSpellbookSelection.m_AllFeatures.AppendToArray(loremasterAntipaladinProgression.ToReference<BlueprintFeatureReference>());
            }
            #endregion
        }
    }
}
