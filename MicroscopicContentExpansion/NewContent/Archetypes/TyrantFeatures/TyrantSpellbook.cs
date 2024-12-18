﻿using Kingmaker.Blueprints;
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

namespace MicroscopicContentExpansion.NewContent.Archetypes.TyrantFeatures;
internal class TyrantSpellbook
{

    public static BlueprintSpellbook AddTyrantSpellbook()
    {

        var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

        var PaladinSpellLevelsRef = GetBPRef<BlueprintSpellsTableReference>("9aed4803e424ae8429c392d8fbfb88ff");
        var PaladinSpellListRef = GetBPRef<BlueprintSpellListReference>("9f5be2f7ea64fe04eb40878347b147bc");

        var protectionFromChaosGood = ProtectionFromChaosGood.AddProtectionFromChaosGood();
        var protectionFromChaosGoodCommunal = ProtectionFromChaosGood.AddProtectionFromChaosGoodCommunal();

        var deadlyJuggernaut = MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("DeadlyJuggernaut");
        var bladeofDarkTriumph = MCEContext.GetModBlueprintReference<BlueprintAbilityReference>("BladeofDarkTriumph");

        var tyrantSpellList = Helpers.CreateBlueprint<BlueprintSpellList>(MCEContext, "TyrantSpelllist", bp =>
        {
            bp.SpellsByLevel = [
                new SpellLevelList(0){},
                new SpellLevelList(1) {
                    m_Spells = new List<BlueprintAbilityReference>() {
                        GetBPRef<BlueprintAbilityReference>("8bc64d869456b004b9db255cdd1ea734"), //Bane
                        GetBPRef<BlueprintAbilityReference>("b7731c2b4fa1c9844a092329177be4c3"), //Boneshaker
                        GetBPRef<BlueprintAbilityReference>("bd81a3931aa285a4f9844585b5d97e51"), //Cause Fear
                        GetBPRef<BlueprintAbilityReference>("feb70aab86cc17f4bb64432c83737ac2"), //Command
                        GetBPRef<BlueprintAbilityReference>("e5cb4c4459e437e49a4cd73fde6b9063"), //Inflict Light Wounds
                        GetBPRef<BlueprintAbilityReference>("d7fdd79f0f6b6a2418298e936bb68e40"), //Magic Weapon
                        protectionFromChaosGood,
                        GetBPRef<BlueprintAbilityReference>("8fd74eddd9b6c224693d9ab241f25e84")  //Summon Monster I
                    }
                },
                new SpellLevelList(2) {
                    m_Spells = new List<BlueprintAbilityReference>() {
                        GetBPRef<BlueprintAbilityReference>("4c3d08935262b6544ae97599b3a9556d"), //Bull Strength
                        GetBPRef<BlueprintAbilityReference>("446f7bf201dc1934f96ac0a26e324803"), //Eagle’s Splendor
                        GetBPRef<BlueprintAbilityReference>("c7104f7526c4c524f91474614054547e"), //Hold Person
                        GetBPRef<BlueprintAbilityReference>("89940cde01689fb46946b2f8cd7b66b7"), //Invisibility
                        GetBPRef<BlueprintAbilityReference>("c9198d9dfd2515d4ba98335b57bb66c7"), //Litany of Eloquence
                        GetBPRef<BlueprintAbilityReference>("16f7754287811724abe1e0ead88f74ca"), //Litany of Entanglement
                        GetBPRef<BlueprintAbilityReference>("dee3074b2fbfb064b80b973f9b56319e"), //Pernicious Poison
                        protectionFromChaosGoodCommunal,
                        GetBPRef<BlueprintAbilityReference>("08cb5f4c3b2695e44971bf5c45205df0"), //Scare
                        GetBPRef<BlueprintAbilityReference>("1724061e89c667045a6891179ee2e8e7")  //Summon Monster II                            
                    }
                },
                new SpellLevelList(3) {
                    m_Spells = new List<BlueprintAbilityReference>() {
                        deadlyJuggernaut,
                        bladeofDarkTriumph,
                        GetBPRef<BlueprintAbilityReference>("4b76d32feb089ad4499c3a1ce8e1ac27"), //Animate Dead
                        GetBPRef<BlueprintAbilityReference>("989ab5c44240907489aba0a8568d0603"), //Bestow Curse
                        GetBPRef<BlueprintAbilityReference>("48e2744846ed04b4580be1a3343a5d3d"), //Contagion
                        GetBPRef<BlueprintAbilityReference>("92681f181b507b34ea87018e8f7a528a"), //Dispel Magic
                        GetBPRef<BlueprintAbilityReference>("14d749ecacca90a42b6bf1c3f580bb0c"), //Inflict Moderate Wounds
                        GetBPRef<BlueprintAbilityReference>("0f92caa35619f234298d95a4b6dda90d"), //Magic Weapon, Greater
                        GetBPRef<BlueprintAbilityReference>("903092f6488f9ce45a80943923576ab3"), //Displacement (Shield of Darkness)
                        GetBPRef<BlueprintAbilityReference>("5d61dde0020bbf54ba1521f7ca0229dc")  //Summon Monster III
                    }
                },
                new SpellLevelList(4) {
                    m_Spells = new List<BlueprintAbilityReference>() {
                        GetBPRef<BlueprintAbilityReference>("d2aeac47450c76347aebbc02e4f463e0"), //Fear
                        GetBPRef<BlueprintAbilityReference>("b0b8a04a3d74e03489862b03f4e467a6"), //Inflict Serious Wounds
                        GetBPRef<BlueprintAbilityReference>("ecaa0def35b38f949bd1976a6c9539e0"), //Invisibility, Greater
                        GetBPRef<BlueprintAbilityReference>("435e73bcff18f304293484f9511b4672"), //Litany of Madness
                        GetBPRef<BlueprintAbilityReference>("d797007a142a6c0409a74b064065a15e"), //Poison
                        GetBPRef<BlueprintAbilityReference>("b56521d58f996cd4299dab3f38d5fe31"), //Profane Nimbus
                        GetBPRef<BlueprintAbilityReference>("9047cb1797639924487ec0ad566a3fea"), //Resounding Blow
                        GetBPRef<BlueprintAbilityReference>("a6e59e74cba46a44093babf6aec250fc"), //Slay Living
                        GetBPRef<BlueprintAbilityReference>("7ed74a3ec8c458d4fb50b192fd7be6ef")  //Summon Monster IV                            
                    }
                }
            ];
        });

        var tyrantSpellbook = Helpers.CreateBlueprint<BlueprintSpellbook>(MCEContext, "TyrantSpellbook", bp =>
        {
            bp.Name = Helpers.CreateString(MCEContext, $"{bp.name}.Name", "Tyrant");
            bp.CastingAttribute = Kingmaker.EntitySystem.Stats.StatType.Charisma;
            bp.AllSpellsKnown = true;
            bp.CantripsType = CantripsType.Cantrips;
            bp.HasSpecialSpellList = false;
            bp.m_SpellsPerDay = PaladinSpellLevelsRef;
            bp.m_SpellSlots = null;
            bp.m_SpellList = tyrantSpellList.ToReference<BlueprintSpellListReference>();
            bp.m_CharacterClass = AntipaladinClassRef;
            bp.IsArcane = false;
            bp.Spontaneous = false;
            bp.CasterLevelModifier = -3;

        });
        SpellTools.Spellbook.AllSpellbooks.Add(tyrantSpellbook);

        return tyrantSpellbook;
    }

    internal static void AddPrestigeProgression()
    {
        var tyrantSpellbook = GetBPRef<BlueprintSpellbookReference>("be009b27-51ce-41db-b5ee-11427a7fc8cd");
        var antipaladinClassRef = GetBPRef<BlueprintCharacterClassReference>("8939eff2-5a0a-4b77-ad1a-b6be4c760a6c");
        var tyrantArchetype = GetBPRef<BlueprintArchetypeReference>("6086bfab-e337-4850-ba16-3d23eb15ef15");
        var prestigeProgressionDescription = GetBP<BlueprintProgression>("c7dd4b32faf088444b5f83033a1228ea").m_Description;

        #region Mystic Theurge support            
        {
            var mtClassRef = GetBPRef<BlueprintCharacterClassReference>("0920ea7e4fd7a404282e3d8b0ac41838");
            var mtDivineSpellbookSelection = GetBP<BlueprintFeatureSelection>("7cd057944ce7896479717778330a4933");

            var mtAntipaladinLevelUp = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "MysticTheurgeAntipaladinTyrantLevelUp", bp =>
            {
                bp.AddComponent<AddSpellbookLevel>(c =>
                {
                    c.m_Spellbook = tyrantSpellbook;
                });
                bp.IsClassFeature = true;
                bp.Ranks = 10;
                bp.ReapplyOnLevelUp = false;
                bp.HideInUI = true;
            });

            var mysticTheurgeAntipaladinProgression = Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, "MysticTheurgeAntipaladinTyrantProgression", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(MCEContext, $"{bp.name}.DisplayName", "Tyrant");
                bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c =>
                {
                    c.m_CharacterClass = antipaladinClassRef;
                    c.RequiredSpellLevel = 2;
                });
                bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c =>
                {
                    c.m_CharacterClass = antipaladinClassRef;
                    c.m_Archetype = tyrantArchetype;
                });
                bp.AddComponent<MysticTheurgeSpellbook>(c =>
                {
                    c.m_CharacterClass = antipaladinClassRef;
                    c.m_MysticTheurge = mtClassRef;
                });
                bp.m_Classes = [
                    new ClassWithLevel(){
                        m_Class = mtClassRef,
                        AdditionalLevel = 0
                    }
                ];
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
            var hksClassRef = GetBPRef<BlueprintCharacterClassReference>("ee6425d6392101843af35f756ce7fefd");
            var hksSpellbookSelection = GetBP<BlueprintFeatureSelection>("68782aa7a302b6d43a42a71c6e9b5277");

            var hksAntipaladinLevelUp = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "HellknightSigniferAntipaladinTyrantLevelUp", bp =>
            {
                bp.AddComponent<AddSpellbookLevel>(c =>
                {
                    c.m_Spellbook = tyrantSpellbook;
                });
                bp.IsClassFeature = true;
                bp.Ranks = 10;
                bp.ReapplyOnLevelUp = false;
                bp.HideInUI = true;
            });
            var hellknightSigniferAntipaladinProgression = Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, "HellknightSigniferAntipaladinTyrantProgression", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(MCEContext, $"{bp.name}.DisplayName", "Tyrant");
                bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c =>
                {
                    c.m_CharacterClass = antipaladinClassRef;
                    c.RequiredSpellLevel = 3;
                });
                bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c =>
                {
                    c.m_CharacterClass = antipaladinClassRef;
                    c.m_Archetype = tyrantArchetype;
                });
                bp.m_Classes = [
                    new ClassWithLevel(){
                        m_Class = hksClassRef,
                        AdditionalLevel = 0
                    }
                ];
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
            var loremasterClassRef = GetBPRef<BlueprintCharacterClassReference>("4a7c05adfbaf05446a6bf664d28fb103");
            var loremasterSpellbookSelection = GetBP<BlueprintFeatureSelection>("7a28ab4dfc010834eabc770152997e87");

            var loremasterAntipaladinLevelUp = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "LoremasterAntipaladinTyrantLevelUp", bp =>
            {
                bp.AddComponent<AddSpellbookLevel>(c =>
                {
                    c.m_Spellbook = tyrantSpellbook;
                });
                bp.IsClassFeature = true;
                bp.Ranks = 10;
                bp.ReapplyOnLevelUp = false;
                bp.HideInUI = true;
            });
            var loremasterTyrantProgression = Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, "LoremasterAntipaladinTyrantProgression", bp =>
            {
                bp.m_DisplayName = Helpers.CreateString(MCEContext, $"{bp.name}.DisplayName", "Tyrant");
                bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c =>
                {
                    c.m_CharacterClass = antipaladinClassRef;
                    c.RequiredSpellLevel = 3;
                });
                bp.AddPrerequisite<PrerequisiteNoArchetype>(c =>
                {
                    c.m_CharacterClass = antipaladinClassRef;
                    c.m_Archetype = tyrantArchetype;
                });
                bp.m_Classes = [
                    new ClassWithLevel(){
                        m_Class = loremasterClassRef,
                        AdditionalLevel = 0
                    }
                ];
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = false;
                bp.HideNotAvailibleInUI = true;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = false;
                bp.IsClassFeature = true;
                bp.ForAllOtherClasses = false;
                bp.m_Description = prestigeProgressionDescription;
                bp.LevelEntries = Enumerable.Range(1, 10).Select(i => Helpers.CreateLevelEntry(i, loremasterAntipaladinLevelUp)).ToArray();
            });
            loremasterSpellbookSelection.m_AllFeatures = loremasterSpellbookSelection.m_AllFeatures.AppendToArray(loremasterTyrantProgression.ToReference<BlueprintFeatureReference>());
        }
        #endregion
    }
}
