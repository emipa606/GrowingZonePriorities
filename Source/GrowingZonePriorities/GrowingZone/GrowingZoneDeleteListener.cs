using HarmonyLib;
using RimWorld;
using Verse;

namespace GrowingZonePriorities
{
    [HarmonyPatch(typeof(Zone), "Deregister", null)]
    public static class GrowingZoneDeleteListener
    {
        public static void Postfix(Zone __instance)
        {
            if (__instance is Zone_Growing growingZone)
            {
                PriorityTracker.growingZonePriorities.Remove(growingZone);
            }
        }
    }
}