﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.AntipaladinFeatures {
    internal class TipoftheSpear {
        private const string NAME = "Tip of the Spear";
        private const string DESCRIPTION = "At 20th level, the antipaladin tears through heroes and " +
            "rival villains alike. The antipaladin gains three additional uses of smite good per day" +
            " and can smite foes regardless of their alignment.";

        public static void AddTipoftheSpear() {
            var AntipaladinClassRef = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");
            var SmiteGoodResource = MCEContext.GetModBlueprintReference<BlueprintAbilityResourceReference>("AntipaladinSmiteGoodResource");

            var TipoftheSpear = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinTipoftheSpear", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.IsClassFeature = true;
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = SmiteGoodResource;
                    c.Value = 3;
                });
            });
        }
    }
}
