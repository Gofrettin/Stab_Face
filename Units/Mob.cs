using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Stab_Face.WoW_Process.Offsets;
using Stab_Face.Misc;
using Stab_Face.Memory;

namespace Stab_Face.Units
{
    class Mob : Unit
    {
        public Mob(UInt32 objBase)
            : base(objBase)
        {

        }

        public Waypoint getLocation()
        {
            float X = MemoryReader.readFloat(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, this.objBase + MobOffsets.X);
            float Y = MemoryReader.readFloat(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, this.objBase + MobOffsets.Y);
            float Z = MemoryReader.readFloat(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, this.objBase + MobOffsets.Z);
            return new Waypoint(X, Y, Z);
        }
    }
}
