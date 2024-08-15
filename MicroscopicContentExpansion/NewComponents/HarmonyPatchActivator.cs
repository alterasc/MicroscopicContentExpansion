using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;

namespace MicroscopicContentExpansion.NewComponents;

/// <summary>
/// Activates patches under specified type on component initialization
/// </summary>
public class HarmonyPatchActivator : UnitFactComponentDelegate
{
    private static readonly HashSet<Type> ActivatedPatches = [];

    public Type PatchType;

    public override void OnTurnOn()
    {
        base.OnTurnOn();
        if (ActivatedPatches.Contains(PatchType))
        {
            return;
        }
        try
        {
            HarmonyInstance.CreateClassProcessor(PatchType).Patch();
            MCEContext.Logger.Log($"Enabled {PatchType.Name} patches");
        }
        catch (Exception ex)
        {
            MCEContext.Logger.LogError(ex, $"Error when enabling {PatchType.Name} patches: {ex.Message}");
        }
        ActivatedPatches.Add(PatchType);
    }
}
