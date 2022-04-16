using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;


namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures {
    internal class AntipaladinAlignmentRestriction {

        public static void AddAntipaladinAlignmentRestriction() {
            var SpellbookRef = BlueprintTools.GetModBlueprintReference<BlueprintSpellbookReference>(MCEContext, "AntipaladinSpellbook");

            Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAlignmentRestriction", bp => {
                bp.SetName(MCEContext, "Alignment Restriction");
                bp.SetDescription(MCEContext, "An antipaladin who ceases to be evil loses all antipaladin {g|Encyclopedia:Spell}spells{/g} " +
                    "and class features. He cannot thereafter gain levels as an antipaladin until he " +
                    "changes the {g|Encyclopedia:Alignment}alignment{/g} back.");
                bp.AddComponent<ForbidSpellbookOnAlignmentDeviation>(c => {
                    c.m_Spellbooks = new BlueprintSpellbookReference[] {
                        SpellbookRef
                    };
                    c.Alignment = Kingmaker.UnitLogic.Alignments.AlignmentMaskType.Evil;
                });
                bp.IsClassFeature = true;
            });
        }
    }
}
