using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace GrowingZonePriorities;

[HarmonyPatch(typeof(Building_PlantGrower), nameof(Building_PlantGrower.GetGizmos), null)]
public static class Building_PlantGrower_GetGizmos
{
    public static void Postfix(ref IEnumerable<Gizmo> __result, Building_PlantGrower __instance)
    {
        var priority = PriorityTracker.plantBuildingPriorities.TryGetValue(__instance, out var buildingPriority)
            ? (Priority)buildingPriority.Int
            : Priority.Normal;

        __result = __result.Append(new Command_GrowingPriority(priority));
    }
}