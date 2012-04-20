using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ReplayParserNET.Warcraft3;

namespace ReplayParserNET.Handlers
{
    class ReplayHandler
    {
        public static void RenameAndRestructureReplays(ReplayType type, FolderStructureFormat format, String rootPath)
        {
            HashSet<String> versions = DatabaseHandler.GetDistinctWarcraft3ReplayAttributes("Version")[0];
            FileHandler.CreateReplayFolderStructure(versions, format, rootPath);
            rootPath = Path.Combine(rootPath, "ReplayParser.NET");

            if (format == FolderStructureFormat.Default)
            {
                //we have to re-parse all of the replays to get the associated information
                var result = ParserHandler.ParseWarcraft3Replays(DatabaseHandler.GetWarcraft3ReplayPaths().ToList());
                List<String> oldPaths = new List<String>(result.Count);
                HashSet<String> newPaths = new HashSet<String>();

                for (int i = 0; i < result.Count; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var team in result[i].teams)
                    {
                        foreach (var player in team.Value)
                        {
                            sb.Append(player.Name);
                            sb.Append("(");
                            sb.Append(Warcraft3DataConverter.ConvertShortRace(player.RaceFlag));
                            sb.Append(") ").Append(",");
                        }

                        //removing the last comma
                        sb.Remove(sb.Length - 2, 2);
                        sb.Append(" v ");
                    }

                    sb.Remove(sb.Length - 3, 3);
                    oldPaths.Add(result[i].Path);

                    String path = Path.Combine(rootPath, String.Concat("1.", result[i].replayHeader.GameVersion));
                    path = Path.Combine(path, sb.ToString().Replace("|", ""));
                    String proposedPath = String.Concat(path, ".w3g");

                    //duplicate handling
                    int j = 1;
                    while (newPaths.Contains(proposedPath))
                        proposedPath = String.Concat(path, j++, ".w3g");

                    newPaths.Add(proposedPath);
                }

                FileHandler.RenameFiles(oldPaths, newPaths.ToList());
                DatabaseHandler.UpdateWarcraft3ReplayNames(oldPaths, newPaths.ToList());
            }
        }

        //the same as MoveFiles(), except the original replay files are preserved
        public static void CopyReplays(List<String> filePaths, String destination)
        {
            FileHandler.CopyFiles(filePaths, destination);
            DatabaseHandler.UpdateWarcraft3ReplayPaths(filePaths, destination);
        }

        //ideally this will be called after a folder has been parsed, and several files were not able to be parsed
        //the user can choose to move the unparsable replays to a folder so they will be ignored in the future
        public static void MoveReplays(List<String> filePaths, String destination)
        {
            FileHandler.MoveFiles(filePaths, destination);
            DatabaseHandler.UpdateWarcraft3ReplayPaths(filePaths, destination);
        }

        public static void RenameReplays(List<String> oldFilePaths, List<String> newFilePaths)
        {
            FileHandler.RenameFiles(oldFilePaths, newFilePaths);
            DatabaseHandler.UpdateWarcraft3ReplayNames(oldFilePaths, newFilePaths);
        }

        //the recursive flag means that all of the subfolders will be traversed for replays too
        public static void AddReplayFolder(String folderPath, ReplayType type, bool recursive = true)
        {
            if (type == ReplayType.Warcraft3)
            {
                HashSet<String> replaysInFolder = FileHandler.GetReplaysInFolder(folderPath, type, recursive);
                HashSet<String> existingReplays = DatabaseHandler.GetWarcraft3ReplayPaths();
                replaysInFolder.ExceptWith(existingReplays);

                //we only want to parse the ones that are unique to the folder
                List<Warcraft3Replay> replays = ParserHandler.ParseWarcraft3Replays(replaysInFolder.ToList());
                DatabaseHandler.AddWarcraft3Replays(replays);
            }

            //add new replay types here, otherwise an exception is thrown
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
