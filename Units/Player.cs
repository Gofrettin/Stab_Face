using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;

using Stab_Face.Memory;
using Stab_Face.WoW_Process;
using Stab_Face;
using Stab_Face.Units.CombatRoutines;
using Stab_Face.Units.LogicStrategies;
using Stab_Face.WoW_Process.Buffs;
using Stab_Face.WoW_Process.Debuffs;
using Stab_Face.WoW_Process.Offsets;
using Stab_Face.Misc;

namespace Stab_Face.Units
{
    public class Player : Unit
    {
        // Profile
        private Profile profile;

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
        }

        ~Player()  // destructor
        {
            Pulse.Abort();
        }

        public void start(Profile p)
        {
            this.profile = p;
            Pulse.Start();
        }

        public void stop()
        {
            Pulse.Abort();
        }

        public Profile getProfile()
        {
            return this.profile;
        }

        public List<Unit> getNearbyUnits()
        {
            return this.units;
        }

        public Boolean isInCombat() {
            try {
                if (MemoryReader.ReadBytes(WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.IN_COMBAT_OFFSET, 1)[0] > 0)
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
            return MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, this.objBase + PlayerOffsets.HP_OFFSET);
        }

        public Waypoint getTargetLocation() {
            return this.target.getLocation();
        }

        public Unit getTargetedUnit()
        {
            UInt64 curTargetGUID = MemoryReader.readUInt64(WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.CUR_TARGET_GUID_OFFSET);
            if (this.target != null)
            {
                if (((Mob)this.target).getHP() > 0)
                {
                    if (curTargetGUID == this.target.getGUID())
                    {
                        return this.target;
                    }
                }
            }

            getObjects();
            foreach(Unit u in this.getNearbyUnits()) {
                if(u.getGUID() == curTargetGUID) {
                    this.target = u;
                    return this.target;
                }
            }

            this.target = null;
            return this.target;
        }

        public String getName() {
            string str = Stab_Face.Misc.Utils.byteArrToString(MemoryReader.ReadBytes(WoW_Instance.getProcess().Handle, 0xC27D88, 12)).Replace("\0", "").Trim();
            string name = "";
            foreach (char c in str) {
                if (Char.IsLetter(c))
                    name += c.ToString();
            }
            return name;
        }

        public UInt32 getMaxHP() {
            return MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.MAX_HP_OFFSET);
        }

        public UInt32 getPower() {
            return MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.POWER_OFFSET);
        }

        public UInt32 getBuff(int slot) {
            return MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, (uint)((objBase + PlayerOffsets.BUFFS_OFFSET) + (slot * 0x04)));
        }

        public UInt32 getDebuff(int slot) {
            return MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, (uint)((objBase + PlayerOffsets.DEBUFFS_OFFSET) + (slot * 0x04)));
        }

        public Boolean isCasting() {
            if (MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.IS_CASTING_OFFSET) > 0 && MemoryReader.readUInt16(WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.IS_CHANNELING_OFFSET) > 0)
                return true;
            else
                return false;
        }

        public Boolean isMoving()
        {
            if (MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, this.objBase + PlayerOffsets.IS_MOVING) > 0)
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
                    MemoryWriter.WriteMem(WoW_Instance.getProcess(), 0xC4D890, BitConverter.GetBytes(wp.getX()));
                    tries++;
                    if (tries >= 3)
                        return;
                }
                tries = 0;
                Thread.Sleep(3);
                while (verifyWrite(wp.getY(), 0xC4D894) == false && tries < 3)
                {
                    MemoryWriter.WriteMem(WoW_Instance.getProcess(), 0xC4D894, BitConverter.GetBytes(wp.getY()));
                    tries++;
                    if (tries >= 3)
                        return;
                }
                tries = 0;
                Thread.Sleep(3);
                while (verifyWrite(wp.getZ(), 0xC4D898) == false && tries < 3)
                {
                    MemoryWriter.WriteMem(WoW_Instance.getProcess(), 0xC4D898, BitConverter.GetBytes(wp.getZ()));
                    tries++;
                    if (tries >= 3)
                        return;
                }
                tries = 0;
                Thread.Sleep(5);

                MemoryWriter.WriteMem(WoW_Instance.getProcess(), 0xC4D888, new byte[] { (byte)0x04 });
                //MemoryWriter.WriteMem(WoW_Instance.getProcess(), 0xC4D888, new byte[] { (byte)0x04 });

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
            float turnaccuracy = 0.2f;

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
                //PostMessage.mouseHold((int)WoW_Instance.getProcess().MainWindowHandle, "right", 100, 100, false);
                
                    if (l < r) {
                        //turnkey = Post.ArrowKeys.Left;
                        // Banned for this method
                        //PostMessage.SendKeys((int)WoW_Instance.getProcess().MainWindowHandle, "{LEFT}");
                        PostMessage.ArrowKey((int)WoW_Instance.getProcess().MainWindowHandle, "left", true);
                        while (!(this.getFacing() < (f + turnaccuracy) && this.getFacing() > (f - turnaccuracy)))
                        {
                            //Thread.Sleep(10);
                            //PostMessage.ArrowKey((int)WoW_Instance.getProcess().MainWindowHandle, "left", 20);
                        }
                        PostMessage.ArrowKey((int)WoW_Instance.getProcess().MainWindowHandle, "left", false);
                        // try with mouse
                        //PostMessage.LinearSmoothMove(100f, 15, "left");
                    } else {
                        //turnkey = Post.ArrowKeys.Right;
                        // Banned for this method
                        //PostMessage.SendKeys((int)WoW_Instance.getProcess().MainWindowHandle, "{RIGHT}");
                        PostMessage.ArrowKey((int)WoW_Instance.getProcess().MainWindowHandle, "right", true);
                        while (!(this.getFacing() < (f + turnaccuracy) && this.getFacing() > (f - turnaccuracy)))
                        {
                            //Thread.Sleep(10);
                            //PostMessage.ArrowKey((int)WoW_Instance.getProcess().MainWindowHandle, "right", 20);
                        }
                        PostMessage.ArrowKey((int)WoW_Instance.getProcess().MainWindowHandle, "right", false);
                        //PostMessage.LinearSmoothMove(100f, 15, "right");
                    }
                    
                    
                //PostMessage.mouseHold((int)WoW_Instance.getProcess().MainWindowHandle, "right", 100, 100, true);
                //MemoryWriter.WriteMem(WoW_Instance.getProcess(), PlayerOffsets.FACING, BitConverter.GetBytes(f));
                //Thread.Sleep(10);
                //PostMessage.SendKeys((int)WoW_Instance.getProcess().MainWindowHandle, "{RIGHT}");
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
            return MemoryReader.readFloat(WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.FACING);
        }

        private Boolean verifyWrite(float verify, UInt32 address) {
            byte[] va = BitConverter.GetBytes(verify);
            byte[] ba = MemoryReader.ReadBytes(WoW_Instance.getProcess().Handle, address, 4);
            for (int i = 0; i < 4; i++)
            {
                if(va[i] != ba[i])
                    return false;
            }
            return true;
        }

        private void targetUnit(Unit u)
        {
            if (target!= null && MemoryReader.readUInt64(WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.CUR_TARGET_GUID_OFFSET) == u.getGUID())
                return; // Already targeting
            else
            {
                while (true)
                {
                    try
                    {
                        //MemoryWriter.WriteMem(WoW_Instance.getProcess(), 0xB4E2E0, BitConverter.GetBytes(GUID));
                        faceLocation(u.getLocation());
                        PostMessage.SendKeys((int)WoW_Instance.getProcess().MainWindowHandle, "F");
                        Thread.Sleep(200);
                        if (MemoryReader.readUInt64(WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.CUR_TARGET_GUID_OFFSET) == u.getGUID())
                            this.target = u;
                            return;
                    }
                    catch (Exception ex) { }
                }
            }
        }

        private void castNoGCDByKey(string key)
        {
            Thread.Sleep(35);
            PostMessage.SendKeys((int)WoW_Instance.getProcess().MainWindowHandle, key);

            if (!this.isCasting())
            {
                int tries = 0; // try and make sure cast went off
                while (tries < 5)
                {
                    PostMessage.SendKeys((int)WoW_Instance.getProcess().MainWindowHandle, key);
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
            str += "Faction: " + this.getFaction() + "\n";

            Waypoint wp = this.getLocation();
            str += "X: " + wp.getX() + "\n";
            str += "Y: " + wp.getY() + "\n";
            str += "Z: " + wp.getZ() + "\n";

            foreach (Unit u in units)
            {
                str += "\nObject Base Pointer: " + u.getObjBase() + "\n";
                str += "Mob GUID: " + ((Mob)u).getGUID() + "\n";
                str += "Mob HP: " + ((Mob)u).getHP() + "\n";
                str += "Mob Name: " + ((Mob)u).getName() + "\n";
                Waypoint mwp = ((Mob)u).getLocation();
                str += "Mob Location: " + "\n";
                str += "X: " + mwp.getX() + "\n";
                str += "Y: " + mwp.getY() + "\n";
                str += "Z: " + mwp.getZ() + "\n";
                str += "Faction: " + u.getFaction() + "\n";
            }

            return str;
        }

        private void getObjects() {
            UInt32 objManagerPointer = MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, 0xB41414);
            UInt64 curTargetGUID = 0;
            if(units != null)
                units.Clear();
            /*
            try
            {
                curTargetGUID = MemoryReader.readUInt64(WoW_Instance.getProcess().Handle, objBase + PlayerOffsets.CUR_TARGET_GUID_OFFSET);
            }
            catch (Exception ex)
            {
            }
            this.target = null;
             *  * */
            this.GUID = MemoryReader.readUInt64(WoW_Instance.getProcess().Handle, (objManagerPointer + 0xC0));
            

            UInt32 curObj = MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, (objManagerPointer + 0xAC));

            while (curObj != 0 && (curObj & 1) == 0) {
                UInt64 curGUID = 0;
                UInt32 objType = 0;
                try {
                    curGUID = MemoryReader.readUInt64(WoW_Instance.getProcess().Handle, (curObj + 0x30));

                    if (curGUID == this.GUID) {
                        //Debug.Print("Found Player Obj at " + curObj);
                        if (this.objBase == 0)
                            this.objBase = curObj;
                        //Debug.Print("Error catch " + MemoryReader.readFloat(WoW_Instance.getProcess().Handle, (curObj + 0x9C0)));
                    }
                    //else if (curGUID == curTargetGUID)
                    //{
                    //    this.target = new Unit(curObj);
                        // TODO: set null if dead
                    //}
                    else
                    {
                        objType = MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, (curObj + 0x14));
                        if (objType == 3) // NPC
                        {
                            units.Add(new Mob(curObj));
                        }
                        else
                        {//if(objType == 4) Player
                            //units.Add(new Mob(curObj));
                        }
                    }

                    //Debug.Print(string.Format("GUID: {0} - X: {1} Y: {2} Z: {3}", curGUID.ToString("X16"), x, y, z));
                    curObj = MemoryReader.readUInt32(WoW_Instance.getProcess().Handle, (curObj + 0x3C));
                }
                catch (Exception e) {
                    Debug.Print(e.Message);
                    break;
                }
            }
        }

        /// <summary>
        /// The Pulse should be responsible for updating the Buffs/Debuffs
        /// and calling CombatRoutine and LogicStrategy
        /// </summary>
        private void doPulse()
        {
            getObjects();

            LogicRequest LR;
            CombatRequest CR;

            while (true)
            {
                if (this.target != null && ((Mob)this.target).getHP() <= 0)
                {
                    this.target = null;
                }

                // Pulse the Logic Strategy
                if (this.target == null) 
                {
                    LR = lStrat.getRequest(this);
                    if (LR.getTarget() != null)
                    {
                        target = LR.getTarget();
                        Debug.WriteLine("Targeting: " + LR.getTarget().getGUID().ToString("X16"));
                        this.targetUnit(LR.getTarget());
                    }
                    if (LR.getMove() != null)
                    {
                        moveToLoc(LR.getMove());
                    }
                }

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

                
                //Debug.WriteLine(this.ToString());
                Thread.Sleep(100);
            }
        }
    }
}