using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Stab_Face.WoW_Process.Offsets;
using Stab_Face.Misc;
using Stab_Face.Memory;

namespace Stab_Face.Units
{
    public class Mob : Unit
    {
        public Mob(UInt32 objBase)
            : base(objBase)
        {

        }

        public UInt32 getHP()
        {
            return MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, this.objBase + MobOffsets.HP_OFFSET);
        }

        public UInt32 getName()
        {
            return MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, this.objBase + MobOffsets.NAME);
        }
    }
}
