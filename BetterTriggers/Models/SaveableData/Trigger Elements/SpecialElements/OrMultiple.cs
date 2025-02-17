﻿using System.Collections.Generic;

namespace BetterTriggers.Models.SaveableData
{
    public class OrMultiple : ECA
    {
        public readonly int ElementType = 3; // DO NOT CHANGE
        public List<TriggerElement> Or = new List<TriggerElement>();

        public OrMultiple()
        {
            function.value = "OrMultiple";
        }

        public override OrMultiple Clone()
        {
            OrMultiple andMultiple = new OrMultiple();
            andMultiple.function = this.function.Clone();
            andMultiple.Or = new List<TriggerElement>();
            Or.ForEach(element => andMultiple.Or.Add(element.Clone()));

            return andMultiple;
        }
    }
}
