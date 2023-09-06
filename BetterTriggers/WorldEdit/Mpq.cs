using CASCLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War3Net.IO.Mpq;

namespace BetterTriggers.WorldEdit
{
    public class Mpq
    {
        private MpqArchive[] archives = null;

        public bool Load(string path)
        {
            try
            {
                archives = new MpqArchive[4];
                var files = new string[]{
                        "war3.mpq",
                        "War3x.mpq",
                        "War3xLocal.mpq",
                        "War3Patch.mpq" };
                for (var i = 3; i >= 0; i--)
                {
                    archives[i] = MpqArchive.Open(Path.Combine(path, files[i]));
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }



        public bool FileExists(string path)
        {
            foreach (var archive in archives)
            {
                if (archive.FileExists(path))
                {
                    return true;
                }
            }
            return false;
        }

        public Stream Open(string path)
        {
            foreach (var archive in archives)
            {
                if (archive.FileExists(path))
                {
                    return MpqFile.OpenRead(archive, path);
                }
            }
            return null;
        }

    }
}
