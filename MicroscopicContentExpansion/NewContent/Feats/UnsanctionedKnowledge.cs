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
        private readonly BlueprintCharacterClassReference AntipaladinClassRef = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(MCEContext, "AntipaladinClass");

        public static void AddUnsanctionedKnowledge() {
            new UnsanctionedKnowledge().AddUnsanctionedKnowledgeInner();
        }

        private void AddUnsanctionedKnowledgeInner() {
            var UnsanctionedKnowledge = Helpers.CreateBlueprint<BlueprintProgression>(MCEContext, "AntipaladinUnsanctionedKnowledge", bp => {
                bp.SetName(MCEContext, NAME);
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
                        CreateSelection(1),
                        CreateSelection(2),
                        CreateSelection(3),
                        CreateSelection(4)
                    )
                };
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = AntipaladinClassRef;
                    c.Level = 4;
                });
                bp.AddPrerequisite<PrerequisiteCasterTypeSpellLevel>(c => {
                    c.IsArcane = false;
                    c.RequiredSpellLevel = 1;
                });
            });

            FeatTools.AddAsFeat(UnsanctionedKnowledge);
        }

        private BlueprintFeatureSelection CreateSelection(int level) {
            var UnsanctionedKnowledgeBard = CreateUnsanctionKnowledgeParametrizedFeature(SpellTools.SpellList.BardSpellList, level, "Bard");
            var UnsanctionedKnowledgeCleric = CreateUnsanctionKnowledgeParametrizedFeature(SpellTools.SpellList.ClericSpellList, level, "Cleric");
            var UnsanctionedKnowledgeInquisitor = CreateUnsanctionKnowledgeParametrizedFeature(SpellTools.SpellList.InquisitorSpellList, level, "Inquisitor");

            return Helpers.CreateBlueprint<BlueprintFeatureSelection>(MCEContext, $"AntipaladinUnsanctionedKnowledgeSelection{level}", bp => {
                bp.SetName(MCEContext, $"{level} level");
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.AddFeatures(UnsanctionedKnowledgeBard, UnsanctionedKnowledgeCleric, UnsanctionedKnowledgeInquisitor);
                bp.Mode = SelectionMode.Default;
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.IsClassFeature = true;
            });
        }

        private BlueprintParametrizedFeature CreateUnsanctionKnowledgeParametrizedFeature(BlueprintSpellList spellList, int spellLevel, string name) {
            var spells = SpellTools.SpellList.BardSpellList.SpellsByLevel
                .First((SpellLevelList s) => s.SpellLevel == spellLevel)
                .Spells
                .Select(s => s.ToReference<AnyBlueprintReference>())
                .ToArray();

            return Helpers.CreateBlueprint<BlueprintParametrizedFeature>(MCEContext, $"AntipaladinUnsanctionedKnowledge{name}{spellLevel}", bp => {
                bp.IsClassFeature = true;
                bp.SetName(MCEContext, $"{name}");
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.AddComponent<LearnSpellParametrized>(c => {
                    c.m_SpellcasterClass = AntipaladinClassRef;
                    c.m_SpellList = spellList.ToReference<BlueprintSpellListReference>();
                    c.SpecificSpellLevel = true;
                    c.SpellLevel = spellLevel;
                });
                bp.ParameterType = FeatureParameterType.LearnSpell;
                bp.m_SpellList = spellList.ToReference<BlueprintSpellListReference>();
                bp.m_SpellcasterClass = AntipaladinClassRef;
                bp.SpecificSpellLevel = true;
                bp.SpellLevel = spellLevel;
                bp.BlueprintParameterVariants = spells;
            });

        }

    }
}
