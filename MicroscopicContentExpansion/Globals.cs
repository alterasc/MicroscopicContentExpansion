using Kingmaker.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion;
internal static class Globals
{
    internal static T GetBP<T>(string id) where T : SimpleBlueprint
    {
        return BlueprintTools.GetBlueprint<T>(id);
    }

    internal static T TryGetBP<T>(string id) where T : SimpleBlueprint
    {
        var parsed = BlueprintGuid.Parse(id);
        T obj = ResourcesLibrary.TryGetBlueprint(parsed) as T;
        return obj;
    }

    internal static T GetBPRef<T>(string id) where T : BlueprintReferenceBase
    {
        return BlueprintTools.GetBlueprintReference<T>(id);
    }

    public static void ApplyForAll<T>(string[] ids, System.Action<T> action) where T : SimpleBlueprint
    {
        foreach (var id in ids)
        {
            var bp = GetBP<T>(id);
            action.Invoke(bp);
        }
    }
}
