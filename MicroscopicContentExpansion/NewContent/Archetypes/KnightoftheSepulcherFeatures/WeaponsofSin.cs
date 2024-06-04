using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Archetypes.KnightoftheSepulcherFeatures;
internal class WeaponsofSin
{
    private const string NAME = "Weapons of Sin";
    private const string DESCRIPTION = "At 14th level, a knight of the sepulcher’s" +
        " weapons are treated as evil-aligned for the purposes of overcoming damage reduction.";

    public static BlueprintFeatureReference AddWeaponsofSin()
    {
        return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "KnightoftheSepulcherWeaponsofSin", bp =>
        {
            bp.SetName(MCEContext, NAME);
            bp.SetDescription(MCEContext, DESCRIPTION);
            bp.IsClassFeature = true;
            bp.AddComponent<AddOutgoingPhysicalDamageProperty>(c =>
            {
                c.Material = Kingmaker.Enums.Damage.PhysicalDamageMaterial.Adamantite;
                c.AddAlignment = true;
                c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Evil;
                c.Reality = Kingmaker.Enums.Damage.DamageRealityType.Ghost;
            });
        }).ToReference<BlueprintFeatureReference>();
    }
}
