using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;


namespace MicroscopicContentExpansion.NewContent.Archetypes.KnightoftheSepulcherFeatures {
    internal class SouloftheCrypt {

        public static BlueprintFeatureReference AddSouloftheCrypt() {
            return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "KnightoftheSepulcherSouloftheCrypt", bp => {
                bp.SetName(MCEContext, "Soul of the Crypt");
                bp.SetDescription(MCEContext, "At 17th level, a knight of the sepulcher gains DR 5/bludgeoning and good.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Material = Kingmaker.Enums.Damage.PhysicalDamageMaterial.Adamantite;
                    c.BypassedByAlignment = true;
                    c.BypassedByForm = true;
                    c.Form = Kingmaker.Enums.Damage.PhysicalDamageForm.Bludgeoning;
                    c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Good;
                    c.Reality = Kingmaker.Enums.Damage.DamageRealityType.Ghost;
                    c.Value = 5;
                    c.Pool = 12;
                });
            }).ToReference<BlueprintFeatureReference>();
        }
    }
}
