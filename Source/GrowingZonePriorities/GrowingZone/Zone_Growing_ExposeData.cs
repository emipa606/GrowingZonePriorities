using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace GrowingZonePriorities;

[HarmonyPatch]
public static class Zone_Growing_ExposeData
{
    private static IEnumerable<MethodBase> TargetMethods()
    {
        // For each Zone subclass that implements IPlantToGrowSettable and has a ExposeData method, patch it.
        foreach (var zoneType in typeof(Zone).AllSubclasses())
        {
            if (!typeof(IPlantToGrowSettable).IsAssignableFrom(zoneType))
            {
                continue;
            }

            var method = AccessTools.Method(zoneType, nameof(Zone.ExposeData));
            if (method != null)
            {
                yield return method;
            }
        }
    }

    public static void Postfix(IPlantToGrowSettable __instance)
    {
        if (!PriorityTracker.growingZonePriorities.ContainsKey(__instance))
        {
            PriorityTracker.growingZonePriorities[__instance] = new PriorityIntHolder((int)Priority.Normal);
        }

        Scribe_Values.Look(ref PriorityTracker.growingZonePriorities[__instance].Int, "growingPriority",
            (int)Priority.Normal);
    }
}