﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS2S_META.Utils.Offsets
{
    internal class DS2VOffsetsV112 : DS2VOffsets
    {
        public DS2VOffsetsV112() : base()
        {
            //// V1.02
            //PlayerStatsOffsets = new int[] { 0x20, 0x28, 0x110, 0x70, 0xA0, 0x170, 0x718 };

            if (Func == null)
                return;

            Func.ApplySpEffectAoB = "89 6c 24 fc 8d 64 24 fc 54 5d 8b 45 08";
        }
    }
}