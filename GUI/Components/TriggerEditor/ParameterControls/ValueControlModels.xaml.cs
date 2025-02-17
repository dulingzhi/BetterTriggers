﻿using BetterTriggers.Controllers;
using BetterTriggers.Models.SaveableData;
using BetterTriggers.Models.War3Data;
using BetterTriggers.Utility;
using BetterTriggers.WorldEdit;
using GUI.Components.Shared;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GUI.Components.TriggerEditor.ParameterControls
{
    public partial class ValueControlModels : UserControl, IValueControl
    {
        public event EventHandler SelectionChanged;
        public event EventHandler OK;


        public ValueControlModels()
        {
            InitializeComponent();

            var models = ControllerMapData.GetModelData();
            List<Searchable> objects = new List<Searchable>();
            for (int i = 0; i < models.Count; i++)
            {
                ListViewItem listItem = new ListViewItem();
                listItem.Content = $"{models[i].DisplayName}";
                listItem.Tag = models[i];
                objects.Add(new Searchable()
                {
                    Object = listItem,
                    Category = models[i].Category,
                    Words = new List<string>()
                    {
                        models[i].DisplayName.ToLower(),
                        models[i].Path.ToLower()
                    },
                });
            }

            var searchables = new Searchables(objects);
            listControl.SetSearchableList(searchables);

            var categoryControl = new GenericCategoryControl(searchables);
            grid.Children.Add(categoryControl);
            Grid.SetRow(categoryControl, 0);
            Grid.SetRowSpan(categoryControl, 2);

            listControl.listView.SelectionChanged += ListView_SelectionChanged;
            listControl.listView.MouseDoubleClick += ListView_MouseDoubleClick;
        }


        public void SetDefaultSelection(Parameter parameter)
        {
            textBoxAsset.Text = parameter.value;
        }

        public int GetElementCount()
        {
            return listControl.listView.Items.Count;
        }

        public Parameter GetSelected()
        {
            return  new Value()
            {
                value = textBoxAsset.Text,
            };
        }

        public void SetVisibility(Visibility visibility)
        {
            this.Visibility = visibility;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = listControl.listView.SelectedItem as ListViewItem;
            if (selectedItem == null)
                return;

            var model = (AssetModel)selectedItem.Tag;
            textBoxAsset.Text = model.Path;

            EventHandler handler = SelectionChanged;
            handler?.Invoke(this, e);
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EventHandler handler = OK;
            handler?.Invoke(this, e);
        }
    }
}
