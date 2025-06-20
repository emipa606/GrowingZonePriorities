using System.Reflection;
using HarmonyLib;
using Verse;

namespace GrowingZonePriorities;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    static HarmonyPatches()
    {
        new Harmony("Ilyaki.GrowingZonePriorities").PatchAll(Assembly.GetExecutingAssembly());
    }
}