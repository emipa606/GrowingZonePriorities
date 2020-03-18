﻿using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace GrowingZonePriorities
{
    [HarmonyPatch(typeof(WorkGiver_Scanner), "GetPriority", new Type[] { typeof(Pawn), typeof(TargetInfo) })]
    public static class GetPriorityPatcher
    {
		public static void Postfix(Pawn pawn, TargetInfo t, ref float __result, WorkGiver_Scanner __instance)
		{
            if (__instance is WorkGiver_Grower)
            {
                IntVec3 cell = t.Cell;
                var zone = (Zone_Growing)pawn.Map.zoneManager.AllZones.FirstOrFallback(x => x is Zone_Growing growZone && growZone.cells.Contains(cell));

                if (zone != null)
                {
                    __result = zone != null && PriorityTracker.growingZonePriorities.TryGetValue(zone, out PriorityIntHolder intp) ? intp.Int : (int)Priority.Normal;
                }
                else
                {
                    foreach (Building b in pawn.Map.listerBuildings.allBuildingsColonist)
                    {
                        if (b is Building_PlantGrower building)
                        {
                            var cri = building.OccupiedRect().GetIterator();
                            while (!cri.Done())
                            {
                                Priority priority = PriorityTracker.plantBuildingPriorities.TryGetValue(building, out PriorityIntHolder intp) ? (Priority)intp.Int : Priority.Normal;

                                __result = (float)priority;

                                cri.MoveNext();
                            }
                        }
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
			if (__instance is WorkGiver_Grower)
			{
				__result = true;
				return false;
			}

			else return true;
		}
	}
	
}
