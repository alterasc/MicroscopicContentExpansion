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
    internal class UnholyChampion {
        private const string NAME = "Unholy Champion";
        private const string DESCRIPTION = "At 20th level, an antipaladin becomes a conduit for the might of the dark " +
            "powers. His DR increases to 10/good. In addition, whenever he channels negative energy or uses touch of " +
            "corruption to damage a creature, he deals the maximum possible amount.";

        public static void AddUnholyChampion() {

            var AntipaladinClassRef = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(MCEContext, "AntipaladinClass");

            var ChannelNegativeEnergyAbility = BlueprintTools.GetModBlueprint<BlueprintAbility>(MCEContext, "AntipaladinChannelNegativeEnergyAbility");
            var TouchOfCorruptionAbility = BlueprintTools.GetModBlueprint<BlueprintAbility>(MCEContext, "AntipaladinTouchOfCorruptionAbility");

            var ProfaneChampion = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinUnholyChampion", bp => {
                bp.SetName(MCEContext, NAME);
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Material = Kingmaker.Enums.Damage.PhysicalDamageMaterial.Adamantite;
                    c.BypassedByAlignment = true;
                    c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Good;
                    c.Reality = Kingmaker.Enums.Damage.DamageRealityType.Ghost;
                    c.Value = 10;
                    c.Pool = 12;
                });
                bp.AddComponent<AutoMetamagic>(c => {
                    c.m_AllowedAbilities = AutoMetamagic.AllowedType.Any;
                    c.Metamagic = Kingmaker.UnitLogic.Abilities.Metamagic.Maximize;
                    c.Abilities = new List<BlueprintAbilityReference> {
                        ChannelNegativeEnergyAbility.ToReference<BlueprintAbilityReference>(),
                        TouchOfCorruptionAbility.ToReference<BlueprintAbilityReference>()
                    };
                });
            });
        }
    }
}
