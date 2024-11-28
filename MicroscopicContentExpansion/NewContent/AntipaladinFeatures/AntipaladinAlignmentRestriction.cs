using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;


namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures;
internal class AntipaladinAlignmentRestriction
{
    internal static void AddAntipaladinAlignmentRestriction()
    {
        var SpellbookRef = MCEContext.GetModBlueprintReference<BlueprintSpellbookReference>("AntipaladinSpellbook");

        Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAlignmentRestriction", bp =>
        {
            bp.SetName(MCEContext, "Alignment Restriction");
            bp.SetDescription(MCEContext, "An antipaladin who ceases to be evil loses all antipaladin {g|Encyclopedia:Spell}spells{/g} " +
                "and class features. He cannot thereafter gain levels as an antipaladin until he " +
                "changes the {g|Encyclopedia:Alignment}alignment{/g} back.");
            bp.AddComponent<ForbidSpellbookOnAlignmentDeviation>(c =>
            {
                c.m_Spellbooks = [
                    SpellbookRef
                ];
                c.Alignment = Kingmaker.UnitLogic.Alignments.AlignmentMaskType.Evil;
                c.m_IgnoreFact = GetBPRef<BlueprintUnitFactReference>("24e78475f0a243e1a810452d14d0a1bd");
            });
            bp.IsClassFeature = true;
        });
    }
}
