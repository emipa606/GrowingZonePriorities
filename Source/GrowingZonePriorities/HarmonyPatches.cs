using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace GrowingZonePriorities;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    // Cache to avoid repeating reflection on every call.
    private static readonly HashSet<Type> plantScannerTypes = [];
    private static readonly HashSet<Type> nonPlantScannerTypes = [];

    static HarmonyPatches()
    {
        new Harmony("Ilyaki.GrowingZonePriorities").PatchAll(Assembly.GetExecutingAssembly());
    }

    public static bool IsPlantZoneScanner(Type type)
    {
        if (plantScannerTypes.Contains(type))
        {
            return true;
        }

        if (nonPlantScannerTypes.Contains(type))
        {
            return false;
        }

        // Look for a public static method with exact signature:
        // ThingDef CalculateWantedPlantDef(IntVec3 c, Map map)
        var method = type.GetMethod(
            "CalculateWantedPlantDef",
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        var match = false;
        if (method != null &&
            method.ReturnType == typeof(ThingDef))
        {
            var p = method.GetParameters();
            match = p.Length == 2 &&
                    p[0].ParameterType == typeof(IntVec3) &&
                    p[1].ParameterType == typeof(Map);
        }

        if (match)
        {
            plantScannerTypes.Add(type);
        }
        else
        {
            nonPlantScannerTypes.Add(type);
        }

        return match;
    }
}