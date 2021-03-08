using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse;

namespace GrowingZonePriorities
{
    [StaticConstructorOnStartup]
    internal class Command_GrowingPriority : Command
    {
        private static readonly string[] priorityNames;
        private readonly Action<int> onChanged;

        private int currentValue;

        static Command_GrowingPriority()
        {
            priorityNames = Enum.GetNames(typeof(Priority));
        }

        public Command_GrowingPriority(Priority currentValue)
        {
            var growingZonePriorities = PriorityTracker.growingZonePriorities;
            var plantBuildingPriorities = PriorityTracker.plantBuildingPriorities;
            var selectedGrowingZones = new List<Zone_Growing>();
            var selectedPlantGrowers = new List<Building_PlantGrower>();
            icon = TexCommand.ForbidOff;
            foreach (var obj in Find.Selector.SelectedObjects)
            {
                switch (obj)
                {
                    case Zone_Growing zone:
                    {
                        selectedGrowingZones.Add(zone);
                        break;
                    }
                    case Building_PlantGrower building:
                    {
                        selectedPlantGrowers.Add(building);
                        break;
                    }
                }
            }

            if (selectedPlantGrowers.Count > 0)
            {
                if (selectedPlantGrowers.Count > 1)
                {
                    defaultLabel = "Set priorities";
                    defaultDesc = "Set priorities for these buildings.";
                }
                else
                {
                    defaultLabel = $"Priority {currentValue}";
                    defaultDesc = $"Set this buildings priority. Current priority = {currentValue}";
                }

                onChanged = i =>
                {
                    foreach (var selectedPlantGrower in selectedPlantGrowers)
                    {
                        if (plantBuildingPriorities.ContainsKey(selectedPlantGrower))
                        {
                            plantBuildingPriorities[selectedPlantGrower].Int = i;
                        }
                        else
                        {
                            plantBuildingPriorities[selectedPlantGrower] = new PriorityIntHolder(i);
                        }
                    }
                };
                return;
            }

            if (selectedGrowingZones.Count <= 0)
            {
                return;
            }

            if (selectedGrowingZones.Count > 1)
            {
                defaultLabel = "Set priorities";
                defaultDesc = "Set priorities for these zones.";
            }
            else
            {
                defaultLabel = $"Priority {currentValue}";
                defaultDesc = $"Set this growing zone's priority. Current priority = {currentValue}";
            }

            onChanged = i =>
            {
                foreach (var selectedGrowingZone in selectedGrowingZones)
                {
                    if (growingZonePriorities.ContainsKey(selectedGrowingZone))
                    {
                        growingZonePriorities[selectedGrowingZone].Int = i;
                    }
                    else
                    {
                        growingZonePriorities[selectedGrowingZone] = new PriorityIntHolder(i);
                    }
                }
            };
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);

            string textGetter(int x)
            {
                return x >= 1 && x <= priorityNames.Length ? priorityNames[x - 1] : "???";
            }

            var groups = (List<List<Gizmo>>) typeof(GizmoGridDrawer)
                .GetField("gizmoGroups", BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null);
            var groupedWithSelf = groups.FirstOrFallback(group => group.Contains(this));
            var growingZonePriorities = PriorityTracker.growingZonePriorities;
            var plantBuildingPriorities = PriorityTracker.plantBuildingPriorities;
            var foundValue = -2;
            foreach (var obj in Find.Selector.SelectedObjects)
            {
                if (obj is Zone_Growing zone && growingZonePriorities.ContainsKey(zone))
                {
                    if (foundValue != -2)
                    {
                        foundValue = -1;
                        break;
                    }

                    foundValue = growingZonePriorities[zone].Int;
                }

                // ReSharper disable once InvertIf, Nicer
                if (obj is Building_PlantGrower building && plantBuildingPriorities.ContainsKey(building))
                {
                    if (foundValue != -2)
                    {
                        foundValue = -1;
                        break;
                    }

                    foundValue = plantBuildingPriorities[building].Int;
                }
            }

            if (foundValue == -2)
            {
                foundValue = 2;
            }

            var window = new Dialog_Slider(textGetter, (int) Priority.Low, (int) Priority.Critical, delegate(int value)
            {
                currentValue = value;
                onChanged?.Invoke(value);

                if (groupedWithSelf == null)
                {
                    return;
                }

                foreach (var gizmo in groupedWithSelf)
                {
                    if (gizmo != null && gizmo != this && !gizmo.disabled && gizmo.InheritInteractionsFrom(this))
                    {
                        gizmo.ProcessInput(ev);
                    }
                }
            }, foundValue);

            Find.WindowStack.Add(window);
        }

        public override bool InheritInteractionsFrom(Gizmo other)
        {
            if (!(other is Command_GrowingPriority otherC))
            {
                return false;
            }

            currentValue = otherC.currentValue;
            onChanged?.Invoke(currentValue);
            return false;
        }
    }
}