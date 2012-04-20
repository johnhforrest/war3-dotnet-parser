using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReplayParserNET.Warcraft3
{
    public class Warcraft3Replay
    {
        public string Path { get; set; }

        public struct ReplayHeader
        {
            public uint EndOfHeader {get; set; }
            public uint CompDataSize { get; set; }
            public uint HeaderVersion { get; set; }
            public uint DecompDataSize { get; set; }
            public uint NumDataBlocks { get; set; } 

            //subheader
            public uint GameVersion { get; set; }
            public ushort Build { get; set; }
            public ushort Flags { get; set; }
            public uint GameLength { get; set; } 
        }

        public struct ReplaySettings
        {
            public ushort GameSpeed { get; set; }
            public ushort Visibility { get; set; }
            public ushort Observers { get; set; }

            public bool TeamsTogether { get; set; }
            public bool LockTeams { get; set; }
            public bool SharedControl { get; set; }
            public bool RandomHero { get; set; }
            public bool RandomRace { get; set; }
        }

        public ReplayHeader replayHeader;
        public ReplaySettings replaySettings;

        //replay info
        public string GameName { get; set; }
        public string MapName { get; set; }
        public string HostName { get; set; }
        public uint PlayerCount { get; set; }
        public byte GameType { get; set; }

        //mapping of slots/players
        public Dictionary<int, Warcraft3Player> players;
        public Dictionary<int, int> slots;

        //teams
        public Dictionary<int, List<Warcraft3Player>> teams;

        //chat log
        public List<string> chatLog;

        public Warcraft3Replay()
        {
            players = new Dictionary<int, Warcraft3Player>();
            slots = new Dictionary<int, int>();
            chatLog = new List<string>();
            teams = new Dictionary<int, List<Warcraft3Player>>();
        }

    }
}
