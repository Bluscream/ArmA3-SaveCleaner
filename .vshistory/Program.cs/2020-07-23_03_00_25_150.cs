﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bluscream;

namespace ArmA3_SaveCleaner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var saveDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).Combine("Arma 3 - Other Profiles");
            foreach (string path in args)
            {
                if (File.Exists(path))
                {
                    ProcessFile(new FileInfo(path));
                }
                else if (Directory.Exists(path))
                {
                    ProcessDirectory(new DirectoryInfo(path));
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }
        }

        public static void ProcessDirectory(DirectoryInfo targetDirectory)
        {
            // Process the list of files found in the directory.
            var fileEntries = targetDirectory.GetFiles();
            foreach (var file in fileEntries)
                ProcessFile(file);

            // Recurse into subdirectories of this directory.
            var subdirectoryEntries = targetDirectory.GetDirectories();
            foreach (var subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(FileInfo file)
        {
            if (file.Extension.ToLowerInvariant() != ".Arma3Save") return;
            Console.WriteLine("Processed file '{0}'.", file.FullName);
        }
    }
}