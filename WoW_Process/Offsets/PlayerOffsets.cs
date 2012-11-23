using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stab_Face.WoW_Process.Offsets {
    public class PlayerOffsets : GlobalOffsets {
        public const UInt16 POWER_OFFSET = 0x1DC0;
        public const UInt16 IS_CASTING_OFFSET = 0x0B4; //0xD5A;
        public const UInt16 IS_CHANNELING_OFFSET = 0x1FB0;
        public const UInt16 CUR_TARGET_GUID_OFFSET = 0x1DB0;
        public const UInt16 HP_OFFSET = 0x1DC8;
        public const UInt16 MAX_HP_OFFSET = 0x1DE0;
        public const UInt16 IN_COMBAT_OFFSET = 0x1E2A;
        public const UInt16 DEBUFFS_OFFSET = 0x1EAC;
        public const UInt16 BUFFS_OFFSET = 0x1E2C;
        public const UInt16 IS_MOVING = 0xA2C;
    }
}
