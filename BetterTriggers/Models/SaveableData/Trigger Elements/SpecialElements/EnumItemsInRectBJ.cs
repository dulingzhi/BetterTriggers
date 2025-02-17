﻿using System.Collections.Generic;

namespace BetterTriggers.Models.SaveableData
{
    public class EnumItemsInRectBJ : ECA
    {
        public readonly int ElementType = 12; // DO NOT CHANGE
        public List<TriggerElement> Actions = new List<TriggerElement>();

        public EnumItemsInRectBJ()
        {
            function.value = "EnumItemsInRectBJMultiple";
        }

        public override EnumItemsInRectBJ Clone()
        {
            EnumItemsInRectBJ enumItems = new EnumItemsInRectBJ();
            enumItems.function = this.function.Clone();
            enumItems.Actions = new List<TriggerElement>();
            Actions.ForEach(element => enumItems.Actions.Add(element.Clone()));

            return enumItems;
        }
    }
}
