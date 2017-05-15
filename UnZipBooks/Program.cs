using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnZipBooks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] filePaths = Directory.GetFiles(@"Books\", "*.zip",
                                        SearchOption.AllDirectories);
            // get folder name
            //string lastFolderName = Path.GetFileName(Path.GetDirectoryName(filePaths[0]));
            foreach (string zipPath in filePaths)
            {
                ZipFile.ExtractToDirectory(zipPath, "UnzipBooks");
            }

            Console.WriteLine("Done!!!!");
            Console.Read();
        }
    }
}