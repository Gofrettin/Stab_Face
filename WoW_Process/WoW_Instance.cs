using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;

using Stab_Face.Memory;
using System.Threading;

namespace Stab_Face.WoW_Process
{

    public static class WoW_Instance
    {
        private static Process WoW;

        /// <summary>
        /// Attempts to find a single running instance of WoW.
        /// In the future this should start a process and log in.
        /// </summary>
        /// <returns>Existing WoW Process Object</returns>
        public static Process getProcess() {
            if (WoW == null)
            {
                Process[] localByName = Process.GetProcessesByName("WoW");
                if (localByName.Length < 1)
                {
                    throw new Exception("No Instances of WoW are running.");
                }
                else if (localByName.Length > 1)
                {
                    throw new Exception("Multiple WoW instances detected.");
                }
                else
                {
                    WoW = localByName[0];
                    return WoW;
                }
            }
            else
            {
                return WoW;
            }
        }
    }
}
