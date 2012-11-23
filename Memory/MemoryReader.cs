using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

namespace Stab_Face.Memory
{
    static class MemoryReader
    {
        // DLL Imports
        [DllImport("kernel32.dll")]
        internal static extern Int32 VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        private static MEMORY_BASIC_INFORMATION MBI;

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }


        /// <summary>
        /// Reads raw bytes from another process' memory.
        /// </summary>
        /// <param name="hProcess">Handle to the external process.</param>
        /// <param name="dwAddress">Address from which to read.</param>
        /// <param name="lpBuffer">[Out] Allocated buffer into which raw bytes will be read. (Hint: Use Marshal.AllocHGlobal)</param>
        /// <param name="nSize">Number of bytes to be read.</param>
        /// <param name="lpBytesRead">[Out] Number of bytes actually read.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        private static extern bool ReadProcessMemory(
            IntPtr hProcess,
            uint dwAddress,
            IntPtr lpBuffer,
            int nSize,
            out int lpBytesRead);

        public static byte[] ReadBytes(IntPtr process, uint address, int len)
        {
            IntPtr lpBuffer = IntPtr.Zero;
            int iBytesRead;
            byte[] baRet;

            try
            {
                lpBuffer = Marshal.AllocHGlobal(len);

                ReadProcessMemory(process, address, lpBuffer, len, out iBytesRead);
                if (iBytesRead != len)
                    throw new Exception("ReadProcessMemory error in ReadBytes");

                baRet = new byte[iBytesRead];
                Marshal.Copy(lpBuffer, baRet, 0, iBytesRead);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (lpBuffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(lpBuffer);
            }

            return baRet;
        }

        public static UInt64 readUInt64(IntPtr process, uint address)
        {
            byte[] ba = ReadBytes(process, address, 8);
            //Array.Reverse(ba);

            return BitConverter.ToUInt64(ba, 0);
        }

        public static UInt32 readUInt32(IntPtr process, uint address)
        {
            byte[] ba = ReadBytes(process, address, 4);
            //Array.Reverse(ba);

            return BitConverter.ToUInt32(ba, 0);
        }

        public static UInt16 readUInt16(IntPtr process, uint address)
        {
            byte[] ba = ReadBytes(process, address, 2);
            //Array.Reverse(ba);

            return BitConverter.ToUInt16(ba, 0);
        }

        public static Single readFloat(IntPtr process, uint address)
        {
            byte[] ba = ReadBytes(process, address, 4);
            //Array.Reverse(ba);

            return BitConverter.ToSingle(ba, 0);
        }

        public static ArrayList getMemoryMap(IntPtr process, UInt32 start)
        {
            ArrayList memoryPages = new ArrayList();
            for (; start < 0x1FFFFFFF; )
            {
                int vq = VirtualQueryEx(process, (IntPtr)start, out MBI, (uint)Marshal.SizeOf(MBI));
                if ((vq > 0) && ((int)MBI.RegionSize > 0))
                {
                    //Debug.Print("Found memory page: " + start.ToString("X8") + " -> " + ((int)MBI.RegionSize).ToString("X8") + " AllocProtect: " + MBI.AllocationProtect.ToString("X8") + " Protect: " + MBI.Protect.ToString("X8") + " State: " + MBI.State.ToString("X8") + " Type: " + MBI.Type.ToString("X8"));
                    if ((int)MBI.AllocationProtect <= 1 || MBI.State != 0x1000 || MBI.Protect == 0x00 || MBI.Protect > 0x100)
                    {
                        start += (UInt32)((int)MBI.RegionSize);
                    }
                    else
                    {
                        //Debug.Print(MBI.AllocationProtect.ToString("X4"));
                        //
                        memoryPages.Add(new MemoryPage(start, (UInt32)MBI.RegionSize, MBI.AllocationProtect, MBI.Protect, MBI.State, MBI.Type));
                        start += (UInt32)((int)MBI.RegionSize);
                    }
                }
                else
                {
                    start += 1;
                }
            }
            return memoryPages;
        }
    }
}
