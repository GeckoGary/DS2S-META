﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DS2S_META
{
    class DS2SItem : IComparable<DS2SItem>
    {

        public enum ItemType
        {
            Weapon = 0,
            Armor = 1,
            Item = 2,
            Ring = 3
        }
       
        private static Regex itemEntryRx = new Regex(@"^\s*(?<id>\S+)\s+(?<name>.+)$");

        private bool ShowID;

        public string Name;
        public int ID;
        public ItemType Type;

        public static Dictionary<int, string> Items = new Dictionary<int, string>()
        {

            {3400000 ,"Fist"}
        };

        public DS2SItem(string config, int type, bool showID)
        {
            Match itemEntry = itemEntryRx.Match(config);
            ID = Convert.ToInt32(itemEntry.Groups["id"].Value);
            Type = (ItemType)type;
            ShowID = showID;
            if (showID)
                Name = ID.ToString() + ": " + itemEntry.Groups["name"].Value;
            else
                Name = itemEntry.Groups["name"].Value;

            Items.Add(ID, itemEntry.Groups["name"].Value);
        }
        public override string ToString()
        {
            return Name;
        }
        public int CompareTo(DS2SItem other)
        {
            if (ShowID)
                return ID.CompareTo(other.ID);
            else
                return Name.CompareTo(other.Name);
        }
    }
}