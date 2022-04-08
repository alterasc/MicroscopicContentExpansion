using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Base.Main;

namespace MicroscopicContentExpansion.Base.NewContent.AntipaladinFeatures {
    internal class Cruelties {
        public static void AddCruelties() {
            var TouchOfCorruptionFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinTouchOfCorruptionFeature");
            var FatigueIcon = BlueprintTools.GetBlueprint<BlueprintBuff>("e6f2fc5d73d88064583cb828801212f4");

        }
    }
}
