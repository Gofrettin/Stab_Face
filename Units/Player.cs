using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;

using Stab_Face.Memory;
using Stab_Face.WoW_Process;
using Stab_Face.WoW_Process.Offsets;
using Stab_Face.Misc;
using Stab_Face;
using Stab_Face.WoW_Process.Buffs;
using Stab_Face.WoW_Process.Debuffs;
using Stab_Face.Units.CombatRoutines;
using Stab_Face.Units.LogicStrategies;

namespace Stab_Face.Units
{
    public class Player : Unit
    {

        // List of all Units near us
        private List<Unit> units;

        // Main Pulse to update variables and run Strats
        private Thread Pulse;

        // Combat Routine, specific to class
        private CombatRoutine cRoutine;
        
        // Logic Strategy
        private LogicStrategy lStrat;

        private List<Buff> buffs;
        private List<Debuff> debuffs;

        public Player(UInt32 objBase)
            : base(objBase)
        {
            // Find the current player object
            if (objBase == 0)
            {
                this.getObjects();
            }

            // TODO: get correct lStrat at runtime
            this.lStrat = new GeneralWaypointStrategy();

            // TODO: get correct cRoutine at runtime
            this.cRoutine = new RogueRoutine();

            // Units
            units = new List<Unit>();

            Pulse = new Thread(new ThreadStart(doPulse));
            Pulse.Start();
        }

        ~Player()  // destructor
        {
            Pulse.Abort();
        }

        public Boolean isInCombat() {
            try {
                if (MemoryReader.ReadBytes(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.IN_COMBAT_OFFSET, 1)[0] > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {

            }
            return false;
        }

        public UInt32 getHP()
        {
            return MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, this.objBase + PlayerOffsets.HP_OFFSET);
        }

        public Waypoint getTargetLocation() {
            return this.target.getLocation();
        }

        public String getName() {
            string str = Stab_Face.Misc.Utils.byteArrToString(MemoryReader.ReadBytes(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, 0xC27D88, 12)).Replace("\0", "").Trim();
            string name = "";
            foreach (char c in str) {
                if (Char.IsLetter(c))
                    name += c.ToString();
            }
            return name;
        }

        public UInt32 getMaxHP() {
            return MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.MAX_HP_OFFSET);
        }

        public UInt32 getPower() {
            return MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.POWER_OFFSET);
        }

        public UInt32 getBuff(int slot) {
            return MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, (uint)((objBase + PlayerOffsets.BUFFS_OFFSET) + (slot * 0x04)));
        }

        public UInt32 getDebuff(int slot) {
            return MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, (uint)((objBase + PlayerOffsets.DEBUFFS_OFFSET) + (slot * 0x04)));
        }

        public Boolean isCasting() {
            if (MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.IS_CASTING_OFFSET) > 0 && MemoryReader.readUInt16(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.IS_CHANNELING_OFFSET) > 0)
                return true;
            else
                return false;
        }

        public Boolean isMoving()
        {
            if (MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, PlayerOffsets.IS_MOVING) > 0)
                return true;
            else
                return false;
        }

        private void moveToLoc(Waypoint wp)
        {
            if (!isInRange(wp, 1.0f))
            {

                int tries = 0;
                while (verifyWrite(wp.getX(), 0xC4D890) == false && tries < 3)
                {
                    MemoryWriter.WriteMem(Stab_Face.WoW_Process.WoW_Instance.getProcess(), 0xC4D890, BitConverter.GetBytes(wp.getX()));
                    tries++;
                    if (tries >= 3)
                        return;
                }
                tries = 0;
                Thread.Sleep(3);
                while (verifyWrite(wp.getY(), 0xC4D894) == false && tries < 3)
                {
                    MemoryWriter.WriteMem(Stab_Face.WoW_Process.WoW_Instance.getProcess(), 0xC4D894, BitConverter.GetBytes(wp.getY()));
                    tries++;
                    if (tries >= 3)
                        return;
                }
                tries = 0;
                Thread.Sleep(3);
                while (verifyWrite(wp.getZ(), 0xC4D898) == false && tries < 3)
                {
                    MemoryWriter.WriteMem(Stab_Face.WoW_Process.WoW_Instance.getProcess(), 0xC4D898, BitConverter.GetBytes(wp.getZ()));
                    tries++;
                    if (tries >= 3)
                        return;
                }
                tries = 0;
                Thread.Sleep(5);

                MemoryWriter.WriteMem(Stab_Face.WoW_Process.WoW_Instance.getProcess(), 0xC4D888, new byte[] { (byte)0x04 });
                //MemoryWriter.WriteMem(Stab_Face.WoW_Process.WoW_Instance.getProcess(), 0xC4D888, new byte[] { (byte)0x04 });

                //Thread.Sleep(200);
                //Debug.Print("Moving: " + isMoving());
                //while (isMoving())
                //    Thread.Sleep(15); //Enjoy the ride

                //Thread.Sleep(20);
            }
        }

        private void moveDistance(float d)
        {
            // First, update player and target location and save them locally
            Waypoint player_loc = getLocation();

            Waypoint target_loc = this.target.getLocation();

            // get the total distance
            float distance = getDistance(player_loc, target_loc);

            // Get the X vector
            float xVector = target_loc.getX() - player_loc.getX();

            // Get the Y vector
            float yVector = target_loc.getY() - player_loc.getY();

            // Find Theta
            float theta = (float)Math.Asin(.01f / distance);

            float newY = (float)Math.Sin(theta) * d;

            // Calc new X
            float newX = (float)Math.Cos(theta) * d;

            Waypoint to_move = new Waypoint(player_loc.getX() + newX, player_loc.getY() + newY, (player_loc.getZ() + target_loc.getZ()) / 2.0f);

            moveToLoc(to_move);
        }

        private void moveInRangeofTarget(float attackRange)
        {
            // First, update player and target location and save them locally
            Waypoint player_loc = getLocation();

            Waypoint target_loc = this.target.getLocation();

            if (!isInRange(target_loc, attackRange))
            {
                // get the total distance
                float distance = getDistance(player_loc, target_loc);

                // Get the X vector
                float xVector = target_loc.getX() - player_loc.getX();

                // Get the Y vector
                float yVector = target_loc.getY() - player_loc.getY();

                // Quadrant
                int quadrant = 0;
                if (xVector >= 0 && yVector >= 0)
                    quadrant = 1;
                else if (xVector < 0 && yVector >= 0)
                    quadrant = 2;
                else if (xVector < 0 && yVector < 0)
                    quadrant = 3;
                else
                    quadrant = 4;

                xVector = Math.Abs(xVector);
                yVector = Math.Abs(yVector);

                // Find Theta
                float theta = (float)Math.Acos(xVector / distance);
                //Debug.Print("Theta: " + theta);


                float toMove = distance - (attackRange - 2.0f);
                //Debug.Print("Distance to move: " + toMove);

                float newX;
                float newY;
                if (toMove < 0.0f)
                {
                    //faceTarget();
                }
                else
                {

                    // Calc new X
                    newX = (float)Math.Cos(theta) * toMove;

                    // Calc new Y
                    newY = (float)Math.Sin(theta) * toMove;

                    if (quadrant == 1)
                    {

                    }
                    else if (quadrant == 2)
                    {
                        newX = newX * -1;

                    }
                    else if (quadrant == 3)
                    {
                        newX = newX * -1;
                        newY = newY * -1;
                    }
                    else
                    {
                        newY = newY * -1;
                    }


                    Waypoint to_move = new Waypoint(player_loc.getX() + newX, player_loc.getY() + newY, (player_loc.getZ() + target_loc.getZ()) / 2.0f);

                    moveToLoc(to_move);
                }
            }

        }

        private void faceLocation(Waypoint wp)
        {
            float turnaccuracy = 0.1f;

            // First, update player and target location and save them locally
            Waypoint player_loc = this.getLocation();
            float PlayerFacing = this.getFacing();

            //get the angle to which we need to turn in order to face our target
            float f = (float)Math.Atan2(wp.getY() - player_loc.getY(), wp.getX() - player_loc.getX());
            //if the turning angle is negative
            //(sometimes happens, depending on negative coordinates and such)
            if (f < 0)
                //add the maximum possible angle (PI x 2) to normalize the negative angle
                f += (float)(Math.PI * 2);
            Debug.Print("Want to face: " + f);
            Debug.Print("Facing: " + PlayerFacing);

            if (PlayerFacing < (f + turnaccuracy) && PlayerFacing > (f - turnaccuracy))
            {
                // We are already facing withing the error margin
                Debug.Print("Already Facing");
            }
            else
            {
                double r, l;

                //if our current facing angle, in radians, is greater than
                //the angle which we desire to face
                if (PlayerFacing > f)
                {
                    //we'd have to turn past North if we're turning left
                    l = ((2 * Math.PI) - PlayerFacing) + f;
                    //we don't have to turn past North if we're turning right
                    r = PlayerFacing - f;
                }
                else
                {
                    //we don't have to turn past North if we're turning left
                    l = f - PlayerFacing;
                    //we have to turn past North if we're turning right
                    r = PlayerFacing + ((2 * Math.PI) - f);
                }

                //let's please turn in the direction where we have to spend
                //the least amount of time turning
                while (!(this.getFacing() < (f + turnaccuracy) && this.getFacing() > (f - turnaccuracy)))
                {
                if (l < r) {
                    //turnkey = Post.ArrowKeys.Left;
                    PostMessage.SendKeys((int)Stab_Face.WoW_Process.WoW_Instance.getProcess().MainWindowHandle, "{LEFT}");
                } else {
                    //turnkey = Post.ArrowKeys.Right;
                    PostMessage.SendKeys((int)Stab_Face.WoW_Process.WoW_Instance.getProcess().MainWindowHandle, "{RIGHT}");
                }
                }
                //MemoryWriter.WriteMem(Stab_Face.WoW_Process.WoW_Instance.getProcess(), PlayerOffsets.FACING, BitConverter.GetBytes(f));
                //Thread.Sleep(10);
                //PostMessage.SendKeys((int)Stab_Face.WoW_Process.WoW_Instance.getProcess().MainWindowHandle, "{RIGHT}");
            }
        }

        public Boolean isInRange(Waypoint target_loc, float range)
        {
            Waypoint player_loc = this.getLocation();

            float distanceToTarget = (float)Math.Sqrt(Math.Pow((target_loc.getY() - player_loc.getY()), 2.0) + Math.Pow((target_loc.getX() - player_loc.getX()), 2.0));
            //Debug.Print("Distance to target: " + distanceToTarget);

            if (distanceToTarget < range)
                return true;
            else
                return false;
        }

        public float getDistance(Waypoint wp_1, Waypoint wp_2)
        {
            return (float)Math.Sqrt(Math.Pow((wp_2.getY() - wp_1.getY()), 2.0) + Math.Pow((wp_2.getX() - wp_1.getX()), 2.0));
        }

        private float getFacing()
        {
            return MemoryReader.readFloat(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.FACING);
        }

        private Boolean verifyWrite(float verify, UInt32 address) {
            byte[] va = BitConverter.GetBytes(verify);
            byte[] ba = MemoryReader.ReadBytes(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, address, 4);
            for (int i = 0; i < 4; i++)
            {
                if(va[i] != ba[i])
                    return false;
            }
            return true;
        }

        private void targetByGUID(UInt64 GUID)
        {
            if (target.getGUID() == GUID)
                return; // Already targeting
            else
            {
                while (true)
                {
                    try
                    {
                        MemoryWriter.WriteMem(Stab_Face.WoW_Process.WoW_Instance.getProcess(), 0xB4E2E0, BitConverter.GetBytes(GUID));
                        Thread.Sleep(50);
                        PostMessage.SendKeys((int)Stab_Face.WoW_Process.WoW_Instance.getProcess().MainWindowHandle, "M");
                        
                        if (this.target.getGUID() == GUID)
                            return;
                    }
                    catch (Exception ex) { }
                }
            }
        }

        private void castNoGCDByKey(string key)
        {
            Thread.Sleep(35);
            PostMessage.SendKeys((int)Stab_Face.WoW_Process.WoW_Instance.getProcess().MainWindowHandle, key);

            if (!this.isCasting())
            {
                int tries = 0; // try and make sure cast went off
                while (tries < 5)
                {
                    PostMessage.SendKeys((int)Stab_Face.WoW_Process.WoW_Instance.getProcess().MainWindowHandle, key);
                    Thread.Sleep(10);
                    tries++;
                }
            }
        }

        public override String ToString()
        {
            String str = "";
            str += "Object Base Pointer: " + this.objBase + "\n";
            str += "MaxHP: " + this.getMaxHP() + "\n";
            str += "HP: " + this.getHP() + "\n";
            str += "Power: " + this.getPower() + "\n";
            str += "Name: " + this.getName() + "\n";

            Waypoint wp = this.getLocation();
            str += "X: " + wp.getX() + "\n";
            str += "Y: " + wp.getY() + "\n";
            str += "Z: " + wp.getZ() + "\n";

            foreach (Unit u in units)
            {
                str += "Mob GUID: " + ((Mob)u).getGUID() + "\n";
                str += "Mob HP: " + ((Mob)u).getHP() + "\n";
                str += "Mob Name: " + ((Mob)u).getName() + "\n";
                Waypoint mwp = ((Mob)u).getLocation();
                str += "Mob Location: " + "\n";
                str += "X: " + mwp.getX() + "\n";
                str += "Y: " + mwp.getY() + "\n";
                str += "Z: " + mwp.getZ() + "\n";
            }

            return str;
        }


        private void getObjects() {
            UInt32 objManagerPointer = MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, 0xB41414);
            UInt64 curTargetGUID = 0;
            if(units != null)
                units.Clear();
            try
            {
                curTargetGUID = MemoryReader.readUInt64(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.CUR_TARGET_GUID_OFFSET);
            }
            catch (Exception ex)
            {
            }
            this.target = null;
            this.GUID = MemoryReader.readUInt64(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, (objManagerPointer + 0xC0));

            UInt32 curObj = MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, (objManagerPointer + 0xAC));

            while (curObj != 0 && (curObj & 1) == 0) {
                UInt64 curGUID = 0;
                UInt32 objType = 0;
                try {
                    curGUID = MemoryReader.readUInt64(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, (curObj + 0x30));

                    if (curGUID == this.GUID) {
                        //Debug.Print("Found Player Obj at " + curObj);
                        if (this.objBase == 0)
                            this.objBase = curObj;
                        //Debug.Print("Error catch " + MemoryReader.readFloat(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, (curObj + 0x9C0)));
                    }
                    else if (curGUID == curTargetGUID)
                    {
                        this.target = new Unit(curObj);
                        // TODO: set null if dead
                    }
                    else
                    {
                        objType = MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, (curObj + 0x14));
                        if (objType == 4)
                        {
                            //units.Add(new Unit(curObj));
                            //MemoryObjects.Add(new PlayerMemoryObject(curObj));
                        }
                        else
                        {//if(objType == 3) 
                            units.Add(new Mob(curObj));
                            //MemoryObjects.Add(new MobMemoryObject(curObj));
                        }
                    }

                    //Debug.Print(string.Format("GUID: {0} - X: {1} Y: {2} Z: {3}", curGUID.ToString("X16"), x, y, z));
                    curObj = MemoryReader.readUInt32(Stab_Face.WoW_Process.WoW_Instance.getProcess().Handle, (curObj + 0x3C));
                }
                catch (Exception e) {
                    Debug.Print(e.Message);
                    break;
                }
            }
        }

        /*
        public void updatePlayerTarget() {

            UInt64 targetGUID = readTargetGUID();

            if (targetGUID != 0) {
                if (target != null) {
                    UInt64 curTargetGUID = MemoryReader.readUInt64(this.gProcess_ptr, target.getGuidPointer());
                    if (curTargetGUID == targetGUID)
                        return; // Already targeting correct object
                }

                if (targetGUID == MemoryReader.readUInt64(this.gProcess_ptr, player.getGuidPointer())) {
                    //Debug.Print("Target Base: " + player.getBasePointer().ToString("X8"));
                    target = new PlayerMemoryObject(player.getBasePointer());
                    return; // target is me!
                }
                else {
                    for (int i = 0; i < 2; i++) {
                        foreach (MemoryObject mo in MemoryObjects) {
                            UInt64 tempGUID = MemoryReader.readUInt64(this.gProcess_ptr, mo.getGuidPointer());
                            if (tempGUID == targetGUID) {
                                //Debug.Print("Target Base: " + mo.getBasePointer().ToString("X8"));
                                UInt32 objType = MemoryReader.readUInt32(this.gProcess_ptr, (mo.getBasePointer() + 0x14));
                                if (objType == 4) {
                                    target = new PlayerMemoryObject(mo.getBasePointer());
                                    return;
                                }
                                else if (objType == 3) {
                                    target = new MobMemoryObject(mo.getBasePointer());
                                    return;
                                }
                            }
                        }

                        // Must not have found it in object list... try updating the list
                        getObjects();
                    }

                    // Still not found, throw error
                    throw new Exception("Unable to find target in object list.");
                }
            }
            else {
                //throw new Exception("No target selected, updatePlayerTarget Failed.");
            }
        }

        public void sendChat(String str) {
            PostMessage.SendKeys((int)this.gProcess_wHnd, "{ENTER}");
            Thread.Sleep(40);
            PostMessage.SendKeys((int)this.gProcess_wHnd, str);
            Thread.Sleep(70);
            PostMessage.SendKeys((int)this.gProcess_wHnd, "{ENTER}");
        }
         * */


        /// <summary>
        /// The Pulse should be responsible for updating the Buffs/Debuffs
        /// and calling CombatRoutine and LogicStrategy
        /// </summary>
        private void doPulse()
        {
            LogicRequest LR;
            CombatRequest CR;

            while (true)
            {
                // Pulse the Logic Strategy
                //LR = lStrat.getRequest(this);


                // Pulse the CombatRoutine strategy
                if (this.target != null)
                {
                    CR = cRoutine.getRequest(this);

                    if (CR.getMove() != null)
                    {
                        faceLocation(CR.getMove());
                        moveToLoc(CR.getMove());
                    }

                    if (CR.getAbility() != '.')
                    {
                        faceLocation(this.target.getLocation());
                        castNoGCDByKey(CR.getAbility().ToString());
                        Thread.Sleep(1000); // TODO: Add GCD Logic Just for testing
                    }
                }

                getObjects();
                Thread.Sleep(100);
            }
        }
    }
}
