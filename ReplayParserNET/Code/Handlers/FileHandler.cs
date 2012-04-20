using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ReplayParserNET.Warcraft3;

namespace ReplayParserNET.Handlers
{
    //handles the communication between the front end to the back end
    class FileHandler
    {
        public static HashSet<String> GetReplaysInFolder(String folderPath, ReplayType type, bool recursive = true)
        {
            HashSet<String> replaysInFolder;

            if (type == ReplayType.Warcraft3)
            {
                //make a set of replays in the folder, and replays that exist
                replaysInFolder = new HashSet<String>(Directory.GetFiles(folderPath, "*.w3g"));

                if (recursive)
                {
                    Queue<String> directories = new Queue<String>(Directory.GetDirectories(folderPath));
                    while (directories.Count > 0)
                    {
                        String dir = directories.Dequeue();
                        replaysInFolder = new HashSet<String>(replaysInFolder.Concat(Directory.GetFiles(dir, "*.w3g")));
                        directories = new Queue<String>(directories.Concat(Directory.GetDirectories(dir)));
                    }

                    HashSet<String> existingReplays = DatabaseHandler.GetWarcraft3ReplayPaths();
                    replaysInFolder.ExceptWith(existingReplays);
                }
            }

            //add new replay types here, otherwise an exception is thrown
            else
            {
                throw new ArgumentException();
            }

            return replaysInFolder;
        }

        //ideally this will be called after a folder has been parsed, and several files were not able to be parsed
        //the user can choose to move the unparsable replays to a folder so they will be ignored in the future
        public static void MoveFiles(List<String> filePaths, String destination)
        {
            foreach (String file in filePaths)
                File.Move(file, Path.Combine(destination, Path.GetFileName(file)));
        }

        //the same as MoveFiles(), except the original replay files are preserved
        public static void CopyFiles(List<String> filePaths, String destination)
        {
            //if a duplicate exists, don't copy
            foreach (String oldPath in filePaths)
            {
                String newPath = Path.Combine(destination, Path.GetFileName(oldPath));
                if (!File.Exists(newPath))
                    File.Copy(oldPath, newPath);
            }
        }

        public static void RenameFiles(List<String> oldFilePaths, List<String> newFilePaths)
        {
            if (oldFilePaths.Count != newFilePaths.Count)
                throw new ArgumentException();

            //if a duplicate exists, don't move
            for (int i = 0; i < oldFilePaths.Count; i++)
                if (!File.Exists(newFilePaths[i]))
                    File.Move(oldFilePaths[i], newFilePaths[i]);
        }

        public static void CreateReplayFolderStructure(HashSet<String> versions, FolderStructureFormat format, String rootPath)
        {
            //simply separate by version number
            if (format == FolderStructureFormat.Default)
            {
                rootPath = Path.Combine(rootPath, "ReplayParser.NET");
                Directory.CreateDirectory(rootPath);

                //we don't need to check if the folder already exists, if it
                //does, the function call will ignore it
                foreach (String version in versions)
                    Directory.CreateDirectory(Path.Combine(rootPath, String.Concat("1.", version)));
            }

            //adding in gametype
            else if (format == FolderStructureFormat.Extended)
            {
                rootPath = Path.Combine(rootPath, "ReplayParser.NET");
                Directory.CreateDirectory(rootPath);

                //we don't need to check if the folder already exists, if it
                //does, the function call will ignore it
                foreach (String version in versions)
                {
                    String versionRoot = Path.Combine(rootPath, String.Concat("1.", version));
                    Directory.CreateDirectory(versionRoot);
                    Directory.CreateDirectory(Path.Combine(versionRoot, "1v1"));
                    Directory.CreateDirectory(Path.Combine(versionRoot, "2v2"));
                    Directory.CreateDirectory(Path.Combine(versionRoot, "3v3"));
                    Directory.CreateDirectory(Path.Combine(versionRoot, "4v4"));
                    Directory.CreateDirectory(Path.Combine(versionRoot, "FFA"));
                    Directory.CreateDirectory(Path.Combine(versionRoot, "Custom"));
                }
            }

            //invalid formats
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
