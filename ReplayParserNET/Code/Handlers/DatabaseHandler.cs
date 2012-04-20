using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using ReplayParserNET.Warcraft3;
using System.IO;

/*
 * Things to keep in mind:
 *      - Always close database connection within the same method
 *      - Use parameterized statements when making a large number of commands
 *        simply change the parameters, rather than build a new command each time
 * 
 * 
 * */

namespace ReplayParserNET.Handlers
{
    static class DatabaseHandler
    {
        public static String FilePath { get; private set; }

        public static String FileName { get; private set; }

        static DatabaseHandler()
        {
            // AppData/Roaming/ReplayParser/
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ReplayParser";
            FileName = Path.Combine(FilePath, "replays.db3");

            //neither the folder or the db file exists
            if (!System.IO.Directory.Exists(FilePath))
            {
                System.IO.Directory.CreateDirectory(FilePath);
                SQLiteConnection.CreateFile(FileName);
            }

            //the folder exists, but not the db file
            else if (!System.IO.File.Exists(FileName))
            {
                SQLiteConnection.CreateFile(FileName);
            }
        }

        public static void InitializeDatabase()
        {
            //creating the tables
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + FileName))
            {
                connection.Open();
                string query1 = "CREATE TABLE IF NOT EXISTS war3replays(ReplayName VARCHAR(255)" +
                    "PRIMARY KEY, GameType VARCHAR(100), Matchup VARCHAR(100), MapName VARCHAR(100), Duration VARCHAR(20), " +
                    "Version TINYINT(50), Build SMALLINT(10000), DateAdded VARCHAR(100), Path VARCHAR(255), " +
                    "GameName VARCHAR(100), HostName VARCHAR(100), NumPlayers SMALLINT(30))";

                string query2 = "CREATE TABLE IF NOT EXISTS war3playerinst(ID INT(50000) PRIMARY KEY, " +
                    "ReplayName VARCHAR(100), PlayerName VARCHAR(100), APM FLOAT(1000), " +
                    "Race VARCHAR(100), LeaveTime VARCHAR(100), Units SMALLINT(1000), " +
                    "Buildings SMALLINT(1000), WinLose TINYINT(10), MapName VARCHAR(100))";

                string query3 = "CREATE TABLE IF NOT EXISTS war3players(PlayerName VARCHAR(100) " +
                    "PRIMARY KEY, GamesPlayed INT(10000), AvgAPM FLOAT(1000), AvgDuration " +
                    "VARCHAR(20), Win FLOAT(100), Elf FLOAT(100), Human FLOAT(100), Orc " +
                    "FLOAT(100), Undead FLOAT(100))";


                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "BEGIN TRANSACTION";
                    command.ExecuteNonQuery();

                    command.CommandText = query1;
                    command.ExecuteNonQuery();

                    command.CommandText = query2;
                    command.ExecuteNonQuery();

                    command.CommandText = query3;
                    command.ExecuteNonQuery();

                    command.CommandText = "END TRANSACTION";
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void AddWarcraft3Replays(List<Warcraft3Replay> replays)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + FileName))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string query1 = "INSERT INTO war3replays " +
                        "(ReplayName,GameType,Matchup,MapName,Duration,Version,Build,DateAdded,Path,GameName,HostName,NumPlayers) " +
                        "VALUES($repname,$type,$matchup,$mapname,$duration,$version,$build,$date,$path,$gamename,$hostname,$numplayers)";

                    string query2 = "INSERT INTO war3playerinst " +
                        "(ID,ReplayName,PlayerName,APM,Race,LeaveTime,Units,Buildings,WinLose,MapName) " +
                        "VALUES($id,$repname,$playername,$apm,$race,$leavetime,$units,$buildings,$winlose,$mapname)";

                    command.CommandText = "BEGIN TRANSACTION";
                    command.ExecuteNonQuery();

                    long id = 1;
                    command.CommandText = "SELECT max(ID) FROM war3playerinst";
                    try
                    {
                        id = (long)command.ExecuteScalar();
                    } catch (Exception) {}

                    for (int i = 0; i < replays.Count; i++)
                    {
                        try
                        {
                            Warcraft3Replay replay = replays[i];

                            command.CommandText = query1;
                            command.Parameters.AddWithValue("$repname", Path.GetFileName(replay.Path));
                            command.Parameters.AddWithValue("$type", replay.GameType);
                            command.Parameters.AddWithValue("$matchup", "");
                            command.Parameters.AddWithValue("$mapname", replay.MapName);
                            command.Parameters.AddWithValue("$duration", replay.replayHeader.GameLength);
                            command.Parameters.AddWithValue("$version", replay.replayHeader.GameVersion);
                            command.Parameters.AddWithValue("$build", replay.replayHeader.Build);
                            command.Parameters.AddWithValue("$date", DateTime.Now);
                            command.Parameters.AddWithValue("$path", replay.Path);
                            command.Parameters.AddWithValue("$gamename", replay.GameName);
                            command.Parameters.AddWithValue("$hostname", replay.HostName);
                            command.Parameters.AddWithValue("$numplayers", replay.PlayerCount);
                            command.ExecuteNonQuery();

                            foreach (KeyValuePair<int, int> slotEntry in replay.slots)
                            {
                                Warcraft3Player player = replay.players[slotEntry.Value];

                                command.CommandText = query2;
                                command.Parameters.AddWithValue("$id", id++);
                                command.Parameters.AddWithValue("$repname", Path.GetFileName(replay.Path));
                                command.Parameters.AddWithValue("$playername", player.Name);
                                command.Parameters.AddWithValue("$apm", 0);
                                command.Parameters.AddWithValue("$race", player.PlayerRace);
                                command.Parameters.AddWithValue("$leavetime", player.LeaveTime);
                                command.Parameters.AddWithValue("$units", 0);
                                command.Parameters.AddWithValue("$buildings", 0);
                                command.Parameters.AddWithValue("$winlose", 0);
                                command.Parameters.AddWithValue("$mapname", replay.MapName);
                                command.ExecuteNonQuery();
                            }
                        }

                        //if an exception is caught, that means a duplicate was attempted to be added somewhere
                        //TODO: add some logging
                        catch (Exception) { }
                    }

                    command.CommandText = "END TRANSACTION";
                    command.ExecuteNonQuery();
                }
            }
        }

        //TODO: create a generic SELECT
        public static HashSet<String> GetWarcraft3ReplayPaths()
        {
            HashSet<String> existingReplays = new HashSet<String>();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + FileName))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "BEGIN TRANSACTION";
                    command.ExecuteNonQuery();

                    command.CommandText = "SELECT Path FROM war3replays";
                    var result = command.ExecuteReader();
                    
                    while (result.Read())
                        existingReplays.Add(result.GetString(0));

                    result.Close();

                    command.CommandText = "END TRANSACTION";
                    command.ExecuteNonQuery();
                }
            }

            return existingReplays;
        }

        //TODO: create a generic UPDATE
        public static void UpdateWarcraft3ReplayPaths(List<String> filePaths, String destination)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + FileName))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "BEGIN TRANSACTION";
                    command.ExecuteNonQuery();

                    command.CommandText = "UPDATE war3replays SET Path = $path WHERE ReplayName = $name";

                    foreach (String filePath in filePaths)
                    {
                        command.Parameters.AddWithValue("$path", Path.Combine(destination, Path.GetFileName(filePath)));
                        command.Parameters.AddWithValue("$name", Path.GetFileName(filePath));
                        command.ExecuteNonQuery();
                    }

                    command.CommandText = "END TRANSACTION";
                    command.ExecuteNonQuery();
                }
            }
        }

        //TODO: see above
        public static void UpdateWarcraft3ReplayNames(List<String> oldFilePaths, List<String> newFilePaths)
        {
            if (oldFilePaths.Count != newFilePaths.Count)
                throw new ArgumentException();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + FileName))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "BEGIN TRANSACTION";
                    command.ExecuteNonQuery();

                    command.CommandText = "UPDATE war3replays " +
                        "SET Path = $newpath, ReplayName = $newname " +
                        "WHERE ReplayName = $oldname";

                    for (int i = 0; i < oldFilePaths.Count; i++)
                    {
                        command.Parameters.AddWithValue("$oldname", Path.GetFileName(oldFilePaths[i]));
                        command.Parameters.AddWithValue("$newpath", newFilePaths[i]);
                        command.Parameters.AddWithValue("$newname", Path.GetFileName(newFilePaths[i]));
                        command.ExecuteNonQuery();
                    }

                    command.CommandText = "END TRANSACTION";
                    command.ExecuteNonQuery();
                }
            }
        }

        //TODO: how about another way of defining the column names instead of a param list?
        //      what about XML or a List object
        //      this would also allow defining different tables possible(not sure how I feel about this though)
        public static List<HashSet<String>> GetDistinctWarcraft3ReplayAttributes(params string[] columnNames)
        {
            if (columnNames.Length == 0)
                throw new ArgumentException();

            List<HashSet<String>> columnContents = new List<HashSet<String>>();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + FileName))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "BEGIN TRANSACTION";
                    command.ExecuteNonQuery();

                    if (columnNames.Length == 1)
                    {
                        command.CommandText = String.Concat("SELECT DISTINCT ", columnNames[0], " FROM war3replays");
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("SELECT DISTINCT ");

                        for (int i = 0; i < columnNames.Length - 1; i++)
                            sb.Append(columnNames[i]).Append(",");

                        sb.Append(columnNames[columnNames.Length - 1]);
                        sb.Append(" FROM war3replays");
                        command.CommandText = sb.ToString();
                    }

                    var result = command.ExecuteReader();

                    //initializing the lists
                    for (int i = 0; i < result.FieldCount; i++)
                        columnContents.Add(new HashSet<String>());

                    //populating the lists
                    while (result.Read())
                        for (int i = 0; i < result.FieldCount; i++)
                            columnContents[i].Add(Convert.ToString(result.GetValue(i)));

                    result.Close();
                    command.CommandText = "END TRANSACTION";
                    command.ExecuteNonQuery();
                }
            }

            return columnContents;
        }

        //TODO: see above
        public static List<List<String>> GetWarcraft3ReplayAttributes(params string[] columnNames)
        {
            if (columnNames.Length == 0)
                throw new ArgumentException();

            List<List<String>> columnContents = new List<List<String>>();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + FileName))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "BEGIN TRANSACTION";
                    command.ExecuteNonQuery();

                    if (columnNames.Length == 1)
                    {
                        command.CommandText = String.Concat("SELECT ", columnNames[0], " FROM war3replays");
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("SELECT ");

                        for (int i = 0; i < columnNames.Length - 1; i++)
                            sb.Append(columnNames[i]).Append(",");

                        sb.Append(columnNames[columnNames.Length - 1]);
                        sb.Append(" FROM war3replays");
                        command.CommandText = sb.ToString();
                    }

                    var result = command.ExecuteReader();

                    //initializing the lists
                    for (int i = 0; i < result.FieldCount; i++)
                        columnContents.Add(new List<String>());

                    //populating the lists
                    while (result.Read())
                        for (int i = 0; i < result.FieldCount; i++)
                            columnContents[i].Add(Convert.ToString(result.GetValue(i)));

                    result.Close();
                    command.CommandText = "END TRANSACTION";
                    command.ExecuteNonQuery();
                }
            }

            return columnContents;
        }

        //TODO: see above
        public static List<List<String>> GetWarcraft3PlayerAttributes(params string[] columnNames)
        {
            if (columnNames.Length == 0)
                throw new ArgumentException();

            List<List<String>> columnContents = new List<List<String>>();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + FileName))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "BEGIN TRANSACTION";
                    command.ExecuteNonQuery();

                    if (columnNames.Length == 1)
                    {
                        command.CommandText = String.Concat("SELECT ", columnNames[0], " FROM war3playerinst");
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("SELECT ");

                        for (int i = 0; i < columnNames.Length - 1; i++)
                            sb.Append(columnNames[i]).Append(",");

                        sb.Append(columnNames[columnNames.Length - 1]);
                        sb.Append(" FROM war3playerinst");
                        command.CommandText = sb.ToString();
                    }

                    var result = command.ExecuteReader();

                    //initializing the lists
                    for (int i = 0; i < result.FieldCount; i++)
                        columnContents.Add(new List<String>());

                    //populating the lists
                    while (result.Read())
                        for (int i = 0; i < result.FieldCount; i++)
                            columnContents[i].Add(Convert.ToString(result.GetValue(i)));

                    result.Close();
                    command.CommandText = "END TRANSACTION";
                    command.ExecuteNonQuery();
                }
            }

            return columnContents;
        }
    }
}
