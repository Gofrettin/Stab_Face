using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stab_Face.Misc;

namespace Stab_Face.Units.CombatRoutines
{
    public class CombatRequest
    {
        private Waypoint move = null;
        private char ability = '.';

        public CombatRequest()
        {
        }

        public Waypoint getMove()
        {
            return move;
        }

        public void setMove(Waypoint wp)
        {
            this.move = wp;
        }

        public char getAbility()
        {
            return ability;
        }

        public void setAbility(char ability)
        {
            this.ability = ability;
        }
    }
}
