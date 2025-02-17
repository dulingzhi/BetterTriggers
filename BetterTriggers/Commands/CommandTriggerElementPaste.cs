﻿
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using BetterTriggers.Containers;
using BetterTriggers.Controllers;
using BetterTriggers.Models.EditorData;
using BetterTriggers.Models.SaveableData;

namespace BetterTriggers.Commands
{
    public class CommandTriggerElementPaste : ICommand
    {
        string commandName = "Paste Trigger Element";
        int pastedIndex = 0;
        ExplorerElementTrigger explorerElement;
        List<TriggerElement> listToPaste;
        List<TriggerElement> parent;

        public CommandTriggerElementPaste(ExplorerElementTrigger element, List<TriggerElement> listToPaste, List<TriggerElement> parent, int pastedIndex)
        {
            this.explorerElement = element;
            this.listToPaste = listToPaste;
            this.parent = parent;
            this.pastedIndex = pastedIndex;
        }

        public void Execute()
        {
            ControllerTrigger.RemoveInvalidReferences(explorerElement.trigger, listToPaste);
            for (int i = 0; i < listToPaste.Count; i++)
            {
                listToPaste[i].SetParent(parent, pastedIndex + i);
                listToPaste[i].Created(pastedIndex + i);
            }

            References.UpdateReferences(explorerElement);
            CommandManager.AddCommand(this);
        }

        public void Redo()
        {
            for (int i = 0; i < listToPaste.Count; i++)
            {
                listToPaste[i].SetParent(parent, pastedIndex + i);
                listToPaste[i].Created(pastedIndex + i);
            }


            References.UpdateReferences(explorerElement);
        }

        public void Undo()
        {
            for (int i = 0; i < listToPaste.Count; i++)
            {
                listToPaste[i].RemoveFromParent();
                listToPaste[i].Deleted();
            }

            References.UpdateReferences(explorerElement);
        }

        public string GetCommandName()
        {
            return commandName;
        }
    }
}