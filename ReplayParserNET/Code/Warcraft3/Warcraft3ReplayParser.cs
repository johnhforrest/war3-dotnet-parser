using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ComponentAce.Compression.Libs.zlib;
using ReplayParserNET.Exceptions;
using System.Threading;

namespace ReplayParserNET.Warcraft3
{
    //FUNCTIONALITY NEEDED:
    //in this order...

    //DONE
    //read in the replay (preferably the whole thing at once)
    //load it as one byte array, which can be parsed
    //minimizes read operations to the hard drive

    //DONE
    //parse header
    //this data does not need to be decompressed

    //DONE
    //decompress replay using zlib
    //the decompressed data should be saved in a separate array
    //the old data is no longer needed after the decompression is done

    //parse replay
    //DONE
    //first the player needs to be loaded, this is the game's host (check this)
    //a bit more general information is found

    //DONE
    //the encoded strings come next, see the C++ implementation

    //DONE
    //the "replay info" section comes next

    //NEEDS WORKS
    //now comes the data blocks
    //need to closely view the actions to isolate the problems
    //may be worthwhile incorporating exceptions to make the parsing more robust
    //generate an action list

    class Warcraft3ReplayParser
    {
        public Warcraft3Replay Replay { get; private set; }

        public String FilePath { get; set; }

        public Warcraft3ReplayParser(String filePath)
        {
            FilePath = filePath;
        }

        public void ParseReplay()
        {
            Parse();
        }

        //add exception handling, each function call should have different errors thrown
        private void Parse()
        {
            Warcraft3Replay replay = new Warcraft3Replay();
            replay.Path = FilePath;

            byte[] replayData = File.ReadAllBytes(FilePath);
            int index = 0;

            //parsing the initial data, verifying it's a valid replay
            ParseHeader(replayData, ref index, ref replay);

            //replayData will now contain all the decompressed data
            replayData = DecompressReplay(replayData, index, replay);

            //the index is reset to the top of the decompressed data
            index = 4;

            //load the first player
            LoadPlayer(replayData, ref index, ref replay);

            //extract the game name
            while (replayData[index] != 0)
                replay.GameName += (char)replayData[index++];
            index += 2; //passing over 2 null bytes

            //parsing the encoded strings
            ParseEncodedStrings(replayData, ref index, ref replay);

            //parse general replay info
            ParseReplayInfo(replayData, ref index, ref replay);

            //parse replay data (ie, actions)
            try
            {
                ParseReplayData(replayData, ref index, ref replay);
                Replay = replay;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void ParseHeader(byte[] replayData, ref int index, ref Warcraft3Replay replay)
        {
            //validating the replay format
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            string w3g = enc.GetString(replayData, 0, 28);
            index += 28;

            if (w3g.CompareTo("Warcraft III recorded game\x1A\0") != 0)
                return; //throw exception

            //note: the bit converter doesn't move index forward, so the next subsequent 
            //call to bitconverter should be moved forward the previous call's number of bytes
            //ie, look at the first 2 lines immediately below this comment

            replay.replayHeader.EndOfHeader = BitConverter.ToUInt32(replayData, index);
            replay.replayHeader.CompDataSize = BitConverter.ToUInt32(replayData, index += 4);
            replay.replayHeader.HeaderVersion = BitConverter.ToUInt32(replayData, index += 4);
            replay.replayHeader.DecompDataSize = BitConverter.ToUInt32(replayData, index += 4);
            replay.replayHeader.NumDataBlocks = BitConverter.ToUInt32(replayData, index += 4);

            //enter subheader (for 1.06 and below)
            if (replay.replayHeader.HeaderVersion == 0x00)
            {
                index += 4;
                replay.replayHeader.GameVersion = BitConverter.ToUInt16(replayData, index += 2);
                replay.replayHeader.Build = BitConverter.ToUInt16(replayData, index += 2);
                replay.replayHeader.Flags = BitConverter.ToUInt16(replayData, index += 2);
                replay.replayHeader.GameLength = BitConverter.ToUInt32(replayData, index += 2);
                index += 8; //skip checksum
            }

            //enter subheader (for only 1.07 and above)
            else if (replay.replayHeader.HeaderVersion == 0x01)
            {
                index += 4;
                replay.replayHeader.GameVersion = BitConverter.ToUInt32(replayData, index += 4);
                replay.replayHeader.Build = BitConverter.ToUInt16(replayData, index += 4);
                replay.replayHeader.Flags = BitConverter.ToUInt16(replayData, index += 2);
                replay.replayHeader.GameLength = BitConverter.ToUInt32(replayData, index += 2);
                index += 8; //skip checksum
            }
        }

        private byte[] DecompressReplay(byte[] replayData, int index, Warcraft3Replay replay)
        {
            uint numChunks = replay.replayHeader.NumDataBlocks;
            ushort compChunkSize = BitConverter.ToUInt16(replayData, index);
            ushort decompChunkSize = BitConverter.ToUInt16(replayData, index += 2);
            index -= 2; //rewind

            byte[] decompressedData = new byte[numChunks * decompChunkSize];
            byte[] compChunk;
            byte[] decompChunk = new byte[decompChunkSize];

            ZStream stream = new ZStream();
            stream.avail_in = 0;
            stream.next_in_index = 0;

            for (int i = 0; i < numChunks; i++)
            {
                if (stream.inflateInit() != zlibConst.Z_OK)
                    return null; //error

                compChunkSize = BitConverter.ToUInt16(replayData, index);
                decompChunkSize = BitConverter.ToUInt16(replayData, index += 2);
                index += 6; //skipping checksum

                compChunk = new byte[compChunkSize];
                for (int j = 0; j < compChunkSize; j++, index++)
                    compChunk[j] = replayData[index];

                stream.avail_in = compChunkSize;
                stream.next_in = compChunk;
                stream.next_in_index = 0;

                stream.avail_out = decompChunkSize;
                stream.next_out = decompressedData;
                stream.next_out_index = i * decompChunkSize;

                stream.inflate(zlibConst.Z_NO_FLUSH);
                stream.inflateEnd();
            }

            return decompressedData;
        }

        private void LoadPlayer(byte[] replayData, ref int index, ref Warcraft3Replay replay)
        {
            Warcraft3Player player = new Warcraft3Player();
            player.Record = replayData[index++];
            player.ID = replayData[index++];

            while (replayData[index] != 0)
                player.Name += (char)replayData[index++];
            index++; //passing over the null byte

            //FFA players are unnamed
            if (player.Name == null)
                player.Name = "Player" + Convert.ToString(player.ID);

            //there are 8 additional bytes if its a ladder game (type = 0x08)
            player.GameType = replayData[index++];
            if (player.GameType == 8)
            {
                player.UpTime = BitConverter.ToUInt32(replayData, index);
                player.PlayerRace = BitConverter.ToUInt32(replayData, index += 4);
                index += 4;
            }

            //otherwise there is just a null byte
            else
            {
                index++;
            }

            replay.players.Add(player.ID, player);
        }

        private void ParseEncodedStrings(byte[] replayData, ref int index, ref Warcraft3Replay replay)
        {
            byte[] encodedString = new byte[200];
            byte[] decodedString;
            int length = 0;

            while (replayData[index] != 0)
                encodedString[length++] = replayData[index++];
            index++; //passing over the null byte

            decodedString = new byte[length];
            decodedString = DecodeStrings(encodedString, decodedString);
            ParseDecodedStrings(decodedString, ref replay);
        }

        private byte[] DecodeStrings(byte[] encodedString, byte[] decodedString)
        {
            ushort pos = 0;
            ushort dpos = 0;
            byte mask = encodedString[0];

            while (encodedString[pos] != 0)
            {
                if (pos % 8 == 0)
                {
                    mask = encodedString[pos];
                }
                else
                {
                    if ((mask & (0x1 << (pos % 8))) == 0)
                        decodedString[dpos++] = (byte)(encodedString[pos] - 1);
                    else
                        decodedString[dpos++] = encodedString[pos];
                }
                pos++;
            }

            return decodedString;
        }

        private void ParseDecodedStrings(byte[] decodedString, ref Warcraft3Replay replay)
        {
            replay.replaySettings.GameSpeed = decodedString[0];

            //visibility settings
            if ((decodedString[1] & 1) == 1)
               replay.replaySettings.Visibility = 1; //hide terrain
            else if ((decodedString[1] & 2) == 2)
                replay.replaySettings.Visibility = 2; //map explored
            else if ((decodedString[1] & 4) == 4)
                replay.replaySettings.Visibility = 3; //always visible
            else if ((decodedString[1] & 8) == 8)
                replay.replaySettings.Visibility = 4; //default

            //observer settings
            if ((decodedString[1] & 16) == 0)
                replay.replaySettings.Observers = 0; //obs off or referees
            else if ((decodedString[1] & 16) == 0 & (decodedString[1] & 32) == 32)
                replay.replaySettings.Observers = 2; //obs on defeat
            else if ((decodedString[1] & 16) == 16 && (decodedString[1] & 32) == 0)
                replay.replaySettings.Observers = 1; //obs unused
            else if ((decodedString[1] & 16) == 61 && (decodedString[1] & 32) == 32)
                replay.replaySettings.Observers = 3; //obs on or referees
            
            if ((decodedString[3] & 64) == 1)
                replay.replaySettings.Observers = 4; //referees

            replay.replaySettings.TeamsTogether = (decodedString[1] & 64) == 64;
            replay.replaySettings.LockTeams = (decodedString[2] > 0);
            replay.replaySettings.SharedControl = (decodedString[3] & 1) == 1;
            replay.replaySettings.RandomHero = (decodedString[3] & 2) == 2;
            replay.replaySettings.RandomRace = (decodedString[3] & 4) == 4;

            int i = 13; //skipping 9 bytes + checksum (4 bytes)
            while (decodedString[i] != 0)
                replay.MapName += (char)decodedString[i++];
            i++; //skipping the null byte

            //removing the folder structure, this may be worth taking out
            //replay.MapName = replay.MapName.Remove(0, replay.MapName.LastIndexOf('\\')+1);

            while (decodedString[i] != 0)
                replay.HostName += (char)decodedString[i++];
            i++; //skipping the nullbyte
        }

        private void ParseReplayInfo(byte[] replayData, ref int index, ref Warcraft3Replay replay)
        {
            //playercount: in ladder, # of players; custom, # of slots available
            replay.PlayerCount = BitConverter.ToUInt32(replayData, index );
            replay.GameType = replayData[index += 4];
            index += 8; //skipping some unused info

            //this is a valid player
            while (replayData[index] == 0x16)
            {
                LoadPlayer(replayData, ref index, ref replay);
                index += 4; //skipping checksum
            }

            //should always be this value
            if (replayData[index++] != 0x19)
                return;

            //parsing the slot records
            ParseSlotRecords(replayData, ref index, ref replay);

            //leftover info
            //random seed, 4 bytes
            //selection mode, 1 byte
            //num start spots, 1 byte
            index += 6;
        }

        //this ensures that only human players and observers are parsed
        //empty, closed, unused, and computer occupied slots are skipped
        //it may be worthwhile to eventually parse the other slots
        private void ParseSlotRecords(byte[] replayData, ref int index, ref Warcraft3Replay replay)
        {
            ushort ID;
            byte numSlots = replayData[index += 2];
            index++;

            for (int i = 0; i < numSlots; i++)
            {
                ID = replayData[index++]; //0x00 for comp

                //if the player exists in the slot
                if (ID != 0x00)
                {
                    Warcraft3Player player = replay.players[ID];
                    player.DownloadPercent = replayData[index++];
                    player.SlotStatus = replayData[index++];
                    player.PlayerFlag = replayData[index++];
                    player.TeamNumber = replayData[index++];
                    player.Color = replayData[index++];
                    player.RaceFlag = replayData[index++];
                    player.CompStrength = replayData[index++];
                    player.Handicap = replayData[index++];

                    //mapping the slot to the player
                    replay.slots[i] = ID;

                    if (player.TeamNumber != 12)
                    {
                        player.Prepare();

                        if (!replay.teams.ContainsKey(player.TeamNumber))
                        {
                            replay.teams.Add(player.TeamNumber, new List<Warcraft3Player>());
                            replay.teams[player.TeamNumber].Add(player);
                        }
                        else
                        {
                            replay.teams[player.TeamNumber].Add(player);
                        }
                    }
                }
                else
                {
                    index += 8;
                }
            }
        }

        private void ParseReplayData(byte[] replayData, ref int index, ref Warcraft3Replay replay)
        {
            byte prevBlockID;
            byte blockID = replayData[index++];
            uint numBlocks = 0;
            uint timePassed = 0;
            uint numLeaves = 0;

            while (index < replayData.Length)
            {
                switch (blockID)
                {
                    case 0x17: //leavegame block, 13 bytes
                        ParseLeaveGameBlock(replayData, ref index, ref replay, ref numLeaves, timePassed);
                        //Console.WriteLine("case 0x17");
                        break;

                    //unknowns, 4 bytes each
                    case 0x1A:
                    case 0x1B:
                    case 0x1C:
                        //Console.WriteLine("case 0x1a/0x1b/0x1c");
                        index += 4;
                        break;

                    case 0x1E: //old timeslot block
                    case 0x1F: //new timeslot block n+2 bytes
                        //Console.WriteLine("case 0x1e/0x1f");
                        ushort numBytes = BitConverter.ToUInt16(replayData, index);
                        ushort timeInc = BitConverter.ToUInt16(replayData, index += 2);
                        index += 2;
                        timePassed += timeInc;
                        if (numBytes != 2)
                            ParseActionBlock(replayData, ref index, ref replay, numBytes, timePassed);
                        break;

                    case 0x20: //chat block, n+3 bytes
                        //Console.WriteLine("case 0x20");
                        ParseChatBlock(replayData, ref index, ref replay, timePassed);
                        break;

                    case 0x22: //random seed or checksum, 5 bytes
                        //Console.WriteLine("case 0x22");
                        index += 5;
                        break;

                    case 0x23: //unknown, 10 bytes
                        //Console.WriteLine("case 0x23");
                        index += 10;
                        break;

                    case 0x2f: //forced game countdown, tourny games 8 bytes
                        //Console.WriteLine("case 0x2f");
                        index += 8;
                        break;

                    //there is occasionally heavy padding of 0s (due to lag?)
                    case 0x00:
                        //Console.WriteLine("case 0x00");
                        break;

                    //error
                    default:
                        throw new ParseDataException();
                        //TextWriter output = new StreamWriter("..\\..\\Resources\\binary.txt");
                        //for (int j = 0; j < 5; j++)
                        //{
                        //    output.WriteLine(BitConverter.ToString(replayData, index, 80));
                        //    index += 80;
                        //}
                        //break;
                }

                numBlocks++;
                prevBlockID = blockID;
                blockID = replayData[index++];
            }
        }

        private void ParseChatBlock(byte[] replayData, ref int index, ref Warcraft3Replay replay, uint timePassed)
        {
            byte ID = replayData[index++];
            ushort numBytes = BitConverter.ToUInt16(replayData, index);
            byte flag = replayData[index += 2];
            uint mode;
            string chatMsg = "";
            
            //mode: 0x00 to all, 0x01 to allies, 0x02 to obs, 0x03+N to player where N = slot #
            if (flag == 0x20)
            {
                mode = BitConverter.ToUInt32(replayData, ++index);
            }

            else //otherwise flag = 0x10, these aren't shown in the replay and have no mode
            {
                //spin over the system messages
                while (replayData[index++] != 0x00) ;
                return;
            }

            //moving the index forward after reading in mode
            index += 4;

            //appending the time to the front of the chat block
            chatMsg += "[" + HelperFunctions.FormatTimeSpan(TimeSpan.FromMilliseconds(timePassed)) + "]";

            switch (mode)
            {
                case 0x00:
                    chatMsg += "[All] ";
                    break;

                case 0x01:
                    chatMsg += "[Allies] ";
                    break;

                case 0x02:
                    chatMsg += "[Observers] ";
                    break;

                default:
                    //chatMsg += "[To " + replay.players[replay.slots[(int)(mode - 0x03)]] + "] ";
                    break;
            }

            //adding the playername
            chatMsg += replay.players[ID].Name + ": ";

            //chat parsing
            while (replayData[index] != 0x00)
                chatMsg += (char)replayData[index++];

            replay.chatLog.Add(chatMsg);
        }

        //the work in here is incomplete, the actual algorithm is complex and not 100% accurate.
        private void ParseLeaveGameBlock(byte[] replayData, ref int index, ref Warcraft3Replay replay, ref uint numLeaves, uint timePassed)
        {
            uint reason = BitConverter.ToUInt32(replayData, index);
            byte ID = replayData[index += 4];
            uint result = BitConverter.ToUInt32(replayData, ++index);
            uint unknown = BitConverter.ToUInt32(replayData, index += 4);
            index += 4;

            if (reason == 0x01)
            {
                if (result == 0x07 || result == 0x08)
                    replay.players[ID].WinStatus = 0; //player lost
                else if (result == 0x09)
                    replay.players[ID].WinStatus = 1; //player won
                else if (result == 0x0A)
                    replay.players[ID].WinStatus = 2; //draw
            }
        }

        private void ParseActionBlock(byte[] replayData, ref int index, ref Warcraft3Replay replay, ushort numBytes, uint timePassed)
        {
            byte playerID = 0;
            byte actionID = 0;
            byte groupNumber;
            string itemID;
            ushort blockLength = 0;
            ushort totalBytesParsed = 0;
            ushort actionBytesParsed = 0;

            //change selection block
            byte prevActionID = 0;
            byte prevMode = 0;
            byte mode = 0;
            ushort numSelected = 0;

            numBytes -= 2;
            while (totalBytesParsed < numBytes)
            {
                playerID = replayData[index++];
                blockLength = BitConverter.ToUInt16(replayData, index);
                index += 2;
                totalBytesParsed += 3;

                //error parsing
                if (playerID <= 0 || playerID >= 12)
                    return;
                Warcraft3Player player = replay.players[playerID];

                actionBytesParsed = 0;
                while (actionBytesParsed < blockLength)
                {
                    actionID = replayData[index++];
                    //while (actionID == 0x00)
                    //    actionID = replayData[index++];
                    actionBytesParsed++;

                    switch (actionID)
                    {
                        case 0x01: //pause game, 0 bytes
                        case 0x02: //resume game, 0 bytes
                            break;

                        case 0x03: //set game speed, 1 byte
                            index++;
                            actionBytesParsed++;
                            break;

                        case 0x04: //increase game speed, 0 bytes
                        case 0x05: //decrease game speed, 0 bytes
                            break;

                        //save game, n bytes with a null terminating character
                        case 0x06: 
                            while (replayData[index] != 0x00)
                            {
                                index++;
                                actionBytesParsed++;
                            }

                            index++;
                            actionBytesParsed++;
                            break;

                        //save game finished, 4 bytes
                        //normally follows 0x06
                        case 0x07: 
                            index += 4;
                            actionBytesParsed += 4;
                            break;

                        //pre 1.07: this block is 5 bytes (build 6031)
                        //pre 1.13: this block is 13 bytes (build 6037)
                        //otherwise: unit/building abilitiy, 14 bytes
                        case 0x10:
                            if (replay.replayHeader.Build < 6037)
                            {
                                index++;
                                actionBytesParsed++;
                            }
                            else
                            {
                                index += 2;
                                actionBytesParsed += 2;
                            }

                            itemID = "";
                            itemID += (char)replayData[index + 3];
                            itemID += (char)replayData[index + 2];
                            itemID += (char)replayData[index + 1];
                            itemID += (char)replayData[index];

                            if (replay.replayHeader.Build < 6031)
                            {
                                //4 itemID bytes
                                index += 4; 
                                actionBytesParsed += 4;
                            }
                            else
                            {
                                //passing over 8 bytes, + 4 itemID bytes
                                index += 12; 
                                actionBytesParsed += 12;
                            }

                            if (timePassed > 1500)
                            {
                                //checking for numeric id
                                if (!itemID.Contains("\0\r"))
                                {
                                    itemID = Warcraft3DataConverter.ConvertItemID(itemID);

                                    //helps mitigate duplicates
                                    if (!player.BuildOrder.ContainsKey(timePassed))
                                    {
                                        player.BuildOrder.Add(timePassed, itemID);
                                        HelperFunctions.AddDictionaryItem(player.BuildingCount, itemID, 1);
                                    }
#if (OUTPUT)
                                    TextWriter output = new StreamWriter("..\\..\\Resources\\output.txt", true);
                                    output.WriteLine(itemID);
                                    output.Close();
#endif
                                }

                            }

                            break;

                        //pre 1.07: this block is 13 bytes (build 6031)
                        //pre 1.13: this block is 21 bytes (build 6037)
                        //otherwise: unit/building ability with target pos, 22 bytes
                        case 0x11:
                            if (replay.replayHeader.Build < 6037)
                            {
                                index++;
                                actionBytesParsed++;
                            }
                            else
                            {
                                index += 2;
                                actionBytesParsed += 2;
                            }

                            itemID = "";
                            itemID += (char)replayData[index + 3];
                            itemID += (char)replayData[index + 2];
                            itemID += (char)replayData[index + 1];
                            itemID += (char)replayData[index];

                            if (replay.replayHeader.Build < 6031)
                            {
                                //passing over 8 bytes + 4 itemID bytes
                                index += 12;
                                actionBytesParsed += 12;
                            }
                            else
                            {
                                //passing over 16 bytes + 4 itemID bytes
                                index += 20;
                                actionBytesParsed += 20;
                            }

                            //checking for numeric id
                            if (!itemID.Contains("\0\r"))
                            {
                                itemID = Warcraft3DataConverter.ConvertItemID(itemID);
                                
                                //helps mitigate duplicates
                                if (!player.BuildOrder.ContainsKey(timePassed))
                                {
                                    player.BuildOrder.Add(timePassed, itemID);
                                    HelperFunctions.AddDictionaryItem(player.BuildingCount, itemID, 1);
                                }
#if (OUTPUT)
                                TextWriter output = new StreamWriter("..\\..\\Resources\\output.txt", true);
                                output.WriteLine(itemID);
                                output.Close();
#endif
                            }

                            break;

                        //pre 1.07: this block is 22 bytes (build 6031)
                        //pre 1.13: this block is 29 bytes (build 6037)
                        //otherwise: unit/building ability with target pos and obj, 30 bytes
                        case 0x12:
                            if (replay.replayHeader.Build < 6031)
                            {
                                index += 22;
                                actionBytesParsed += 22;
                            }
                            else if (replay.replayHeader.Build < 6037)
                            {
                                index += 29;
                                actionBytesParsed += 29;
                            }
                            else
                            {
                                index += 30;
                                actionBytesParsed += 30;
                            }
                            break;

                        //pre 1.07: this block is 30 bytes (build 6031)
                        //pre 1.13: this block is 37 bytes (build 6037)
                        //otherwise: pass item, 38 bytes
                        case 0x13:
                            if (replay.replayHeader.Build < 6031)
                            {
                                index += 30;
                                actionBytesParsed += 30;
                            }
                            else if (replay.replayHeader.Build < 6037)
                            {
                                index += 37;
                                actionBytesParsed += 37;
                            }
                            else
                            {
                                index += 38;
                                actionBytesParsed += 38;
                            }
                            break;

                        //pre 1.07: this block is 35 bytes (build 6031)
                        //pre 1.13: this block is 42 bytes (build 6037)
                        //otherwise: unit/building ability with 2 target pos and 2 itemID, 43 bytes
                        case 0x14:
                            if (replay.replayHeader.Build < 6031)
                            {
                                index += 35;
                                actionBytesParsed += 35;
                            }
                            else if (replay.replayHeader.Build < 6037)
                            {
                                index += 42;
                                actionBytesParsed += 42;
                            }
                            else
                            {
                                index += 43;
                                actionBytesParsed += 43;
                            }
                            break;

                        //change selection 3+n*8 bytes
                        case 0x16:
                            prevMode = mode;

                            mode = replayData[index++]; //0x01 select, 0x02 deselect
                            numSelected = BitConverter.ToUInt16(replayData, index);
                            index += (numSelected * 8) + 2;

                            //apm handling

                            actionBytesParsed += (ushort)(3 + numSelected * 8);
                            break;

                        //assign group hotkey 3+n*8 bytes
                        //group number is shifted by 1: ie, ctrl+1 = 0
                        case 0x17:
                            groupNumber = replayData[index++];
                            numSelected = BitConverter.ToUInt16(replayData, index);
                            index += (numSelected * 8) + 2;

                            //action handling

                            actionBytesParsed += (ushort)(3 + numSelected * 8);
                            break;

                        //select group hotkey, 2 bytes
                        //group number is shifted by 1: ie, ctrl+1 = 0
                        case 0x18:
                            groupNumber = replayData[index++];
                            index++; //skipping unknown byte
                            actionBytesParsed += 2;
                            break;

                        //pre 1.14b: 1 byte (build 6040)
                        //otherwise: select subgroup, 12 bytes
                        case 0x19:
                            // <= 1.14b this byte is the subgroup number
                            if (replay.replayHeader.Build < 6040) 
                            {
                                index++;
                                actionBytesParsed++;
                            }
                            else
                            {

                                index += 12;
                                actionBytesParsed += 12;
                            }
                            break;

                        //pre 1.14b: 9 bytes, unknown
                        //otherwise: pre subselection, 0 bytes
                        case 0x1A:
                            if (replay.replayHeader.Build < 6040)
                            {
                                index += 9;
                                actionBytesParsed += 9;
                            }
                            break;

                        //pre 1.14b: 9 bytes, select ground item
                        //otherwise: 9 bytes, unknown
                        case 0x1B:
                            index += 9;
                            actionBytesParsed += 9;
                            break;

                        //pre 1.14b: cancel hero revival, 8 bytes
                        //otherwise: 9 bytes, select ground item
                        case 0x1C:
                            if (replay.replayHeader.Build < 6040)
                            {
                                index += 8;
                                actionBytesParsed += 8;
                            }
                            else
                            {
                                index += 9;
                                actionBytesParsed += 9;
                            }
                            break;


                        //pre 1.14b: remove unit from building queue, 5 bytes,
                        //otherwise: cancel hero revival, 8 bytes
                        case 0x1D:
                            if (replay.replayHeader.Build < 6040)
                            {
                                index += 5;
                                actionBytesParsed += 5;
                            }
                            else
                            {
                                index += 8;
                                actionBytesParsed += 8;
                            }
                            break;

                        //remove unit from building queue, 5 bytes
                        case 0x1E:
                            index += 5;
                            actionBytesParsed += 5;
                            break;

                        //unknown, 8 bytes
                        case 0x21:
                            index += 8;
                            actionBytesParsed += 8;
                            break;

                        //case 0x20, 0x22-0x32 single player cheats

                        //change ally options, 5 bytes
                        case 0x50:
                            index += 5;
                            actionBytesParsed += 5;
                            break;

                        //transfer ally resources, 9 bytes
                        case 0x51:
                            index += 9;
                            actionBytesParsed += 9;
                            break;

                        //map trigger command, n+8 bytes
                        case 0x60:
                            index += 8;
                            actionBytesParsed += 8;

                            //while (replayData[index++] != 0)
                            //    actionBytesParsed++;

                            while (replayData[index] != 0x00)
                            {
                                index++;
                                actionBytesParsed++;
                            }

                            index++;
                            actionBytesParsed++;
                            break;

                        //esc pressed, 0 bytes
                        case 0x61:
                            break;

                        //scenario trigger
                        //pre 1.07: 8 bytes
                        //otherwise: 12 bytes
                        case 0x62:
                            if (replay.replayHeader.Build < 6031)
                            {
                                index += 8;
                                actionBytesParsed += 8;
                            }
                            else
                            {
                                index += 12;
                                actionBytesParsed += 12;
                            }
                            break;

                        //enter hero skill menu, 0 bytes
                        case 0x66:
                            break;

                        //enter build submenu, 0 bytes
                        case 0x67:
                            break;

                        //map ping, 12 bytes
                        case 0x68:
                            index += 12;
                            actionBytesParsed += 12;
                            break;

                        //continue game block A, 16 bytes
                        case 0x69:
                            index += 16;
                            actionBytesParsed += 16;
                            break;

                        //continue game block B, 16 bytes
                        case 0x6A:
                            index += 16;
                            actionBytesParsed += 16;
                            break;

                        //DOTA cases?
                        //case 0x6B:
                        //    break;

                        //case 0x70:
                        //    break;

                        //unknown 1 byte
                        case 0x75:
                            index++;
                            actionBytesParsed++;
                            break;

                        //error or unknown action
                        default:
                            throw new ParseActionException();
                    };

                    prevActionID = actionID;
                }

                totalBytesParsed += actionBytesParsed;

            }
        }

        internal class HelperFunctions
        {
            public static string FormatTimeSpan(TimeSpan time)
            {
                return time.Hours.ToString("00") + ":" +
                    time.Minutes.ToString("00") + ":" +
                    time.Seconds.ToString("00");
            }

            //returns true if added, false if not
            public static void AddDictionaryItem(Dictionary<string, int> dictionary, string key, int value)
            {
                if (!dictionary.ContainsKey(key))
                    dictionary.Add(key, value);
                else
                    dictionary[key]++;
            }
        }
    }
}
