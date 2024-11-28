using Kingmaker.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion;
internal static class Globals
{
    internal static T GetBP<T>(string id) where T : SimpleBlueprint
    {
        return BlueprintTools.GetBlueprint<T>(id);
    }

    internal static T GetBPRef<T>(string id) where T : BlueprintReferenceBase
    {
        return BlueprintTools.GetBlueprintReference<T>(id);
    }
}
