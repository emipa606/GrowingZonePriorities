using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace GrowingZonePriorities;

[HarmonyPatch(typeof(WorkGiver_Scanner), nameof(WorkGiver_Scanner.GetPriority), typeof(Pawn), typeof(TargetInfo))]
public static class WorkGiver_Scanner_GetPriority
{
    private static List<Building> plantGrowers;
    private static int lastUpdateTick;

    private static List<Building> getPlantGrowers(Pawn pawn)
    {
        if (plantGrowers == null)
        {
            plantGrowers = (from Building grower in pawn.Map.listerBuildings.allBuildingsColonist
                where grower is Building_PlantGrower
                select grower).ToList();
            lastUpdateTick = GenTicks.TicksAbs;
            return plantGrowers;
        }

        if (lastUpdateTick < GenTicks.TicksAbs - GenTicks.TickLongInterval)
        {
            plantGrowers = (from Building grower in pawn.Map.listerBuildings.allBuildingsColonist
                where grower is Building_PlantGrower
                select grower).ToList();
        }

        return plantGrowers;
    }

    public static void Postfix(Pawn pawn, TargetInfo t, ref float __result, WorkGiver_Scanner __instance)
    {
        if (!HarmonyPatches.IsPlantZoneScanner(__instance.GetType()))
        {
            return;
        }


        var cell = t.Cell;
        var zone = pawn.Map.zoneManager.AllZones.FirstOrFallback(x =>
            x is IPlantToGrowSettable && x.cells.Contains(cell));
        var growZone = (IPlantToGrowSettable)zone;

        if (zone != null)
        {
            __result = PriorityTracker.growingZonePriorities.TryGetValue(growZone, out var intp)
                ? intp.Int
                : (int)Priority.Normal;
            return;
        }

        foreach (var plantGrower in getPlantGrowers(pawn))
        {
            if (!plantGrower.OccupiedRect().Contains(cell))
            {
                continue;
            }

            __result = PriorityTracker.plantBuildingPriorities.TryGetValue((Building_PlantGrower)plantGrower,
                out var intp)
                ? intp.Int
                : (int)Priority.Normal;
            return;
        }
    }
}