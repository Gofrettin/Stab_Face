using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stab_Face.WoW_Process.Factions
{
    public class FactionsHostileToAll
    {
        private static List<UInt16> list = null;
                                    //0x1F // Worgen     
        private FactionsHostileToAll() {}

        public static List<UInt16> getList() 
        {
            if(list == null)
            {
                list = new List<UInt16>();
                list.Add(0x18); // Worgen
                list.Add(0x2C); // Bears
                list.Add(189);  // Durotar piglets
                list.Add(7);    // Zombies
            }
            return list;
        }
    }
}
