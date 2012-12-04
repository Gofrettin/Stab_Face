using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stab_Face.Misc;

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
            if (p.isInRange(p.getTargetLocation(), attackRange))
            {
                // Test with sinister strike
                CR.setAbility('o');
            }

            if (!(p.isInRange(p.getTargetLocation(), attackRange)))
            {
                CR.setMove(p.getTargetLocation());
            }

            return CR;
        }
    }
}
