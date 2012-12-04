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
        private UInt64 targetGUID = 0;

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

        public UInt64 getTarget()
        {
            return targetGUID;
        }

        public void setTarget(UInt64 GUID)
        {
            this.targetGUID = GUID;
        }

    }
}
