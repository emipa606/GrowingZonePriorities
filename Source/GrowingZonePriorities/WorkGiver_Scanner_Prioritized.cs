using HarmonyLib;
using RimWorld;

namespace GrowingZonePriorities;

[HarmonyPatch(typeof(WorkGiver_Scanner), nameof(WorkGiver_Scanner.Prioritized), MethodType.Getter)]
public static class WorkGiver_Scanner_Prioritized
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