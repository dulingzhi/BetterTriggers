﻿using BetterTriggers.Controllers;
using BetterTriggers.Models.SaveableData;
using BetterTriggers.Utility;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GUI.Components.TriggerEditor.ParameterControls
{
    public partial class ParameterVariableControl : UserControl, IParameterControl
    {
        private ListViewItem selectedItem;

        /// <summary>
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="trig"></param>
        public ParameterVariableControl(string returnType, BetterTriggers.Models.SaveableData.Trigger trig)
        {
            InitializeComponent();

            if (returnType == "VarAsString_Real")
                returnType = "real";
            else if (returnType == "StringExt")
                returnType = "string";

            List<VariableRef> variables = ControllerVariable.GetVariableRefs(returnType, trig, ControllerVariable.includeLocals);
            List<Searchable> objects = new List<Searchable>();

            for (int i = 0; i < variables.Count; i++)
            {
                string varName = ControllerVariable.GetVariableNameById(variables[i].VariableId);
                ListViewItem listItem = new ListViewItem();
                listItem.Content = varName;
                listItem.Tag = variables[i];

                objects.Add(new Searchable()
                {
                    Object = listItem,
                    Words = new List<string>()
                    {
                        varName.ToLower()
                    },
                });
            }
            var searchables = new Searchables(objects);
            listControl.SetSearchableList(searchables);
            listControl.listView.SelectionChanged += ListView_SelectionChanged;
        }

        public void SetDefaultSelection(Parameter parameter)
        {
            int i = 0;
            bool found = false;
            Variable selected = null;
            if (parameter is VariableRef)
                selected = ControllerVariable.GetByReference(parameter as VariableRef);

            if (selected == null)
                return;

            while (!found && i < listControl.listView.Items.Count)
            {
                var item = listControl.listView.Items[i] as ListViewItem;
                var variableRef = item.Tag as VariableRef;
                var variable = ControllerVariable.GetByReference(variableRef);
                if (variable == selected)
                    found = true;
                else
                    i++;
            }
            if (!found)
                return;

            var defaultSelected = listControl.listView.Items[i] as ListViewItem;
            defaultSelected.IsSelected = true;
            listControl.listView.ScrollIntoView(defaultSelected);
        }


        public int GetElementCount()
        {
            return listControl.listView.Items.Count;
        }

        public Parameter GetSelectedItem()
        {
            if (selectedItem == null)
                return null;

            var variables = (VariableRef)selectedItem.Tag;
            return variables;
        }

        public void SetVisibility(Visibility visibility)
        {
            this.Visibility = visibility;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = listControl.listView.SelectedItem as ListViewItem;
        }
    }
}
