using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;


namespace MicroscopicContentExpansion.NewContent.Archetypes.KnightoftheSepulcherFeatures;
internal class CloakoftheCrypt
{

    public static BlueprintFeatureReference AddCloakoftheCrypt()
    {
        return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "KnightoftheSepulcherCloakoftheCrypt", bp =>
        {
            bp.SetName(MCEContext, "Cloak of the Crypt");
            bp.SetDescription(MCEContext, "At 10th level, the knight of the sepulcher gains immunity to energy drain " +
                "and harmful negative energy effects. His chance of ignoring critical hits and sneak attacks increases" +
                " to 50%, as though he were wearing armor of moderate fortification.");
            bp.AddComponent<AddImmunityToEnergyDrain>();
            bp.AddComponent<AddFortification>(c =>
            {
                c.Bonus = 50;
            });
            bp.IsClassFeature = true;
        }).ToReference<BlueprintFeatureReference>();
    }
}
