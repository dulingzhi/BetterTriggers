using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterTriggers.WorldEdit
{
    public class DataReader
    {
        private static bool reforge = false;
        private static Mpq mpq = null;

        public static System.Version GameVersion;

        public static bool Load()
        {
            Settings settings = Settings.Load();
            reforge = File.Exists(Path.Combine(settings.war3root, @"Data\data\data.000"));
            if (reforge)
            {
                if (Casc.Load())
                {
                    GameVersion = Casc.GameVersion;
                    return true;
                }
                return false;
            }
            mpq = new Mpq();
            return mpq.Load(settings.war3root);
        }

        public static bool FileExists(string path)
        {
            if (reforge)
            {
                var file = Casc.GetWar3ModFolder();
                path = Path.Combine(file.Name, path);
                return Casc.GetCasc().FileExists(path);
            }
            return mpq.FileExists(path);
        }

        public static Stream OpenFile(string path)
        {
            if (reforge)
            {
                var file = Casc.GetWar3ModFolder();
                path = Path.Combine(file.Name, path);
                return Casc.GetCasc().OpenFile(path);
            }

            return mpq.Open(path);
        }

        public static void Export(string srcPath, string destPath)
        {
            var stream = OpenFile(srcPath);
            using var destStream = File.Create(destPath);
            stream.CopyTo(destStream);
        }
    }
}
