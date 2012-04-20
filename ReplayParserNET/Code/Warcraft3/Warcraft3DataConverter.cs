using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReplayParserNET.Warcraft3
{
    class Warcraft3DataConverter
    {
        private static Dictionary<string, string> itemMap;

        static Warcraft3DataConverter()
        {
            itemMap = new Dictionary<string, string>();

            //human units
	        itemMap["hfoo"] = "u_Footman";
	        itemMap["hkni"] = "u_Knight";
	        itemMap["hmpr"] = "u_Priest";
	        itemMap["hmtm"] = "u_Mortar Team";
	        itemMap["hpea"] = "u_Peasant";
	        itemMap["hrif"] = "u_Rifleman";
	        itemMap["hsor"] = "u_Sorceress";
            itemMap["hmtt"] = "u_Siege Engine";
            itemMap["hrtt"] = "u_Siege Engine";
	        itemMap["hgry"] = "u_Gryphon Rider";
	        itemMap["hgyr"] = "u_Flying Machine";
	        itemMap["hspt"] = "u_Spell Breaker";
	        itemMap["hdhw"] = "u_Dragonhawk Rider";

            //human building upgrades
            itemMap["hkee"] = "b_Keep";
            itemMap["hcas"] = "b_Castle";
            itemMap["hctw"] = "b_Cannon Tower";
            itemMap["hgtw"] = "b_Guard Tower";
            itemMap["hatw"] = "b_Arcane Tower";

            //elf units
			itemMap["ebal"] = "u_Glaive Thrower";
			itemMap["echm"] = "u_Chimaera";
			itemMap["edoc"] = "u_Druid of the Claw";
			itemMap["edot"] = "u_Druid of the Talon";
			itemMap["ewsp"] = "u_Wisp";
			itemMap["esen"] = "u_Huntress";
			itemMap["earc"] = "u_Archer";
			itemMap["edry"] = "u_Dryad";
			itemMap["ehip"] = "u_Hippogryph";
			itemMap["emtg"] = "u_Mountain Giant";
			itemMap["efdr"] = "u_Faerie Dragon";
			
            //elf building upgrades
			itemMap["etoa"] = "b_Tree of Ages";
			itemMap["etoe"] = "b_Tree of Eternity";

            //orc units
			itemMap["ocat"] = "u_Demolisher";
			itemMap["odoc"] = "u_Troll Witch Doctor";
			itemMap["ogru"] = "u_Grunt";
			itemMap["ohun"] = "u_Troll Headhunter/Berserker";
            itemMap["otbk"] = "u_Troll Headhunter/Berserker";
			itemMap["okod"] = "u_Kodo Beast";
			itemMap["opeo"] = "u_Peon";
			itemMap["orai"] = "u_Raider";
			itemMap["oshm"] = "u_Shaman";
			itemMap["otau"] = "u_Tauren";
			itemMap["owyv"] = "u_Wind Rider";
			itemMap["ospw"] = "u_Spirit Walker";
            itemMap["ospm"] = "u_Spirit Walker";
			itemMap["otbr"] = "u_Troll Batrider";
			
            //orc building upgrades
			itemMap["ofrt"] = "b_Fortress";
			itemMap["ostr"] = "b_Stronghold";

            //undead units
			itemMap["uaco"] = "u_Acolyte";
			itemMap["uabo"] = "u_Abomination";
			itemMap["uban"] = "u_Banshee";
			itemMap["ucry"] = "u_Crypt Fiend";
			itemMap["ufro"] = "u_Frost Wyrm";
			itemMap["ugar"] = "u_Gargoyle";
			itemMap["ugho"] = "u_Ghoul";
			itemMap["unec"] = "u_Necromancer";
			itemMap["umtw"] = "u_Meatwagon";
			itemMap["ushd"] = "u_Shade";
			itemMap["uobs"] = "u_Obsidian Statue";
			itemMap["ubsp"] = "u_Destroyer";
			
            //undead building upgrades
			itemMap["unp1"] = "b_Halls of the Dead";
			itemMap["unp2"] = "b_Black Citadel";
			itemMap["uzg1"] = "b_Spirit Tower";
			itemMap["uzg2"] = "b_Nerubian Tower";

            //human heroes
			itemMap["Hamg"] = "h_Archmage";
			itemMap["Hmkg"] = "h_Mountain King";
			itemMap["Hpal"] = "h_Paladin";
			itemMap["Hblm"] = "h_Blood Mage";

            //elf heroes
			itemMap["Edem"] = "h_Demon Hunter";
			itemMap["Ekee"] = "h_Keeper of the Grove";
			itemMap["Emoo"] = "h_Priestess of the Moon";
			itemMap["Ewar"] = "h_Warden";

            //orc heroes
		    itemMap["Obla"] = "h_Blademaster";
			itemMap["Ofar"] = "h_Far Seer";
			itemMap["Otch"] = "h_Tauren Chieftain";
			itemMap["Oshd"] = "h_Shadow Hunter";

            //undead heroes
			itemMap["Udea"] = "h_Death Knight";
			itemMap["Udre"] = "h_Dreadlord";
			itemMap["Ulic"] = "h_Lich";
			itemMap["Ucrl"] = "h_Crypt Lord";

            //neutral heroes
			itemMap["Npbm"] = "h_Pandaren Brewmaster";
			itemMap["Nbrn"] = "h_Dark Ranger";
			itemMap["Nngs"] = "h_Naga Sea Witch";
			itemMap["Nplh"] = "h_Pit Lord";
			itemMap["Nbst"] = "h_Beastmaster";
			itemMap["Ntin"] = "h_Goblin Tinker";
			itemMap["Nfir"] = "h_Firelord";
			itemMap["Nalc"] = "h_Goblin Alchemist";

            //human hero abilities
			itemMap["AHbz"] = "a_Archmage:Blizzard";
			itemMap["AHwe"] = "a_Archmage:Summon Water Elemental";
			itemMap["AHab"] = "a_Archmage:Brilliance Aura";
			itemMap["AHmt"] = "a_Archmage:Mass Teleport";
			itemMap["AHtb"] = "a_Mountain King:Storm Bolt";
			itemMap["AHtc"] = "a_Mountain King:Thunder Clap";
			itemMap["AHbh"] = "a_Mountain King:Bash";
			itemMap["AHav"] = "a_Mountain King:Avatar";
			itemMap["AHhb"] = "a_Paladin:Holy Light";
			itemMap["AHds"] = "a_Paladin:Divine Shield";
			itemMap["AHad"] = "a_Paladin:Devotion Aura";
			itemMap["AHre"] = "a_Paladin:Resurrection";
			itemMap["AHdr"] = "a_Blood Mage:Siphon Mana";
			itemMap["AHfs"] = "a_Blood Mage:Flame Strike";
			itemMap["AHbn"] = "a_Blood Mage:Banish";
			itemMap["AHpx"] = "a_Blood Mage:Summon Phoenix";

            //elf hero abilities
			itemMap["AEmb"] = "a_Demon Hunter:Mana Burn";
			itemMap["AEim"] = "a_Demon Hunter:Immolation";
			itemMap["AEev"] = "a_Demon Hunter:Evasion";
			itemMap["AEme"] = "a_Demon Hunter:Metamorphosis";
			itemMap["AEer"] = "a_Keeper of the Grove:Entangling Roots";
			itemMap["AEfn"] = "a_Keeper of the Grove:Force of Nature";
			itemMap["AEah"] = "a_Keeper of the Grove:Thorns Aura";
			itemMap["AEtq"] = "a_Keeper of the Grove:Tranquility";
			itemMap["AEst"] = "a_Priestess of the Moon:Scout";
			itemMap["AHfa"] = "a_Priestess of the Moon:Searing Arrows";
			itemMap["AEar"] = "a_Priestess of the Moon:Trueshot Aura";
			itemMap["AEsf"] = "a_Priestess of the Moon:Starfall";
			itemMap["AEbl"] = "a_Warden:Blink";
			itemMap["AEfk"] = "a_Warden:Fan of Knives";
			itemMap["AEsh"] = "a_Warden:Shadow Strike";
			itemMap["AEsv"] = "a_Warden:Spirit of Vengeance";

            //orc hero abilities
			itemMap["AOwk"] = "a_Blademaster:Wind Walk";
			itemMap["AOmi"] = "a_Blademaster:Mirror Image";
			itemMap["AOcr"] = "a_Blademaster:Critical Strike";
			itemMap["AOww"] = "a_Blademaster:Bladestorm";
			itemMap["AOcl"] = "a_Far Seer:Chain Lighting";
			itemMap["AOfs"] = "a_Far Seer:Far Sight";
			itemMap["AOsf"] = "a_Far Seer:Feral Spirit";
			itemMap["AOeq"] = "a_Far Seer:Earth Quake";
			itemMap["AOsh"] = "a_Tauren Chieftain:Shockwave";
			itemMap["AOae"] = "a_Tauren Chieftain:Endurance Aura";
			itemMap["AOws"] = "a_Tauren Chieftain:War Stomp";
			itemMap["AOre"] = "a_Tauren Chieftain:Reincarnation";
			itemMap["AOhw"] = "a_Shadow Hunter:Healing Wave";
			itemMap["AOhx"] = "a_Shadow Hunter:Hex";
			itemMap["AOsw"] = "a_Shadow Hunter:Serpent Ward";
			itemMap["AOvd"] = "a_Shadow Hunter:Big Bad Voodoo";

            //undead hero abilities
			itemMap["AUdc"] = "a_Death Knight:Death Coil";
			itemMap["AUdp"] = "a_Death Knight:Death Pact";
			itemMap["AUau"] = "a_Death Knight:Unholy Aura";
			itemMap["AUan"] = "a_Death Knight:Animate Dead";
			itemMap["AUcs"] = "a_Dreadlord:Carrion Swarm";
			itemMap["AUsl"] = "a_Dreadlord:Sleep";
			itemMap["AUav"] = "a_Dreadlord:Vampiric Aura";
			itemMap["AUin"] = "a_Dreadlord:Inferno";
			itemMap["AUfn"] = "a_Lich:Frost Nova";
			itemMap["AUfa"]= "a_Lich:Frost Armor";
            itemMap["AUfu"] = "a_Lich:Frost Armor";
			itemMap["AUdr"] = "a_Lich:Dark Ritual";
			itemMap["AUdd"] = "a_Lich:Death and Decay";
			itemMap["AUim"] = "a_Crypt Lord:Impale";
			itemMap["AUts"] = "a_Crypt Lord:Spiked Carapace";
			itemMap["AUcb"] = "a_Crypt Lord:Carrion Beetles";
			itemMap["AUls"] = "a_Crypt Lord:Locust Swarm";

            //neutral hero abilities
			itemMap["ANbf"] = "a_Pandaren Brewmaster:Breath of Fire";
			itemMap["ANdb"] = "a_Pandaren Brewmaster:Drunken Brawler";
			itemMap["ANdh"] = "a_Pandaren Brewmaster:Drunken Haze";
			itemMap["ANef"] = "a_Pandaren Brewmaster:Storm Earth and Fire";
			itemMap["ANdr"] = "a_Dark Ranger:Life Drain";
			itemMap["ANsi"] = "a_Dark Ranger:Silence";
			itemMap["ANba"] = "a_Dark Ranger:Black Arrow";
			itemMap["ANch"] = "a_Dark Ranger:Charm";
			itemMap["ANms"] = "a_Naga Sea Witch:Mana Shield";
			itemMap["ANfa"] = "a_Naga Sea Witch:Frost Arrows";
			itemMap["ANfl"] = "a_Naga Sea Witch:Forked Lightning";
			itemMap["ANto"] = "a_Naga Sea Witch:Tornado";
			itemMap["ANrf"] = "a_Pit Lord:Rain of Fire";
			itemMap["ANca"] = "a_Pit Lord:Cleaving Attack";
			itemMap["ANht"] = "a_Pit Lord:Howl of Terror";
			itemMap["ANdo"] = "a_Pit Lord:Doom";
			itemMap["ANsg"] = "a_Beastmaster:Summon Bear";
			itemMap["ANsq"] = "a_Beastmaster:Summon Quilbeast";
			itemMap["ANsw"] = "a_Beastmaster:Summon Hawk";
			itemMap["ANst"] = "a_Beastmaster:Stampede";
			itemMap["ANeg"] = "a_Goblin Tinker:Engineering Upgrade";
			itemMap["ANcs"] = "a_Goblin Tinker:Cluster Rockets";
            itemMap["ANc1"] = "a_Goblin Tinker:Cluster Rockets";
            itemMap["ANc2"] = "a_Goblin Tinker:Cluster Rockets";
            itemMap["ANc3"] = "a_Goblin Tinker:Cluster Rockets";
			itemMap["ANsy"] = "a_Goblin Tinker:Pocket Factory";
            itemMap["ANs1"] = "a_Goblin Tinker:Pocket Factory";
            itemMap["ANs2"] = "a_Goblin Tinker:Pocket Factory";
            itemMap["ANs3"] = "a_Goblin Tinker:Pocket Factory";
			itemMap["ANrg"] = "a_Goblin Tinker:Robo-Goblin";
            itemMap["ANg1"] = "a_Goblin Tinker:Robo-Goblin";
            itemMap["ANg2"] = "a_Goblin Tinker:Robo-Goblin";
            itemMap["ANg3"] = "a_Goblin Tinker:Robo-Goblin";
			itemMap["ANic"] = "a_Firelord:Incinerate";
            itemMap["ANia"] = "a_Firelord:Incinerate";
			itemMap["ANso"] = "a_Firelord:Soul Burn";
			itemMap["ANlm"] = "a_Firelord:Summon Lava Spawn";
			itemMap["ANvc"] = "a_Firelord:Volcano";
			itemMap["ANhs"] = "a_Goblin Alchemist:Healing Spray";
			itemMap["ANab"] = "a_Goblin Alchemist:Acid Bomb";
			itemMap["ANcr"] = "a_Goblin Alchemist:Chemical Rage";
			itemMap["ANtm"] = "a_Goblin Alchemist:Transmute";

            //neutral units
			itemMap["nskm"] = "u_Skeletal Marksman";
			itemMap["nskf"] = "u_Burning Archer";
			itemMap["nws1"] = "u_Dragon Hawk";
			itemMap["nban"] = "u_Bandit";
			itemMap["nrog"] = "u_Rogue";
			itemMap["nenf"] = "u_Enforcer";
			itemMap["nass"] = "u_Assassin";
			itemMap["nbdk"] = "u_Black Drake";
			itemMap["nrdk"] = "u_Red Dragon Whelp";
			itemMap["nbdr"] = "u_Black Dragon Whelp";
			itemMap["nrdr"] = "u_Red Drake";
			itemMap["nbwm"] = "u_Black Dragon";
			itemMap["nrwm"] = "u_Red Dragon";
			itemMap["nadr"] = "u_Blue Dragon";
			itemMap["nadw"] = "u_Blue Dragon Whelp";
			itemMap["nadk"] = "u_Blue Drake";
			itemMap["nbzd"] = "u_Bronze Dragon";
			itemMap["nbzk"] = "u_Bronze Drake";
			itemMap["nbzw"] = "u_Bronze Dragon Whelp";
			itemMap["ngrd"] = "u_Green Dragon";
			itemMap["ngdk"] = "u_Green Drake";
			itemMap["ngrw"] = "u_Green Dragon Whelp";
			itemMap["ncea"] = "u_Centaur Archer";
			itemMap["ncen"] = "u_Centaur Outrunner";
			itemMap["ncer"] = "u_Centaur Drudge";
			itemMap["ndth"] = "u_Dark Troll High Priest";
			itemMap["ndtp"] = "u_Dark Troll Shadow Priest";
			itemMap["ndtb"] = "u_Dark Troll Berserker";
			itemMap["ndtw"] = "u_Dark Troll Warlord";
			itemMap["ndtr"] = "u_Dark Troll";
			itemMap["ndtt"] = "u_Dark Troll Trapper";
			itemMap["nfsh"] = "u_Forest Troll High Priest";
			itemMap["nfsp"] = "u_Forest Troll Shadow Priest";
			itemMap["nftr"] = "u_Forest Troll";
			itemMap["nftb"] = "u_Forest Troll Berserker";
			itemMap["nftt"] = "u_Forest Troll Trapper";
			itemMap["nftk"] = "u_Forest Troll Warlord";
			itemMap["ngrk"] = "u_Mud Golem";
			itemMap["ngir"] = "u_Goblin Shredder";
			itemMap["nfrs"] = "u_Furbolg Shaman";
			itemMap["ngna"] = "u_Gnoll Poacher";
			itemMap["ngns"] = "u_Gnoll Assassin";
			itemMap["ngno"] = "u_Gnoll";
			itemMap["ngnb"] = "u_Gnoll Brute";
			itemMap["ngnw"] = "u_Gnoll Warden";
			itemMap["ngnv"] = "u_Gnoll Overseer";
			itemMap["ngsp"] = "u_Goblin Sapper";
			itemMap["nhrr"] = "u_Harpy Rogue";
			itemMap["nhrw"] = "u_Harpy Windwitch";
			itemMap["nits"] = "u_Ice Troll Berserker";
			itemMap["nitt"] = "u_Ice Troll Trapper";
			itemMap["nkob"] = "u_Kobold";
			itemMap["nkog"] = "u_Kobold Geomancer";
			itemMap["nthl"] = "u_Thunder Lizard";
			itemMap["nmfs"] = "u_Murloc Flesheater";
			itemMap["nmrr"] = "u_Murloc Huntsman";
			itemMap["nowb"] = "u_Wildkin";
			itemMap["nrzm"] = "u_Razormane Medicine Man";
			itemMap["nnwa"] = "u_Nerubian Warrior";
			itemMap["nnwl"] = "u_Nerubian Webspinner";
			itemMap["nogr"] = "u_Ogre Warrior";
			itemMap["nogm"] = "u_Ogre Mauler";
			itemMap["nogl"] = "u_Ogre Lord";
			itemMap["nomg"] = "u_Ogre Magi";
			itemMap["nrvs"] = "u_Frost Revenant";
			itemMap["nslf"] = "u_Sludge Flinger";
			itemMap["nsts"] = "u_Satyr Shadowdancer";
			itemMap["nstl"] = "u_Satyr Soulstealer";
			itemMap["nzep"] = "u_Goblin Zeppelin";
			itemMap["ntrt"] = "u_Giant Sea Turtle";
			itemMap["nlds"] = "u_Makrura Deepseer";
			itemMap["nlsn"] = "u_Makrura Snapper";
			itemMap["nmsn"] = "u_Mur\'gul Snarecaster";
			itemMap["nscb"] = "u_Spider Crab Shorecrawler";
			itemMap["nbot"] = "u_Transport Ship";
			itemMap["nsc2"] = "u_Spider Crab Limbripper";
			itemMap["nsc3"] = "u_Spider Crab Behemoth";
			itemMap["nbdm"] = "u_Blue Dragonspawn Meddler";
			itemMap["nmgw"] = "u_Magnataur Warrior";
			itemMap["nanb"] = "u_Barbed Arachnathid";
			itemMap["nanm"] = "u_Barbed Arachnathid";
			itemMap["nfps"] = "u_Polar Furbolg Shaman";
			itemMap["nmgv"] = "u_Magic Vault";
			itemMap["nitb"] = "u_Icy Treasure Box";
			itemMap["npfl"] = "u_Fel Beast";
			itemMap["ndrd"] = "u_Draenei Darkslayer";
			itemMap["ndrm"] = "u_Draenei Disciple";
			itemMap["nvdw"] = "u_Voidwalker";
			itemMap["nvdg"] = "u_Greater Voidwalker";
			itemMap["nnht"] = "u_Nether Dragon Hatchling";
			itemMap["nndk"] = "u_Nether Drake";
			itemMap["nndr"] = "u_Nether Dragon";

            //human upgrades
			itemMap["Rhss"] = "p_Control Magic";
			itemMap["Rhme"] = "p_Swords";
			itemMap["Rhra"] = "p_Gunpowder";
			itemMap["Rhar"] = "p_Plating";
			itemMap["Rhla"] = "p_Armor";
			itemMap["Rhac"] = "p_Masonry";
			itemMap["Rhgb"] = "p_Flying Machine Bombs";
			itemMap["Rhlh"] = "p_Lumber Harvesting";
			itemMap["Rhde"] = "p_Defend";
			itemMap["Rhan"] = "p_Animal War Training";
			itemMap["Rhpt"] = "p_Priest Training";
			itemMap["Rhst"] = "p_Sorceress Training";
			itemMap["Rhri"] = "p_Long Rifles";
			itemMap["Rhse"] = "p_Magic Sentry";
			itemMap["Rhfl"] = "p_Flare";
			itemMap["Rhhb"] = "p_Storm Hammers";
			itemMap["Rhrt"] = "p_Barrage";
			itemMap["Rhpm"] = "p_Backpack";
			itemMap["Rhfc"] = "p_Flak Cannons";
			itemMap["Rhfs"] = "p_Fragmentation Shards";
			itemMap["Rhcd"] = "p_Cloud";

            //elf upgrades
			itemMap["Resm"] = "p_Strength of the Moon";
			itemMap["Resw"] = "p_Strength of the Wild";
			itemMap["Rema"] = "p_Moon Armor";
			itemMap["Rerh"] = "p_Reinforced Hides";
			itemMap["Reuv"] = "p_Ultravision";
			itemMap["Renb"] = "p_Nature\'s Blessing";
			itemMap["Reib"] = "p_Improved Bows";
			itemMap["Remk"] = "p_Marksmanship";
			itemMap["Resc"] = "p_Sentinel";
			itemMap["Remg"] = "p_Upgrade Moon Glaive";
			itemMap["Redt"] = "p_Druid of the Talon Training";
			itemMap["Redc"] = "p_Druid of the Claw Training";
			itemMap["Resi"] = "p_Abolish Magic";
			itemMap["Reht"] = "p_Hippogryph Taming";
			itemMap["Recb"] = "p_Corrosive Breath";
			itemMap["Repb"] = "p_Vorpal Blades";
			itemMap["Rers"] = "p_Resistant Skin";
			itemMap["Rehs"] = "p_Hardened Skin";
			itemMap["Reeb"] = "p_Mark of the Claw";
			itemMap["Reec"] = "p_Mark of the Talon";
			itemMap["Rews"] = "p_Well Spring";
			itemMap["Repm"] = "p_Backpack";
			itemMap["Roch"] = "p_Chaos";

            //orc upgrades
			itemMap["Rome"] = "p_Melee Weapons";
			itemMap["Rora"] = "p_Ranged Weapons";
			itemMap["Roar"] = "p_Armor";
			itemMap["Rwdm"] = "p_War Drums Damage Increase";
			itemMap["Ropg"] = "p_Pillage";
			itemMap["Robs"] = "p_Berserker Strength";
			itemMap["Rows"] = "p_Pulverize";
			itemMap["Roen"] = "p_Ensnare";
			itemMap["Rovs"] = "p_Envenomed Spears";
			itemMap["Rowd"] = "p_Witch Doctor Training";
			itemMap["Rost"] = "p_Shaman Training";
			itemMap["Rosp"] = "p_Spiked Barricades";
			itemMap["Rotr"] = "p_Troll Regeneration";
			itemMap["Rolf"] = "p_Liquid Fire";
			itemMap["Ropm"] = "p_Backpack";
			itemMap["Rowt"] = "p_Spirit Walker Training";
			itemMap["Robk"] = "p_Berserker Upgrade";
			itemMap["Rorb"] = "p_Reinforced Defenses";
			itemMap["Robf"] = "p_Burning Oil";

            //undead upgrades
			itemMap["Rusp"] = "p_Destroyer Form";
			itemMap["Rume"] = "p_Unholy Strength";
			itemMap["Rura"] = "p_Creature Attack";
			itemMap["Ruar"] = "p_Unholy Armor";
			itemMap["Rucr"] = "p_Creature Carapace";
			itemMap["Ruac"] = "p_Cannibalize";
			itemMap["Rugf"] = "p_Ghoul Frenzy";
			itemMap["Ruwb"] = "p_Web";
			itemMap["Rusf"] = "p_Stone Form";
			itemMap["Rune"] = "p_Necromancer Training";
			itemMap["Ruba"] = "p_Banshee Training";
			itemMap["Rufb"] = "p_Freezing Breath";
			itemMap["Rusl"] = "p_Skeletal Longevity";
			itemMap["Rupc"] = "p_Disease Cloud";
			itemMap["Rusm"] = "p_Skeletal Mastery";
			itemMap["Rubu"] = "p_Burrow";
			itemMap["Ruex"] = "p_Exhume Corpses";
			itemMap["Rupm"] = "p_Backpack";

            //items
			itemMap["amrc"] = "i_Amulet of Recall";
			itemMap["ankh"] = "i_Ankh of Reincarnation";
			itemMap["belv"] = "i_Boots of Quel\'Thalas +6";
			itemMap["bgst"] = "i_Belt of Giant Strength +6";
			itemMap["bspd"] = "i_Boots of Speed";
			itemMap["ccmd"] = "i_Scepter of Mastery";
			itemMap["ciri"] = "i_Robe of the Magi +6";
			itemMap["ckng"] = "i_Crown of Kings +5";
			itemMap["clsd"] = "i_Cloak of Shadows";
			itemMap["crys"] = "i_Crystal Ball";
			itemMap["desc"] = "i_Kelen\'s Dagger of Escape";
			itemMap["gemt"] = "i_Gem of True Seeing";
			itemMap["gobm"] = "i_Goblin Land Mines";
			itemMap["gsou"] = "i_Soul Gem";
			itemMap["guvi"] = "i_Glyph of Ultravision";
			itemMap["gfor"] = "i_Glyph of Fortification";
			itemMap["soul"] = "i_Soul";
			itemMap["mdpb"] = "i_Medusa Pebble";
			itemMap["rag1"] = "i_Slippers of Agility +3";
			itemMap["rat3"] = "i_Claws of Attack +3";
			itemMap["rin1"] = "i_Mantle of Intelligence +3";
			itemMap["rde1"] = "i_Ring of Protection +2";
			itemMap["rde2"] = "i_Ring of Protection +3";
			itemMap["rde3"] = "i_Ring of Protection +4";
			itemMap["rhth"] = "i_Khadgar\'s Gem of Health";
			itemMap["rst1"] = "i_Gauntlets of Ogre Strength +3";
			itemMap["ofir"] = "i_Orb of Fire";
			itemMap["ofro"] = "i_Orb of Frost";
			itemMap["olig"] = "i_Orb of Lightning";
			itemMap["oli2"] = "i_Orb of Lightning";
			itemMap["oven"] = "i_Orb of Venom";
			itemMap["odef"] = "i_Orb of Darkness";
			itemMap["ocor"] = "i_Orb of Corruption";
			itemMap["pdiv"] = "i_Potion of Divinity";
			itemMap["phea"] = "i_Potion of Healing";
			itemMap["pghe"] = "i_Potion of Greater Healing";
			itemMap["pinv"] = "i_Potion of Invisibility";
			itemMap["pgin"] = "i_Potion of Greater Invisibility";
			itemMap["pman"] = "i_Potion of Mana";
			itemMap["pgma"] = "i_Potion of Greater Mana";
			itemMap["pnvu"] = "i_Potion of Invulnerability";
			itemMap["pnvl"] = "i_Potion of Lesser Invulnerability";
			itemMap["pres"] = "i_Potion of Restoration";
			itemMap["pspd"] = "i_Potion of Speed";
			itemMap["rlif"] = "i_Ring of Regeneration";
			itemMap["rwiz"] = "i_Sobi Mask";
			itemMap["sfog"] = "i_Horn of the Clouds";
			itemMap["shea"] = "i_Scroll of Healing";
			itemMap["sman"] = "i_Scroll of Mana";
			itemMap["spro"] = "i_Scroll of Protection";
			itemMap["sres"] = "i_Scroll of Restoration";
			itemMap["ssil"] = "i_Staff of Silence";
			itemMap["stwp"] = "i_Scroll of Town Portal";
			itemMap["tels"] = "i_Goblin Night Scope";
			itemMap["tdex"] = "i_Tome of Agility";
			itemMap["texp"] = "i_Tome of Experience";
			itemMap["tint"] = "i_Tome of Intelligence";
			itemMap["tkno"] = "i_Tome of Power";
			itemMap["tstr"] = "i_Tome of Strength";
			itemMap["ward"] = "i_Warsong Battle Drums";
			itemMap["will"] = "i_Wand of Illusion";
			itemMap["wneg"] = "i_Wand of Negation";
			itemMap["rdis"] = "i_Rune of Dispel Magic";
			itemMap["rwat"] = "i_Rune of the Watcher";
			itemMap["fgrd"] = "i_Red Drake Egg";
			itemMap["fgrg"] = "i_Stone Token";
			itemMap["fgdg"] = "i_Demonic Figurine";
			itemMap["fgfh"] = "i_Spiked Collar";
			itemMap["fgsk"] = "i_Book of the Dead";
			itemMap["engs"] = "i_Enchanted Gemstone";
			itemMap["k3m1"] = "i_Mooncrystal";
			itemMap["modt"] = "i_Mask of Death";
			itemMap["sand"] = "i_Scroll of Animate Dead";
			itemMap["srrc"] = "i_Scroll of Resurrection";
			itemMap["sror"] = "i_Scroll of the Beast";
			itemMap["infs"] = "i_Inferno Stone";
			itemMap["shar"] = "i_Ice Shard";
			itemMap["wild"] = "i_Amulet of the Wild";
			itemMap["wswd"] = "i_Sentry Wards";
			itemMap["whwd"] = "i_Healing Wards";
			itemMap["wlsd"] = "i_Wand of Lightning Shield";
			itemMap["wcyc"] = "i_Wand of the Wind";
			itemMap["rnec"] = "i_Rod of Necromancy";
			itemMap["pams"] = "i_Anti-magic Potion";
			itemMap["clfm"] = "i_Cloak of Flames";
			itemMap["evtl"] = "i_Talisman of Evasion";
			itemMap["nspi"] = "i_Necklace of Spell Immunity";
			itemMap["lhst"] = "i_The Lion Horn of Stormwind";
			itemMap["kpin"] = "i_Khadgar\'s Pipe of Insight";
			itemMap["sbch"] = "i_Scourge Bone Chimes";
			itemMap["afac"] = "i_Alleria\'s Flute of Accuracy";
			itemMap["ajen"] = "i_Ancient Janggo of Endurance";
			itemMap["lgdh"] = "i_Legion Doom-Horn";
			itemMap["hcun"] = "i_Hood of Cunning";
			itemMap["mcou"] = "i_Medallion of Courage";
			itemMap["hval"] = "i_Helm of Valor";
			itemMap["cnob"] = "i_Circlet of Nobility";
			itemMap["prvt"] = "i_Periapt of Vitality";
			itemMap["tgxp"] = "i_Tome of Greater Experience";
			itemMap["mnst"] = "i_Mana Stone";
			itemMap["hlst"] = "i_Health Stone";
			itemMap["tpow"] = "i_Tome of Knowledge";
			itemMap["tst2"] = "i_Tome of Strength +2";
			itemMap["tin2"] = "i_Tome of Intelligence +2";
			itemMap["tdx2"] = "i_Tome of Agility +2";
			itemMap["rde0"] = "i_Ring of Protection +1";
			itemMap["rde4"] = "i_Ring of Protection +5";
			itemMap["rat6"] = "i_Claws of Attack +6";
			itemMap["rat9"] = "i_Claws of Attack +9";
			itemMap["ratc"] = "i_Claws of Attack +12";
			itemMap["ratf"] = "i_Claws of Attack +15";
			itemMap["manh"] = "i_Manual of Health";
			itemMap["pmna"] = "i_Pendant of Mana";
			itemMap["penr"] = "i_Pendant of Energy";
			itemMap["gcel"] = "i_Gloves of Haste";
			itemMap["totw"] = "i_Talisman of the Wild";
			itemMap["phlt"] = "i_Phat Lewt";
			itemMap["gopr"] = "i_Glyph of Purification";
			itemMap["ches"] = "i_Cheese";
			itemMap["mlst"] = "i_Maul of Strength";
			itemMap["rnsp"] = "i_Ring of Superiority";
			itemMap["brag"] = "i_Bracer of Agility";
			itemMap["sksh"] = "i_Skull Shield";
			itemMap["vddl"] = "i_Voodoo Doll";
			itemMap["sprn"] = "i_Spider Ring";
			itemMap["tmmt"] = "i_Totem of Might";
			itemMap["anfg"] = "i_Ancient Figurine";
			itemMap["lnrn"] = "i_Lion\'s Ring";
			itemMap["iwbr"] = "i_Ironwood Branch";
			itemMap["jdrn"] = "i_Jade Ring";
			itemMap["drph"] = "i_Druid Pouch";
			itemMap["hslv"] = "i_Healing Salve";
			itemMap["pclr"] = "i_Clarity Potion";
			itemMap["plcl"] = "i_Lesser Clarity Potion";
			itemMap["rej1"] = "i_Minor Replenishment Potion";
			itemMap["rej2"] = "i_Lesser Replenishment Potion";
			itemMap["rej3"] = "i_Replenishment Potion";
			itemMap["rej4"] = "i_Greater Replenishment Potion";
			itemMap["rej5"] = "i_Lesser Scroll of Replenishment ";
			itemMap["rej6"] = "i_Greater Scroll of Replenishment ";
			itemMap["sreg"] = "i_Scroll of Regeneration";
			itemMap["gold"] = "i_Gold Coins";
			itemMap["lmbr"] = "i_Bundle of Lumber";
			itemMap["fgun"] = "i_Flare Gun";
			itemMap["pomn"] = "i_Potion of Omniscience";
			itemMap["gomn"] = "i_Glyph of Omniscience";
			itemMap["wneu"] = "i_Wand of Neutralization";
			itemMap["silk"] = "i_Spider Silk Broach";
			itemMap["lure"] = "i_Monster Lure";
			itemMap["skul"] = "i_Sacrificial Skull";
			itemMap["moon"] = "i_Moonstone";
			itemMap["brac"] = "i_Runed Bracers";
			itemMap["vamp"] = "i_Vampiric Potion";
			itemMap["woms"] = "i_Wand of Mana Stealing";
			itemMap["tcas"] = "i_Tiny Castle";
			itemMap["tgrh"] = "i_Tiny Great Hall";
			itemMap["tsct"] = "i_Ivory Tower";
			itemMap["wshs"] = "i_Wand of Shadowsight";
			itemMap["tret"] = "i_Tome of Retraining";
			itemMap["sneg"] = "i_Staff of Negation";
			itemMap["stel"] = "i_Staff of Teleportation";
			itemMap["spre"] = "i_Staff of Preservation";
			itemMap["mcri"] = "i_Mechanical Critter";
			itemMap["spsh"] = "i_Amulet of Spell Shield";
			itemMap["sbok"] = "i_Spell Book";
			itemMap["ssan"] = "i_Staff of Sanctuary";
			itemMap["shas"] = "i_Scroll of Speed";
			itemMap["dust"] = "i_Dust of Appearance";
			itemMap["oslo"] = "i_Orb of Slow";
			itemMap["dsum"] = "i_Diamond of Summoning";
			itemMap["sor1"] = "i_Shadow Orb +1";
			itemMap["sor2"] = "i_Shadow Orb +2";
			itemMap["sor3"] = "i_Shadow Orb +3";
			itemMap["sor4"] = "i_Shadow Orb +4";
			itemMap["sor5"] = "i_Shadow Orb +5";
			itemMap["sor6"] = "i_Shadow Orb +6";
			itemMap["sor7"] = "i_Shadow Orb +7";
			itemMap["sor8"] = "i_Shadow Orb +8";
			itemMap["sor9"] = "i_Shadow Orb +9";
			itemMap["sora"] = "i_Shadow Orb +10";
			itemMap["sorf"] = "i_Shadow Orb Fragment";
			itemMap["fwss"] = "i_Frost Wyrm Skull Shield";
			itemMap["ram1"] = "i_Ring of the Archmagi";
			itemMap["ram2"] = "i_Ring of the Archmagi";
			itemMap["ram3"] = "i_Ring of the Archmagi";
			itemMap["ram4"] = "i_Ring of the Archmagi";
			itemMap["shtm"] = "i_Shamanic Totem";
			itemMap["shwd"] = "i_Shimmerweed";
			itemMap["btst"] = "i_Battle Standard";
			itemMap["skrt"] = "i_Skeletal Artifact";
			itemMap["thle"] = "i_Thunder Lizard Egg";
			itemMap["sclp"] = "i_Secret Level Powerup";
			itemMap["gldo"] = "i_Orb of Kil\'jaeden";
			itemMap["tbsm"] = "i_Tiny Blacksmith";
			itemMap["tfar"] = "i_Tiny Farm";
			itemMap["tlum"] = "i_Tiny Lumber Mill";
			itemMap["tbar"] = "i_Tiny Barracks";
			itemMap["tbak"] = "i_Tiny Altar of Kings";
			itemMap["mgtk"] = "i_Magic Key Chain";
			itemMap["stre"] = "i_Staff of Reanimation";
			itemMap["horl"] = "i_Sacred Relic";
			itemMap["hbth"] = "i_Helm of Battlethirst";
			itemMap["blba"] = "i_Bladebane Armor";
			itemMap["rugt"] = "i_Runed Gauntlets";
			itemMap["frhg"] = "i_Firehand Gauntlets";
			itemMap["gvsm"] = "i_Gloves of Spell Mastery";
			itemMap["crdt"] = "i_Crown of the Deathlord";
			itemMap["arsc"] = "i_Arcane Scroll";
			itemMap["scul"] = "i_Scroll of the Unholy Legion";
			itemMap["tmsc"] = "i_Tome of Sacrifices";
			itemMap["dtsb"] = "i_Drek\'thar\'s Spellbook";
			itemMap["grsl"] = "i_Grimoire of Souls";
			itemMap["arsh"] = "i_Arcanite Shield";
			itemMap["shdt"] = "i_Shield of the Deathlord";
			itemMap["shhn"] = "i_Shield of Honor";
			itemMap["shen"] = "i_Enchanted Shield";
			itemMap["thdm"] = "i_Thunderlizard Diamond";
			itemMap["stpg"] = "i_Clockwork Penguin";
			itemMap["shrs"] = "i_Shimmerglaze Roast";
			itemMap["bfhr"] = "i_Bloodfeather\'s Heart";
			itemMap["cosl"] = "i_Celestial Orb of Souls";
			itemMap["shcw"] = "i_Shaman Claws";
			itemMap["srbd"] = "i_Searing Blade";
			itemMap["frgd"] = "i_Frostguard";
			itemMap["envl"] = "i_Enchanted Vial";
			itemMap["rump"] = "i_Rusty Mining Pick";
			itemMap["mort"] = "i_Mogrin\'s Report";
			itemMap["srtl"] = "i_Serathil";
			itemMap["stwa"] = "i_Sturdy War Axe";
			itemMap["klmm"] = "i_Killmaim";
			itemMap["rots"] = "i_Scepter of the Sea";
			itemMap["axas"] = "i_Ancestral Staff";
			itemMap["mnsf"] = "i_Mindstaff";
			itemMap["schl"] = "i_Scepter of Healing";
			itemMap["asbl"] = "i_Assassin\'s Blade";
			itemMap["kgal"] = "i_Keg of Ale";
			itemMap["dphe"] = "i_Thunder Phoenix Egg";
			itemMap["dkfw"] = "i_Keg of Thunderwater";
			itemMap["dthb"] = "i_Thunderbloom Bulb";

            //human buildings
		    itemMap["halt"] = "Altar of Kings";
		    itemMap["harm"] = "Workshop";
		    itemMap["hars"] = "Arcane Sanctum";
		    itemMap["hbar"] = "Barracks";
		    itemMap["hbla"] = "Blacksmith";
		    itemMap["hhou"] = "Farm";
		    itemMap["hgra"] = "Gryphon Aviary";
		    itemMap["hwtw"] = "Scout Tower";
		    itemMap["hvlt"] = "Arcane Vault";
		    itemMap["hlum"] = "Lumber Mill";
		    itemMap["htow"] = "Town Hall";

            //elf buildings
		    itemMap["etrp"] = "Ancient Protector";
		    itemMap["etol"] = "Tree of Life";
		    itemMap["edob"] = "Hunter\'s Hall";
		    itemMap["eate"] = "Altar of Elders";
		    itemMap["eden"] = "Ancient of Wonders";
		    itemMap["eaoe"] = "Ancient of Lore";
		    itemMap["eaom"] = "Ancient of War";
		    itemMap["eaow"] = "Ancient of Wind";
		    itemMap["edos"] = "Chimaera Roost";
		    itemMap["emow"] = "Moon Well";

            //orc buildings
		    itemMap["oalt"] = "Altar of Storms";
		    itemMap["obar"] = "Barracks";
		    itemMap["obea"] = "Beastiary";
		    itemMap["ofor"] = "War Mill";
		    itemMap["ogre"] = "Great Hall";
		    itemMap["osld"] = "Spirit Lodge";
		    itemMap["otrb"] = "Orc Burrow";
		    itemMap["orbr"] = "Reinforced Orc Burrow";
		    itemMap["otto"] = "Tauren Totem";
		    itemMap["ovln"] = "Voodoo Lounge";
		    itemMap["owtw"] = "Watch Tower";

            //undead buildings
		    itemMap["uaod"] = "Altar of Darkness";
		    itemMap["unpl"] = "Necropolis";
		    itemMap["usep"] = "Crypt";
		    itemMap["utod"] = "Temple of the Damned";
		    itemMap["utom"] = "Tomb of Relics";
		    itemMap["ugol"] = "Haunted Gold Mine";
		    itemMap["uzig"] = "Ziggurat";
		    itemMap["ubon"] = "Boneyard";
		    itemMap["usap"] = "Sacrificial Pit";
		    itemMap["uslh"] = "Slaughterhouse";
		    itemMap["ugrv"] = "Graveyard";
        }

        public static string ConvertColor(byte val)
        {
            switch (val)
            {
		        case 0: return "Red";
		        case 1: return "Blue";
		        case 2: return "Teal";
		        case 3: return "Purple";
		        case 4: return "Yellow";
		        case 5: return "Orange";
		        case 6: return "Green";
		        case 7: return "Pink";
		        case 8: return "Gray";
		        case 9: return "Light-Blue";
		        case 10: return "Dark-Green";
		        case 11: return "Brown";
		        case 12: return "Observer";
                default: return "";
            }
        }

        public static string ConvertRace(byte val)
        {
            switch (val)
            {
                case 0x01:
                case 0x41: return "Human";
                case 0x02:
                case 0x42: return "Orc";
                case 0x04:
                case 0x44: return "Night Elf";
                case 0x08:
                case 0x48: return "Undead";
                case 0x20: return "Random";
                default: return "Observer";
            };
        }

        public static string ConvertShortRace(byte val)
        {
            switch (val)
            {
                case 0x01:
                case 0x41: return "H";
                case 0x02:
                case 0x42: return "O";
                case 0x04:
                case 0x44: return "E";
                case 0x08:
                case 0x48: return "U";
                case 0x20: return "R";
                default: return "Obs";
            };
        }

        public static string ConvertItemID(string itemID)
        {
            try
            {
                return itemMap[itemID];
            }
            catch (KeyNotFoundException)
            {
                return itemID;
            }
        }
    }
}
