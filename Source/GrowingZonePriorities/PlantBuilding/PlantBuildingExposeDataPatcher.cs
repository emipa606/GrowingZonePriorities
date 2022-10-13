﻿using HarmonyLib;
using RimWorld;
using Verse;

namespace GrowingZonePriorities;

[HarmonyPatch(typeof(Building_PlantGrower), "ExposeData", null)]
public static class PlantBuildingExposeDataPatcher
{
    public static void Postfix(Building_PlantGrower __instance)
    {
        if (!PriorityTracker.plantBuildingPriorities.ContainsKey(__instance))
        {
            PriorityTracker.plantBuildingPriorities[__instance] = new PriorityIntHolder((int)Priority.Normal);
        }


        Scribe_Values.Look(ref PriorityTracker.plantBuildingPriorities[__instance].Int, "growingPriority",
            (int)Priority.Normal);
    }
}