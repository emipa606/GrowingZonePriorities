using HarmonyLib;
using RimWorld;
using Verse;

namespace GrowingZonePriorities
{
    [HarmonyPatch(typeof(Zone_Growing), "ExposeData", null)]
    public static class GrowingZoneExposeDataPatcher
    {
		public static void Postfix(Zone_Growing __instance)
		{
			if (!PriorityTracker.growingZonePriorities.ContainsKey(__instance))
				PriorityTracker.growingZonePriorities[__instance] = new PriorityIntHolder((int)Priority.Normal);


			Scribe_Values.Look<int>(ref PriorityTracker.growingZonePriorities[__instance].Int, "growingPriority", (int)Priority.Normal, false);
		}
	}
}
