﻿using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace GrowingZonePriorities.PlantBuilding
{
    [HarmonyPatch(typeof(Building_PlantGrower), "DeSpawn", null)]
    public static class PlantBuildingDeSpawnListener
    {
		public static void Postfix(Building_PlantGrower __instance)
		{
			PriorityTracker.plantBuildingPriorities.Remove(__instance);
		}
	}
}
