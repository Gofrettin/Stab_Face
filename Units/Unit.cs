using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stab_Face.Memory;
using Stab_Face.WoW_Process.Offsets;
using Stab_Face.Misc;

namespace Stab_Face.Units
{
    public class Unit
    {
        protected UInt32 objBase;
        protected String name;
        protected UInt64 GUID;
        protected UInt32 HP;
        protected UInt32 power;
        protected UInt64 targetGUID;
        protected UInt32 unitType;
        protected Unit target;

        public Unit(UInt32 objBase)
        {
            this.objBase = objBase;
        }

        // A few variables are common among all Units:
        // GUID, Health, target, location
        // override these if you need localized Offsets

        /// <summary>
        /// All GUID offsets should be the same, override if needed
        /// </summary>
        /// <returns>GUID of this unit</returns>
        public UInt64 getGUID()
        {
            return MemoryReader.readUInt64(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, objBase + GlobalOffsets.GUID);
        }

        /// <summary>
        /// Reccomend overriding this
        /// </summary>
        /// <returns></returns>
        public Waypoint getLocation()
        {
            float X = MemoryReader.readFloat(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, this.objBase + GlobalOffsets.X);
            float Y = MemoryReader.readFloat(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, this.objBase + GlobalOffsets.Y);
            float Z = MemoryReader.readFloat(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, this.objBase + GlobalOffsets.Z);
            return new Waypoint(X, Y, Z);
        }


    }
}
