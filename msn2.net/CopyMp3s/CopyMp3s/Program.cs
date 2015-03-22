using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CopyMp3s
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = @"d:\localdata\zune\audio";
            string dest = @"d:\onedrive\music";

            ScanFolder(source, source, dest);
        }

        static void ScanFolder(string path, string source, string dest)
        {
            string[] files = Directory.GetFiles(path);
            string mp3 = files.FirstOrDefault(f => f.ToLower().EndsWith("mp3"));
            if (mp3 != null)
            {
                // Copy directory
                string newDest = path.Replace(source, dest);
                if (!Directory.Exists(newDest))
                {
                    Console.WriteLine(newDest);
                    Directory.CreateDirectory(newDest);
                }

                foreach (string file in files)
                {
                    if (!file.ToLower().Contains("thumbs.db") && !file.ToLower().Contains("zunealbumart"))
                    {
                        string destFile = file.Replace(source, dest);
                        if (!File.Exists(destFile))
                        {
                            Console.WriteLine(destFile);
                            File.Copy(file, destFile);
                        }
                    }
                }
            }

            foreach (string subDirectory in Directory.GetDirectories(path))
            {
                ScanFolder(subDirectory, source, dest);
            }
        }
    }
}
