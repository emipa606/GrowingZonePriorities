using System.Collections.Generic;
using RimWorld;

namespace GrowingZonePriorities;

internal class PriorityTracker
{
    public static readonly Dictionary<IPlantToGrowSettable, PriorityIntHolder> growingZonePriorities = new();

    public static readonly Dictionary<Building_PlantGrower, PriorityIntHolder> plantBuildingPriorities = new();
}