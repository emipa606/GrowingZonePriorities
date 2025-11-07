using HarmonyLib;
using RimWorld;

namespace GrowingZonePriorities;

[HarmonyPatch(typeof(WorkGiver_Scanner), nameof(WorkGiver_Scanner.Prioritized), MethodType.Getter)]
public static class WorkGiver_Scanner_Prioritized
{
    public static bool Prefix(ref bool __result, ref WorkGiver_Scanner __instance)
    {
        if (!HarmonyPatches.IsPlantZoneScanner(__instance.GetType()))
        {
            return true;
        }

        __result = true;
        return false;
    }
}