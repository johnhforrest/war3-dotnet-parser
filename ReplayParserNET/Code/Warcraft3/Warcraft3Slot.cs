using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReplayParserNET.Warcraft3
{
    public class Warcraft3Slot
    {
        //slot record
        public byte DownloadPercent { get; set; }
        public byte SlotStatus { get; set; }
        public byte PlayerFlag { get; set; }
        public byte TeamNumber { get; set; }
        public byte Color { get; set; }
        public byte RaceFlag { get; set; }
        public byte CompStrength { get; set; }
        public byte Handicap { get; set; }
    }
}
