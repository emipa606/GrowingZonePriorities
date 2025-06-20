using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse;

namespace GrowingZonePriorities;

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
        activateSound = SoundDef.Named("Click");

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
                var isTheSame = true;
                foreach (var selectedPlantGrower in selectedPlantGrowers)
                {
                    if ((int)currentValue == 2)
                    {
                        if (plantBuildingPriorities.ContainsKey(selectedPlantGrower) &&
                            plantBuildingPriorities[selectedPlantGrower].Int != (int)currentValue)
                        {
                            isTheSame = false;
                            break;
                        }

                        continue;
                    }

                    if (plantBuildingPriorities.ContainsKey(selectedPlantGrower) &&
                        plantBuildingPriorities[selectedPlantGrower].Int == (int)currentValue)
                    {
                        continue;
                    }

                    isTheSame = false;
                    break;
                }

                if (isTheSame)
                {
                    defaultLabel = "GZP.Priority".Translate(currentValue.ToString());
                    defaultDesc = "GZP.AllBuildings".Translate(currentValue.ToString());
                    icon = PriorityTexLib.PriorityTextures[(int)currentValue - 1];
                }
                else
                {
                    defaultLabel = "GZP.SetPriorities".Translate();
                    defaultDesc = "GZP.SetPrioritiesTT".Translate();
                    icon = PriorityTexLib.PriorityTextures[5];
                }
            }
            else
            {
                defaultLabel = "GZP.Priority".Translate(currentValue.ToString());
                defaultDesc = "GZP.ThisBuilding".Translate(currentValue.ToString());
                icon = PriorityTexLib.PriorityTextures[(int)currentValue - 1];
            }

            onChanged = i =>
            {
                foreach (var selectedPlantGrower in selectedPlantGrowers)
                {
                    if (plantBuildingPriorities.TryGetValue(selectedPlantGrower, out var priority))
                    {
                        priority.Int = i;
                    }
                    else
                    {
                        plantBuildingPriorities[selectedPlantGrower] = new PriorityIntHolder(i);
                    }
                }
            };
            return;
        }

        switch (selectedGrowingZones.Count)
        {
            case <= 0:
                return;
            case > 1:
                var isTheSame = true;
                foreach (var selectedGrowingZone in selectedGrowingZones)
                {
                    if ((int)currentValue == 2)
                    {
                        if (growingZonePriorities.ContainsKey(selectedGrowingZone) &&
                            growingZonePriorities[selectedGrowingZone].Int != (int)currentValue)
                        {
                            isTheSame = false;
                            break;
                        }

                        continue;
                    }

                    if (growingZonePriorities.ContainsKey(selectedGrowingZone) &&
                        growingZonePriorities[selectedGrowingZone].Int == (int)currentValue)
                    {
                        continue;
                    }

                    isTheSame = false;
                    break;
                }

                if (isTheSame)
                {
                    defaultLabel = "GZP.Priority".Translate(currentValue.ToString());
                    defaultDesc = "GZP.AllZones".Translate(currentValue.ToString());
                    icon = PriorityTexLib.PriorityTextures[(int)currentValue - 1];
                }
                else
                {
                    defaultLabel = "GZP.MixedPriorities".Translate();
                    defaultDesc = "GZP.SetPrioritiesZonesTT".Translate();
                    icon = PriorityTexLib.PriorityTextures[5];
                }

                break;
            default:
                defaultLabel = "GZP.Priority".Translate(currentValue.ToString());
                defaultDesc = "GZP.ThisZone".Translate(currentValue.ToString());
                icon = PriorityTexLib.PriorityTextures[(int)currentValue - 1];
                break;
        }

        onChanged = i =>
        {
            foreach (var selectedGrowingZone in selectedGrowingZones)
            {
                if (growingZonePriorities.TryGetValue(selectedGrowingZone, out var priority))
                {
                    priority.Int = i;
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

        var groups = (List<List<Gizmo>>)typeof(GizmoGridDrawer)
            .GetField("gizmoGroups", BindingFlags.Static | BindingFlags.NonPublic)
            ?.GetValue(null);
        var groupedWithSelf = groups.FirstOrFallback(group => group.Contains(this));
        var growingZonePriorities = PriorityTracker.growingZonePriorities;
        var plantBuildingPriorities = PriorityTracker.plantBuildingPriorities;
        var foundValue = -2;
        foreach (var obj in Find.Selector.SelectedObjects)
        {
            if (obj is Zone_Growing zone && growingZonePriorities.TryGetValue(zone, out var priority))
            {
                if (foundValue != -2)
                {
                    foundValue = -1;
                    break;
                }

                foundValue = priority.Int;
            }

            // ReSharper disable once InvertIf, Nicer
            if (obj is Building_PlantGrower building &&
                plantBuildingPriorities.TryGetValue(building, out var buildingPriority))
            {
                if (foundValue != -2)
                {
                    foundValue = -1;
                    break;
                }

                foundValue = buildingPriority.Int;
            }
        }

        if (foundValue == -2)
        {
            foundValue = 2;
        }

        if (Find.Selector.SelectedObjects.Count > 1)
        {
            currentValue = 2;
            applyGzpSettings();
        }
        else
        {
            currentValue = foundValue;
        }

        DrawGZPFloatMenu();

        return;

        void DrawGZPFloatMenu()
        {
            var options = new List<FloatMenuOption>
            {
                new(priorityNames[0], () =>
                {
                    currentValue = 1;
                    applyGzpSettings();
                }),
                new(priorityNames[1], () =>
                {
                    currentValue = 2;
                    applyGzpSettings();
                }),
                new(priorityNames[2], () =>
                {
                    currentValue = 3;
                    applyGzpSettings();
                }),
                new(priorityNames[3], () =>
                {
                    currentValue = 4;
                    applyGzpSettings();
                }),
                new(priorityNames[4], () =>
                {
                    currentValue = 5;
                    applyGzpSettings();
                })
            };

            Find.WindowStack.Add(new FloatMenu(options));
        }

        void applyGzpSettings()
        {
            onChanged?.Invoke(currentValue);
            if (groupedWithSelf == null)
            {
                return;
            }

            foreach (var gizmo in groupedWithSelf)
            {
                if (gizmo != null && gizmo != this && !gizmo.Disabled && gizmo.InheritInteractionsFrom(this))
                {
                    gizmo.ProcessInput(ev);
                }
            }
        }
    }

    public override bool InheritInteractionsFrom(Gizmo other)
    {
        if (other is not Command_GrowingPriority otherC)
        {
            return false;
        }

        currentValue = otherC.currentValue;
        onChanged?.Invoke(currentValue);
        return false;
    }
}