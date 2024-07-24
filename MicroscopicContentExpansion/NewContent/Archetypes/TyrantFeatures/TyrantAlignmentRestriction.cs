﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;


namespace MicroscopicContentExpansion.NewContent.Archetypes.TyrantFeatures;
internal class TyrantAlignmentRestriction
{

    public static BlueprintFeatureReference AddAntipaladinAlignmentRestriction()
    {
        var SpellbookRef = MCEContext.GetModBlueprintReference<BlueprintSpellbookReference>("AntipaladinSpellbook");

        return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "TyrantAlignmentRestriction", bp =>
        {
            bp.SetName(MCEContext, "Alignment Restriction");
            bp.SetDescription(MCEContext, "A tyrant who ceases to be lawful evil loses all antipaladin {g|Encyclopedia:Spell}spells{/g} " +
                "and class features. He cannot thereafter gain levels as an antipaladin until he " +
                "changes the {g|Encyclopedia:Alignment}alignment{/g} back.");
            bp.AddComponent<ForbidSpellbookOnAlignmentDeviation>(c =>
            {
                c.m_Spellbooks = [
                    SpellbookRef
                ];
                c.Alignment = Kingmaker.UnitLogic.Alignments.AlignmentMaskType.LawfulEvil;
            });
            bp.IsClassFeature = true;
        }).ToReference<BlueprintFeatureReference>();
    }
}
