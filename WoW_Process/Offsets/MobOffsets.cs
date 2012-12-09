using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stab_Face.WoW_Process.Offsets {
    public class MobOffsets : GlobalOffsets
    {
        public const UInt16 HP_OFFSET = 0xEC0;
        public const UInt16 NAME = 0xDB8;
        public const UInt16 TARGET_GUID = 0xEA8; //0xC48; // EA8
    }
}
