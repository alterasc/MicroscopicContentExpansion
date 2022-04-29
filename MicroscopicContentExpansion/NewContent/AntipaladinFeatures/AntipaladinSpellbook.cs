using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using MicroscopicContentExpansion.NewContent.Spells;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures {
    internal class AntipaladinSpellbook {

        public static void AddAntipaladinSpellbook() {

            var AntipaladinClassRef = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(MCEContext, "AntipaladinClass");

            var PaladinSpellLevelsRef = BlueprintTools.GetBlueprintReference<BlueprintSpellsTableReference>("9aed4803e424ae8429c392d8fbfb88ff");
            var PaladinSpellListRef = BlueprintTools.GetBlueprintReference<BlueprintSpellListReference>("9f5be2f7ea64fe04eb40878347b147bc");

            var protectionFromLawGood = ProtectionFromLawGood.AddProtectionFromLawGood();
            var protectionFromLawGoodCommunal = ProtectionFromLawGood.AddProtectionFromLawGoodCommunal();

            var deadlyJuggernaut = DeadlyJuggernaut.AddDeadlyJuggernaut();

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

            var AntipaladinSpellbook = Helpers.CreateBlueprint<BlueprintSpellbook>(MCEContext, "AntipaladinSpellbook", bp => {
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
            SpellTools.Spellbook.AllSpellbooks.Add(AntipaladinSpellbook);

        }
    }
}
