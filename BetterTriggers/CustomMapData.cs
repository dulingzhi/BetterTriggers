﻿using BetterTriggers.Containers;
using BetterTriggers.Controllers;
using BetterTriggers.Models.EditorData;
using BetterTriggers.WorldEdit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using War3Net.Build;
using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.Build.Object;
using War3Net.Build.Widget;
using War3Net.IO.Mpq;

namespace BetterTriggers
{
    public class CustomMapData
    {
        public static string mapPath;
        internal static Map MPQMap;
        private static FileSystemWatcher watcher;
        public static event FileSystemEventHandler OnSaving;

        private static void InvokeOnSaving(object sender, FileSystemEventArgs e)
        {
            if (OnSaving != null)
                OnSaving(sender, e);
        }

        public static void Init(string _mapPath)
        {
            mapPath = _mapPath;
            while (IsMapSaving())
            {
                Thread.Sleep(1000);
            }

            if (watcher != null)
                watcher.Created -= Watcher_Created;

            watcher = new System.IO.FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(mapPath);
            watcher.EnableRaisingEvents = true;
            watcher.Created += Watcher_Created;
        }

        private static void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.Name == Path.GetFileName(mapPath) + "Temp")
                InvokeOnSaving(sender, e);
        }

        public static bool IsMapSaving()
        {
            if (Directory.Exists(mapPath + "Temp"))
                return true;
            else if (Directory.Exists(mapPath + "Backup"))
                return true;
            else
                return false;
        }


        public static void Load()
        {
            while (IsMapSaving())
            {
                Thread.Sleep(1000);
            }
            MPQMap = Map.Open(mapPath);

            Info.Load();
            MapStrings.Load();
            UnitTypes.Load();
            ItemTypes.Load();
            DestructibleTypes.Load();
            DoodadTypes.Load();
            AbilityTypes.Load();
            BuffTypes.Load();
            UpgradeTypes.Load();
            SkinFiles.Load();

            Cameras.Load();
            Destructibles.Load();
            Regions.Load();
            Sounds.Load();
            Units.Load();
        }

        /// <summary>
        /// Removes all used map data that no longer exists in the map.
        /// </summary>
        /// <returns>A list of modified triggers.</returns>
        public static List<IExplorerElement> RemoveInvalidReferences()
        {
            List<IExplorerElement> modified = new List<IExplorerElement>();
            var triggers = Triggers.GetAll();
            for (int i = 0; i < triggers.Count; i++)
            {
                bool wasRemoved = ControllerTrigger.RemoveInvalidReferences(triggers[i]);
                if (wasRemoved)
                    modified.Add(triggers[i]);

                triggers[i].Notify();
            }
            var variables = Variables.GetGlobals();
            for (int i = 0; i < variables.Count; i++)
            {
                bool wasRemoved = ControllerVariable.RemoveInvalidReference(variables[i]);
                if (wasRemoved)
                    modified.Add(variables[i]);
            }

            return modified;
        }

    }
}
