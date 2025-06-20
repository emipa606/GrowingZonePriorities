using HarmonyLib;
using RimWorld;

namespace GrowingZonePriorities.PlantBuilding;

[HarmonyPatch(typeof(Building_PlantGrower), nameof(Building_PlantGrower.DeSpawn), null)]
public static class Building_PlantGrower_DeSpawn
{
    public static void Postfix(Building_PlantGrower __instance)
    {
        PriorityTracker.plantBuildingPriorities.Remove(__instance);
    }
}