using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;


namespace MicroscopicContentExpansion.NewContent.Archetypes.KnightoftheSepulcherFeatures {
    internal class FortitudeoftheCrypt {

        public static BlueprintFeatureReference AddFortitudeoftheCrypt() {
            return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "KnightoftheSepulcherFortitudeoftheCrypt", bp => {
                bp.SetName(MCEContext, "Fortitude of the Crypt");
                bp.SetDescription(MCEContext, "At 8th level, a knight of the sepulcher gains immunity to poison.");
                bp.AddComponent<BuffDescriptorImmunity>(c => {
                    c.Descriptor = SpellDescriptor.Poison;
                });
                bp.IsClassFeature = true;
            }).ToReference<BlueprintFeatureReference>();
        }
    }
}
