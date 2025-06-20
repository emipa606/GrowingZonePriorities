using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace GrowingZonePriorities;

[HarmonyPatch(typeof(Zone_Growing), nameof(Zone_Growing.GetGizmos), null)]
public static class Zone_Growing_GetGizmos
{
    public static void Postfix(ref IEnumerable<Gizmo> __result, Zone_Growing __instance)
    {
        var priority = PriorityTracker.growingZonePriorities.TryGetValue(__instance, out var intp)
            ? (Priority)intp.Int
            : Priority.Normal;

        __result = __result.Append(new Command_GrowingPriority(priority));
    }
}