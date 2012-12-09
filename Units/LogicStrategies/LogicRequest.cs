using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stab_Face.Misc;

namespace Stab_Face.Units.LogicStrategies
{
    public class LogicRequest
    {
        private Waypoint move = null;
        private Unit target = null;

        public LogicRequest()
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

        public Unit getTarget()
        {
            return target;
        }

        public void setTarget(Unit u)
        {
            this.target = u;
        }

    }
}
