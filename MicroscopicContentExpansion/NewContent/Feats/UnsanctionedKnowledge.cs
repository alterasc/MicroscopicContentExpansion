using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Feats {
    internal class UnsanctionedKnowledge {

        private readonly string NAME = "Unsanctioned Knowledge";
        private readonly string DESCRIPTION = "Pick one 1st-level spell, one 2nd-level spell, one 3rd-level spell," +
            " and one 4th-level spell from the bard, cleric or inquisitor spell lists. Add these spells to " +
            "your antipaladin spell list as antipaladin spells of the appropriate level. Once chosen, these spells cannot be changed.";
        private readonly string DESCRIPTION_PALADIN = "Pick one 1st-level spell, one 2nd-level spell, one 3rd-level spell," +
            " and one 4th-level spell from the bard, cleric or inquisitor spell lists. Add these spells to " +
            "your paladin spell list as antipaladin spells of the appropriate level. Once chosen, these spells cannot be changed.";        

        public static void AddUnsanctionedKnowledge() {
            new UnsanctionedKnowledge().AddUnsanctionedKnowledgeInner("Antipaladin");
            new UnsanctionedKnowledge().AddUnsanctionedKnowledgePaladin();
        }

        private void AddUnsanctionedKnowledgeInner(string @class) {
        var antipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");
        var UnsanctionedKnowledge = Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, $"{@class}UnsanctionedKnowledge", bp => {
                bp.SetName(MCEContext, $"{NAME} ({@class})");
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[0];
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1,
                        CreateSelection(1, @class, antipaladinClassRef),
                        CreateSelection(2, @class, antipaladinClassRef),
                        CreateSelection(3, @class, antipaladinClassRef),
                        CreateSelection(4, @class, antipaladinClassRef)
                    )
                };
                bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                    c.m_CharacterClass = antipaladinClassRef;
                    c.RequiredSpellLevel = 1;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.Intelligence;
                    c.Value = 13;
                });
            });

            if (MCEContext.AddedContent.NewClasses.IsDisabled("AntipaladinClass")) { return; }
            FeatTools.AddAsFeat(UnsanctionedKnowledge);
        }

        private void AddUnsanctionedKnowledgePaladin() {
            var paladinClassRef = ClassTools.ClassReferences.PaladinClass;
            var @class = "Paladin";
            var UnsanctionedKnowledge = Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, $"{@class}UnsanctionedKnowledge", bp => {
                bp.SetName(MCEContext, $"{NAME} ({@class})");
                bp.SetDescription(MCEContext, DESCRIPTION_PALADIN);
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[0];
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1,
                        CreateSelection(1, @class, paladinClassRef),
                        CreateSelection(2, @class, paladinClassRef),
                        CreateSelection(3, @class, paladinClassRef),
                        CreateSelection(4, @class, paladinClassRef)
                    )
                };
                bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                    c.m_CharacterClass = ClassTools.ClassReferences.PaladinClass;
                    c.RequiredSpellLevel = 1;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.Intelligence;
                    c.Value = 13;
                });
            });

            if (MCEContext.AddedContent.Feats.IsDisabled("PaladinUnsanctionedKnowdledge")) { return; }
            FeatTools.AddAsFeat(UnsanctionedKnowledge);
        }

        private BlueprintFeatureSelection CreateSelection(int level, string @class, BlueprintCharacterClassReference classRef) {
            var UnsanctionedKnowledgeBard = CreateUnsanctionKnowledgeParametrizedFeature(SpellTools.SpellList.BardSpellList, level, "Bard", @class, classRef);
            var UnsanctionedKnowledgeCleric = CreateUnsanctionKnowledgeParametrizedFeature(SpellTools.SpellList.ClericSpellList, level, "Cleric", @class, classRef);
            var UnsanctionedKnowledgeInquisitor = CreateUnsanctionKnowledgeParametrizedFeature(SpellTools.SpellList.InquisitorSpellList, level, "Inquisitor", @class, classRef);

            return Helpers.CreateBlueprint<BlueprintFeatureSelection>(MCEContext, $"{@class}UnsanctionedKnowledgeSelection{level}", bp => {
                bp.SetName(MCEContext, $"{level} level");
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.AddFeatures(UnsanctionedKnowledgeBard, UnsanctionedKnowledgeCleric, UnsanctionedKnowledgeInquisitor);
                bp.Mode = SelectionMode.Default;
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.IsClassFeature = true;
            });
        }

        private BlueprintParametrizedFeature CreateUnsanctionKnowledgeParametrizedFeature(BlueprintSpellList spellList, int spellLevel, string name
            , string @class, BlueprintCharacterClassReference classRef) {
            var spells = spellList.SpellsByLevel
                .First((SpellLevelList s) => s.SpellLevel == spellLevel)
                .Spells
                .Select(s => s.ToReference<AnyBlueprintReference>())
                .ToArray();

            return Helpers.CreateBlueprint<BlueprintParametrizedFeature>(MCEContext, $"{@class}UnsanctionedKnowledge{name}{spellLevel}", bp => {
                bp.IsClassFeature = true;
                bp.SetName(MCEContext, $"{name}");
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.AddComponent<LearnSpellParametrized>(c => {
                    c.m_SpellcasterClass = classRef;
                    c.m_SpellList = spellList.ToReference<BlueprintSpellListReference>();
                    c.SpecificSpellLevel = true;
                    c.SpellLevel = spellLevel;
                });
                bp.ParameterType = FeatureParameterType.LearnSpell;
                bp.m_SpellList = spellList.ToReference<BlueprintSpellListReference>();
                bp.m_SpellcasterClass = classRef;
                bp.SpecificSpellLevel = true;
                bp.SpellLevel = spellLevel;
                bp.BlueprintParameterVariants = spells;
            });

        }

    }
}
