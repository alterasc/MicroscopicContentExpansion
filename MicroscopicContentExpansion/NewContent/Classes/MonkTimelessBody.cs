using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Classes;
internal class MonkTimelessBody {
    internal static void AddTimelessBody() {
        var flawlessMind = Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "MonkTimelessBody", a => {
            a.SetName(MCEContext, "Timeless Body");
            a.SetDescription(MCEContext, "At 17th level, a monk no longer takes penalties to his ability scores for aging and cannot be magically aged.");
            a.AddComponent<SpecificBuffImmunity>(c => {
                c.m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("a1aecb0c003a49b9ae385035875f1b92"); // DLC3_HasteIslandStacks
            });
            a.AddComponent<SpecificBuffImmunity>(c => {
                c.m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("6ed8dfef694e4189bbe2fea3e8e70216"); // BythosAgeBuff3
            });
            a.AddComponent<SpecificBuffImmunity>(c => {
                c.m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("280882479c3e4e6c8c287237d6269f65"); // BythosAgeBuff2
            });
            a.AddComponent<SpecificBuffImmunity>(c => {
                c.m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e7227f04b30d4eba9c186a7be747d5bf"); // BythosAgeBuff1
            });
        });
        var monkProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8a91753b978e3b34b9425419179aafd6");
        var lvlEntries = monkProgression.LevelEntries;
        var lvl17Entry = lvlEntries.FirstOrDefault(x => x.Level == 17);
        if (lvl17Entry != null) {
            lvl17Entry.m_Features.Add(flawlessMind.ToReference<BlueprintFeatureBaseReference>());
        }
    }
}
