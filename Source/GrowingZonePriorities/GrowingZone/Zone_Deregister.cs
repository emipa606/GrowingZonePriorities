using HarmonyLib;
using RimWorld;
using Verse;

namespace GrowingZonePriorities;

[HarmonyPatch(typeof(Zone), nameof(Zone.Deregister), null)]
public static class Zone_Deregister
{
    public static void Postfix(Zone __instance)
    {
        if (__instance is Zone_Growing growingZone)
        {
            PriorityTracker.growingZonePriorities.Remove(growingZone);
        }
    }
}