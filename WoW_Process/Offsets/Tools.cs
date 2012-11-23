using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Stab_Face.Memory;
using System.Diagnostics;

namespace Stab_Face.WoW_Process.Offsets
{
    class Tools
    {
        private Process gProcess;
        private IntPtr gProcess_ptr;
        private IntPtr gProcess_wHnd;
        private ProcessModuleCollection gProcessModules;

        private ArrayList MemoryPages;
        private ArrayList MemoryObjects = new ArrayList();

        public Tools(Process gProcess)
        {
            this.gProcess = gProcess;
            this.gProcess_ptr = gProcess.Handle;
            this.gProcess_wHnd = gProcess.MainWindowHandle;
            this.gProcessModules = gProcess.Modules;
            this.MemoryPages = MemoryReader.getMemoryMap(gProcess_ptr, (UInt32)gProcess.MainModule.EntryPointAddress.ToInt32());
        }

        private void findPointersToPlayerStruct(UInt32 playerBase)
        {
            foreach (MemoryPage mp in MemoryPages)
            {
                for (UInt32 loc = mp.getStartAddress(); loc < ((mp.getSize() + mp.getStartAddress()) - 0x04); loc += 1)
                {
                    UInt32 pointer = MemoryReader.readUInt32(this.gProcess_ptr, loc);
                    if (pointer >= playerBase && pointer <= (playerBase + 0x100))
                    {
                        Debug.Print("Possible pointer to PlayerBase@ " + loc.ToString("X8") + " -> " + pointer.ToString("X8"));
                    }
                }
            }
        }

        private void findObjManager() // Gonna take a while...
        {
            UInt32 firstLink = 0;


            //foreach (MemoryPage mp in MemoryPages)
            //{
            //if (mp.getSize() < 0x1000)
            //    continue; // Don't Bother


            //for (UInt32 loc = mp.getStartAddress(); loc < (mp.getSize() + mp.getStartAddress()); loc += 4)
            for (UInt32 loc = 0x00A00000; loc < 0x00FFFFFF; loc += 1)
            {
                String info = "";
                if (isLegitLocation(loc))
                {
                    //firstLink = MemoryReader.readUInt32(this.gProcess_ptr, loc);
                    info += loc.ToString("X8") + "\n"; // +" -> " + firstLink.ToString("X8") + "\n";
                    //if (isLegitLocation(firstLink))
                    //{
                    //for (UInt32 distance = 0x0C; distance < 0x50; distance++) // Start at 0x0C, I see a lot of mirrored wierd shit in the memory.
                    //{
                    UInt32 distance = 0x3C;
                    int largestChain = 0;
                    UInt32 tempLoc = loc;
                    UInt32 currentLink;
                    UInt32 lastLink = 0;
                    int occurances = 0;
                    do
                    {
                        if (lastLink == 0)
                            tempLoc += 0xAC;
                        else
                            tempLoc += distance;
                        if (isLegitLocation(tempLoc + 0x04))
                        {
                            currentLink = MemoryReader.readUInt32(this.gProcess_ptr, (tempLoc));
                            if (isLegitLocation(currentLink) && currentLink != lastLink && currentLink > (tempLoc + 0x400)) // pointers to themself??
                            {
                                info += tempLoc.ToString("X8") + " -> " + currentLink.ToString("X8") + "\n";
                                occurances += 1;

                                if (occurances >= 3)
                                {
                                    Debug.Print("Possible linkedList Start@ " + loc.ToString("X8") + ", " + occurances + " occurances at a distance of " + distance.ToString("X2"));
                                    //break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                        lastLink = currentLink;
                        tempLoc = currentLink;

                    } while (true);

                    if (occurances > 4 /*&& occurances > largestChain*/)
                    {
                        largestChain = occurances;
                        Debug.Print(info);
                        Debug.Print("New Chain Found in this page@ " + loc.ToString("X8") + ", " + occurances + " occurances at a distance of " + distance.ToString("X2"));
                        // Update Loc to blacklist
                        //loc += (UInt32)(0x3C * occurances);
                    }
                    //}
                    //}
                }
            }
            //}
        }

        private Boolean isLegitLocation(UInt32 location)
        {
            foreach (MemoryPage mp in MemoryPages)
            {
                if (location > mp.getStartAddress() && location < mp.getStartAddress() + mp.getSize())
                {
                    return true;
                }
            }
            return false;
        }

        private UInt32 findPlayerStructStart()
        {

            UInt32 playerStructStart = 0;
            string x_static = Stab_Face.Misc.Utils.byteArrToHexString(MemoryReader.ReadBytes(this.gProcess_ptr, 0xC7B544, 4));
            string y_static = Stab_Face.Misc.Utils.byteArrToHexString(MemoryReader.ReadBytes(this.gProcess_ptr, 0xC7B548, 4));
            string z_static = Stab_Face.Misc.Utils.byteArrToHexString(MemoryReader.ReadBytes(this.gProcess_ptr, 0xC7B54C, 4));
            string toFind = x_static + y_static + z_static;

            for (UInt32 loc = (UInt32)(gProcess.MainModule.BaseAddress.ToInt32() + 0x9C0 + 0x10000000); loc < ((MemoryPage)MemoryPages[MemoryPages.Count - 1]).getStartAddress(); loc += 0x1000)
            {
                try
                {
                    // figure out what page this address is in...
                    Boolean inPage = false;
                    string lastMP;
                    while (!inPage)
                    {
                        foreach (MemoryPage mp in MemoryPages)
                        {
                            if (loc > mp.getStartAddress() && loc < mp.getStartAddress() + mp.getSize())
                            {
                                inPage = true;
                                lastMP = mp.getProtection().ToString("X4");
                                break;
                            }
                        }
                        if (!inPage)
                            loc += 0x1000;
                        if (loc >= 0x1FFFFFFF)
                            break;
                    }
                    inPage = false;

                    string temp = Stab_Face.Misc.Utils.byteArrToHexString(MemoryReader.ReadBytes(this.gProcess_ptr, loc, 12));
                    //Debug.Print(loc.ToString("X8") + ": " + temp);
                    if (toFind == temp)
                    {
                        Debug.Print("HOLY FUCK WE FOUND IT @: " + loc.ToString("X8"));
                        playerStructStart = (loc - 0x9C0);
                        break;
                    }

                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                }
                if (playerStructStart != 0)
                    break;
            }

            return playerStructStart;
        }

        private void /*GameInstanceLocation*/ searchMemory(String hexToFind)
        {
            ArrayList occurances = new ArrayList();

            UInt32 loc = 0x000000;
            String temp = "";
            foreach (MemoryPage mp in MemoryPages)
            {
                for (loc = mp.getStartAddress(); loc < mp.getSize(); loc += 4)
                {
                    try
                    {
                        temp = Stab_Face.Misc.Utils.byteArrToHexString(MemoryReader.ReadBytes(this.gProcess_ptr, loc, hexToFind.Length));
                        if (temp == hexToFind)
                        {
                            Debug.Print("Found");
                        }

                    }
                    catch (Exception e)
                    {
                        Debug.Print(e.Message);
                    }
                }
                Debug.Print("Last Read Loc: " + loc.ToString("X16"));
            }

            /*
            Debug.Print(gProcess.PrivateMemorySize64 + "");
            while (loc < gProcess.PrivateMemorySize64)
            {
                try
                {
                    //Debug.Print(loc + "");
                    temp = byteArrToHexString(MemoryReader.ReadBytes(this.gProcess_ptr, loc, 12));
                    if (temp == hexToFind)
                    {
                        Debug.Print("Found!");
                        temp = byteArrToHexString(MemoryReader.ReadBytes(this.gProcess_ptr, loc, 20));
                        occurances.Add(loc);
                        occurances.Add(temp);
                    }
                }
                catch (Exception e)
                {
                    Debug.Print("Loc: " + loc + ". " + e.Message);
                }
                temp = "";
                loc += 0x08;
            }

            Debug.Print(occurances.Count + "");
            //return new GameInstanceLocation();
             * */
        }
    }
}
