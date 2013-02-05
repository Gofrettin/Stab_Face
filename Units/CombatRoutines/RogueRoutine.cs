using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stab_Face.Misc;
using System.Diagnostics;

namespace Stab_Face.Units.CombatRoutines
{
    class RogueRoutine : CombatRoutine
    {
        protected Stopwatch Evasion = null;
        protected UInt32 EvasionCD = 310000;

        float attackRange = 4.0f;

        public RogueRoutine()
        {
            Evasion = new Stopwatch();
        }

        public CombatRequest getRequest(Player p)
        {

            // Check on timers
            Boolean EvasionReady = true;
            if(Evasion.IsRunning) {
                TimeSpan ts = Evasion.Elapsed;
                UInt32 EvasionElapsedTime = (UInt32)((ts.Seconds * 1000) + ts.Milliseconds);
                if (EvasionElapsedTime < EvasionCD)
                {
                    EvasionReady = false;
                }
                else
                {
                    Evasion.Stop();
                    Evasion.Reset();
                }
            }


            CombatRequest CR = new CombatRequest();
            Waypoint tar_loc = p.getTargetedUnit().getLocation();
            if (p.isInRange(tar_loc, attackRange))
            {
                if ((((float)p.getHP() / (float)p.getMaxHP()) < .2f)
                    && EvasionReady)
                {
                    // evasion
                    CR.setAbility('2');
                    Evasion.Start();
                }
                if (p.getComboPoints() >= 3 && p.getPower() >= 35)
                {
                    // evis
                    CR.setAbility('i');
                }
                else if (p.getPower() >= 40)
                {
                    // ss
                    CR.setAbility('o');
                }
            }

            if (!(p.isInRange(tar_loc, attackRange)))
            {
                CR.setMove(tar_loc);
                //Debug.WriteLine("Moving in Range of Target: " + p.getTargetedUnit().getGUID().ToString("X16"));
            }

            return CR;
        }
    }
}
