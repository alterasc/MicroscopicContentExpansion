using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Base.Main;

namespace MicroscopicContentExpansion.Base.NewContent.AntipaladinFeatures {
    internal class PlagueBringer {
        public static void AddPlagueBringer() {
            var PlagueBringer = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinPlagueBringer", bp => {
                bp.SetName(MCEContext, "Plague Bringer");
                bp.SetDescription(MCEContext, "At 3rd level, the powers of darkness make an antipaladin a beacon of " +
                    "corruption and disease. An antipaladin does not take any damage or take any penalty from diseases.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<BuffDescriptorImmunity>(c => { c.Descriptor = SpellDescriptor.Disease; });
            });
        }
    }
}
