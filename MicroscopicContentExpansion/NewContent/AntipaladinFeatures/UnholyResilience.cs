using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Base.Main;

namespace MicroscopicContentExpansion.Base.NewContent.AntipaladinFeatures {
    internal class UnholyResilience {
        public static void AddUnholyResilience() {
            var UnholyResilience = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "AntipaladinUnholyResilience", bp => {
                bp.SetName(MCEContext, "Unholy Resilience");
                bp.SetDescription(MCEContext, "At 2nd level, an antipaladin gains a bonus equal to his Charisma bonus (if any) on all saving throws.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<DerivativeStatBonus>(c => {
                    c.BaseStat = StatType.Charisma;
                    c.DerivativeStat = StatType.SaveFortitude;
                });
                bp.AddComponent<DerivativeStatBonus>(c => {
                    c.BaseStat = StatType.Charisma;
                    c.DerivativeStat = StatType.SaveWill;
                });
                bp.AddComponent<DerivativeStatBonus>(c => {
                    c.BaseStat = StatType.Charisma;
                    c.DerivativeStat = StatType.SaveReflex;
                });
            });
        }
    }
}
