using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using ReplayParser.Warcraft3;
using ReplayParser.Handlers;

namespace ReplayParser
{
    enum ReplayType
    {
        Warcraft3,
        Starcraft2
    };

    enum FolderStructureFormat
    {
        Default,
        Extended
    }

    static class ReplayParser
    {
            //const String REPLAY_FOLDER = "C:\\Users\\johnhforrestPC2\\My Dropbox\\Personal\\ReplayParser.NET\\";
            //const String DESKTOP = "C:\\Users\\johnhforrestPC2\\Desktop\\";

            //DatabaseHandler.InitializeDatabase();
            //ReplayHandler.AddReplayFolder(REPLAY_FOLDER, ReplayType.Warcraft3);
            //ReplayHandler.RenameAndRestructureReplays(ReplayType.Warcraft3, FolderStructureFormat.Default, DESKTOP);
    }
}
