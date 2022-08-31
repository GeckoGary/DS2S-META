﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS2S_META.Resources.Randomizer
{
    internal enum KEYID : int
    {
        NONE = 0x0,                 // default
        BELFRYLUNA      = 0xBB0,    // Shorthand: Branch && Pharros
        SINNERSRISE     = 0xBB1,    // Shorthand: Branch || Antiquated 
        DRANGLEIC       = 0xBB2,    // Shorthand: All conditions for drangleic
        AMANA           = 0xBB3,    // Shorthand: Drangleic + King's passage
        ALDIASKEEP      = 0xBB4,    // Shorthand: Branch + King's Ring
        MEMORYJEIGH     = 0xBB5,    // Shorthand: King's Ring + Ashen Mist
        GANKSQUAD       = 0xBB6,    // Shorthand: DLC1 + Eternal Sanctum
        PUZZLINGSWORD   = 0xBB7,    // Shorthand TO CONSIDER: Might need range
        ELANA           = 0xBB8,    // Shorthand: DLC1 + Dragon Stone
        FUME            = 0xBB9,    // Shorthand: DLC2 + Scorching Sceptor
        BLUESMELTER     = 0xBBA,    // Shorthand: DLC2 + Tower Key
        ALONNE          = 0xBBB,    // Shorthand: DLC2 + Tower Key + Scorching Scepter + Ashen Mist
        DLC3            = 0xBBC,    // Shorthand: DLC3 + Drangleic
        FRIGIDOUTSKIRTS = 0xBBD,    // Shorthand: DLC3 + Garrison Ward Key
        CREDITS         = 0xBBE,    // Shorthand: Everything required to beat Nash

        BRANCH          = 0xAA0,    // Shorthand: Enough Branches available
        PHARROS         = 0xAA1,    // Shorthand: Enough Pharros Lockstones available
        NADALIA         = 0xAA2,    // Shorthand: DLC2 + enough Smelter Wedges 
        

        SOLDIER         = 50600000,
        FORGOTTEN       = 50820000,
        TOWER           = 52400000,
        KINSHIP         = 50900000,
        ASHENMIST       = 50910000,
        GARRISONWARD    = 52500000,
        ALDIASKEY       = 51030000,
        SCEPTER         = 53100000,
        KINGSRING       = 40510000,
        DRAGONSTONE     = 52650000,
        DLC1            = 52000000,
        DLC2            = 52100000,
        DLC3KEY         = 52200000,
        EMBEDDED        = 1980000,
        KINGSPASSAGE    = 50610000,
        ETERNALSANCTUM  = 52300000,
        UNDEADLOCKAWAY  = 50970000,
        BASTILLEKEY     = 50800000,
        IRON            = 50810000,
        ANTIQUATED      = 50840000,
        MANSION         = 50860000,
        BRIGHTSTONE     = 50830000,
        FANG            = 50850000,
        ROTUNDA         = 50890000,
        TSELDORADEN     = 50930000,
        BLACKSMITH      = 50870000, // (Lenigrast's key)
        DULLEMBER       = 50990000,
        TORCH           = 60420000,
        PETRIFIEDEGG    = 62190000,
        WHISPERS        = 40610000,
        SOULOFAGIANT    = 50920000,
        CRUSHEDEYEORB   = 51000000,

    }

    internal enum PICKUPTYPE : int
    {
        COVENANT,
        NPC,
        WOODCHEST,
        METALCHEST,
        VOLATILE, // misc volatile
        NONVOLATILE, // this is basically corpse pickups now
        BOSS,
        NGPLUS,
        UNRESOLVED,
    }

    internal class RandoInfo
    {
        internal Dictionary<int, RandoInfo> D = new Dictionary<int, RandoInfo>();

        // Overloads for quick construction, single or no key requirements:
        internal RandoInfo NpcInfo(string desc, KEYID reqkey = KEYID.NONE)
        {
            return new RandoInfo(desc, PICKUPTYPE.NPC, new KeySet(reqkey));
        }
        internal RandoInfo NpcSafeInfo(string desc, KEYID reqkey = KEYID.NONE)
        {
            return new RandoInfo(desc, TypeArray(PICKUPTYPE.NPC, PICKUPTYPE.NONVOLATILE), new KeySet(reqkey));
        }
        internal RandoInfo CovInfo(string desc, KEYID reqkey = KEYID.NONE)
        {

            return new RandoInfo(desc, PICKUPTYPE.COVENANT, new KeySet(reqkey));
        }
        internal RandoInfo WChestInfo(string desc, KEYID reqkey = KEYID.NONE)
        {
            return new RandoInfo(desc, PICKUPTYPE.WOODCHEST, new KeySet(reqkey));
        }
        internal RandoInfo MChestInfo(string desc, KEYID reqkey = KEYID.NONE)
        {
            return new RandoInfo(desc, PICKUPTYPE.METALCHEST, new KeySet(reqkey));
        }
        internal RandoInfo NGPlusInfo(string desc, KEYID reqkey = KEYID.NONE)
        {
            return new RandoInfo(desc, PICKUPTYPE.NGPLUS, new KeySet(reqkey));
        }
        internal RandoInfo WChestNGPlusInfo(string desc, KEYID reqkey = KEYID.NONE)
        {
            return new RandoInfo(desc, TypeArray(PICKUPTYPE.WOODCHEST, PICKUPTYPE.NGPLUS), new KeySet(reqkey));
        }
        internal RandoInfo MChestNGPlusInfo(string desc, KEYID reqkey = KEYID.NONE)
        {
            return new RandoInfo(desc, TypeArray(PICKUPTYPE.METALCHEST, PICKUPTYPE.NGPLUS), new KeySet(reqkey));
        }
        internal RandoInfo SafeInfo(string desc, KEYID reqkey = KEYID.NONE)
        {
            return new RandoInfo(desc, PICKUPTYPE.NONVOLATILE, new KeySet(reqkey));
        }
        internal RandoInfo VolInfo(string desc, KEYID reqkey = KEYID.NONE)
        {
            return new RandoInfo(desc, PICKUPTYPE.VOLATILE, new KeySet(reqkey));
        }
        internal RandoInfo UnresolvedInfo(string desc, KEYID reqkey = KEYID.NONE)
        {
            return new RandoInfo(desc, PICKUPTYPE.UNRESOLVED, new KeySet(reqkey));
        }
        internal RandoInfo BossInfo(string desc, KEYID reqkey = KEYID.NONE)
        {
            // This is essentially a flag on top of safeinfo
            return new RandoInfo(desc, PICKUPTYPE.BOSS, new KeySet(reqkey));
        }
        internal RandoInfo BossNGPlusInfo(string desc, KEYID reqkey = KEYID.NONE)
        {
            // This is essentially a flag on top of safeinfo
            return new RandoInfo(desc, TypeArray(PICKUPTYPE.BOSS, PICKUPTYPE.NGPLUS), new KeySet(reqkey));
        }

        // Overloads for multiple key options:
        internal RandoInfo NpcInfo(string desc, params KeySet[] keysets)
        {
            return new RandoInfo(desc, PICKUPTYPE.NPC, keysets);
        }
        internal RandoInfo NpcSafeInfo(string desc, params KeySet[] keysets)
        {
            return new RandoInfo(desc, TypeArray(PICKUPTYPE.NPC, PICKUPTYPE.NONVOLATILE), keysets);
        }
        internal RandoInfo CovInfo(string desc, params KeySet[] keysets)
        {
            return new RandoInfo(desc, PICKUPTYPE.COVENANT, keysets);
        }
        internal RandoInfo WChestInfo(string desc, params KeySet[] keysets)
        {
            return new RandoInfo(desc, PICKUPTYPE.WOODCHEST, keysets);
        }
        internal RandoInfo MChestInfo(string desc, params KeySet[] keysets)
        {
            return new RandoInfo(desc, PICKUPTYPE.METALCHEST, keysets);
        }
        internal RandoInfo NGPlusInfo(string desc, params KeySet[] keysets)
        {
            return new RandoInfo(desc, PICKUPTYPE.NGPLUS, keysets);
        }
        internal RandoInfo WChestNGPlusInfo(string desc, params KeySet[] keysets)
        {
            return new RandoInfo(desc, TypeArray(PICKUPTYPE.WOODCHEST, PICKUPTYPE.NGPLUS), keysets);
        }
        internal RandoInfo MChestNGPlusInfo(string desc, params KeySet[] keysets)
        {
            return new RandoInfo(desc, TypeArray(PICKUPTYPE.METALCHEST, PICKUPTYPE.NGPLUS), keysets);
        }
        internal RandoInfo SafeInfo(string desc, params KeySet[] keysets)
        {
            return new RandoInfo(desc, PICKUPTYPE.NONVOLATILE, keysets);
        }
        internal RandoInfo VolInfo(string desc, params KeySet[] keysets)
        {
            return new RandoInfo(desc, PICKUPTYPE.VOLATILE, keysets);
        }
        internal RandoInfo BossInfo(string desc, params KeySet[] keysets)
        {
            // This is essentially a flag on top of safeinfo
            return new RandoInfo(desc, PICKUPTYPE.BOSS, keysets);
        }
        internal RandoInfo BossNGPlusInfo(string desc, params KeySet[] keysets)
        {
            return new RandoInfo(desc, TypeArray(PICKUPTYPE.BOSS, PICKUPTYPE.NGPLUS), keysets);
        }
        
        // Main class constructor
        internal RandoInfo(string desc, PICKUPTYPE type, params KeySet[] reqkeys)
        {
        }
        internal RandoInfo(string desc, PICKUPTYPE[] flags, params KeySet[] reqkeys)
        {
        }

        // Utility shorthand methods (for common purposes):
        internal PICKUPTYPE[] TypeArray(params PICKUPTYPE[] types)
        {
            return types;
        }
        internal KeySet KSO(params KEYID[] keys) // KeySetOption
        {
            return new KeySet(keys);
        }
        
        // Main info setup (no skips):
        internal void setupLootInfoDictionary()
        {
            // Multi-location events:
            D.Add(1726000, NpcInfo("Gift from Gavlan after spending 16000 souls"));
            D.Add(1752010, NpcInfo("Gift from Lucatiel after speaking to her the second time"));
            D.Add(1752020, NpcInfo("Gift from Lucatiel after speaking to her the third time"));
            D.Add(1753020, CovInfo("belfry luna/sol, bell guardians join"));
            D.Add(1754000, NpcInfo("Gift from Melentia after spending 10000 souls"));
            D.Add(1766000, NpcInfo("Gift from Carhillion when over 30INT"));
            D.Add(1769000, NpcInfo("Gift from Licia when over 30FTH"));
            D.Add(2004011, CovInfo("Bell Keepers 1st rank price", KEYID.PHARROS));
            D.Add(2004012, CovInfo("Bell Keepers 2nd rank price", KEYID.PHARROS));
            D.Add(2004013, CovInfo("Bell Keepers 3rd rank price", KEYID.PHARROS));
            D.Add(2005000, CovInfo("Rat King covenant join"));
            D.Add(2005011, CovInfo("Rat King covenant 1st rank price"));
            D.Add(2005012, CovInfo("Rat King covenant 2nd rank price"));
            D.Add(2005013, CovInfo("Rat King covenant 3rd rank price"));
            D.Add(60006000, NpcInfo("Reward for killing Licia using the Crushed Eye Orb", KSO(KEYID.CRUSHEDEYEORB, KEYID.BRANCH))); // consider branch number todo
            D.Add(60007000, CovInfo("Vanilla Lingering Dragoncrest +2: reward for killing 1000 invading Red Phantoms"));
            D.Add(60007100, CovInfo("Vanilla Ring of Thorns +2: reward for invading and killing 1000 other worlds"));
            D.Add(60007200, VolInfo("Vanilla Illusory Ring of a Conqueror: reward for completing the game without dying", KEYID.CREDITS));
            D.Add(60007300, VolInfo("Vanilla Illusory Ring of the Exalted: reward for completing the game without taking a bonfire", KEYID.CREDITS));
            D.Add(99996000, UnresolvedInfo("One soul of a lost undead for something?"));

            // Betwixt things:
            D.Add(1705000, NpcInfo("Gift from the fire keepers after getting the King's Ring", KEYID.DRANGLEIC));
            D.Add(1723000, NpcInfo("Gift from Milibeth after killing 3 hippos", KEYID.BRANCH));
            D.Add(10025010, WChestInfo("Wooden chest at the attic of Fire Keeper's cottage"));
            D.Add(10026000, SafeInfo("On the right side ledge before Fire Keeper's cottage"));
            D.Add(10026001, NGPlusInfo("On the right side ledge before Fire Keeper's cottage in NG+"));
            D.Add(10026020, SafeInfo("Guarded by hippo before Fire Keeper's cottage"));
            D.Add(10026030, SafeInfo("Under Betwixt waterfall"));
            D.Add(10026031, NGPlusInfo("Under Betwixt waterfall in NG+"));
            D.Add(10026040, SafeInfo("one ledge down from the crow nest"));
            D.Add(10026050, SafeInfo("Behind the wagon next to bonfire"));
            D.Add(10026060, SafeInfo("Behind a door in small alcove"));
            D.Add(10026070, SafeInfo("On path after the backstab tutorial"));
            D.Add(10026080, SafeInfo("Next to fog gate and archer"));
            D.Add(10026090, SafeInfo("Other side of the jumpable gap"));
            D.Add(10026100, SafeInfo("In the basilisk pit", KEYID.BRANCH));
            D.Add(60008100, SafeInfo("Betwixt Pursuer drop"));
            D.Add(60008110, NGPlusInfo("Betwixt Pursuer drop in NG+"));

            // Majula:
            D.Add(1700000, NpcInfo("Gift from the Emerald Herald"));
            D.Add(1704000, NpcInfo("Gift from Gilligan after buying the longest ladders", KEYID.ROTUNDA));
            D.Add(1741000, NpcInfo("Gift from Saulden after 100 deaths"));
            D.Add(1741010, NpcInfo("Gift from Saulden after bringing 4 people to Majula"));
            D.Add(1751010, NpcInfo("Gift from Cale after lighting all fires in the map"));
            D.Add(1761000, NpcInfo("Gift from Maughlin after spending 15000 souls and then speaking to him with no souls"));
            D.Add(1762000, NpcInfo("Gift from Chloanne after spending 20000 souls", KEYID.ROTUNDA));
            D.Add(1763000, NpcInfo("Gift from Rosabeth after unpetrifying her", KEYID.BRANCH));
            D.Add(1764000, NpcInfo("Gift from Lenigrast after spending 8000 souls on upgrades", KEYID.BLACKSMITH));
            D.Add(2001000, CovInfo("Way of Blue join"));
            D.Add(2001011, CovInfo("Way of Blue 1st rank price"));
            D.Add(2001012, CovInfo("Way of Blue 2nd rank price"));
            D.Add(2001013, CovInfo("Way of Blue 3rd rank price"));
            D.Add(2009000, CovInfo("Company of Champions join"));
            D.Add(2009011, CovInfo("Company of Champions 1st rank price"));
            D.Add(2009012, CovInfo("Company of Champions 2nd rank price"));
            D.Add(2009013, CovInfo("Company of Champions 3rd rank price"));
            D.Add(10045000, WChestInfo("Wooden chest on the attic of Maughlin the Armorer"));
            D.Add(10045001, WChestNGPlusInfo("Wooden chest on the attic of Maughlin the Armorer in NG+1"));
            D.Add(10045002, WChestNGPlusInfo("Wooden chest on the attic of Maughlin the Armorer in NG+2"));
            D.Add(10045010, WChestInfo("Wooden chest on the attic of Cale's house", KEYID.MANSION));
            D.Add(10045040, WChestInfo("Wooden chest in Lenigrast's workshop", KEYID.BLACKSMITH));
            D.Add(10045060, MChestInfo("Metal chest on a way towards forest"));
            D.Add(10045070, MChestInfo("Metal chest in staircase towards Heide's Tower of Flame"));
            D.Add(10045600, MChestInfo("Metal chest in Cale's house basement", KEYID.MANSION));
            D.Add(10046000, SafeInfo("Majula well"));
            D.Add(10046010, SafeInfo("Under the tree on the way from Things Betwixt"));
            D.Add(10046020, SafeInfo("Drop from cliff"));
            D.Add(10046030, SafeInfo("Vanilla Binos: Drop from cliff and end of the path"));
            D.Add(10046040, SafeInfo("Next to Lenigrast's workshop"));
            D.Add(10046070, SafeInfo("Corpse in Cale's house basement", KEYID.MANSION));
            D.Add(10046100, SafeInfo("Library room in Cale's house", KEYID.MANSION));
            D.Add(10046110, SafeInfo("Next to Champion's covenant"));
            D.Add(10046120, SafeInfo("Tent next to cat Shalquoir"));
            D.Add(10296000, SafeInfo("In small room next to petrified Rosabeth"));
            D.Add(10296010, SafeInfo("Corpse next to Benhart"));

            // Forest of Fallen Giants:
            D.Add(309600, BossInfo("Last giant drop"));
            D.Add(318000, BossInfo("Pursuer (in the proper arena) drop", KEYID.SOLDIER));
            D.Add(1744020, NpcInfo("Gift from Pate after escaping the gate trap room"));
            D.Add(1751000, NpcInfo("Gift from Cale when talking to him"));
            D.Add(10105010, WChestInfo("Wooden chest in upper floor of cardinal tower"));
            D.Add(10105020, WChestNGPlusInfo("Wooden chest under the bridge after the drawgate"));
            D.Add(10105021, WChestNGPlusInfo("Wooden chest under the bridge after the drawgate in NG+"));
            D.Add(10105030, MChestInfo("Metal chest in the side room under the ballista-trap"));
            D.Add(10105040, WChestInfo("Trapped wooden chest under the ballista-trap"));
            D.Add(10105041, WChestNGPlusInfo("Trapped wooden chest under the ballista-trap in NG+"));
            D.Add(10105050, WChestInfo("Wooden chest in a side corridor on the way to the king's door", KSO(KEYID.SOLDIER) ) );
            D.Add(10105070, MChestInfo("Metal chest in upper floor of cardinal tower"));
            D.Add(10105080, MChestInfo("First metal chest behind Pharros' contraption under the ballista-trap", KEYID.PHARROS));
            D.Add(10105090, MChestInfo("Second metal chest behind Pharros' contraption under the ballista-trap", KEYID.PHARROS));
            D.Add(10105100, MChestInfo("Metal chest in the guard room over the courtyard"));
            D.Add(10105110, MChestInfo("Metal chest in the top-level of Salamander pit"));
            D.Add(10105120, WChestInfo("Wooden chest near soldier's rest bonfire", KEYID.SOLDIER));
            D.Add(10105130, MChestInfo("Vanilla fire longsword: Metal chest in the tunnel where you get fireballed by salamander"));
            D.Add(10105140, MChestInfo("Metal chest behind a illusory wall after the gate trap room"));
            D.Add(10105150, WChestInfo("Wooden chest next to king's door", KSO(KEYID.SOLDIER)) );
            D.Add(10106000, SafeInfo("At the end of the small stream"));
            D.Add(10106010, SafeInfo("First corpse at rooftop near soldier's rest bonfire", KEYID.SOLDIER));
            D.Add(10106030, SafeInfo("Next to turtle in seashore hall"));
            D.Add(10106050, SafeInfo("At the tree branch in Cardinal Tower"));
            D.Add(10106060, SafeInfo("Behind a shelf in small side room in the beginning of the area"));
            D.Add(10106061, NGPlusInfo("Behind a shelf in small side room in the beginning of the area in NG+"));
            D.Add(10106070, SafeInfo("In the beginning of the skeleton tunnel", KEYID.SOLDIER));
            D.Add(10106080, SafeInfo("In the end of the skeleton tunnel", KEYID.SOLDIER));
            D.Add(10106090, SafeInfo("Above the door leading to pursuer arena"));
            D.Add(10106100, SafeInfo("On the huge sword"));
            D.Add(10106110, SafeInfo("Corpse in the watery cave in the beginning of the area"));
            D.Add(10106120, SafeInfo("In the small stone house near Soldier's Rest bonfire", KEYID.SOLDIER));
            D.Add(60002000, SafeInfo("Vanilla Seed of a Tree of Giants: on the tree near Solider's Rest bonfire", KEYID.SOLDIER));
            D.Add(10106130, SafeInfo("Next to ruined house by water"));
            D.Add(10106140, SafeInfo("Behind the soldier that stands on tree root and throws fire bombs"));
            D.Add(10106150, SafeInfo("Just before Cale on boulder path"));
            D.Add(10106160, SafeInfo("At corridor after the gate trap"));
            D.Add(10106170, SafeInfo("On higher level where you jump from circular room at the beginning of the area"));
            D.Add(10106180, SafeInfo("Behind the stairs outside the ballista trap room"));
            D.Add(10106190, SafeInfo("Behind the giant-tree in the middle courtyard"));
            D.Add(10106200, SafeInfo("On upper ledge of the circular room"));
            D.Add(10106210, SafeInfo("On a giant tree root"));
            D.Add(10106220, SafeInfo("On the wall after climbing the ladder near a Giant"));
            D.Add(10106230, SafeInfo("A corpse in the circular room before the first fog gate"));
            D.Add(10106240, SafeInfo("In the round tower where you drop from a tree root"));
            D.Add(10106250, SafeInfo("Lower end of dead end stairs near the seaside fog gate"));
            D.Add(10106260, SafeInfo("A corpse in the circular room before the first fog gate"));
            D.Add(10106270, SafeInfo("On ledge near the scaffolding in front of Cardinal Tower"));
            D.Add(10106280, SafeInfo("A corpse in the circular room before the first fog gate"));
            D.Add(10106290, SafeInfo("Vanilla Drangleic set: In a crevasse in floor near the eagles nest", KEYID.SOLDIER));
            D.Add(10106300, SafeInfo("On the other end of the small bridge going over the fire area"));
            D.Add(10106310, SafeInfo("In small tunnel guarded by turtle"));
            D.Add(10106320, SafeInfo("On ledge next to Cale"));
            D.Add(10106321, NGPlusInfo("On ledge next to Cale in NG+"));
            D.Add(10106340, SafeInfo("On floor in the salamander pit"));
            D.Add(10106350, SafeInfo("First corpse in the lower fire area"));
            D.Add(10106360, SafeInfo("Second corpse in the lower fire area"));
            D.Add(10106370, SafeInfo("Just before pursuer arena", KEYID.SOLDIER));
            D.Add(10106371, SafeInfo("Just before pursuer arena in NG+", KEYID.SOLDIER));
            D.Add(10106380, SafeInfo("On topmost ledge of the circular room"));
            D.Add(10106390, SafeInfo("Behind corner outside cardinal tower"));
            D.Add(10106400, SafeInfo("In front of the pile of garbage that blocks the big gate to the fire area"));
            D.Add(10106410, SafeInfo("Next to upper level of the elevator"));
            D.Add(10106420, SafeInfo("On scaffolding near the ”place unbeknownst” bonfire", KSO(KEYID.SOLDIER, KEYID.KINGSRING)));
            D.Add(10106430, SafeInfo("Next to portcullis near Soldier's rest” bonfire", KEYID.SOLDIER));
            D.Add(10106440, SafeInfo("In the middle of the ballista trap area"));
            D.Add(10106450, SafeInfo("In the middle of the ballista trap area"));
            D.Add(10106460, SafeInfo("First corpse in the upper fire area"));
            D.Add(10106470, SafeInfo("Second corpse in the upper fire area"));
            D.Add(10106480, SafeInfo("Third corpse in the upper fire area"));
            D.Add(10106490, SafeInfo("Behind the wagon at Cardinal Tower upper floor"));
            D.Add(10106500, SafeInfo("On wooden floor boards above the guard room over the courtyard"));
            D.Add(10106510, SafeInfo("Corpse next to king's door", KEYID.SOLDIER));
            D.Add(10106520, SafeInfo("Second corpse on the platform where you meet pursuer for first time"));
            D.Add(10106530, SafeInfo("Third corpse on the platform where you meet pursuer for first time"));
            D.Add(10106540, SafeInfo("Fourth corpse on the platform where you meet pursuer for first time"));
            D.Add(10106550, SafeInfo("First corpse on the platform where you meet pursuer for first time"));
            D.Add(10106560, SafeInfo("At the hole in the wall before cardinal tower"));
            D.Add(10106570, SafeInfo("Behind a table at cardinal tower upper floor"));
            D.Add(10106580, SafeInfo("A corpse in the circular room before the first fog gate"));
            D.Add(10106590, SafeInfo("A corpse in the circular room before the first fog gate"));
            D.Add(10106600, SafeInfo("Under a tree above the small tunnel guarded by turtle"));
            D.Add(10106610, SafeInfo("Second corpse at rooftop near Soldier's rest bonfire", KEYID.SOLDIER));
            D.Add(10106620, SafeInfo("Third corpse at rooftop near Soldier's rest bonfire", KEYID.SOLDIER));
            D.Add(10106630, SafeInfo("Fourth corpse at rooftop near soldier's rest bonfire", KEYID.SOLDIER));
            D.Add(60008000, NpcInfo("Pursuer (on the platform) drop")); // unsure about this category, probably needs a misc-volatile subset

            // Heide's Tower of Flame:
            D.Add(611000, BossInfo("Dragonrider boss drop"));
            D.Add(625000, BossInfo("Old Dragonslayer boss drop"));
            D.Add(10305010, MChestInfo("Metal chest behind petrified hollow after Dragonrider", KEYID.BRANCH));
            D.Add(10305020, MChestInfo("Vanilla bone dust: Metal chest guarded by syan soldier after Dragonrider"));
            D.Add(10306000, SafeInfo("In alcove, far end of top floor after Dragonrider"));
            D.Add(10306010, SafeInfo("Corpse on railing, far end of top floor after Dragonrider"));
            D.Add(10306020, SafeInfo("In alcove, far end of top floor after Dragonrider"));
            D.Add(10306030, SafeInfo("On railing behind petrified hollow", KEYID.BRANCH));
            D.Add(10315000, MChestInfo("Metal chest before Dragonrider"));
            D.Add(10315001, MChestNGPlusInfo("Metal chest before Dragonrider in NG+"));
            D.Add(10316010, SafeInfo("On the railing near first bonfire"));
            D.Add(10316040, SafeInfo("Under the spiral staircase after Dragonrider"));
            D.Add(10316041, NGPlusInfo("Under the spiral staircase after Dragonrider in NG+"));
            D.Add(10316050, SafeInfo("On the ledge on the way towards the cathedral"));
            D.Add(10316090, SafeInfo("In the waterway from Majula"));
            D.Add(10316100, SafeInfo("On a corpse near the first lever"));
            D.Add(10316101, SafeInfo("Behind the big opened metal door in NG+"));
            D.Add(2002000, CovInfo("Blue sentinels join"));
            D.Add(2002011, CovInfo("Blue sentinels 1st rank reward"));
            D.Add(2002012, CovInfo("Blue sentinels 2nd rank reward"));
            D.Add(2002013, CovInfo("Blue sentinels 3rd rank reward"));
            D.Add(1785040, CovInfo("Gift from Blue Sentinel Targray after getting to 3rd rank in the Blue Sentinels"));
            D.Add(10315010, MChestInfo("Metal chest after Old Dragonslayer"));
            D.Add(10315020, WChestInfo("Wooden chest after Old Dragonslayer"));
            D.Add(10315030, MChestInfo("Metal chest before Old Dragonslayer"));
            D.Add(10316110, SafeInfo("Next to metal chest before Old Dragonslayer"));

            // No Man's Wharf:
            D.Add(303300, BossInfo("Flexile Sentry drop"));
            D.Add(10185000, WChestInfo("Wooden chest in the first house in Wharf"));
            D.Add(10185001, WChestNGPlusInfo("Wooden chest in the first house in Wharf in NG+"));
            D.Add(10185030, WChestInfo("Wooden chest on the 2nd floor of the house next to central courtyard"));
            D.Add(10185040, MChestInfo("Metal chest in house next to central courtyard"));
            D.Add(10185050, MChestInfo("Metal chest in the topmost house with dark stalkers"));
            D.Add(10185060, MChestInfo("Metal chest in the secret alcove in the poison jar room"));
            D.Add(10185070, MChestInfo("Metal chest behind illusory wall in the house where gavlan is"));
            D.Add(10185071, MChestInfo("Metal chest behind illusory wall in the house where Gavlan is in NG+"));
            D.Add(10185080, WChestInfo("Wooden chest behind illusory wall in the house where Gavlan is"));
            D.Add(10185081, WChestNGPlusInfo("Wooden chest behind illusory wall in the house where Gavlan is in NG+"));
            D.Add(10185100, WChestInfo("Trapped wooden chest in the topmost house with dark stalkers"));
            D.Add(10185110, MChestInfo("Metal chest in the poison jar room"));
            D.Add(10185120, MChestInfo("Metal chest after Flexile Sentry"));
            D.Add(10186000, SafeInfo("Corpse on top balcony in the first building on left"));
            D.Add(10186010, SafeInfo("On tipped boat in water"));
            D.Add(10186020, SafeInfo("In staircase in a house after the ship-calling bell"));
            D.Add(10186021, NGPlusInfo("In staircase in a house after the ship-calling bell in NG+"));
            D.Add(10186030, SafeInfo("Behind a shelf in alcove"));
            D.Add(10186050, SafeInfo("In the shallow water"));
            D.Add(10186070, SafeInfo("In the house under the Pharros' contraption"));
            D.Add(10186071, NGPlusInfo("In the house under the Pharros' contraption in NG+"));
            D.Add(10186100, SafeInfo("On the roof of the house under the Pharros' contraption"));
            D.Add(10186110, SafeInfo("On the high ledge"));
            D.Add(10186120, SafeInfo("Corpse in the secret alcove of the poison jar room"));
            D.Add(10186130, SafeInfo("Rooftop after the ship-calling bell"));
            D.Add(10186140, SafeInfo("Corpse in the secret alcove of the poison jar room"));
            D.Add(10186150, SafeInfo("Vanilla dark pine resin: in the left house before the first stairs"));
            D.Add(10186160, SafeInfo("Corpse in the secret alcove of the poison jar room"));
            D.Add(10186170, SafeInfo("In alcove behind the poison jars"));

            // Lost Bastille:
            D.Add(325000, BossInfo("Ruin sentinels drop", KEYID.BRANCH));
            D.Add(1768000, NpcInfo("Gift from Straid after trading 4 boss souls", KEYID.BRANCH)); // TODO: consider how many branches
            D.Add(10165000, MChestInfo("First metal chest in Pharros/elevator room"));
            D.Add(10165010, WChestInfo("Wooden chest behind Pharros' contraption in Pharros/elevator room", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10165020, MChestInfo("Second metal chest near in Pharros/elevator room", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10165040, WChestInfo("Wooden chest behind illusory wall from stairs after Sentinels", KEYID.BRANCH));
            D.Add(10165041, WChestNGPlusInfo("Wooden chest behind illusory wall from stairs after Sentinels in NG+", KEYID.BRANCH));
            D.Add(10165050, MChestInfo("Vanilla estus shard: metal chest behind in the dog/Pursuer courtyard"));
            D.Add(10165070, MChestInfo("Third metal chest in Pharros/elevator room", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10165080, WChestInfo("Wooden chest behind illusory wall from Sentinel's room", KEYID.BRANCH));
            D.Add(10165110, MChestInfo("Metal chest next to elevator in Pharros/elevator room", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10165130, MChestInfo("Metal chest in the room under Servants' Quarters bonfire", KEYID.BRANCH));
            D.Add(10165140, MChestInfo("Metal chest where Pursuer attacks near the Tower Apart", KEYID.SOLDIER));
            D.Add(10165150, MChestInfo("Metal chest in McDuff's workshop", KSO(KEYID.TORCH), KSO(KEYID.DULLEMBER))); // TODO check this edge case!));
            D.Add(10165160, WChestInfo("First wooden chest in McDuff's workshop"));
            D.Add(10165170, WChestInfo("Second wooden chest in McDuff's workshop"));
            D.Add(10165180, WChestInfo("Third wooden chest in McDuff's workshop"));
            D.Add(10165190, WChestInfo("Fourth wooden chest in McDuff's workshop"));
            D.Add(10165210, MChestInfo("Vanilla Hush: metal chest behind illusory wall in Sentinel's room", KEYID.BRANCH));
            D.Add(10165240, MChestInfo("Metal chest next to Lucatiel"));
            D.Add(10165250, MChestInfo("Vanilla effigies: metal chest next to Tower Apart bonfire", KEYID.SOLDIER));
            D.Add(10165260, MChestInfo("Vanilla: Dull ember: metal chest next to Tower Apart bonfire", KEYID.SOLDIER));
            D.Add(10166000, SafeInfo("On upper ledge in Sentinel's room", KEYID.BRANCH));
            D.Add(10166010, SafeInfo("On top of the scaffolding in dog courtyard"));
            D.Add(10166020, SafeInfo("Item hidden immediately behind door after Servants' Quarters bonfire", KEYID.BRANCH));
            D.Add(10166030, SafeInfo("End of the rubble path, on mid-level above dog/Pursuer courtyard"));
            D.Add(10166040, SafeInfo("On the ledge above Mcduff's workshop"));
            D.Add(10166050, SafeInfo("Corpse up the ladder, looking into Pharros/elevator room", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10166070, SafeInfo("Vanilla gold pine resin: on the left-hand-side on the way to McDuff"));
            D.Add(10166080, SafeInfo("On ledge near tower apart bonfire", KEYID.SOLDIER));
            D.Add(10166100, SafeInfo("Corpse behind illusory wall in Sentinel's room", KEYID.BRANCH));
            D.Add(10166130, SafeInfo("Corpse at top of elevator", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10166150, SafeInfo("In a cell in the narrow corridor after Sentinels", KEYID.BRANCH));
            D.Add(10166180, SafeInfo("Vanilla Bastille Key: on ledge behind boxes, turn left after Servants' Quarters door", KEYID.BRANCH));
            D.Add(10166190, SafeInfo("Behind the corner at beginning of the bridge to Sinner's Rise", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10166191, NGPlusInfo("Behind the corner at beginning of the bridge to Sinner's Rise in NG+", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10166200, SafeInfo("End of the path after double illusory wall above dog/Pursuer courtyard", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10166260, SafeInfo("On corpse after jumping gap after the illusory wall", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10166270, SafeInfo("At bottom of ladder before ruin sentinels", KEYID.BRANCH));
            D.Add(10166290, SafeInfo("Vanilla flame butterflies: on roof after Servants' Quarters", KEYID.BRANCH));
            D.Add(10166300, SafeInfo("Cell near Exile Holding Cells bonfire"));
            D.Add(10166310, SafeInfo("In a room with explosive hollows near Antiquated door"));
            D.Add(10166320, SafeInfo("In a cell before Sentinels", KEYID.BRANCH));
            D.Add(10166370, SafeInfo("On opposite roof (jump) after Servants' Quarters", KEYID.BRANCH));
            D.Add(10166380, SafeInfo("In the room under the Servants' Quarters bonfire", KEYID.BRANCH));
            D.Add(10166410, SafeInfo("Vanilla ascetic: on ledge halfway up elevator shaft", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10166420, SafeInfo("Upper floor of tower above Lucatiel"));
            D.Add(10166421, NGPlusInfo("Upper floor of tower above Lucatiel in NG+"));
            D.Add(10166430, SafeInfo("Vanilla Archdrake shield: Blow up wall by Pharros/elevator room", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10166440, SafeInfo("In a cell next to Straid's cell", KSO(KEYID.ANTIQUATED, KEYID.BASTILLEKEY), KSO(KEYID.BRANCH,KEYID.BASTILLEKEY)));
            D.Add(10166441, NGPlusInfo("In a cell next to straid's cell in NG+", KEYID.BASTILLEKEY));
            D.Add(10166460, SafeInfo("In a cell at top of elevator ride after Flexile"));
            D.Add(10166470, SafeInfo("In hidden tunnel after riding down on top of the elevator after Flexile"));
            D.Add(10166480, SafeInfo("In the corridor after one illusory wall, two level's below Straid", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));
            D.Add(10166490, SafeInfo("In the well", KSO(KEYID.ANTIQUATED), KSO(KEYID.BRANCH)));

            // Belfry Luna:
            D.Add(324000, BossInfo("Belfry Gargoyles drop", KEYID.BELFRYLUNA));
            D.Add(324001, BossNGPlusInfo("Belfry Gargoyles drop in NG+", KEYID.BELFRYLUNA));
            D.Add(10165200, WChestInfo("Wooden chest at topmost floor", KEYID.BELFRYLUNA));
            D.Add(10165220, MChestInfo("Metal chest after dropping through hole from 2nd floor", KEYID.BELFRYLUNA));
            D.Add(10165230, MChestInfo("Metal chest after Gargoyles", KEYID.BELFRYLUNA));
            D.Add(10166140, SafeInfo("On railing 2nd floor", KEYID.BELFRYLUNA));
            D.Add(10166160, SafeInfo("Corpse after dropping through hole from 2nd floor", KEYID.BELFRYLUNA));
            D.Add(10166170, SafeInfo("Corpse on topmost floor", KEYID.BELFRYLUNA));
            D.Add(10166250, SafeInfo("At lower level in pit after Gargoyles", KEYID.BELFRYLUNA));
            D.Add(10166390, SafeInfo("At upper level in pit after Gargoyles", KEYID.BELFRYLUNA));
            D.Add(10166400, SafeInfo("On ledge at Gargoyle arena", KEYID.BELFRYLUNA));
            
            // Sinner's Rise:
            D.Add(626000, BossInfo("Lost Sinner drop", KEYID.SINNERSRISE));
            D.Add(626001, BossNGPlusInfo("Lost Sinner drop in NG+", KEYID.SINNERSRISE));
            D.Add(10165120, MChestInfo("Metal chest after the lost sinner", KEYID.SINNERSRISE));
            D.Add(10166060, SafeInfo("On small ledge very near bonfire", KEYID.SINNERSRISE));
            D.Add(10166090, SafeInfo("Right side just after bottom of elevator", KEYID.SINNERSRISE));
            D.Add(10166110, SafeInfo("Item in elevator shaft, drop off early", KEYID.SINNERSRISE));
            D.Add(10166120, SafeInfo("Left side just after bottom of elevator", KEYID.SINNERSRISE));
            D.Add(10166230, SafeInfo("Behind illusory wall at right side just after elevator", KEYID.SINNERSRISE));
            D.Add(10166280, SafeInfo("In open cell left side lower level", KEYID.SINNERSRISE));
            D.Add(10166330, SafeInfo("In locked cell left side upper level", KSO(KEYID.SINNERSRISE,KEYID.BASTILLEKEY)));
            D.Add(10166350, SafeInfo("In right side oil-sconce room just before the Sinner", KSO(KEYID.SINNERSRISE,KEYID.BASTILLEKEY)));
            D.Add(10166360, SafeInfo("Left side just before the Sinner", KEYID.SINNERSRISE));
            D.Add(10166450, SafeInfo("On ledge outside tower at top of elevator", KEYID.SINNERSRISE));

            // The Pit:
            D.Add(10045020, MChestInfo("First metal chest behind the forgotten door", KEYID.FORGOTTEN));
            D.Add(10045030, MChestInfo("Third metal chest behind the forgotten door", KEYID.FORGOTTEN));
            D.Add(10045050, MChestInfo("Second metal chest behind the forgotten door", KEYID.FORGOTTEN));
            D.Add(10046140, SafeInfo("Vanilla DLC1 key: corpse behind the forgotten door", KEYID.FORGOTTEN));
            D.Add(10046130, SafeInfo("First board in the pit"));
            D.Add(10046060, SafeInfo("Second board in the pit"));
            D.Add(10046050, SafeInfo("Third board in the pit"));
            D.Add(10046090, SafeInfo("Fourth board in the pit"));
            D.Add(10046150, SafeInfo("Vanilla homeward bone: entrance to corridor before bridge crystal lizard"));
            D.Add(10345000, MChestInfo("Metal chest after jumping across the wooden bridge in the circular room with explosive hollows"));
            D.Add(10345010, MChestInfo("Vanilla token of spite: wooden chest under the scaffolding before the Gutter"));
            D.Add(10345020, MChestInfo("Vanilla bone dust: metal chest guarded by syan soldier"));
            D.Add(10346000, SafeInfo("First corpse on scaffolding before the Gutter"));
            D.Add(10346010, SafeInfo("Second corpse on scaffolding before the Gutter"));
            D.Add(10346070, SafeInfo("First corpse in the circular room"));
            D.Add(10346090, SafeInfo("On the higher bridge  of the circular room after Rat Vanguard"));
            D.Add(10346091, NGPlusInfo("On the higher bridge of the circular room after Rat Vanguard in NG+"));
            D.Add(10346100, SafeInfo("On a small unconnected ledge offset from the wooden bridge in the circular room"));
            D.Add(10346110, SafeInfo("Second corpse in the circular room with explosive hollows"));

            // Grave of Saints:
            D.Add(226100, BossInfo("Royal rat vanguard drop"));
            D.Add(10346020, SafeInfo("2nd floor on other side of the drawbridges", KEYID.PHARROS));
            D.Add(10346030, SafeInfo("In the first circular room of Grave of Saints"));
            D.Add(10346031, SafeInfo("In the first circular room of Grave of Saints in NG+"));
            D.Add(10346040, SafeInfo("2nd floor left side next to table"));
            D.Add(10346050, SafeInfo("1st floor on other side of the drawbridges", KEYID.PHARROS));
            D.Add(10346060, SafeInfo("In the middle circle of the circular room, after Rat Vanguard"));
            D.Add(10346080, SafeInfo("1st floor left side"));

            // The Gutter & Black Gulch:
            D.Add(326000, BossInfo("The Rotten drop"));
            D.Add(326001, BossInfo("The Rotten drop in NG+"));
            D.Add(10255010, WChestInfo("Wooden chest on top of small structure in the end of the first long bridge"));
            D.Add(10255030, WChestInfo("Wooden chest on ledge in multiple ladders area"));
            D.Add(10255040, MChestInfo("Metal chest near where melinda the butcher invades"));
            D.Add(10255100, MChestInfo("Metal chest in the cave with corrosive bugs"));
            D.Add(10255110, MChestInfo("Metal chest in the base of the tower where the zip-line starts"));
            D.Add(10256000, SafeInfo("Urn behind the forgotten door"));
            D.Add(10256030, SafeInfo("Urn inside a small structure on top of the tower where the zip-line starts"));
            D.Add(10256060, SafeInfo("Urn in top level near the three torches side by side"));
            D.Add(10256090, SafeInfo("Urn in lower level of the structure where second long bridge ends"));
            D.Add(10256130, SafeInfo("Second urn at the bottom of the multiple ladders area"));
            D.Add(10256160, SafeInfo("Before the entrance to the Black Gulch"));
            D.Add(10256170, SafeInfo("First urn behind the ant queen"));
            D.Add(10256220, SafeInfo("Urn next to upper gutter bonfire"));
            D.Add(10256230, SafeInfo("Urn in area with tar pits"));
            D.Add(10256240, SafeInfo("Urn next to upper gutter bonfire on a little bit higher ledge"));
            D.Add(10256250, SafeInfo("Urn in lower level near the ring of torches"));
            D.Add(10256260, SafeInfo("Urn in the 2nd floor of the tower where the zip-line starts"));
            D.Add(10256270, SafeInfo("Urn next to hanging poison statue in the base of the tower where the zip-line starts"));
            D.Add(10256280, SafeInfo("Urn on top of the tower where the zip-line starts"));
            D.Add(10256290, SafeInfo("Urn in tight corner on middle level of the tower where the zip-line starts"));
            D.Add(10256300, SafeInfo("Second urn behind the ant queen"));
            D.Add(10256310, SafeInfo("Urn in between the fog gate and the ant queen"));
            D.Add(10256320, SafeInfo("Urn in the area with multiple ladders"));
            D.Add(10256330, SafeInfo("Urn in wooden floor in multiple ladders area"));
            D.Add(10256340, SafeInfo("First urn at the bottom of the multiple ladders area"));
            D.Add(10256400, SafeInfo("Corpse on top of the tower where the zip-line starts"));
            D.Add(10256410, SafeInfo("First corpse in top level; near Jeff jump"));
            D.Add(10256420, SafeInfo("Second corpse in top level; near Jeff jump"));
            D.Add(10256430, SafeInfo("Third corpse in top level; near Jeff jump"));
            D.Add(10256440, SafeInfo("Fourth corpse in top level; near Jeff jump"));
            D.Add(10256450, SafeInfo("Fifth corpse in top level; near Jeff jump"));
            D.Add(10255050, WChestInfo("Wooden chest in the side tunnel after the worms"));
            D.Add(10255090, MChestInfo("Metal chest in side room next to Gulch Giants"));
            D.Add(10255120, MChestInfo("Metal chest after the Rotten"));
            D.Add(10255130, MChestInfo("Vanilla shotel: metal chest in the side tunnel before the worms"));
            D.Add(10256210, SafeInfo("In the fire in the Rotten arena"));
            D.Add(10256350, SafeInfo("Urn at the bottom of the elevator near Gulch Giants"));
            D.Add(10256360, SafeInfo("Urn next to the second bonfire", KEYID.BRANCH));
            D.Add(10256370, SafeInfo("First urn in the front of the worms"));
            D.Add(10256380, SafeInfo("Second urn in the front of the worms"));
            D.Add(10256390, SafeInfo("Urn in the wide area before the rotten"));
            D.Add(10256500, NpcInfo("Dropped from rotten's arm")); // who knows what category this is
            D.Add(60001000, BossInfo("Drop from Gulch giants")); // ?

            // Huntman's Copse:
            D.Add(154000, BossInfo("Skeleton Lords drop", KEYID.ROTUNDA));
            D.Add(154001, BossInfo("Skeleton Lords drop in NG+", KEYID.ROTUNDA));
            D.Add(1770000, NpcInfo("Gift from Felkin when over 20INT and 20FTH", KEYID.ROTUNDA));
            D.Add(10046080, SafeInfo("On ground after rotating rotunda towards Huntsman's Copse", KEYID.ROTUNDA));
            D.Add(10235010, MChestInfo("Metal chest in the cave with the giant basilisk", KEYID.ROTUNDA));
            D.Add(10235020, MChestInfo("Metal chest in a round hut where you drop from above", KEYID.ROTUNDA));
            D.Add(10236000, SafeInfo("On ledge right above the Bridge Approach bonfire", KEYID.ROTUNDA));
            D.Add(10236020, SafeInfo("Next to a big cliff face guarded by a sickle undead", KEYID.ROTUNDA));
            D.Add(10236021, NGPlusInfo("Next to a big cliff face guarded by a sickle undead in NG+", KEYID.ROTUNDA));
            D.Add(10236030, SafeInfo("On the left side of the path before the first stone bridge", KEYID.ROTUNDA));
            D.Add(10236040, SafeInfo("On a ledge on the way to Executioner's Chariot", KEYID.ROTUNDA));
            D.Add(10236050, SafeInfo("Corpse in the necromancer cave closest to the Undead Lockaway” bonfire", KEYID.ROTUNDA));
            D.Add(10236060, SafeInfo("On small ledge with a necromancer", KEYID.ROTUNDA));
            D.Add(10236070, SafeInfo("Hanging on the side of the big circular hole in the room in the beginning of the area", KEYID.ROTUNDA));
            D.Add(10236071, NGPlusInfo("Hanging on the side of the big circular hole in the room in the beginning of the area in NG+", KEYID.ROTUNDA));
            D.Add(10236080, SafeInfo("On the roof of the room with the big circular hole in the beginning of the area", KEYID.ROTUNDA));
            D.Add(10236090, SafeInfo("Round hut next to a big cliff face", KEYID.ROTUNDA));
            D.Add(10236100, SafeInfo("First corpse in a round hut where you drop from above", KEYID.ROTUNDA));
            D.Add(10236110, SafeInfo("Second corpse in a round hut where you drop from above", KEYID.ROTUNDA));
            D.Add(10236120, SafeInfo("In the room with the big circular hole in the beginning of the area", KEYID.ROTUNDA));
            D.Add(10236130, SafeInfo("On the left side of the path after the first stone bridge", KEYID.ROTUNDA));
            D.Add(10236131, NGPlusInfo("On the left side of the path after the first stone bridge in NG+", KEYID.ROTUNDA));
            D.Add(10236140, SafeInfo("Next to portcullis in the necromancer cave", KEYID.ROTUNDA));
            D.Add(10236150, SafeInfo("Vanilla Token of Fidelity: on the pillar under the Chariot bridge", KEYID.ROTUNDA));
            D.Add(10236160, SafeInfo("Where Merciless Roenna invades", KEYID.ROTUNDA));
            D.Add(10236170, SafeInfo("On a small ledge where you drop from the area where Merciless Roenna invades", KEYID.ROTUNDA));
            D.Add(10236230, SafeInfo("Round hut on the way to Skeleton Lords"));
            D.Add(10236240, SafeInfo("On the side ledge just before Executioner's Chariot boss", KEYID.ROTUNDA));
            D.Add(10236250, SafeInfo("On lower end of the elevator from the cave with the giant basilisk", KEYID.ROTUNDA));
            D.Add(10236260, SafeInfo("On a stone bridge in the cave with the giant basilisk", KEYID.ROTUNDA));
            D.Add(10236270, SafeInfo("On a stone pillar inside the necromancer cave", KEYID.ROTUNDA));

            // Chariot Arena:
            D.Add(619100, BossInfo("Executioner's Chariot drop", KEYID.ROTUNDA));
            D.Add(619101, BossInfo("Executioner's Chariot drop in NG+", KEYID.ROTUNDA));
            D.Add(2003000, CovInfo("Brotherhood of Blood join", KEYID.ROTUNDA));
            D.Add(2003011, CovInfo("Brotherhood of Blood 1st rank price", KEYID.ROTUNDA));
            D.Add(2003012, CovInfo("Brotherhood of Blood 2nd rank price", KEYID.ROTUNDA));
            D.Add(2003013, CovInfo("Brotherhood of Blood 3rd rank price", KEYID.ROTUNDA));
            D.Add(1783040, CovInfo("Gift from Titchy Gren after getting to 3rd rank in Brotherhood of Blood", KEYID.ROTUNDA));
            D.Add(10236010, SafeInfo("Above the stairs leading to the bonfire", KEYID.ROTUNDA));
            D.Add(10236180, SafeInfo("First corpse in the chariot arena", KEYID.ROTUNDA));
            D.Add(10236190, SafeInfo("Second corpse in the chariot arena", KEYID.ROTUNDA));
            D.Add(10236200, SafeInfo("Third corpse in the chariot arena", KEYID.ROTUNDA));
            D.Add(10236210, SafeInfo("Fourth corpse in the chariot arena", KEYID.ROTUNDA));
            D.Add(10236220, SafeInfo("Fifth corpse in the chariot arena", KEYID.ROTUNDA));

            // Harvest Valley:
            D.Add(2007000, CovInfo("Heirs of the Sun join", KEYID.ROTUNDA));
            D.Add(2007011, CovInfo("Heirs of the Sun 1st rank price", KEYID.ROTUNDA));
            D.Add(2007012, CovInfo("Heirs of the Sun 2nd rank price", KEYID.ROTUNDA));
            D.Add(2007013, CovInfo("Heirs of the Sun 3rd rank price", KEYID.ROTUNDA));
            D.Add(10175020, WChestInfo("Wooden chest inside the small cave where you meet Gavlan", KEYID.ROTUNDA));
            D.Add(10175021, WChestNGPlusInfo("Wooden chest inside the small cave where you meet Gavlan in NG+", KEYID.ROTUNDA));
            D.Add(10175030, MChestInfo("Metal chest next to ladders leading down into the poison filled tunnel", KEYID.ROTUNDA));
            D.Add(10175110, MChestInfo("Metal chest inside the small cave where you meet Gavlan", KEYID.ROTUNDA));
            D.Add(10176000, SafeInfo("Behind a plank wall in the area with multiple sickle undeads", KEYID.ROTUNDA));
            D.Add(10176020, SafeInfo("On a ledge before the first poison area", KEYID.ROTUNDA));
            D.Add(10176030, SafeInfo("On a ledge right after the first poison area", KEYID.ROTUNDA));
            D.Add(10176060, SafeInfo("On upper end of the ladders before the gate guarded by desert sorceress", KEYID.ROTUNDA));
            D.Add(10176070, SafeInfo("A corpse behind a plank wall you need a mounted overseer to break", KEYID.ROTUNDA));
            D.Add(10176080, SafeInfo("Behind a plank wall in the area with multiple sickle undeads", KEYID.ROTUNDA));
            D.Add(10176090, SafeInfo("A corpse behind a plank wall you need a mounted overseer to break", KEYID.ROTUNDA));
            D.Add(10176130, SafeInfo("A corpse behind a plank wall you need a mounted overseer to break", KEYID.ROTUNDA));
            D.Add(10176160, SafeInfo("A corpse behind a plank wall you need a mounted overseer to break", KEYID.ROTUNDA));
            D.Add(10176180, SafeInfo("A corpse inside the small cave where you meet Gavlan", KEYID.ROTUNDA));
            D.Add(10176200, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176210, SafeInfo("In the poison filled tunnel closer to Earthen Peak end", KEYID.ROTUNDA));
            D.Add(10176220, SafeInfo("In a small poison filled alcove from the poison area in front of Earthen Peak", KEYID.ROTUNDA));
            D.Add(10176221, NGPlusInfo("In a small poison filled alcove from the poison area in front of Earthen Peak in NG+", KEYID.ROTUNDA));
            D.Add(10176230, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176231, SafeInfo("A corpse in the first poison area in NG+", KEYID.ROTUNDA));
            D.Add(10176250, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176260, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176270, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176280, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176290, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176300, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176340, SafeInfo("In the poison filled tunnel closer to Earthen Peak", KEYID.ROTUNDA));
            D.Add(10176350, SafeInfo("A corpse in the poison area before Earthen Peak", KEYID.ROTUNDA));
            D.Add(10176370, SafeInfo("A corpse in the poison area before earthen peak", KEYID.ROTUNDA));
            D.Add(10176390, SafeInfo("On a ledge after the tunnel leading away from the area with multiple sickle undeads", KEYID.ROTUNDA));
            D.Add(10176400, SafeInfo("Behind a plank wall in the area with multiple sickle undeads", KEYID.ROTUNDA));
            D.Add(10176410, SafeInfo("In the poison filled tunnel further Earthen Peak", KEYID.ROTUNDA));
            D.Add(10176460, SafeInfo("In a tunnel leading away from the area with multiple sickle undeads", KEYID.ROTUNDA));
            D.Add(10176461, SafeInfo("In a tunnel leading away from the area with multiple sickle undeads in NG+", KEYID.ROTUNDA));
            D.Add(10176470, SafeInfo("Behind a plank wall in the area with multiple sickle undeads", KEYID.ROTUNDA));
            D.Add(10176480, SafeInfo("Behind a plank wall in the area with multiple sickle undeads", KEYID.ROTUNDA));
            D.Add(10176490, SafeInfo("In the middle of the area with multiple sickle undeads", KEYID.ROTUNDA));
            D.Add(10176500, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176510, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176520, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176530, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176540, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176550, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176560, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176570, SafeInfo("A corpse in the first poison area", KEYID.ROTUNDA));
            D.Add(10176580, SafeInfo("A corpse in the poison area before earthen peak", KEYID.ROTUNDA));
            D.Add(10176590, SafeInfo("A corpse in the poison area before earthen peak", KEYID.ROTUNDA));
            D.Add(10176600, SafeInfo("A corpse in the poison area before earthen peak", KEYID.ROTUNDA));
            D.Add(10176610, SafeInfo("A corpse in the poison area before earthen peak", KEYID.ROTUNDA));
            D.Add(10176620, SafeInfo("A corpse in the poison area before earthen peak", KEYID.ROTUNDA));

            // Earthern Peak:
            D.Add(500000, BossInfo("Covetous Demon drop", KEYID.ROTUNDA));
            D.Add(501000, BossInfo("Mytha, the Baneful Queen drop", KEYID.ROTUNDA));
            D.Add(501001, BossNGPlusInfo("Mytha, the Baneful Queen drop in NG+", KEYID.ROTUNDA));
            D.Add(1744010, NpcInfo("Gift from Pate if player has summoned him to the Last Giant fight", KEYID.ROTUNDA));
            D.Add(10175040, WChestInfo("Wooden chest on a lower side ledge of the main hall", KEYID.ROTUNDA));
            D.Add(10175050, WChestInfo("Wooden chest behind illusory wall in the top floor", KEYID.ROTUNDA));
            D.Add(10175060, MChestInfo("Metal chest in side room where you need to jump before Covetous Demon", KEYID.ROTUNDA));
            D.Add(10175070, WChestInfo("Trapped wooden chest in side room before Covetous Demon", KEYID.ROTUNDA));
            D.Add(10175090, WChestInfo("Trapped wooden chest next to two desert sorceress", KEYID.ROTUNDA));
            D.Add(10175120, MChestInfo("Metal chest on the other side of a jumpable gap", KEYID.ROTUNDA));
            D.Add(10175130, MChestInfo("Metal chest behind Pharros contraption in the lowest level next to Lucatiel", KSO(KEYID.ROTUNDA, KEYID.PHARROS)));
            D.Add(10175140, MChestInfo("Metal chest in a small room behind a locked door next to Pate (need to drop from above)", KEYID.ROTUNDA));
            D.Add(10175150, MChestInfo("Metal chest in a bigger room behind a locked door next to pate (need to drop from above)", KEYID.ROTUNDA));
            D.Add(10175160, MChestInfo("Metal chest on upper end of the raisable wooden platform", KEYID.ROTUNDA));
            D.Add(10176010, SafeInfo("On a broken bridge part with manikin in the central hall", KEYID.ROTUNDA));
            D.Add(10176040, SafeInfo("Vanilla Spell Quartz +1: behind illusory wall on Mytha's foggate level near the ladders", KEYID.ROTUNDA));
            D.Add(10176050, SafeInfo("Under the raisable wooden platform", KEYID.ROTUNDA));
            D.Add(10176100, SafeInfo("In a poison filled corridor in the lowest level", KEYID.ROTUNDA));
            D.Add(10176110, SafeInfo("In a narrow crevasse right before Covetous Demon", KEYID.ROTUNDA));
            D.Add(10176120, SafeInfo("In the corner of the first room after Covetous bonfire", KEYID.ROTUNDA));
            D.Add(10176140, SafeInfo("On a ledge where you need to buy ladders from gilligan", KEYID.ROTUNDA));
            D.Add(10176150, SafeInfo("Outside of the wooden breakable railing", KEYID.ROTUNDA));
            D.Add(10176170, SafeInfo("Corpse on the other side of a jumpable gap", KEYID.ROTUNDA));
            D.Add(10176171, NGPlusInfo("Corpse on the other side of a jumpable gap in NG+", KEYID.ROTUNDA));
            D.Add(10176190, SafeInfo("Corpse in side room where you need to jump before Covetous Demon", KEYID.ROTUNDA));
            D.Add(10176420, SafeInfo("One floor up from the second bonfire", KEYID.ROTUNDA));
            D.Add(10176430, SafeInfo("One floor up from the second bonfire", KEYID.ROTUNDA));
            D.Add(10176440, SafeInfo("Next to mimic on upper floor", KEYID.ROTUNDA));
            D.Add(10176450, SafeInfo("In the poison pool just before mytha", KEYID.ROTUNDA));
            D.Add(10176630, SafeInfo("Short corridor with poison on floor", KEYID.ROTUNDA));


            // Iron Keep & Belfry Sol:
            D.Add(305000, BossInfo("Smelter demon drop", KEYID.ROTUNDA));
            D.Add(607000, BossInfo("Old Iron King drop", KEYID.ROTUNDA));
            D.Add(607001, BossNGPlusInfo("Old Iron King drop in NG+", KEYID.ROTUNDA));
            D.Add(1772000, NpcInfo("Gift from Magerold after spending 16000 souls", KEYID.ROTUNDA));
            D.Add(2008000, CovInfo("Dragon Remnants join", KSO(KEYID.ROTUNDA, KEYID.PETRIFIEDEGG)));
            D.Add(2008011, CovInfo("Dragon Remnants 1st rank price", KSO(KEYID.ROTUNDA, KEYID.PETRIFIEDEGG)));
            D.Add(2008012, CovInfo("Dragon Remnants 2nd rank price", KSO(KEYID.ROTUNDA, KEYID.PETRIFIEDEGG)));
            D.Add(2008013, CovInfo("Dragon Remnants 3rd rank price", KSO(KEYID.ROTUNDA, KEYID.PETRIFIEDEGG)));
            D.Add(10195000, WChestInfo("Wooden chest at upper level of the room with changing platforms", KEYID.ROTUNDA));
            D.Add(10195001, WChestNGPlusInfo("Wooden chest at upper level of the room with changing platforms in NG+", KEYID.ROTUNDA));
            D.Add(10195030, MChestInfo("Metal chest next to Pharros contraption after Dull ember jump, in the first big lava room", KEYID.ROTUNDA));
            D.Add(10195040, MChestInfo("Metal chest next to archer on upper left side of the first big lava room", KEYID.ROTUNDA));
            D.Add(10195050, MChestInfo("Metal chest behind illusory wall after Belfry Sol exit", KSO(KEYID.ROTUNDA, KEYID.PHARROS)));
            D.Add(10195060, MChestInfo("Metal chest behind illusory wall after Belfry Sol exit", KSO(KEYID.ROTUNDA, KEYID.PHARROS)));
            D.Add(10195090, MChestInfo("Metal chest at the top of the high tower with a hole in the middle of it", KEYID.ROTUNDA));
            D.Add(10195100, MChestInfo("Metal chest in lava at right side of the first big lava room", KEYID.ROTUNDA));
            D.Add(10195110, MChestInfo("Metal chest in lava in the room with changing platforms", KEYID.ROTUNDA));
            D.Add(10195130, MChestInfo("Metal chest next to Belfry Sol bonfire", KSO(KEYID.ROTUNDA, KEYID.PHARROS)));
            D.Add(10195140, MChestInfo("Metal chest immediately after Smelter demon", KEYID.ROTUNDA));
            D.Add(10195150, MChestInfo("Metal chest after Old Iron King", KEYID.ROTUNDA));
            D.Add(10196030, SafeInfo("On lower left side of the first big lava room", KEYID.ROTUNDA));
            D.Add(10196040, SafeInfo("Corpse at the top of the high tower with three Alonne captains", KEYID.ROTUNDA));
            D.Add(10196050, SafeInfo("Under the fire of bull statues before Old Iron King", KEYID.ROTUNDA));
            D.Add(10196060, SafeInfo("On pilar on right side of the first big lava room", KEYID.ROTUNDA));
            D.Add(10196070, SafeInfo("Behind ladder after furnace in the first big lava room", KEYID.ROTUNDA));
            D.Add(10196080, SafeInfo("Under the fire of bull statue in the first hall", KEYID.ROTUNDA));
            D.Add(10196090, SafeInfo("On ledge above the first hall", KEYID.ROTUNDA));
            D.Add(10196100, SafeInfo("On crisscrossing platforms in a hole in the middle of the high tower", KEYID.ROTUNDA));
            D.Add(10196110, SafeInfo("On a central platform in the room with changing platforms", KEYID.ROTUNDA));
            D.Add(10196111, NGPlusInfo("On a central platform in the room with changing platforms in NG+", KEYID.ROTUNDA));
            D.Add(10196120, SafeInfo("On right side of the room with changing platforms", KEYID.ROTUNDA));
            D.Add(10196130, SafeInfo("Next to fog gate in the room with changing platforms", KEYID.ROTUNDA));
            D.Add(10196140, SafeInfo("On lava on the right of bridge by first bonfire", KEYID.ROTUNDA));
            D.Add(10196150, SafeInfo("Behind illusory wall next to ballista", KEYID.ROTUNDA));
            D.Add(10196160, SafeInfo("On broken stairs in the first big lava room", KEYID.ROTUNDA));
            D.Add(10196170, SafeInfo("Vanilla covetous gold ring: in large smelting pot hanging above bridges", KEYID.ROTUNDA));
            D.Add(10196180, SafeInfo("On lava next to the first bonfire", KEYID.ROTUNDA));
            D.Add(10196190, SafeInfo("Corpse in lava in the room with changing platforms", KEYID.ROTUNDA));
            D.Add(10196210, SafeInfo("Inside the central kiln", KEYID.ROTUNDA));
            D.Add(10196211, NGPlusInfo("Inside the central kiln in NG+", KEYID.ROTUNDA));
            D.Add(10196220, SafeInfo("Corpse at the top of the high tower with three Alonne captains", KEYID.ROTUNDA));
            D.Add(10195120, MChestInfo("Metal chest immediately after completing Belfy Sol", KSO(KEYID.ROTUNDA, KEYID.PHARROS)));
            D.Add(10196000, SafeInfo("Behind the Belfry, near the lever in Belfry Sol", KSO(KEYID.ROTUNDA, KEYID.PHARROS)));
            D.Add(10196010, SafeInfo("On the far side of the sloped roof in Belfry Sol", KSO(KEYID.ROTUNDA, KEYID.PHARROS)));
            D.Add(10196020, SafeInfo("Next to the ladder near the exit of Belfry Sol", KSO(KEYID.ROTUNDA, KEYID.PHARROS)));
            D.Add(10196200, SafeInfo("Corpse at bottom of stairs after Belfy Sol exit", KSO(KEYID.ROTUNDA, KEYID.PHARROS)));

            // Shaded Woods:
            D.Add(1307000, NpcInfo("Gift from Head of Vengarl after exhausting his dialogue", KEYID.BRANCH));
            D.Add(10295000, MChestInfo("Metal chest next to the Old Akelarre bonfire", KEYID.BRANCH));
            D.Add(10296020, SafeInfo("Corpse in the room above the Old Akelarre bonfire", KEYID.BRANCH));
            D.Add(10325020, MChestInfo("Vanilla Chlo +1: metal chest on left side of the fog area", KEYID.BRANCH));
            D.Add(10325050, MChestInfo("Vanilla Old Sun ring: metal chest on right side of the fog area", KEYID.BRANCH));
            D.Add(10325080, MChestInfo("Vanilla Clear Bluetone+1: metal chest next to a big tree in fog area", KEYID.BRANCH));
            D.Add(10326000, SafeInfo("On ledge left side of the path to Ruined Fork Road bonfire", KEYID.BRANCH));
            D.Add(10326020, SafeInfo("On a ledge on left side of the path between Ruined Fork Road bonfire and Shrine of Winter", KEYID.BRANCH));
            D.Add(10326030, SafeInfo("Vanilla Golden Falcon shield: in front of the ruined gate just before Shrine of Winter", KEYID.BRANCH));
            D.Add(10326060, SafeInfo("Corpse just before the stone circle next to Head of Vengarl", KEYID.BRANCH));
            D.Add(10326240, SafeInfo("Behind the stone circle by Head of Vengarl", KEYID.BRANCH));
            D.Add(10326070, SafeInfo("Corpse next to a tree in fog area", KEYID.BRANCH));
            D.Add(10326080, SafeInfo("Corpse next to a big tree in fog area", KEYID.BRANCH));
            D.Add(10326081, NGPlusInfo("Corpse next to a big tree in fog area in NG+", KEYID.BRANCH));
            D.Add(10326090, SafeInfo("Corpse in the middle of the fog area", KEYID.BRANCH));
            D.Add(10326100, SafeInfo("Corpse on right side of the fog area", KEYID.BRANCH));
            D.Add(10326101, NGPlusInfo("Corpse on right side of the fog area in NG+", KEYID.BRANCH));
            D.Add(10326110, SafeInfo("On the path to Ruined Fork Road” bonfire", KEYID.BRANCH));
            D.Add(10326170, SafeInfo("Vanilla RTSR: in the mud next to hippo",KEYID.BRANCH));
            D.Add(10326220, SafeInfo("Behind the ruined gate, to the right after Shrine of Winter", KEYID.DRANGLEIC));
            D.Add(10326250, SafeInfo("On the path after Shrine of Winter", KEYID.DRANGLEIC));
            D.Add(10326260, SafeInfo("On a wall along the path after Shrine of Winter", KEYID.DRANGLEIC));
            D.Add(10326270, SafeInfo("Corpse getting eaten by goblins just before Ruined Fork Road bonfire", KEYID.BRANCH));
            D.Add(10326280, SafeInfo("Corpse getting eaten by goblins just before the Ruined Fork Road bonfire", KEYID.BRANCH));
            D.Add(503000, BossInfo("Scorpioness Najka drop", KEYID.BRANCH));
            D.Add(503001, NGPlusInfo("Scorpioness Najka drop in NG+", KEYID.BRANCH));
            D.Add(1502000, NpcInfo("Gift from Manscorpion Tark after defeating Najka", KSO(KEYID.BRANCH, KEYID.WHISPERS)));
            D.Add(1502010, NpcInfo("Gift from manscorpion Tark after defeating Freja", KSO(KEYID.BRANCH, KEYID.WHISPERS)));
            D.Add(10325000, WChestInfo("Wooden chest on lower floor of the main Shaded Ruins bridge", KEYID.BRANCH));
            D.Add(10325001, WChestNGPlusInfo("Wooden chest On lower floor of the main Shaded Ruins bridge in NG+", KEYID.BRANCH));
            D.Add(10325010, WChestInfo("Wooden chest on upper floor of the main Shaded Ruins bridge", KEYID.BRANCH));
            D.Add(10325030, MChestInfo("Metal chest on upper area by Manscorpion tark", KEYID.BRANCH));
            D.Add(10325040, MChestInfo("Metal chest in room blocked by petrified statue", KEYID.BRANCH)); // Double branch :thinking:
            D.Add(10325060, MChestInfo("Metal chest next to Darkdiver Grandahl", KEYID.BRANCH));
            D.Add(10325100, MChestInfo("Metal chest blocked by petrified statue", KEYID.BRANCH));
            D.Add(10325110, MChestInfo("Vanilla BKH: metal chest under the main Shaded Ruins bridge", KEYID.BRANCH));
            D.Add(10325120, MChestInfo("Vanilla bone dust: metal chest in area behind two petrified statues and Vengarl's body", KEYID.BRANCH));
            D.Add(10326010, SafeInfo("Left of the building leading to Manscorpion Tark", KEYID.BRANCH));
            D.Add(10326040, SafeInfo("On 2nd floor of the area with many lion warriors", KEYID.BRANCH));
            D.Add(10326120, SafeInfo("On ledge next to the exit from the corrosive puddles area", KEYID.BRANCH));
            D.Add(10326130, SafeInfo("On upper floor above Shaded Ruins bonfire", KEYID.BRANCH));
            D.Add(10326140, SafeInfo("Cave on right side of the door to Ornifex", KEYID.BRANCH));
            D.Add(10326141, NGPlusInfo("Cave on right side of the door to Ornifex in NG+", KEYID.BRANCH));
            D.Add(10326150, SafeInfo("Corpse next to chest in area behind two petrified statues and Vengarl's body", KEYID.BRANCH));
            D.Add(10326160, SafeInfo("Room where Ornifex is locked", KSO(KEYID.BRANCH, KEYID.FANG)));
            D.Add(10326180, SafeInfo("On collapsed stairs next to Creighton", KEYID.BRANCH));
            D.Add(10326190, SafeInfo("Corpse next to the big collapsing floor", KEYID.BRANCH));
            D.Add(10326191, NGPlusInfo("Corpse next to the big collapsing floor in NG+", KEYID.BRANCH));
            D.Add(10326200, NpcInfo("On tree in Najka's arena", KEYID.BRANCH)); // To consider fairness
            D.Add(10326210, SafeInfo("On lower floor of the building leading to Manscorpion Tark", KEYID.BRANCH));
            D.Add(10326230, SafeInfo("Next to Vengarl's body", KEYID.BRANCH)); // Multi branch
            D.Add(60009000, SafeInfo("Drop from the petrified lion warrior by the tree bridge", KEYID.BRANCH)); // Multi-branch

            // Doors of Pharros:
            D.Add(223500, BossInfo("Royal Rat Authority drop", KEYID.BRANCH));
            D.Add(10335000, WChestInfo("Wooden chest in room after using top Pharros contraption and dropping down near the toxic rats", KSO(KEYID.BRANCH, KEYID.PHARROS)));
            D.Add(10335010, WChestInfo("Vanilla chunk/pdb: trapped wooden chest, after climbing ladder and jumping gap", KEYID.BRANCH));
            D.Add(10335020, WChestNGPlusInfo("Trapped wooden chest behind (floor) Pharros contraption in the upper level", KSO(KEYID.BRANCH, KEYID.PHARROS)));
            D.Add(10335021, WChestNGPlusInfo("Trapped wooden chest behind (floor) Pharros contraption in the upper level in NG+", KSO(KEYID.BRANCH, KEYID.PHARROS)));
            D.Add(10335030, WChestInfo("Wooden chest in an alcove guarded by dwarf in the beginning of the long stairs to Brightstone", KEYID.BRANCH));
            D.Add(10335031, WChestNGPlusInfo("Wooden chest in an alcove guarded by dwarf in the beginning of the long stairs to Brightstone in NG+", KEYID.BRANCH));
            D.Add(10335040, MChestInfo("Vanilla Santier's spear: metal chest behind three-part Pharros door in the lower level", KSO(KEYID.BRANCH, KEYID.PHARROS)));
            D.Add(10336000, SafeInfo("In water before the first bonfire", KEYID.BRANCH));
            D.Add(10336010, SafeInfo("In the room with Gavlan", KEYID.BRANCH));
            D.Add(10336011, SafeInfo("In the room with Gavlan in NG+", KEYID.BRANCH));
            D.Add(10336020, SafeInfo("Corpse, behind three-part Pharros' door in the upper level", KSO(KEYID.BRANCH, KEYID.PHARROS)));
            D.Add(10336040, SafeInfo("In water in the far side of the first (lower) big hall", KEYID.BRANCH));
            D.Add(10336041, NGPlusInfo("In water in the big hall right after Gyrm's Respite bonfire in NG+", KEYID.BRANCH));
            D.Add(10336050, SafeInfo("Vanilla Gyrm axe: corpse on a middle-level ledge in the first room", KEYID.BRANCH));
            D.Add(10336060, SafeInfo("On ledge after climbing the ladder up from the water", KEYID.BRANCH));
            D.Add(10336070, SafeInfo("Upper level, hidden behind small stairs near Rat Authrotiy", KEYID.BRANCH));
            D.Add(10336080, SafeInfo("On 2nd floor next to dwarf statues", KEYID.BRANCH));

            // Tseldora:
            D.Add(106000, BossInfo("Prowling Magus and Congregation drop", KEYID.BRANCH));
            D.Add(603000, BossInfo("Duke's Dear Freja drop", KEYID.BRANCH));
            D.Add(603001, BossNGPlusInfo("Duke's Dear Freja drop in NG+", KEYID.BRANCH));
            D.Add(1742000, NpcInfo("Gift from Pate or Creighton when helping in the fight against the other", KEYID.BRANCH));
            D.Add(1742010, NpcInfo("Gift from Creighton when helping in the fight against Pate", KEYID.BRANCH));
            D.Add(1784000, NpcInfo("Gift from Cromwell when over 35FTH", KEYID.BRANCH));
            D.Add(10145060, WChestInfo("Spider-trapped wooden chest before Congregation", KEYID.BRANCH));
            D.Add(10145061, WChestNGPlusInfo("Spider-trapped wooden chest before Congregation in NG+", KEYID.BRANCH));
            D.Add(10145070, WChestInfo("Wooden chest in Tseldora den", KSO(KEYID.BRANCH, KEYID.TSELDORADEN)));
            D.Add(10145080, MChestInfo("Metal chest in tseldora den", KSO(KEYID.BRANCH, KEYID.TSELDORADEN)));
            D.Add(10145110, MChestInfo("Metal chest in secret alcove behind shelf in room next to Ornifex's Tseldora room", KEYID.BRANCH));
            D.Add(10145120, MChestInfo("Metal chest on lowerable platform", KEYID.BRANCH));
            D.Add(10145130, MChestInfo("Vanilla BKUGS: metal chest behind locked door in pickaxe room", KSO(KEYID.BRANCH, KEYID.BRIGHTSTONE)));
            D.Add(10146000, SafeInfo("Behind a bench in the pickaxe room", KEYID.BRANCH));
            D.Add(10146010, SafeInfo("In urn in Ornifex's Tseldora room", KEYID.BRANCH));
            D.Add(10146030, SafeInfo("On balcony of the house just before the spiky mining field", KEYID.BRANCH));
            D.Add(10146040, SafeInfo("Vanilla Priestess set: hanging from window behind illusory wall before Congregation", KEYID.BRANCH));
            D.Add(10146050, SafeInfo("Hanging on side of the well in falconer camp", KEYID.BRANCH));
            D.Add(10146051, NGPlusInfo("Hanging on side of the well in falconer camp in NG+", KEYID.BRANCH));
            D.Add(10146060, SafeInfo("A corpse around miner in falconer camp", KEYID.BRANCH));
            D.Add(10146110, SafeInfo("A corpse around miner in falconer camp", KEYID.BRANCH));
            D.Add(10146140, SafeInfo("A corpse around miner in falconer camp", KEYID.BRANCH));
            D.Add(10146150, SafeInfo("A corpse around miner in falconer camp", KEYID.BRANCH));
            D.Add(10146070, SafeInfo("On ledge before the first spider-decorated door", KEYID.BRANCH));
            D.Add(10146080, SafeInfo("Under the stairs next to Chapel Threshold bonfire", KEYID.BRANCH));
            D.Add(10146090, SafeInfo("On ledge above the sand whirlpool", KEYID.BRANCH));
            D.Add(10146100, SafeInfo("On roof of a house next to the sand whirlpool", KEYID.BRANCH));
            D.Add(10146120, SafeInfo("Vanilla chunk: on ledge guarded by parasitized undead", KEYID.BRANCH));
            D.Add(10146130, SafeInfo("In the well in falconer camp", KEYID.BRANCH));
            D.Add(10146160, SafeInfo("On path under the first boulder", KEYID.BRANCH));
            D.Add(10146170, SafeInfo("Room on upper floor from Lower Brightstone Cove bonfire", KEYID.BRANCH));
            D.Add(10146180, SafeInfo("Vanilla chunk: in back room between the two sand areas", KEYID.BRANCH));
            D.Add(10146181, NGPlusInfo("Vanilla chunk: in back room between the two sand areas in NG+", KEYID.BRANCH));
            D.Add(10146190, SafeInfo("Behind shelf in Ornifex's Tseldora room", KEYID.BRANCH));
            D.Add(10146200, SafeInfo("In a house next to the sand whirlpool", KEYID.BRANCH));
            D.Add(10146210, SafeInfo("In a house in cliffside above the sand whirlpool", KEYID.BRANCH));
            D.Add(10146230, SafeInfo("In urn next to the ruined house in falconer camp", KEYID.BRANCH));
            D.Add(10146020, SafeInfo("On a corpse next to Cromwell", KEYID.BRANCH));
            D.Add(10146240, SafeInfo("On a corpse next to Cromwell", KEYID.BRANCH));
            D.Add(10146250, SafeInfo("On a corpse next to Cromwell", KEYID.BRANCH));
            D.Add(10146260, SafeInfo("On a corpse next to Cromwell", KEYID.BRANCH));
            D.Add(10146270, SafeInfo("On a corpse next to Cromwell", KEYID.BRANCH));
            D.Add(10146280, SafeInfo("In the right side alcove of the pickaxe room", KEYID.BRANCH));
            D.Add(10146290, SafeInfo("On top of the wooden tower in falconer camp", KEYID.BRANCH));
            D.Add(10146300, SafeInfo("In urn in the first room on a way from doors of pharros", KEYID.BRANCH));
            D.Add(10146310, SafeInfo("In urn in the first room after Chapel Threshold bonfire", KEYID.BRANCH));
            D.Add(10146320, SafeInfo("Vanilla CPRx3: in urn in room with Pate/Creighton fight", KEYID.BRANCH));
            D.Add(10146220, SafeInfo("Vanilla torch: on mini staircase in room after Pate/Creighton fight", KEYID.BRANCH));
            D.Add(10146330, SafeInfo("In urn on middle level of the web room before Freja", KEYID.BRANCH));
            D.Add(10146340, SafeInfo("On middle level of the web room before Freja", KEYID.BRANCH));
            D.Add(10146350, SafeInfo("On spider web in the web room before Freja", KEYID.BRANCH));
            D.Add(10146360, SafeInfo("Behind pilar in the lowest level of the web room before Freja", KEYID.BRANCH));
            D.Add(10146370, SafeInfo("Under spider web in the web room before Freja", KEYID.BRANCH));
            D.Add(10146380, SafeInfo("On middle level of the web room before Freja", KEYID.BRANCH));
            D.Add(10146381, NGPlusInfo("On middle level of the final hall before Freja in NG+", KEYID.BRANCH));
            D.Add(10146390, SafeInfo("Next to a boulder in the beginning of the area", KEYID.BRANCH));
            D.Add(10146400, SafeInfo("On ground close to the sand whirlpool", KEYID.BRANCH));
            D.Add(10146410, SafeInfo("A corpse in the spiky mining field", KEYID.BRANCH));
            D.Add(10146420, SafeInfo("A corpse in the spiky mining field", KEYID.BRANCH));
            D.Add(10146480, SafeInfo("In urn next to Congregation foggate", KEYID.BRANCH));
            D.Add(10146490, SafeInfo("In urn on right side when leaving Congregation fight", KEYID.BRANCH));
            D.Add(10146500, SafeInfo("Guide a pig from the campsite to lower tseldora and let it eat mushrooms", KEYID.BRANCH));
            D.Add(10146510, SafeInfo("In urn in the 2nd floor of a house just before the spiky mining field", KEYID.BRANCH));
            D.Add(10146520, SafeInfo("In urn in the pickaxe room", KEYID.BRANCH));
            D.Add(60005000, SafeInfo("From the Ancient dragon corpse in the memory after Freja", KSO(KEYID.BRANCH, KEYID.ASHENMIST)));

            // Drangleic Castle / Throne of Want:
            D.Add(309610, BossInfo("Twin Dragonriders drop", KEYID.DRANGLEIC));
            D.Add(506100, BossInfo("Darklurker drop", KSO(KEYID.FORGOTTEN, KEYID.DRANGLEIC)));
            D.Add(332000, BossInfo("Throne Watcher and Defender drop", KSO(KEYID.DRANGLEIC, KEYID.KINGSRING)));
            D.Add(332001, BossNGPlusInfo("Throne Watcher and Defender drop in NG+", KSO(KEYID.DRANGLEIC, KEYID.KINGSRING)));
            D.Add(627000, BossInfo("Nashandra drop", KSO(KEYID.DRANGLEIC, KEYID.KINGSRING)));
            D.Add(2006000, CovInfo("Pilgrims of Dark join", KSO(KEYID.FORGOTTEN, KEYID.DRANGLEIC)));
            D.Add(2006011, CovInfo("Pilgrims of Dark, one dungeon cleared", KSO(KEYID.FORGOTTEN, KEYID.DRANGLEIC, KEYID.TORCH)));
            D.Add(2006012, CovInfo("Pilgrims of Dark, three dungeons cleared", KSO(KEYID.FORGOTTEN, KEYID.DRANGLEIC, KEYID.TORCH)));
            D.Add(1725000, CovInfo("Pilgrams of Dark, Darklurker defeated", KSO(KEYID.FORGOTTEN, KEYID.DRANGLEIC, KEYID.TORCH))); // Is this an NpcInfo too? a covenant too??
            D.Add(1721000, NpcSafeInfo("Gift from Chancellor Wellager after defeating the Giant Lord", KSO(KEYID.DRANGLEIC, KEYID.ASHENMIST)));
            D.Add(1760200, NpcInfo("Gift when releasing Milfanito from highest tower up the Drangleic elevator", KSO(KEYID.DRANGLEIC, KEYID.EMBEDDED)));
            D.Add(20215000, MChestInfo("Metal chest one level down from the Forgotten Chamber” bonfire", KEYID.DRANGLEIC));
            D.Add(20215010, WChestInfo("Wooden chest in acid pool", KEYID.DRANGLEIC));
            D.Add(20215011, WChestNGPlusInfo("Wooden chest in acid pool in NG+", KEYID.DRANGLEIC));
            D.Add(20215020, WChestInfo("Vanilla dark arrows: wooden chest right after the multi-door Sentinel room", KEYID.DRANGLEIC));
            D.Add(20215021, WChestNGPlusInfo("Vanilla dark arrows: wooden chest right after the multi-door Sentinel room in NG+", KEYID.DRANGLEIC));
            D.Add(20215050, MChestInfo("Metal chest in alcove of one of the doors in the multi-door Sentinel room", KEYID.DRANGLEIC));
            D.Add(20215130, MChestInfo("Metal chest in alcove of one of the doors in the multi-door Sentinel room", KEYID.DRANGLEIC));
            D.Add(20216000, SafeInfo("In alcove of one of the doors in the multi-door Sentinel room", KEYID.DRANGLEIC));
            D.Add(20215070, MChestInfo("Metal chest in the desert sorceress room", KEYID.DRANGLEIC));
            D.Add(20215080, MChestInfo("Metal chest on the right side of the Drangleic gate", KEYID.DRANGLEIC));
            D.Add(20215090, MChestInfo("Metal chest in the upper level of the Drangleic Executioner Chariot room", KEYID.DRANGLEIC));
            D.Add(20215060, MChestInfo("Metal chest in the embedded room", KEYID.DRANGLEIC));
            D.Add(20215100, MChestInfo("Metal chest in the embedded room", KEYID.DRANGLEIC));
            D.Add(20215110, MChestInfo("Metal chest in the embedded room", KEYID.DRANGLEIC));
            D.Add(20215120, MChestInfo("Metal chest in room after the soul-catching golem", KEYID.DRANGLEIC));
            D.Add(20215140, MChestInfo("Metal chest in room after the soul-catching golem", KEYID.DRANGLEIC));
            D.Add(20215160, MChestInfo("Vanilla bone dust: metal chest on left-hand side behind Wellager", KEYID.DRANGLEIC));
            D.Add(20215170, WChestInfo("Wooden chest in the poison dart trap room", KEYID.DRANGLEIC));
            D.Add(20216010, SafeInfo("Vanilla Faraam set: in the cave next to the entrance to the Dark Chasm", KEYID.DRANGLEIC));
            D.Add(20216020, SafeInfo("Vanilla old radiants: next to stairs right after the multi-door Sentinel room", KEYID.DRANGLEIC));
            D.Add(20216021, NGPlusInfo("Vanilla old radiants: next to stairs right after the multi-door Sentinel room in NG+", KEYID.DRANGLEIC));
            D.Add(20216030, SafeInfo("Vanilla elizabeth mushroom: in acid pool", KEYID.DRANGLEIC));
            D.Add(20216050, SafeInfo("Next to Forgotten Chamber” bonfire", KEYID.DRANGLEIC));
            D.Add(20216090, SafeInfo("Vanilla holy water urns: behind a rock on the path towards Drangleic castle", KEYID.DRANGLEIC));
            D.Add(20216100, SafeInfo("Corpse in the desert sorceress room", KEYID.DRANGLEIC));
            D.Add(20216110, SafeInfo("Vanilla DLC3 key: in front of the stairs right after the multi-door Sentinel room", KEYID.DRANGLEIC));
            D.Add(20216040, SafeInfo("In the room with cursed painting of Nashandra", KEYID.DRANGLEIC));
            D.Add(20216130, SafeInfo("In the room with cursed painting of Nashandra", KEYID.DRANGLEIC));
            D.Add(20216140, SafeInfo("In the room with cursed painting of Nashandra", KEYID.DRANGLEIC));
            D.Add(504000, BossInfo("Looking Glass Knight drop", KEYID.AMANA));
            D.Add(504001, BossNGPlusInfo("Looking Glass Knight drop in NG+", KEYID.AMANA));
            D.Add(20215150, MChestInfo("Vanilla ascetic chest: metal chest after Looking Glass Knight", KEYID.AMANA));
            D.Add(20216060, SafeInfo("Between the stone horse-knights", KEYID.AMANA));
            D.Add(20216061, SafeInfo("Between the stone horse-knights in NG+", KEYID.AMANA));
            D.Add(20216070, SafeInfo("Between the stone horse-knights", KEYID.AMANA));
            D.Add(20216120, SafeInfo("Between the stone horse-knights", KEYID.AMANA));
            D.Add(20216080, SafeInfo("Vanilla skulls: first right side in Mirror Knight approach corridor", KEYID.AMANA));

            // Shrine of Amana:
            D.Add(602000, BossInfo("Demon of Song drop", KEYID.AMANA));
            D.Add(1760010, NpcInfo("Gift from Milfanito near Tower of Prayer bonfire", KEYID.AMANA));
            D.Add(1760000, NpcInfo("Gift from Milfanito near Tower of Prayer bonfire after defeating Demon of song", KEYID.AMANA));
            D.Add(1760020, NpcInfo("Gift from Milfanito near Tower of Prayer bonfire after releasing the Milfanito in Drangleic castle", KEYID.AMANA));
            D.Add(1760110, NpcInfo("Gift from Milfanito near Rise of the Dead bonfire", KEYID.AMANA));
            D.Add(1760100, NpcInfo("Gift from Milfanito near Rise of the Dead bonfire after defeating Demon of song", KEYID.AMANA));
            D.Add(1760120, NpcInfo("Gift from Milfanito near Rise of the Dead bonfire after releasing the Milfanito in Drangleic castle", KEYID.AMANA));
            D.Add(20115000, WChestInfo("Wooden chest on side path right through door a few steps below the first bonfire", KEYID.AMANA));
            D.Add(20115010, WChestInfo("Wooden chest submerged in water immediately left from the bottom of the first staircase", KEYID.AMANA));
            D.Add(20115020, WChestInfo("Wooden chest in water on left before the first fog gate", KEYID.AMANA));
            D.Add(20115030, WChestInfo("Wooden chest behind the first hut", KEYID.AMANA));
            D.Add(20115040, MChestInfo("Metal chest in cave before the first fog gate", KEYID.AMANA));
            D.Add(20115050, WChestInfo("Wooden chest in water to the right of the door to Undead Crypt", KEYID.AMANA));
            D.Add(20115051, WChestNGPlusInfo("Wooden chest in water to the right of the door to Undead Crypt in NG+", KEYID.AMANA));
            D.Add(20115060, MChestInfo("Vanilla sunlight blade: metal chest in the end of the narrow underwater walkway", KEYID.AMANA));
            D.Add(20115080, MChestInfo("Metal chest in water side from the second hut", KEYID.AMANA));
            D.Add(20115090, MChestInfo("Vanilla Helix Halberd: metal chest behind a Pharros contraption near the Crumbled Ruins bonfire", KSO(KEYID.AMANA, KEYID.PHARROS)));
            D.Add(20115100, MChestInfo("Metal chest in left-side water at bottom of stairs by the praying Milfanito", KEYID.AMANA));
            D.Add(20115110, MChestInfo("Metal chest in right-side water at bottom of stairs by the praying Milfanito", KEYID.AMANA));
            D.Add(20115070, MChestInfo("Vanilla King's crown: metal chest behind a door that opens after defeating Vendrick", KSO(KEYID.AMANA, KEYID.SOULOFAGIANT)));
            D.Add(20115500, SafeInfo("Vanilla Soul of the King: on a throne behind a door that opens after defeating Vendrick", KSO(KEYID.AMANA, KEYID.SOULOFAGIANT)));
            D.Add(20116000, SafeInfo("On narrow walkway just after Rhoy's Resting Place” bonfire", KEYID.AMANA));
            D.Add(20116010, SafeInfo("Under the circular staircase in the beginning of the area", KEYID.AMANA));
            D.Add(20116011, NGPlusInfo("Under the circular staircase in the beginning of the area in NG+", KEYID.AMANA));
            D.Add(20116030, SafeInfo("In the end of the narrow underwater walkway", KEYID.AMANA));
            D.Add(20116040, SafeInfo("Near the half circle of pillars", KEYID.AMANA));
            D.Add(20116060, SafeInfo("Vanilla near RITB: in the middle of circle of pillars after the Rhoy's resting place” bonfire", KEYID.AMANA));
            D.Add(20116070, SafeInfo("Vanilla near RITB: in the middle of circle of pillars after the Rhoy's resting place” bonfire", KEYID.AMANA));
            D.Add(20116080, SafeInfo("Vanilla near RITB: in the middle of circle of pillars after the Rhoy's resting place” bonfire", KEYID.AMANA));
            D.Add(20116090, SafeInfo("Behind a fallen pilar next to the circle of pillars after the Rhoy's Resting place bonfire", KEYID.AMANA));
            D.Add(20116100, SafeInfo("Next to torch before the demon of song", KEYID.AMANA));
            D.Add(20116110, SafeInfo("In cave under the stairs leading down from the praying Milfanito", KEYID.AMANA));
            D.Add(20116130, SafeInfo("Corpse hanging from branch in the beginning of the area", KEYID.AMANA));
            D.Add(20116140, SafeInfo("On narrow walkway to the right of the second fog gate", KEYID.AMANA));
            D.Add(20116150, SafeInfo("Behind roots before the first bonfire", KEYID.AMANA));
            D.Add(20116120, SafeInfo("On island with three archdrakes before the first fog gate", KEYID.AMANA));
            D.Add(20116020, SafeInfo("On island with three archdrakes before the first fog gate", KEYID.AMANA));
            D.Add(20116160, SafeInfo("On island with three archdrakes before the first fog gate", KEYID.AMANA));
            D.Add(20116170, SafeInfo("In the second hut", KEYID.AMANA));
            D.Add(20116171, NGPlusInfo("In the second hut in NG+", KEYID.AMANA));
            D.Add(20116210, SafeInfo("In the second hut", KEYID.AMANA));
            D.Add(20116190, SafeInfo("On rising-beam to the left of the second fog gate", KEYID.AMANA));
            D.Add(20116200, SafeInfo("Vanilla Life Ring+2: in a cave behind roots with a hippo", KEYID.AMANA));
            D.Add(20116220, SafeInfo("On small island surrounding by lizardmen, left side of the path before the first hut", KEYID.AMANA));

            // Undead Crypt:
            D.Add(333000, BossInfo("Velstadt drop", KEYID.AMANA));
            D.Add(333001, BossNGPlusInfo("Velstadt drop in NG+", KEYID.AMANA));
            D.Add(1506000, NpcInfo("Gift from Agdayne after getting King's Ring", KSO(KEYID.AMANA, KEYID.KINGSRING)));
            D.Add(20245000, MChestInfo("Metal chest near the torch that lights up the big statues", KEYID.AMANA));
            D.Add(20245010, WChestInfo("Wooden chest on left side of the doorway leading to the great hall and Velstadt", KEYID.AMANA));
            D.Add(20245020, MChestInfo("Metal chest on balcony of the second graveyard room", KEYID.AMANA));
            D.Add(20245030, MChestInfo("Vanilla Crushed Eye Orb: metal chest above the bridge after Agdayne", KEYID.AMANA));
            D.Add(20245040, MChestInfo("Metal chest behind a illusory wall and a Pharros contraption from the third graveyard room", KSO(KEYID.AMANA, KEYID.PHARROS)));
            D.Add(20245050, MChestInfo("Metal chest behind a illusory wall from the room where Nameless Usurper invades", KEYID.AMANA));
            D.Add(20245070, MChestInfo("Metal chest right before Velstadt", KEYID.AMANA));
            D.Add(20245080, MChestInfo("Metal chest right before Velstadt", KEYID.AMANA));
            D.Add(20245090, MChestInfo("Metal chest right before Velstadt", KEYID.AMANA));
            D.Add(20245100, MChestInfo("Metal chest right before Velstadt", KEYID.AMANA));
            D.Add(20246000, SafeInfo("Right side of the stairs next to Undead Crypt Entrance bonfire", KEYID.AMANA));
            D.Add(20246010, SafeInfo("Next to Agdayne", KEYID.AMANA));
            D.Add(20246011, NGPlusInfo("Next to Agdayne in NG+", KEYID.AMANA));
            D.Add(20246030, SafeInfo("Under the bridge leading from Agdayne to the second bonfire", KEYID.AMANA));
            D.Add(20246040, SafeInfo("Corpse in the first graveyard room", KEYID.AMANA));
            D.Add(20246050, SafeInfo("Corpse in the second graveyard room", KEYID.AMANA));
            D.Add(20246070, SafeInfo("In side tunnel of the third graveyard room", KEYID.AMANA));
            D.Add(20246100, SafeInfo("In small room above the second bonfire", KEYID.AMANA));
            D.Add(20246110, SafeInfo("Corpse in the first graveyard room", KEYID.AMANA));
            D.Add(20246111, NGPlusInfo("Corpse in the first graveyard room in NG+", KEYID.AMANA));
            D.Add(20246120, SafeInfo("Corpse in the second graveyard room", KEYID.AMANA));
            D.Add(20246121, NGPlusInfo("Corpse in the second graveyard room in NG+", KEYID.AMANA));
            D.Add(20246130, SafeInfo("In room where Nameless Usurper invades", KEYID.AMANA));
            D.Add(20246140, SafeInfo("In room where Nameless Usurper invades", KEYID.AMANA));
            D.Add(20246150, SafeInfo("Corpse in the third graveyard room", KEYID.AMANA));
            D.Add(20246151, NGPlusInfo("Corpse in the third graveyard room in NG+", KEYID.AMANA));
            D.Add(20246180, SafeInfo("Corpse in the first graveyard room", KEYID.AMANA));
            D.Add(20246190, SafeInfo("Corpse in the first graveyard room", KEYID.AMANA));
            D.Add(20246200, SafeInfo("Corpse in the first graveyard room", KEYID.AMANA));
            D.Add(20246210, SafeInfo("Corpse in the first graveyard room", KEYID.AMANA));
            D.Add(20246220, SafeInfo("Corpse in the first graveyard room", KEYID.AMANA));
            D.Add(20246500, SafeInfo("Vanilla King's Ring: looted from Vendrick's armor", KEYID.AMANA));

            // Aldia's Keep
            D.Add(212000, BossInfo("Guardian Dragon drop", KEYID.ALDIASKEEP));
            D.Add(1752000, NpcInfo("Gift from Lucatiel if she has survived at least three summons", KSO(KEYID.ROTUNDA, KEYID.KINGSRING)));
            D.Add(1771010, NpcInfo("Gift from Navlaan after bringing him Ladder Miniature", KSO(KEYID.ALDIASKEEP, KEYID.ROTUNDA)));
            D.Add(1771020, NpcInfo("Gift from Navlaan after bringing him Cale's Helm", KSO(KEYID.ALDIASKEEP, KEYID.ROTUNDA)));
            D.Add(1771030, NpcInfo("Gift from Navlaan after bringing him Sunset Staff", KSO(KEYID.ALDIASKEEP, KEYID.ROTUNDA)));
            D.Add(1771040, NpcInfo("Gift from Navlaan after bringing him Aged Feather", KSO(KEYID.ALDIASKEEP, KEYID.ROTUNDA)));
            D.Add(1771000, NpcInfo("Gift from Navlaan after completing all assassination jobs", KSO(KEYID.ALDIASKEEP, KEYID.ROTUNDA)));
            D.Add(10155000, MChestInfo("Metal chest in a side corridor next to the mirror room", KEYID.ALDIASKEEP));
            D.Add(10155010, MChestInfo("Metal chest in a side corridor next to the giant basilisk", KEYID.ALDIASKEEP));
            D.Add(10155020, MChestInfo("Metal chest behind the breakable chained door", KEYID.ALDIASKEEP));
            D.Add(10155030, MChestInfo("Metal chest in the mirror room", KEYID.ALDIASKEEP));
            D.Add(10156000, SafeInfo("In front of the skeleton dragon", KEYID.ALDIASKEEP));
            D.Add(10156010, SafeInfo("In the mirror room", KEYID.ALDIASKEEP));
            D.Add(10156030, SafeInfo("Under a table in the long corridor", KEYID.ALDIASKEEP));
            D.Add(10156031, NGPlusInfo("Under a table in the long corridor in ng+", KEYID.ALDIASKEEP));
            D.Add(10156040, SafeInfo("Inside a barrel in the corner in side room with caged Gargoyle", KSO(KEYID.ALDIASKEEP, KEYID.ALDIASKEY)));
            D.Add(10156140, SafeInfo("On table in side room with caged Gargoyle", KSO(KEYID.ALDIASKEEP, KEYID.ALDIASKEY)));
            D.Add(10156050, SafeInfo("Room just before Guardian dragon, where the second ogre breaks through", KEYID.ALDIASKEEP));
            D.Add(10156060, SafeInfo("In front of the giant basilisk", KEYID.ALDIASKEEP));
            D.Add(10156130, SafeInfo("Behind a shelf in the potion room", KEYID.ALDIASKEEP));
            D.Add(10156070, SafeInfo("In the acid pool", KEYID.ALDIASKEEP));
            D.Add(10156100, SafeInfo("In the acid pool", KEYID.ALDIASKEEP));
            D.Add(10156160, SafeInfo("In the acid pool", KEYID.ALDIASKEEP));
            D.Add(10156161, NGPlusInfo("In the acid pool in ng+", KEYID.ALDIASKEEP));
            D.Add(10156020, SafeInfo("Vanilla alluring skulls: in the Foregarden courtyard", KEYID.ALDIASKEEP));
            D.Add(10156080, SafeInfo("In the Foregarden courtyard", KEYID.ALDIASKEEP));
            D.Add(10156170, SafeInfo("In the Foregarden courtyard", KEYID.ALDIASKEEP));
            D.Add(10156090, SafeInfo("To the right at the start of the Foregarden courtyard stairs", KEYID.ALDIASKEEP));
            D.Add(10156180, SafeInfo("In the water-feature in the middle of the stairs approaching Aslatiel", KEYID.ALDIASKEEP));
            D.Add(10156190, SafeInfo("In the left side of the front door, before Aslatiel", KEYID.ALDIASKEEP));
            D.Add(10156200, SafeInfo("Stuck in a mirror in the mirror room", KEYID.ALDIASKEEP));
            D.Add(10156150, SafeInfo("Just before the elevator to the dragon aerie", KEYID.ALDIASKEEP));
            D.Add(60050000, SafeInfo("Drop from the skeleton Dragon after defeating four invaders", KSO(KEYID.ALDIASKEEP, KEYID.TORCH)));

            // Dragon Aerie:
            D.Add(1701000, NpcInfo("Gift from the Emerald Herald", KEYID.ALDIASKEEP));
            D.Add(10276010, SafeInfo("Corpse in the first dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276020, SafeInfo("Corpse in the first dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276030, SafeInfo("Corpse in the first dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276120, SafeInfo("Corpse in the first dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276040, SafeInfo("In the cave under the first dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276041, SafeInfo("In the cave under the first dragon nest in NG+", KEYID.ALDIASKEEP));
            D.Add(10276190, SafeInfo("In the cave under the first dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276060, SafeInfo("Near the hollow priest by the second dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276061, SafeInfo("Near the hollow priest by the second dragon nest in NG+", KEYID.ALDIASKEEP));
            D.Add(10276180, SafeInfo("Corpse in the second dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276080, SafeInfo("Corpse in the second dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276070, SafeInfo("In the middle of Aerie, at the end of one of the rope bridges, near multiple exploding hollows", KEYID.ALDIASKEEP));
            D.Add(10276000, SafeInfo("On droppable small ledge before the third dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276050, SafeInfo("On droppable small ledge before the third dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276090, SafeInfo("Corpse in the third dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276100, SafeInfo("Corpse in the third dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276110, SafeInfo("Corpse in the third dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276170, SafeInfo("Corpse in the third dragon nest", KEYID.ALDIASKEEP));
            D.Add(10276130, SafeInfo("On droppable small ledge close to the zip-line start", KEYID.ALDIASKEEP));
            D.Add(10276160, SafeInfo("On a stone pillar you need to drop to from the zip-line", KEYID.ALDIASKEEP));

            // Dragon Shrine:
            D.Add(600000, BossInfo("Ancient dragon drop", KEYID.ALDIASKEEP));
            D.Add(1787000, NpcInfo("Gift from ancient dragon when speaking to him", KEYID.ALDIASKEEP));
            D.Add(10275000, MChestInfo("Metal chest in a side room right after the bonfire", KEYID.ALDIASKEEP));
            D.Add(10275060, MChestInfo("Metal chest behind the Pharros contraption under the staircase", KEYID.ALDIASKEEP));
            D.Add(10275010, WChestInfo("Wooden chest on right side of the stairs, towards petrified egg door", KEYID.ALDIASKEEP));
            D.Add(10275020, WChestInfo("Wooden chest on roof  on the path to the left of the second Drakekeeper", KEYID.ALDIASKEEP));
            D.Add(10275021, WChestNGPlusInfo("Wooden chest on roof  on the path to the left of the second Drakekeeper in NG+", KEYID.ALDIASKEEP));
            D.Add(10276140, SafeInfo("On a corpse hanging off the ledge in the watchtower", KEYID.ALDIASKEEP));
            D.Add(10275030, MChestInfo("Metal chest on a ledge that you jump to from the watchtower", KEYID.ALDIASKEEP));
            D.Add(10275040, MChestInfo("Vanilla Third Dragon Ring: metal chest just past petrified egg door", KEYID.ALDIASKEEP));
            D.Add(10275050, MChestInfo("Metal chest on outer ledge next to the petrified egg"));
            D.Add(10275070, MChestInfo("Metal chest next to the petrified egg", KEYID.ALDIASKEEP));
            D.Add(10276150, SafeInfo("On a corpse in the corner before petrified egg door", KEYID.ALDIASKEEP));
            D.Add(60003000, SafeInfo("Vanilla Petrified Egg: Near the invader in Dragon Shrine")); // Petrified Egg pickup

            // Memories of Jeigh (Giant Lord):
            D.Add(309700, BossInfo("Giant Lord drop", KEYID.MEMORYJEIGH));
            D.Add(309701, BossNGPlusInfo("Giant Lord drop in NG+", KEYID.MEMORYJEIGH));
            D.Add(60004000, SafeInfo("Vanilla Soul of a Giant: Giant corpse at end of all memories", KEYID.ASHENMIST));
            D.Add(20106100, SafeInfo("First left in Giant Lord's memory", KEYID.MEMORYJEIGH));
            D.Add(20106110, SafeInfo("On the battlefield, just before Giant Lord", KEYID.MEMORYJEIGH));
            D.Add(20106111, SafeInfo("On the battlefield, just before Giant Lord in NG+", KEYID.MEMORYJEIGH));
            D.Add(20106120, SafeInfo("Up the second set of stairs in the Giant Lord memory", KEYID.MEMORYJEIGH));

            // Memory of Orro
            D.Add(1743000, NpcInfo("Gift from Benhart if he has survived at least three summons", KSO(KEYID.DRANGLEIC, KEYID.ASHENMIST)));
            D.Add(20105020, WChestInfo("Trapped wooden chest behind a Pharros' contraption on the second floor", KSO(KEYID.ASHENMIST, KEYID.PHARROS)));
            D.Add(20105030, MChestInfo("Metal chest behind a Pharros contraption and an illusory wall on the second floor", KSO(KEYID.ASHENMIST, KEYID.PHARROS)));
            D.Add(20105040, MChestInfo("Metal chest behind a Pharros contraption and an illusory wall on the second floor", KSO(KEYID.ASHENMIST, KEYID.PHARROS)));
            D.Add(20105050, MChestInfo("Metal chest in the room where you need to drop the crane", KEYID.ASHENMIST));
            D.Add(20105060, MChestInfo("Metal chest in the room where you need to drop the crane", KEYID.ASHENMIST));
            D.Add(20106050, SafeInfo("In the corner of the second floor room of the entrace stairs", KEYID.ASHENMIST));
            D.Add(20106070, SafeInfo("On the bridge above courtyard", KEYID.ASHENMIST));
            D.Add(20106080, SafeInfo("In the corner of the courtyard", KEYID.ASHENMIST));
            D.Add(20106090, SafeInfo("At the side of the center of the courtyard", KEYID.ASHENMIST));
            D.Add(20106130, SafeInfo("On the roof looking onto courtyard after climbing ladder", KEYID.ASHENMIST));
            D.Add(20106140, SafeInfo("On scaffolding after climbing a ladder from the courtyard", KEYID.ASHENMIST));
            D.Add(20106141, NGPlusInfo("On scaffolding after climbing a ladder from the courtyard in NG+", KEYID.ASHENMIST));

            // Memory of Vammar
            D.Add(1724000, NpcInfo("Gift from Captain Drummond after defeating Giant lord", KEYID.MEMORYJEIGH));
            D.Add(20105000, WChestInfo("Wooden chest in side room opposite Captain Drummond", KEYID.ASHENMIST));
            D.Add(20105001, WChestNGPlusInfo("Wooden chest in side room opposite Captain Drummond in NG+", KEYID.ASHENMIST));
            D.Add(20105010, MChestInfo("Metal chest in the corner of the main battlefield area", KEYID.ASHENMIST));
            D.Add(20106000, SafeInfo("Behind a corner in the first corridor after Drummond", KEYID.ASHENMIST));
            D.Add(20106010, SafeInfo("On the second floor of the ruined house, just left before the main Vammar battlefield", KEYID.ASHENMIST));
            D.Add(20106011, SafeInfo("On the second floor of the ruined house, just left before the main Vammar battlefield in NG+", KEYID.ASHENMIST));
            D.Add(20106020, SafeInfo("In the corner of the roof", KEYID.ASHENMIST));
            D.Add(20106030, SafeInfo("Vanilla Giant Warrior Club: in the far right corner of the main battlefield area", KEYID.ASHENMIST));
            D.Add(20106040, SafeInfo("At the top of the final stairs in Memory of Vammar", KEYID.ASHENMIST));
            D.Add(20106060, SafeInfo("Next to the stairs just before the end of Memory of Vammar", KEYID.ASHENMIST));
            D.Add(20106061, NGPlusInfo("Next to the stairs just before the end of Memory of Vammar in NG+", KEYID.ASHENMIST));
            D.Add(20106150, SafeInfo("In the corner of the last platform in Memory of Vammar", KEYID.ASHENMIST));

            // DLC1:
            D.Add(682000, BossInfo("Elana, Squalid Queen drop", KEYID.ELANA));
            D.Add(681000, BossInfo("Sinh, the Slumbering Dragon drop", KEYID.ELANA));
            D.Add(862000, BossInfo("Gank Squad drop", KEYID.GANKSQUAD));
            D.Add(862001, BossNGPlusInfo("Gank Squad drop in NG+", KEYID.GANKSQUAD));
            D.Add(50356130, SafeInfo("In the corner of a corridor just after the DLC1 opening door", KEYID.DLC1));
            D.Add(50356000, SafeInfo("On the top of the first building by Photoshop Jump", KEYID.DLC1));
            D.Add(50356010, SafeInfo("In the first corrosive bug room", KEYID.DLC1));
            D.Add(50356020, SafeInfo("Beneath the switch in front of you directly after Photoshop Jump, near stairs", KEYID.DLC1));
            D.Add(50356030, SafeInfo("On ledge outside the first corrosive bug room", KEYID.DLC1));
            D.Add(50356140, SafeInfo("On ledge one stairs down from the Sanctum Walk bonfire", KEYID.DLC1));
            D.Add(50356160, SafeInfo("On ledge next to the two first raising towers", KEYID.DLC1));
            D.Add(50356560, SafeInfo("Vanilla Dark Quartz +3: on top of a stationary tower in the first raising tower area", KEYID.DLC1));
            D.Add(50356400, SafeInfo("On top of a raising tower in the first area", KEYID.DLC1));
            D.Add(50356170, SafeInfo("Hanging from the ledge on the far side just below the first raising towers", KEYID.DLC1));
            D.Add(50356190, SafeInfo("On ledge in the second corrosive bug room", KEYID.DLC1));
            D.Add(50356590, SafeInfo("Next to the first poison statue clump", KEYID.DLC1));
            D.Add(50356600, SafeInfo("Next to the first poison statue clump", KEYID.DLC1));
            D.Add(50356050, SafeInfo("On outer ledge of the Tower of Prayer bonfire", KEYID.DLC1));
            D.Add(50356040, SafeInfo("Stationary tower next to the Tower of Prayer bonfire", KEYID.DLC1));
            D.Add(50356200, SafeInfo("In the base of the tower with the Tower of Prayer bonfire", KEYID.DLC1));
            D.Add(50356060, SafeInfo("On the path next to the row of raising towers below Tower of Prayer", KEYID.DLC1));
            D.Add(50356220, SafeInfo("On the path next to the row of raising towers below Tower of Prayer", KEYID.DLC1));
            D.Add(50356570, SafeInfo("In the middle level of one of the raising towers next to Tower of Prayer", KEYID.DLC1));
            D.Add(50356580, SafeInfo("In the middle level of one of the raising towers next to Tower of Prayer", KEYID.DLC1));
            D.Add(50356410, SafeInfo("In the middle level of the raising tower that creates the path to the whipping tree", KEYID.DLC1));
            D.Add(50356380, SafeInfo("Next to the whipping tree", KEYID.DLC1));
            D.Add(50356530, SafeInfo("Next to the whipping tree", KEYID.DLC1));
            D.Add(50356240, SafeInfo("In the lower elevator room that connects to the Cave of the Dead", KEYID.DLC1));
            D.Add(50356250, SafeInfo("Next to the bridge which Sinh fireballs leading to the Dragon's Sanctum", KEYID.DLC1));
            D.Add(50356430, SafeInfo("Vanilla 20k/Dragon Charms: in small room that opens from button in the beginning of Sanctum", KEYID.DLC1));
            D.Add(50355050, MChestInfo("Metal chest in the room with the first two ghosts", KEYID.DLC1));
            D.Add(50355250, MChestInfo("Metal chest in the room with the two ghosts", KEYID.DLC1));
            D.Add(50355260, MChestInfo("Metal chest in the room with the two ghosts", KEYID.DLC1));
            D.Add(50355270, MChestInfo("Metal chest in the room with the two ghosts", KEYID.DLC1));
            D.Add(50355280, MChestInfo("Metal chest in the room with the two ghosts", KEYID.DLC1));
            D.Add(50355010, WChestInfo("Trapped wooden chest close to the first rotating door", KEYID.DLC1));
            D.Add(50355350, MChestInfo("Metal chest in room that opens from ceiling button close to the first rotating door", KEYID.DLC1));
            D.Add(50356260, SafeInfo("In the room with the first rotating door", KEYID.DLC1));
            D.Add(50356280, SafeInfo("In the bug room before Flynn's Ring", KEYID.DLC1));
            D.Add(50356290, SafeInfo("In the bug room before Flynn's Ring", KEYID.DLC1));
            D.Add(50356300, SafeInfo("In the bug room before Flynn's Ring", KEYID.DLC1));
            D.Add(50356310, SafeInfo("On the stairs in the bug room before Flynn's Ring", KEYID.DLC1));
            D.Add(50355090, MChestInfo("Vanilla Flynn's Ring: in a metal chest in the room up the ladder above the bug room", KEYID.DLC1));
            D.Add(50356270, SafeInfo("Vanilla repair powder: where you drop back to the corridor after Flynn's Ring", KEYID.DLC1));
            D.Add(50356090, SafeInfo("In the spike trapped stairs by Puzzling Sword", KEYID.PUZZLINGSWORD));
            D.Add(50356100, SafeInfo("In the spike trapped stairs by Puzzling Sword", KEYID.PUZZLINGSWORD));
            D.Add(50355020, WChestInfo("Wooden chest in the room after the spike trapped stairs by Puzzling Sword", KEYID.PUZZLINGSWORD));
            D.Add(50355030, WChestInfo("Wooden chest in the room after the spike trapped stairs by Puzzling Sword", KEYID.PUZZLINGSWORD)); 
            D.Add(50355060, MChestInfo("Vanilla Puzzling Sword: metal chest in the room after the spike trapped stairs", KEYID.PUZZLINGSWORD));
            D.Add(50356320, SafeInfo("In the spiky field near Dragon Stone", KEYID.DLC1));
            D.Add(50356330, SafeInfo("In the spiky field near Dragon Stone", KEYID.DLC1));
            D.Add(50356340, SafeInfo("In the spiky field near Dragon Stone", KEYID.DLC1));
            D.Add(50356350, SafeInfo("In the spiky field near Dragon Stone", KEYID.DLC1));
            D.Add(50355120, MChestInfo("Vanilla Dragon Stone: metal chest in room opened by single button next to the spiky field", KEYID.DLC1));
            D.Add(50356420, SafeInfo("Vanilla Sanctum Crossbow: next to the Hidden Sanctum Chamber bonfire", KEYID.DLC1));
            D.Add(50355110, WChestInfo("Trapped wooden chest next to the entrance to the ghost armor room", KEYID.DLC1));
            D.Add(50355070, MChestInfo("Metal chest next to the entrance to the ghost armor room", KEYID.DLC1));
            D.Add(50356630, SafeInfo("Vanilla Gank Squad Key: In the ghost armor room", KEYID.DLC1));
            D.Add(50356640, SafeInfo("In the ghost armor room", KEYID.DLC1));
            D.Add(50355130, MChestInfo("Vanilla Denial: metal chest behind the rotating door next to the spiky field", KEYID.DLC1));
            D.Add(50356360, SafeInfo("Corpse near the stairs before Jester Thomas invades", KEYID.DLC1));
            D.Add(50356370, SafeInfo("In the water area with Dinobutts", KEYID.DLC1));
            D.Add(50356440, SafeInfo("In the water area with Dinobutts", KEYID.DLC1));
            D.Add(50356650, SafeInfo("In the water area with Dinobutts", KEYID.DLC1));
            D.Add(50356660, SafeInfo("In the water area with Dinobutts", KEYID.DLC1));
            D.Add(50356510, SafeInfo("Vanilla DBGS: near the bottom of the elevator that takes you to Elana's bridge", KEYID.DLC1));
            D.Add(50356150, SafeInfo("Just before the bridge activated by Dragon Stone, near the elevator shortcut back to start", KEYID.DLC1));
            D.Add(50356450, SafeInfo("In corridor one level up from Sanctum Interior bonfire", KEYID.ELANA));
            D.Add(50356460, SafeInfo("In side corridor one level up from Dragon's rest", KEYID.ELANA));
            D.Add(50356470, SafeInfo("In side corridor one level up from Dragon's rest", KEYID.ELANA));
            D.Add(50356480, SafeInfo("In side corridor one level up from Dragon's rest", KEYID.ELANA));
            D.Add(50356490, SafeInfo("In side corridor one level up from Dragon's rest", KEYID.ELANA));
            D.Add(50356500, SafeInfo("In side corridor one level up from Dragon's rest", KEYID.ELANA));
            D.Add(50356520, SafeInfo("In the room with many opened chests one level down from Sanctum Interior bonfire", KEYID.ELANA));
            D.Add(50356540, SafeInfo("In Sinh's arena", KEYID.ELANA));
            D.Add(60020000, SafeInfo("Vanilla Crown: in Sinh's arena after defeating him", KEYID.ELANA));
            D.Add(50355190, MChestInfo("Metal chest behind the door that is opened with Eternal Sanctum key", KEYID.GANKSQUAD));
            D.Add(50355200, MChestInfo("Metal chest behind the door that is opened with eternal sanctum key", KEYID.GANKSQUAD));
            D.Add(50355210, MChestInfo("Metal chest behind the door that is opened with eternal sanctum key", KEYID.GANKSQUAD));
            D.Add(50355220, MChestInfo("Metal chest behind the door that is opened with eternal sanctum key", KEYID.GANKSQUAD));
            D.Add(50355230, MChestInfo("Metal chest behind the door that is opened with eternal sanctum key", KEYID.GANKSQUAD));
            D.Add(50355240, MChestInfo("Vanilla Sanctum ShieLd: metal chest in the topmost room (up ladder on the way to Gank Squad)", KEYID.GANKSQUAD));
            D.Add(50356390, SafeInfo("Hanging from ledge in the middle level of the elevator that connects to the Cave of the Dead", KEYID.GANKSQUAD));
            D.Add(50355180, MChestInfo("Vanilla Flower Skirt: metal chest after Gank Squad fight", KEYID.GANKSQUAD));
            D.Add(50356610, SafeInfo("Vanilla brightbugs: In the Cave of the Dead", KEYID.GANKSQUAD));
            D.Add(50356620, SafeInfo("Vanilla ascetics: in the Cave of the Dead", KEYID.GANKSQUAD));
            D.Add(50356670, SafeInfo("In the lower level of the Cave of the Dead", KEYID.GANKSQUAD));
            D.Add(50355140, MChestInfo("Metal chest in the middle level of the elevator that connects to the Cave of the Dead", KEYID.GANKSQUAD));
            D.Add(50355150, MChestInfo("Metal chest in the Cave of the Dead", KEYID.GANKSQUAD));
            D.Add(50356210, SafeInfo("Corpse after Gank Squad fight", KEYID.GANKSQUAD));

            // DLC2:
            D.Add(675000, BossInfo("Fume Knight drop", KEYID.DLC2));
            D.Add(675010, BossInfo("Fume Knight drop in NG+", KEYID.DLC2));
            D.Add(305010, BossInfo("Blue Smelter demon drop", KEYID.BLUESMELTER));
            D.Add(680000, BossInfo("Sir Alonne drop", KEYID.ALONNE));
            D.Add(60019000, SafeInfo("Vanilla Smelter Wedge x6: Very first ash pile before the first chain bridge", KEYID.DLC2));
            D.Add(50367140, SafeInfo("Vanilla Baneful Bird Ring: on railing on top of the tower next to the Throne Floor bonfire requiring elevator", KEYID.FUME));
            D.Add(50366340, SafeInfo("Vanilla Dex Ring: in the end of the stairs by the first Ashen Idol at the top of the tower", KEYID.DLC2));
            D.Add(50366020, SafeInfo("On the topmost round platform of the central pillar", KEYID.DLC2));
            D.Add(50366360, SafeInfo("On a round platform of the central pillar close to the top of the tower", KEYID.DLC2));
            D.Add(50366380, SafeInfo("On a round platform of the central pillar close to the top of the tower", KEYID.DLC2));
            D.Add(50366350, SafeInfo("In first (top) ash ledge beneath the chain; near a gate", KEYID.DLC2));
            D.Add(50368000, SafeInfo("Inside ash statue in the first (top) ashe ledge beneath the chain", KEYID.DLC2));
            D.Add(50366030, SafeInfo("On a corpose underneath the first chain towards Throne Floor bonfire", KEYID.DLC2));
            D.Add(50365000, MChestInfo("Metal chest just above the second Ashen Idol guarded by possessed armor", KEYID.DLC2));
            D.Add(50366900, SafeInfo("In the first area with Cask Runners", KEYID.DLC2));
            D.Add(50365010, WChestInfo("Wooden chest outside just before the Upper Floor bonfire", KEYID.DLC2));
            D.Add(50366000, SafeInfo("In the corner of small room next to the second Ashen Idol", KEYID.DLC2));
            D.Add(50365560, MChestInfo("Metal chest up the ladder from the second Ashen Idol"));
            D.Add(50366370, SafeInfo("In a corridor up the ladder from the second Ashen Idol", KEYID.DLC2));
            D.Add(50366390, SafeInfo("On ledge next to the Leap of Faith illusory rock after the second Ashen Idol", KEYID.DLC2));
            D.Add(50365500, MChestInfo("Metal chest behind Leap of Faith illusory rock after the second Ashen Idol", KEYID.DLC2));
            D.Add(50365510, MChestInfo("Metal chest behind Leap of Faith illusory rock after the second Ashen Idol", KEYID.DLC2));
            D.Add(50365090, WChestInfo("Wooden chest in a side corridor next to the Upper Floor bonfire", KEYID.DLC2));
            D.Add(50366260, SafeInfo("In Lever Room with the third Ashen Idol and multiple other enemies", KEYID.DLC2));
            D.Add(50366440, SafeInfo("On a corpse, up a ladder from Lever Room (third Ashen Idol)", KEYID.DLC2));
            D.Add(50366280, SafeInfo("In a circle of loot just after Lever Room guarded by possessed armor", KEYID.DLC2));
            D.Add(50366300, SafeInfo("In a circle of loot just after Lever Room guarded by possessed armor", KEYID.DLC2));
            D.Add(50366310, SafeInfo("In a circle of loot just after Lever Room guarded by possessed armor", KEYID.DLC2));
            D.Add(50366320, SafeInfo("In a circle of loot just after Lever Room guarded by possessed armor", KEYID.DLC2));
            D.Add(50366480, SafeInfo("On ledge with two Fume Sorcerers above the outside ash field", KEYID.DLC2));
            D.Add(50368020, SafeInfo("Inside ash statue on ledge after the second Ashen Idol", KEYID.DLC2));
            D.Add(50368030, SafeInfo("Inside ash statue on bigger ash covered field before the Foyer bonfire", KEYID.DLC2));
            D.Add(50368040, SafeInfo("Inside ash statue on bigger ash covered field before the Foyer bonfire", KEYID.DLC2));
            D.Add(50368050, SafeInfo("Inside ash statue on bigger ash covered field before the Foyer bonfire", KEYID.DLC2));
            D.Add(50366510, SafeInfo("In corner just before the chain bridge to the tower where Maldron invades", KEYID.DLC2));
            D.Add(50365700, MChestInfo("Metal chest next to Maldron", KEYID.DLC2));
            D.Add(50366810, SafeInfo("On a round platform of the central pillar of the tower where Maldron invades", KEYID.DLC2));
            D.Add(50368060, SafeInfo("Inside ash statue at the bottom of the tower where Maldron invades", KEYID.DLC2));
            D.Add(50366820, SafeInfo("In the stairs of the tower where Maldron invades", KEYID.DLC2));
            D.Add(50366800, MChestInfo("Metal chest in the bottom of the tower where Maldron invades", KEYID.DLC2));
            D.Add(50366170, SafeInfo("Vanilla Spell Quartz +3: in the corner next to Foyer bonfire", KEYID.DLC2));
            D.Add(50366580, SafeInfo("In the room immediately after the Foyer bonfire", KEYID.DLC2));
            D.Add(50365540, MChestInfo("Vanilla Catarina Set: metal chest behind explodable wall in the corridor with Fume Sorcerers", KEYID.DLC2));
            D.Add(50365590, MChestInfo("Vanilla PDBx8 :Metal chest behind explodable wall in the corridor with Fume Sorcerers", KEYID.DLC2));
            D.Add(50366570, SafeInfo("In the shortcut between the tower where Scorching Iron Scepter is and the Foyer bonfire", KEYID.DLC2));
            D.Add(50367090, SafeInfo("In a small sideroom above the Quicksword Rachel invasion", KEYID.DLC2));
            D.Add(50367100, SafeInfo("In the curved corridor with crawlers before Quicksword Rachel", KEYID.DLC2));
            D.Add(50367110, SafeInfo("In a small sideroom next to the room where Quicksword Rachel invades", KEYID.DLC2));
            D.Add(50367120, SafeInfo("In a small sideroom next to the room where Quicksword Rachel invades", KEYID.DLC2));
            D.Add(60014000, SafeInfo("Vanilla Scorching Scepter: bottom floor of side tower after Foyer", KEYID.DLC2));
            D.Add(50366830, SafeInfo("On an ash covered area one level down from the Foyer bonfire near vanilla Tower Key", KEYID.FUME));
            D.Add(50366850, SafeInfo("On an ash covered area one level down from the Foyer bonfire near vanilla Tower Key", KEYID.FUME));
            D.Add(50366860, SafeInfo("On an ash covered area one level down from the Foyer bonfire near vanilla Tower Key", KEYID.FUME));
            D.Add(50366870, SafeInfo("On an ash covered area one level down from the Foyer bonfire near vanilla Tower Key", KEYID.FUME));
            D.Add(50366710, SafeInfo("On an ash covered area one level down from the Foyer bonfire near vanilla Tower Key", KEYID.FUME));
            D.Add(50366720, SafeInfo("On an ash covered area one level down from the Foyer bonfire near vanilla Tower Key", KEYID.FUME));
            D.Add(50368070, SafeInfo("In ash statue in area one level down from the Foyer bonfire near vanilla Tower Key", KEYID.FUME));
            D.Add(50366210, SafeInfo("Vanilla Tower Key: corpse on the cliff edge of ash area one level below Foyer bonfire", KEYID.FUME));
            D.Add(50365680, MChestInfo("Vanilla Strength Ring: netal chest on the top floor of the multi-level room", KEYID.FUME));
            D.Add(50366760, SafeInfo("In the lower room of the multi-level room on the way to Fume Knight", KEYID.FUME));
            D.Add(50365080, MChestInfo("Vanilla Sorcery Clutch Ring: metal chest in a side room above Fume's elevator", KEYID.FUME));
            D.Add(50368010, SafeInfo("Inside ash statue on the left side of the Fume Knight arena", KEYID.FUME));
            D.Add(50368080, SafeInfo("Inside ash statue on the right side of the Fume Knight arena", KEYID.FUME));
            D.Add(60016000, SafeInfo("Vanilla Crown: in ash pile after defeating Fume Knight", KEYID.FUME));
            D.Add(50365020, MChestInfo("Vanilla Life Ring +3: metal chest next to Ashen Idol on the same level as the Upper Floor bonfire", KEYID.FUME));
            D.Add(50365550, MChestInfo("Vanilla Pilgrim's Spontoon (Pseudo jumps): metal chest in a small room where you need to jump from an elevator next to the Upper floor bonfire", KEYID.FUME));
            D.Add(50366240, SafeInfo("Behind a door next to Ashen idol on the same level as the Upper Floor bonfire", KEYID.FUME));
            D.Add(50366250, SafeInfo("Behind a door next to Ashen idol on the same level as the Upper Floor bonfire", KEYID.FUME));
            D.Add(50367130, SafeInfo("Behind a door next to Ashen idol on the same level as the Upper Floor bonfire", KEYID.FUME));
            D.Add(50366880, SafeInfo("Behind a door next to Ashen Idol on the same level as the Upper Floor bonfire", KEYID.FUME));
            D.Add(50366890, SafeInfo("Behind a door next to Ashen Idol on the same level as the Upper Floor bonfire", KEYID.FUME));
            D.Add(50365690, MChestInfo("Metal chest behind illusory wall from the elevator shaft going up from the Upper Floor bonfire", KEYID.FUME));
            D.Add(50365580, MChestInfo("Vanilla brightbugs: metal chest in room with lizard; jump from the elevator going up from Foyer", KEYID.FUME));
            D.Add(50366070, SafeInfo("Vanilla Simpleton's Ring: on outside ledge accessed from a room with a rotating fiery bull statue next to the Upper Floor bonfire", KEYID.FUME));
            D.Add(50366530, SafeInfo("In front of a rotating fiery bull statue in a room next to the Upper Floor bonfire", KEYID.FUME));
            D.Add(50365650, MChestInfo("Metal chest in the upper floor of curved corridor opposite the gate to Sir Alonne", KEYID.FUME));
            D.Add(50366680, SafeInfo("In the lower floor of curved corridor opposite the gate to Sir Alonne", KEYID.FUME));
            D.Add(50366700, SafeInfo("Vanilla Dispelling Ring +1: behind explodable wall the curved corridor opposite the gate to Sir Alonne", KEYID.FUME));
            D.Add(50365570, MChestInfo("Metal chest in the dark cursed area next to the Foyer bonfire", KEYID.BLUESMELTER));
            D.Add(50366520, SafeInfo("On altar in the dark cursed area next to the Foyer bonfire", KEYID.BLUESMELTER));
            D.Add(50365030, WChestInfo("Wooden chest in the left of the dark cursed area next to the Foyer bonfire", KEYID.BLUESMELTER));
            D.Add(50366740, SafeInfo("One elevator up from the Iron Passage bonfire", KEYID.BLUESMELTER));
            D.Add(50367010, SafeInfo("In a cell on the right just before the first gate of Iron Passage", KEYID.BLUESMELTER));
            D.Add(50367020, SafeInfo("In a cell just before the second gate of Iron Passage", KEYID.BLUESMELTER));
            D.Add(50367030, SafeInfo("In a cell just before the second gate", KEYID.BLUESMELTER));
            D.Add(50367040, SafeInfo("On an upper ledge of the first big room after passing through first gate", KEYID.BLUESMELTER));
            D.Add(50367050, SafeInfo("On an upper ledge of the second big room after passing through first gate and dropping", KEYID.BLUESMELTER));
            D.Add(50366980, SafeInfo("On an upper ledge of the third big room after passing through the second gate", KEYID.BLUESMELTER));
            D.Add(50366990, SafeInfo("On an upper ledge of the third big room after passing through the second gate", KEYID.BLUESMELTER));
            D.Add(50367000, SafeInfo("On lower level of the third big room before Blue Smelter", KEYID.BLUESMELTER));
            D.Add(50367060, SafeInfo("Right after the Blue Smelter Demon", KEYID.BLUESMELTER));
            D.Add(50366910, SafeInfo("In the middle of the first hall in Alonne memory", KEYID.ALONNE));
            D.Add(50366920, SafeInfo("In the middle of the first hall in Alonne memory", KEYID.ALONNE));
            D.Add(50366930, SafeInfo("In the middle of the first hall in Alonne memory", KEYID.ALONNE));
            D.Add(50366940, SafeInfo("On side alcove on the left side of the first hall", KEYID.ALONNE));
            D.Add(50366950, SafeInfo("In the middle of the second hall in Alonne memory", KEYID.ALONNE));
            D.Add(50366970, SafeInfo("In the middle of the second hall in Alonne memory", KEYID.ALONNE));
            D.Add(50366960, SafeInfo("On side alcove on the lower level of the second hall", KEYID.ALONNE));
            D.Add(60013000, SafeInfo("On throne after Sir Alonne", KEYID.ALONNE));
            D.Add(60012100, SafeInfo("Ashen Idol in the top of the tower next to the Throne Floor bonfire", KEYID.NADALIA));
            D.Add(60012000, SafeInfo("Ashen Idol (the second ashen idol from the top of the tower)", KEYID.NADALIA));
            D.Add(60012050, SafeInfo("Ashen Idol in the Lever Rooom with multiple other enemies", KEYID.NADALIA));
            D.Add(60012080, SafeInfo("Ashen idol in the tower where Maldron invades", KEYID.NADALIA));
            D.Add(60012010, SafeInfo("Ashen Idol on the left side of the Fume Knight arena", KSO(KEYID.FUME, KEYID.NADALIA)));
            D.Add(60012020, SafeInfo("Ashen Idol on the left side of the Fume Knight arena", KSO(KEYID.FUME, KEYID.NADALIA)));
            D.Add(60012030, SafeInfo("Ashen Idol on the right side of the Fume Knight arena", KSO(KEYID.FUME, KEYID.NADALIA)));
            D.Add(60012040, SafeInfo("Ashen Idol on the right side of the Fume Knight arena", KSO(KEYID.FUME, KEYID.NADALIA)));
            D.Add(60012060, SafeInfo("Ashen Idol down the stairs from the Smelter Throne bonfire", KSO(KEYID.FUME, KEYID.TOWER, KEYID.NADALIA)));
            D.Add(60012070, SafeInfo("Ashen Idol in the dark curse area next to Foyer bonfire", KSO(KEYID.BLUESMELTER, KEYID.NADALIA)));
            D.Add(60012090, SafeInfo("Ashen Idol on ledge behind a door on the same level as the Upper floor bonfire", KSO(KEYID.FUME, KEYID.NADALIA)));

            // DLC3:
            D.Add(679000, BossInfo("Aava, the King's Pet drop", KEYID.DLC3));
            D.Add(690000, BossInfo("Ivory King drop", KEYID.DLC3));
            D.Add(679010, BossInfo("Lud and Zallen drop", KEYID.FRIGIDOUTSKIRTS));
            D.Add(50375710, MChestInfo("Vanilla Vessel Shield: netal chest after Aava", KEYID.DLC3));
            D.Add(1788000, CovInfo("Gift from Alsanna after bringing her 5 Loyce souls", KEYID.DLC3));
            D.Add(1788010, CovInfo("Gift from Alsanna after bringing her 15 Loyce souls", KEYID.DLC3));
            D.Add(1788020, CovInfo("Gift from Alsanna after bringing her 35 Loyce souls", KEYID.DLC3));
            D.Add(1788030, CovInfo("Gift from Alsanna after bringing her 50 Loyce souls", KEYID.DLC3));
            D.Add(60031000, SafeInfo("Vanilla Crown: in the Ivory King arena after defeating him", KEYID.DLC3));
            D.Add(50376340, SafeInfo("In a corner of the first room right from the Outer Wall bonfire", KEYID.DLC3));
            D.Add(50376410, SafeInfo("In a narrow corridor in the beginning of the area", KEYID.DLC3));
            D.Add(50376350, SafeInfo("In an area with ice dogs in the beginning", KEYID.DLC3));
            D.Add(50376050, SafeInfo("Under the stairs before the square with a fountain", KEYID.DLC3));
            D.Add(50376750, SafeInfo("Behind ice just before the square with a fountain", KEYID.DLC3));
            D.Add(50376760, SafeInfo("Behind ice just before the square with a fountain", KEYID.DLC3));
            D.Add(50376070, SafeInfo("In small room right after the square with a fountain", KEYID.DLC3));
            D.Add(50376060, SafeInfo("Under ice in the square with a fountain", KEYID.DLC3));
            D.Add(50376010, SafeInfo("Behind ice just before the square with a fountain", KEYID.DLC3));
            D.Add(50375510, MChestInfo("Metal chest on roof of a house near the square with a fountain", KEYID.DLC3));
            D.Add(50375520, MChestInfo("Metal chest on roof of a house near the square with a fountain", KEYID.DLC3));
            D.Add(50375540, MChestInfo("Metal chest on roof of a house near the square with a fountain", KEYID.DLC3));
            D.Add(50376000, SafeInfo("Near Vanilla Dark Clutch On snowy ledge accessed by dropping from the roof before fountain square", KEYID.DLC3));
            D.Add(50376080, SafeInfo("Vanilla Dark Clutch Ring: under a tree accessed by dropping from the roof before fountain square", KEYID.DLC3));
            D.Add(50376090, SafeInfo("Vanilla Old Bell Helm: on ledge at the end of the path on left side before Abandoned Dwelling bonfire", KEYID.DLC3));
            D.Add(50376580, SafeInfo("In the right side of the cave opened by lighting torches", KSO(KEYID.DLC3, KEYID.TORCH)));
            D.Add(50376300, SafeInfo("Vanilla Garrison Ward Key: guarded by Flexile Sentry in the cave opened by lighting torches", KSO(KEYID.DLC3, KEYID.TORCH)));
            D.Add(50376570, SafeInfo("Behind illusory wall in the left side of the cave opened by lighting torches", KSO(KEYID.DLC3, KEYID.TORCH)));
            D.Add(50376540, SafeInfo("Vanilla Ring of the Embedded: On a snow ledge after a narrow gap between buildings after Abandoned Dwelling bonfire", KEYID.DLC3));
            D.Add(50376530, SafeInfo("On a higher part of the courtyard after Abandoned Dwelling bonfire", KEYID.DLC3));
            D.Add(50376100, SafeInfo("On the courtyard after Abandoned Dwelling bonfire", KEYID.DLC3));
            D.Add(50376110, SafeInfo("Vanilla Radiants: next to a Rampart Golem by the lever to Outer Wall", KEYID.DLC3));
            D.Add(50375580, MChestInfo("Metal chest in round room with mimic near Expulsion Chamber bonfire", KEYID.DLC3));
            D.Add(50375590, MChestInfo("Metal chest in round room with mimic near Expulsion Chamber bonfire", KEYID.DLC3));
            D.Add(50375600, MChestInfo("Metal chest in round room with mimic near Expulsion Chamber bonfire", KEYID.DLC3));
            D.Add(50375610, MChestInfo("Metal chest in round room with mimic near Expulsion Chamber bonfire", KEYID.DLC3));
            D.Add(50376120, SafeInfo("On the main ballista bridge", KEYID.DLC3));
            D.Add(50376130, SafeInfo("On the main ballista bridge", KEYID.DLC3));
            D.Add(50376140, SafeInfo("On the main ballista bridge", KEYID.DLC3));
            D.Add(50375550, MChestInfo("Vanilla Elizabeth Mushrooms: metal chest at the end of the main ballista bridge"));
            D.Add(50375530, MChestInfo("Metal chest behind the elevator full of coffins", KEYID.DLC3));
            D.Add(50376560, SafeInfo("On upper floor behind the elevator full of coffins", KEYID.DLC3));
            D.Add(50375740, MChestInfo("Metal chest behind Pharros contraption on the ballista bridge", KSO(KEYID.DLC3, KEYID.PHARROS)));
            D.Add(50376590, SafeInfo("On lower floor of the tower with the invisible ladder", KEYID.DLC3));
            D.Add(50376360, SafeInfo("On middle floor balcony of the tower with the invisible ladder", KEYID.DLC3));
            D.Add(50375640, MChestInfo("Metal chest on top floor of the tower with the invisible ladder", KEYID.DLC3));
            D.Add(50376150, SafeInfo("Next to a Rampart Golem near the tower with the invisible ladder", KEYID.DLC3));
            D.Add(50376370, SafeInfo("In small room between stairs leading to the Eye of the Priestess", KEYID.DLC3));
            D.Add(50375500, SafeInfo("Vanilla Eye of the Priestess: on altar", KEYID.DLC3));
            D.Add(50376160, SafeInfo("Just before the fog gate leading to the Inner Wall bonfire", KEYID.DLC3));
            D.Add(50376170, SafeInfo("Just before the fog gate leading to the Inner Wall bonfire", KEYID.DLC3));
            D.Add(50375690, MChestInfo("Metal chest under ice in the first room of Inner Wall", KEYID.DLC3));
            D.Add(50376600, SafeInfo("In the large empty room in the middle floor of Inner Wall", KEYID.DLC3));
            D.Add(50375700, MChestInfo("Metal chest surrounded by three Frozen Golems on upper floor of Inner Wall", KEYID.DLC3));
            D.Add(50375730, MChestInfo("Metal chest in the corridor where Holy Knight Aurheim invades", KEYID.DLC3));
            D.Add(50376380, SafeInfo("In a dead end corridor in Inner Wall near the Eleum Knight and by the illusory wall", KEYID.DLC3));
            D.Add(50375670, MChestInfo("Metal chest behind Inner Wall illusory near route back to bonfire", KEYID.DLC3));
            D.Add(50375660, MChestInfo("Vanilla Fire Clutch Ring: metal chest behind Inner Wall illusory wall and up stairs", KEYID.DLC3));
            D.Add(50376510, SafeInfo("On upper floor balcony of the (wooden) multi-level area down the middle ballista bridge stairs", KEYID.DLC3));
            D.Add(50376770, SafeInfo("Next to the shortcut door up a ladder in multi-level area down the middle ballista bridge stairs", KEYID.DLC3));
            D.Add(50376400, SafeInfo("On lower-middle floor balcony of the (wooden) multi-level area down the middle ballista bridge stairs", KEYID.DLC3));
            D.Add(50375680, WChestInfo("Wooden chest on bottom floor of (wooden) multi-level area down the middle ballista bridge stairs", KEYID.DLC3));
            D.Add(50376310, SafeInfo("Guarded by three hedgehogs at the start of the Lower Garrison area", KEYID.DLC3));
            D.Add(50376320, SafeInfo("Guarded by three hedgehogs just at the start of the Lower Garrison area", KEYID.DLC3));
            D.Add(50376180, SafeInfo("Before Hexer Nicolai invades behind a pillar near hedgehogs", KEYID.DLC3));
            D.Add(50376190, SafeInfo("Where Hexer Nicolai invades guarded by a hedgehog", KEYID.DLC3));
            D.Add(50376660, SafeInfo("Where Hexer Nicolai invades, near the shortcut back to the Lower Garrison courtyard", KEYID.DLC3));
            D.Add(50376200, SafeInfo("Hanging from ledge near Lower Garrison bonfire", KEYID.DLC3));
            D.Add(50375560, MChestInfo("Metal chest on upper level of the Lower Garrison courtyard", KEYID.DLC3));
            D.Add(50376520, SafeInfo("Hanging from the ledge outside the tower of the Eleum Knight near snowball start", KEYID.DLC3));
            D.Add(50376610, SafeInfo("In Covetous Demons cave", KEYID.DLC3));
            D.Add(50376620, SafeInfo("In Covetous Demons cave", KEYID.DLC3));
            D.Add(50376630, SafeInfo("In Covetous Demons cave", KEYID.DLC3));
            D.Add(50376670, SafeInfo("In Covetous Demons cave", KEYID.DLC3));
            D.Add(50376680, SafeInfo("In Covetous Demons cave", KEYID.DLC3));
            D.Add(50376690, SafeInfo("In Covetous Demons cave", KEYID.DLC3));
            D.Add(50376640, SafeInfo("On a corpse by hedgehogs and frozen golems on the way to snowball", KEYID.DLC3));
            D.Add(50376650, SafeInfo("On a corpse by hedgehogs and frozen golems on the way to snowball", KEYID.DLC3));
            D.Add(50376420, SafeInfo("On the broken where the snowball finishes", KEYID.DLC3));
            D.Add(50376430, SafeInfo("On the broken where the snowball finishes", KEYID.DLC3));
            D.Add(50376440, SafeInfo("On the broken where the snowball finishes", KEYID.DLC3));
            D.Add(50376730, SafeInfo("Under the coffin you ride in on in Frigid Outskirts", KEYID.FRIGIDOUTSKIRTS));
            D.Add(50376740, SafeInfo("In one of the coffins at the start of Frigid Outskirts", KEYID.FRIGIDOUTSKIRTS));
            D.Add(50376220, SafeInfo("In the first house in Frigid Outskirts", KEYID.FRIGIDOUTSKIRTS));
            D.Add(50376450, SafeInfo("Outside the first house in Frigid Outskirts", KEYID.FRIGIDOUTSKIRTS));
            D.Add(50376230, SafeInfo("Between the first and the second house in Frigid Outskirts", KEYID.FRIGIDOUTSKIRTS));
            D.Add(50376460, SafeInfo("Between the second and the third house in Frigid Outskirts", KEYID.FRIGIDOUTSKIRTS));
            D.Add(50376470, SafeInfo("In the third house in Frigid Outskirts", KEYID.FRIGIDOUTSKIRTS));
            D.Add(50376710, SafeInfo("On ledge just before Lud and Zallen", KEYID.FRIGIDOUTSKIRTS));
    }



    };

    internal class KeySet
    {
        internal KEYID[] Keys;
        internal KeySet(params KEYID[] keys)
        {
            Keys = keys;
        }
    }
}
