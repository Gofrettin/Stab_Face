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
        float attackRange = 4.0f;
        public RogueRoutine()
        {
            
        }

        public CombatRequest getRequest(Player p)
        {
            CombatRequest CR = new CombatRequest();
            Waypoint tar_loc = p.getTargetedUnit().getLocation();
            if (p.isInRange(tar_loc, attackRange))
            {
                // Test with sinister strike
                CR.setAbility('o');
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
