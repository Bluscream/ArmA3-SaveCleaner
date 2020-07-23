using System;
using System.Collections.Generic;
using System.IO;
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
            toDelete.Sort((x, y) => y.Length.CompareTo(x.Length));
            long totalSize = 0;
            foreach (var file in toDelete) { Console.WriteLine($"{file.FullName.Quote()} ({file.Length.Bytes().Humanize("#.##")})"); totalSize += file.Length; }
            Console.WriteLine();
            var delete = Utils.Console.Confirm($"Found {toDelete.Count} files taking up {totalSize.Bytes().Humanize("#.##")} in {searchedPaths} directories. Delete them now?");
            if (delete) toDelete.ForEach(a => a.Delete());
            Console.ReadKey(true);
        }

        public static void ProcessDirectory(DirectoryInfo targetDirectory)
        {
            searchedPaths++;
            var fileEntries = targetDirectory.GetFiles();
            foreach (var file in fileEntries)
                ProcessFile(file);

            var subdirectoryEntries = targetDirectory.GetDirectories();
            foreach (var subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

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