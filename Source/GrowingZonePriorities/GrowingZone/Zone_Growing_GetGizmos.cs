using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace GrowingZonePriorities;

[HarmonyPatch]
public static class Zone_Growing_GetGizmos
{
    private static IEnumerable<MethodBase> TargetMethods()
    {
        // For each Zone subclass that implements IPlantToGrowSettable and has a GetGizmos method, patch it.
        foreach (var zoneType in typeof(Zone).AllSubclasses())
        {
            if (!typeof(IPlantToGrowSettable).IsAssignableFrom(zoneType))
            {
                continue;
            }

            var method = AccessTools.Method(zoneType, nameof(Zone.GetGizmos));
            if (method != null)
            {
                yield return method;
            }
        }
    }

    public static void Postfix(ref IEnumerable<Gizmo> __result, IPlantToGrowSettable __instance)
    {
        var priority = PriorityTracker.growingZonePriorities.TryGetValue(__instance, out var intp)
            ? (Priority)intp.Int
            : Priority.Normal;

        __result = __result.Append(new Command_GrowingPriority(priority));
    }
}