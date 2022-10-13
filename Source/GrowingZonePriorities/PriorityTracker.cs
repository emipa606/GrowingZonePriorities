using System.Collections.Generic;
using RimWorld;

namespace GrowingZonePriorities;

internal class PriorityTracker
{
    public static readonly Dictionary<Zone_Growing, PriorityIntHolder> growingZonePriorities =
        new Dictionary<Zone_Growing, PriorityIntHolder>();

    public static readonly Dictionary<Building_PlantGrower, PriorityIntHolder> plantBuildingPriorities =
        new Dictionary<Building_PlantGrower, PriorityIntHolder>();
}