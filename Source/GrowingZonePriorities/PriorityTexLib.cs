using UnityEngine;
using Verse;

namespace GrowingZonePriorities;

[StaticConstructorOnStartup]
public static class PriorityTexLib
{
    public static Texture2D[] priorityTexs =
    {
        ContentFinder<Texture2D>.Get("UI/Commands/PriorityLow"),
        ContentFinder<Texture2D>.Get("UI/Commands/PriorityNormal"),
        ContentFinder<Texture2D>.Get("UI/Commands/PriorityPreferred"),
        ContentFinder<Texture2D>.Get("UI/Commands/PriorityImportant"),
        ContentFinder<Texture2D>.Get("UI/Commands/PriorityCritical"),
        ContentFinder<Texture2D>.Get("UI/Commands/PriorityMulti")
    };
}