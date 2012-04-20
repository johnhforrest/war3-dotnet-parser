using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReplayParserNET.Warcraft3
{
    public class Warcraft3Player : Warcraft3Slot
    {
        #region player_record

        //0x00 = host, 0x16 = other player
        public byte Record { get; set; }
        public byte ID { get; set; }
        public string Name { get; set; }

        //0x01 = custom, 0x08 = ladder (additional bytes following record)
        public byte GameType { get; set; }
        public byte WinStatus { get; set; }
        public uint LeaveTime { get; set; }

        //ladder games only
        public uint UpTime { get; set; }
        public uint PlayerRace { get; set; }

        #endregion

        #region player_data

        //actions
        public Dictionary<string, int> Actions { get; set;}

        //buildings, units, heroes & research
        public Dictionary<uint, string> BuildOrder { get; set; }
        public Dictionary<string, int> UnitCount { get; set; }
        public Dictionary<string, int> BuildingCount { get; set; }
        public Dictionary<string, int> ResearchCount { get; set; }

        public struct Hero
        {
            public string Name { get; set; }
            public ushort Level { get; set; }
            public Dictionary<string, int> Abilities { get; set; }

            public void Prepare()
            {
                Abilities = new Dictionary<string, int>();
            }
        };

        public Hero[] Heroes { get; set; }

        #endregion

        //initializes all the data structures needed for parsing
        public void Prepare()
        {
            Actions = new Dictionary<string, int>();
            BuildOrder = new Dictionary<uint, string>();
            UnitCount = new Dictionary<string, int>();
            BuildingCount = new Dictionary<string, int>();
            ResearchCount = new Dictionary<string, int>();
            Heroes = new Hero[3];
            foreach (Hero h in Heroes)
                h.Prepare();
        }
    }
}
