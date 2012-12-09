using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stab_Face.Memory;
using Stab_Face.WoW_Process.Offsets;
using Stab_Face.Misc;
using Stab_Face.WoW_Process;

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
        protected UInt16 faction;

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
            return MemoryReader.readUInt64(WoW_Instance.getProcess().Handle, objBase + GlobalOffsets.GUID);
        }

        public UInt32 getObjBase()
        {
            return this.objBase;
        }

        /// <summary>
        /// Reccomend overriding this
        /// </summary>
        /// <returns></returns>
        public Waypoint getLocation()
        {
            float X = MemoryReader.readFloat(WoW_Instance.getProcess().Handle, this.objBase + GlobalOffsets.X);
            float Y = MemoryReader.readFloat(WoW_Instance.getProcess().Handle, this.objBase + GlobalOffsets.Y);
            float Z = MemoryReader.readFloat(WoW_Instance.getProcess().Handle, this.objBase + GlobalOffsets.Z);
            return new Waypoint(X, Y, Z);
        }

        public UInt16 getFaction()
        {
            //return MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, (MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, (this.objBase + GlobalOffsets.FACTION_1)) + GlobalOffsets.FACTION_2 ));
            this.faction = MemoryReader.readUInt16(WoW_Instance.getProcess().Handle, this.objBase + GlobalOffsets.FACTION);
            return this.faction;
        }


    }
}
