using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stab_Face.Misc;

namespace Stab_Face.Units.LogicStrategies
{
    public class GeneralWaypointStrategy : LogicStrategy
    {
        List<Waypoint> route;

        public GeneralWaypointStrategy()
        {
            // TODO: load a waypoint list at runtime
        }

        public LogicRequest getRequest(Player p)
        {
            throw new NotImplementedException();
        }
    }
}
