using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;


namespace MicroscopicContentExpansion.NewContent.Archetypes.KnightoftheSepulcherFeatures {
    internal class UndyingChampion {

        public static BlueprintFeatureReference AddUndyingChampion() {
            var undeadType = BlueprintTools.GetBlueprint<BlueprintFeature>("734a29b693e9ec346ba2951b27987e33");

            return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "KnightoftheSepulcherUndyingChampion", bp => {
                bp.SetName(MCEContext, "Undying Champion");
                bp.SetDescription(MCEContext, "At 20th level, a knight of the sepulcher joins the ranks of the " +
                    "undead. His DR increases to 10/bludgeoning and good. His type changes to undead, and he " +
                    "acquires all undead traits. Although immune to disease, he can still carry and spread " +
                    "diseases with the antipaladin’s plague bringer ability. The undying champion no longer has " +
                    "a Constitution score. He uses his Charisma score for calculating hit points, Fortitude saves," +
                    " and any special abilities that rely on Constitution. ");
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { undeadType.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Material = Kingmaker.Enums.Damage.PhysicalDamageMaterial.Adamantite;
                    c.BypassedByAlignment = true;
                    c.BypassedByForm = true;
                    c.Form = Kingmaker.Enums.Damage.PhysicalDamageForm.Bludgeoning;
                    c.Alignment = Kingmaker.Enums.Damage.DamageAlignment.Good;
                    c.Reality = Kingmaker.Enums.Damage.DamageRealityType.Ghost;
                    c.Value = 10;
                    c.Pool = 12;
                });
            }).ToReference<BlueprintFeatureReference>();
        }
    }
}
