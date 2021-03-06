﻿using System;
using System.Collections.Generic;
using System.IO;
using Bluscream;
using Humanizer;

namespace ArmA3_SaveCleaner
{
    internal class Program
    {
        static List<FileInfo> toDelete = new List<FileInfo>();
        private static void Main(string[] args)
        {
            var saveDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).Combine("Arma 3 - Other Profiles");
            Console.WriteLine(saveDir.FullName);
            ProcessDirectory(saveDir);
            Console.ReadKey(true);
            toDelete.Sort(a => a.Length);
            foreach (var file in toDelete)
            {
                Console.WriteLine($"\"{file.FullName}\"({file.Length.Bytes().Humanize("MB")})");
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
            if (file.Extension.ToLowerInvariant() != ".arma3save") return;
            if (!file.Name.ToLowerInvariant().StartsWith("save")) return;
            if (file.Length < 1048576) return;
            Console.WriteLine($"Processed file \"{file.FullName}\"({file.Length.Bytes().Humanize("MB")})");
            toDelete.Add(file);
        }
    }
}