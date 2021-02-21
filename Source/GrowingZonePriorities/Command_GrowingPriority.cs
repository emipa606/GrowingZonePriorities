using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;

namespace GrowingZonePriorities
{
    [StaticConstructorOnStartup]
    internal class Command_GrowingPriority : Command
    {
        private static readonly string[] priorityNames;

        private int currentValue;
        public Action<int> onChanged;

        static Command_GrowingPriority()
        {
            priorityNames = Enum.GetNames(typeof(Priority));
        }

        public Command_GrowingPriority(int currentValue)
        {
            this.currentValue = currentValue;
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
            }, currentValue);

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