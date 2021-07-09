using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace GrowingZonePriorities
{
    [HarmonyPatch(typeof(WorkGiver_Scanner), "GetPriority", typeof(Pawn), typeof(TargetInfo))]
    public static class GetPriorityPatcher
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
            if (!(__instance is WorkGiver_Grower))
            {
                return;
            }

            var cell = t.Cell;
            var zone = (Zone_Growing) pawn.Map.zoneManager.AllZones.FirstOrFallback(x =>
                x is Zone_Growing growZone && growZone.cells.Contains(cell));

            if (zone != null)
            {
                __result = PriorityTracker.growingZonePriorities.TryGetValue(zone, out var intp)
                    ? intp.Int
                    : (int) Priority.Normal;
            }
            else
            {
                foreach (var plantGrower in getPlantGrowers(pawn))
                {
                    foreach (var unused in plantGrower.OccupiedRect())
                    {
                        var priority =
                            PriorityTracker.plantBuildingPriorities.TryGetValue((Building_PlantGrower) plantGrower,
                                out var intp)
                                ? (Priority) intp.Int
                                : Priority.Normal;

                        __result = (float) priority;
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(WorkGiver_Scanner), "get_Prioritized", null)]
    public static class GetIsPrioritizedPatcher
    {
        public static bool Prefix(ref bool __result, ref WorkGiver_Scanner __instance)
        {
            if (!(__instance is WorkGiver_Grower))
            {
                return true;
            }

            __result = true;
            return false;
        }
    }
}