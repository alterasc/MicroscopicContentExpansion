using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Archetypes.KnightoftheSepulcherFeatures;
internal class WilloftheCrypt
{

    public static BlueprintFeatureReference AddWilloftheCrypt()
    {
        //nothing done here as increase is coded in TouchoftheCrypt.cs
        return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "KnightoftheSepulcherWilloftheCrypt", bp =>
        {
            bp.SetName(MCEContext, "Will of the Crypt");
            bp.SetDescription(MCEContext, "At 11th level, a knight of the sepulcher’s bonus on saving" +
                " throws against mind-affecting effects and death effects increases to +4.");
            bp.IsClassFeature = true;
        }).ToReference<BlueprintFeatureReference>();
    }
}
