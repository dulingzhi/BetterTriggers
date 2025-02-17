﻿using BetterTriggers.Models.SaveableData;
using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Components.TriggerEditor.ParameterControls
{
    public interface IValueControl
    {
        Parameter GetSelected();
        event EventHandler SelectionChanged;
        event EventHandler OK;
        int GetElementCount();
        void SetDefaultSelection(Parameter parameter);
    }
}
