using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;


namespace MicroscopicContentExpansion.NewContent.Archetypes.KnightoftheSepulcherFeatures {
    internal class CryptLord {

        public static BlueprintFeatureReference AddCryptLord() {
            return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "KnightoftheSepulcherCryptLord", bp => {
                bp.SetName(MCEContext, "Crypt Lord");
                bp.SetDescription(MCEContext, "At 15th level, a knight of the sepulcher’s chance of ignoring critical hits " +
                    "and sneak attacks increases to 75%, as though he were wearing armor of heavy fortification. " +
                    "He gains immunity to death effects, paralysis, sleep effects, and stunning. He no longer sleeps. " +
                    "The knight of the sepulcher also gains immunity to fatigue.");
                bp.AddComponent<AddFortification>(c => {
                    c.Bonus = 75;
                });
                bp.AddComponent<BuffDescriptorImmunity>(c => {
                    c.Descriptor = SpellDescriptor.Sleep | SpellDescriptor.Paralysis | SpellDescriptor.Death | SpellDescriptor.Fatigue | SpellDescriptor.Stun;
                });
                bp.IsClassFeature = true;
            }).ToReference<BlueprintFeatureReference>();
        }
    }
}
