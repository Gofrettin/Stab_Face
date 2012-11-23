using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stab_Face
{
    class MemoryPage
    {
        private UInt32 startAddress;
        private UInt32 size;
        private uint protection;
        private uint state;
        private uint protect;
        private uint type;

        public MemoryPage(UInt32 startAddress, UInt32 size, uint protection, uint protect, uint state, uint type)
        {
            this.startAddress = startAddress;
            this.size = size;
            this.protection = protection;
            this.protect = protect;
            this.state = state;
            this.type = type;
        }

        public UInt32 getStartAddress() {
            return this.startAddress;
        }

        public UInt32 getSize()
        {
            return this.size;
        }

        public uint getProtection()
        {
            return this.protection;
        }

        public uint getProtect()
        {
            return this.protect;
        }

        public uint getState()
        {
            return this.state;
        }

        public uint getType()
        {
            return this.type;
        }
    }
}
