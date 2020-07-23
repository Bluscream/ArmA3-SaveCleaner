using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bluscream;
using Humanizer;

namespace ArmA3_SaveCleaner
{
    internal class Program
    {
        private static List<FileInfo> toDelete = new List<FileInfo>();
        private static long searchedPaths = 0;

        private static void Main()
        {
            var saveDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).Combine("Arma 3 - Other Profiles");
            Console.WriteLine($"Searching {saveDir.FullName.Quote()}");
            ProcessDirectory(saveDir);
            // var newtoDelete = toDelete.OrderByDescending(a => a.Length).ToList();
            toDelete.Sort((x, y) => y.Length.CompareTo(x.Length));
            // toDelete.Reverse();
            long totalSize = 0;
            foreach (var file in toDelete) { Console.WriteLine($"{file.FullName.Quote()} ({file.Length.Bytes().Humanize("#.##")})"); totalSize += file.Length; }
            Console.WriteLine();
            Utils.Console.Confirm($"Found {toDelete.Count} files taking up {totalSize.Bytes().Humanize("#.##")} in {searchedPaths} directories. Delete them now?");
            toDelete.ForEach(a => a.Delete());
            Console.ReadKey(true);
        }

        public static void ProcessDirectory(DirectoryInfo targetDirectory)
        {
            searchedPaths++;
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
            var name = file.Name.ToLowerInvariant();
            if (!name.StartsWith("save") && !name.StartsWith("autosaveold")) return;
            if (file.Length < 1048576) return;
            toDelete.Add(file);
        }
    }
}