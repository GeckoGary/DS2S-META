﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS2S_META.Utils
{
    /// <summary>
    /// Data Class for storing Weapons
    /// </summary>
    public class WeaponRow : Param.Row
    {
        // Behind-fields:
        public int _reinforceID;
        public int _weapontypeID;
        public float _damageMultiplier;

        // Properties:
        public int ReinforceID
        {
            get => _reinforceID;
            set
            {
                _reinforceID = value;
                WriteAt(2, BitConverter.GetBytes(value));
            }
        }
        public int WeaponTypeID
        {
            get => _weapontypeID;
            set
            {
                _weapontypeID = value;
                WriteAt(4, BitConverter.GetBytes(value));
            }
        }
        public float DamageMultiplier
        {
            get => _damageMultiplier;
            set
            {
                _damageMultiplier = value;
                WriteAt(34, BitConverter.GetBytes(value));
            }
        }
        public short CounterDmg;

        // Linked param:
        internal WeaponReinforceRow? ReinforceRow => ParamMan.GetLink<WeaponReinforceRow>(ParamMan.WeaponReinforceParam, ReinforceID);
        internal WeaponTypeRow? WTypeRow => ParamMan.GetLink<WeaponTypeRow>(ParamMan.WeaponTypeParam, WeaponTypeID);

        // Wrappers
        internal int MaxUpgrade => ReinforceRow == null ? 0 : ReinforceRow.MaxReinforce;
        public List<DS2SInfusion> GetInfusionList()
        {
            if (ReinforceRow == null)
                return new List<DS2SInfusion>() { DS2SInfusion.Infusions[0] };
            return ReinforceRow.GetInfusionList();
        }
            

        // Constructor:
        public WeaponRow(Param param, string name, int id, int offset) : base(param, name, id, offset)
        {
            ReinforceID = (int)ReadAt(2);
            WeaponTypeID = (int)ReadAt(4);
            DamageMultiplier = (float)ReadAt(34);
            CounterDmg = (short)ReadAt(46);
        }
    }
}
