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
                    defaultLabel = $"Priority {currentValue}";
                    defaultDesc = $"Set all buildings priorities. Current priority = {currentValue}";
                    icon = PriorityTexLib.priorityTexs[(int)currentValue - 1];
                }
                else
                {
                    defaultLabel = "Set priorities";
                    defaultDesc = "Set priorities for these buildings.";
                    icon = PriorityTexLib.priorityTexs[5];
                }
            }
            else
            {
                defaultLabel = $"Priority {currentValue}";
                defaultDesc = $"Set this buildings priority. Current priority = {currentValue}";
                icon = PriorityTexLib.priorityTexs[(int)currentValue - 1];
                //activateSound = SoundDef.Named("Click");
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
                    defaultLabel = $"Priority {currentValue}";
                    defaultDesc = $"Set all growing zone's priorities. Current priority = {currentValue}";
                    icon = PriorityTexLib.priorityTexs[(int)currentValue - 1];
                }
                else
                {
                    defaultLabel = "Mixed priorities";
                    defaultDesc = "Set priorities for these zones.";
                    icon = PriorityTexLib.priorityTexs[5];
                }

                break;
            default:
                defaultLabel = $"Priority {currentValue}";
                defaultDesc = $"Set this growing zone's priority. Current priority = {currentValue}";
                icon = PriorityTexLib.priorityTexs[(int)currentValue - 1];
                break;
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

        //string textGetter(int x)
        //{
        //    return x >= 1 && x <= priorityNames.Length ? priorityNames[x - 1] : "???";
        //}


        var groups = (List<List<Gizmo>>)typeof(GizmoGridDrawer)
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

        if (Find.Selector.SelectedObjects.Count > 1)
        {
            //if (Event.current.button == 1)
            //{
            currentValue = 2;
            ApplyGZPSettings();
            DrawGZPFloatMenu();
            //}
            //else
            //{
            //    currentValue = 2;
            //    ApplyGZPSettings();
            //}
            //var window = new Dialog_Slider(textGetter, (int)Priority.Low, (int)Priority.Critical, delegate (int value)
            //{
            //    currentValue = value;
            //    onChanged?.Invoke(value);

            //    if (groupedWithSelf == null)
            //    {
            //        return;
            //    }

            //    foreach (var gizmo in groupedWithSelf)
            //    {
            //        if (gizmo != null && gizmo != this && !gizmo.disabled && gizmo.InheritInteractionsFrom(this))
            //        {
            //            gizmo.ProcessInput(ev);
            //        }
            //    }
            //}, 2);

            //Find.WindowStack.Add(window);
        }
        else
        {
            currentValue = foundValue;

            DrawGZPFloatMenu();
            //if (Event.current.button == 1)
            //{
            //    if (currentValue > 1)
            //    {
            //        currentValue--;
            //    }
            //}

            //else
            //{
            //    if (currentValue < 5)
            //    {
            //        currentValue++;
            //    }
            //}

            //onChanged?.Invoke(currentValue);
        }

        void DrawGZPFloatMenu()
        {
            var options = new List<FloatMenuOption>();
            //for (int i = 0; i < priorityNames.Length; i++)
            //{
            options.Add(new FloatMenuOption(priorityNames[0], () =>
            {
                currentValue = 1;
                ApplyGZPSettings();
            }));
            options.Add(new FloatMenuOption(priorityNames[1], () =>
            {
                currentValue = 2;
                ApplyGZPSettings();
            }));
            options.Add(new FloatMenuOption(priorityNames[2], () =>
            {
                currentValue = 3;
                ApplyGZPSettings();
            }));
            options.Add(new FloatMenuOption(priorityNames[3], () =>
            {
                currentValue = 4;
                ApplyGZPSettings();
            }));
            options.Add(new FloatMenuOption(priorityNames[4], () =>
            {
                currentValue = 5;
                ApplyGZPSettings();
            }));
            //}

            Find.WindowStack.Add(new FloatMenu(options));
        }

        void ApplyGZPSettings()
        {
            onChanged?.Invoke(currentValue);
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
        }
        //if (groupedWithSelf == null)
        //{
        //    return;
        //}

        //foreach (var gizmo in groupedWithSelf)
        //{
        //    if (gizmo != null && gizmo != this && !gizmo.disabled && gizmo.InheritInteractionsFrom(this))
        //    {
        //        gizmo.ProcessInput(ev);
        //    }
        //}
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