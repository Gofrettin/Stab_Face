using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;

using Stab_Face.Memory;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Stab_Face.WoW_Process
{

    public static class WoW_Instance
    {
        private static Process WoW;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(int hWnd, out RECT lpRect);

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

        public static Rectangle getWindowDimensions()
        {
            RECT rec;
            GetWindowRect((int)WoW.MainWindowHandle, out rec);

            Rectangle r = new Rectangle(rec.Left, rec.Top, (rec.Right - rec.Left), (rec.Bottom - rec.Top));

            return r;
        }
    }
}
