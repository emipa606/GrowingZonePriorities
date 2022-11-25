using HarmonyLib;
using RimWorld;

namespace GrowingZonePriorities;

[HarmonyPatch(typeof(WorkGiver_Scanner), "get_Prioritized", null)]
public static class GetIsPrioritizedPatcher
{
    public static bool Prefix(ref bool __result, ref WorkGiver_Scanner __instance)
    {
        if (__instance is not WorkGiver_Grower)
        {
            return true;
        }

        __result = true;
        return false;
    }
}