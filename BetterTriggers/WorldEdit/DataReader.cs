using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterTriggers.WorldEdit
{
    public class DataReader
    {
        private static bool _reforge = false;
        public static bool reforge { get => _reforge; }
        private static Mpq mpq = null;

        public static System.Version GameVersion;

        public static bool Load()
        {
            Settings settings = Settings.Load();
            _reforge = File.Exists(Path.Combine(settings.war3root, @"Data\data\data.000"));
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
            if (mpq.Load(settings.war3root))
            {
                var ver = FileVersionInfo.GetVersionInfo(Path.Combine(settings.war3root, "war3.exe"));
                GameVersion = new Version(ver.FileMajorPart, ver.FileMinorPart, ver.FileBuildPart, ver.FilePrivatePart);
                return true;
            }

            return false;
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

        public static string GetImageExtension()
        {
            return reforge ? ".dds" : ".blp";
        }
    }
}
