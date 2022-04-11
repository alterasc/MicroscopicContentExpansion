using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Base.Main;

namespace MicroscopicContentExpansion.Base.NewContent.AntipaladinFeatures {
    internal class TipoftheSpear {
        private const string NAME = "Tip of the Spear";
        private const string DESCRIPTION = "At 20th level, the antipaladin tears through heroes and " +
            "rival villains alike. The antipaladin gains three additional uses of smite good per day" +
            " and can smite foes regardless of their alignment.";

        public static void AddTipoftheSpear() {
            var AntipaladinClassRef = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(MCEContext, "AntipaladinClass");
            var SmiteGoodResource = BlueprintTools.GetModBlueprintReference<BlueprintAbilityResourceReference>(MCEContext, "AntipaladinSmiteGoodResource");

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
