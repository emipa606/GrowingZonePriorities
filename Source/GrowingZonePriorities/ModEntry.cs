using HarmonyLib;
using Verse;

namespace GrowingZonePriorities
{
	[StaticConstructorOnStartup]
	class HarmonyPatches : Mod
	{

        public HarmonyPatches(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("Ilyaki.GrowingZonePriorities");
            harmony.PatchAll();
			Log.Message("Growing Zone Priorities loaded");
        }
    }
}
