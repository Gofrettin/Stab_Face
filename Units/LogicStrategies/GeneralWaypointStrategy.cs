using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stab_Face.Misc;
using Stab_Face.WoW_Process.Factions;

namespace Stab_Face.Units.LogicStrategies
{
    public class GeneralWaypointStrategy : LogicStrategy
    {
        private Waypoint lastWP;
        private Waypoint currentWP;

        public GeneralWaypointStrategy()
        {

        }

        public LogicRequest getRequest(Player p)
        {
            LogicRequest LR = new LogicRequest();
            Waypoint myLoc = p.getLocation();

            // Handle Waypoints first
            if (lastWP == null)
            {
                float dist = 9999.0f;
                Waypoint start = null;
                // We should move to the closest WP on startup
                foreach (Waypoint wp in p.getProfile().getWaypoints())
                {
                    float towp = myLoc.getDistance(wp);
                    if (towp < dist)
                    {
                        start = wp;
                        dist = towp;
                    }
                }

                LR.setMove(start);
                lastWP = start;
                return LR;
            }

            if (currentWP != null)
            {
                if (p.getLocation().getDistance(currentWP) < 2.0f)
                {
                    lastWP = currentWP;
                    currentWP = null;
                }
                else
                {
                    LR.setMove(currentWP);
                }
            }

            if (currentWP == null)
            {
                int currentIndex = p.getProfile().getWaypoints().IndexOf(lastWP);
                if (currentIndex < (p.getProfile().getWaypoints().Count - 1))
                {
                    currentWP = p.getProfile().getWaypoints()[currentIndex + 1];
                }
                else
                {
                    currentWP = p.getProfile().getWaypoints()[0];
                }
            }
            
            Unit candidate = null;
            float cdist = 600.0f;

            foreach( Unit u in p.getNearbyUnits()) {
                Waypoint uLoc = u.getLocation();
                float distanceToUnit = myLoc.getDistance(uLoc);
                if (distanceToUnit < 15.0
                    && myLoc.getDistance(currentWP) < 20.0
                    && p.getProfile().getFactions().Contains(u.getFaction())
                    && ((Mob)u).getHP() > 0)
                {
                    if (candidate == null)
                    {
                        candidate = u;
                        cdist = distanceToUnit;
                    }
                    else if (distanceToUnit < cdist)
                    {
                        candidate = u;
                        cdist = distanceToUnit;
                    }
                }
            }
            LR.setTarget(candidate);
            return LR;
        }
    }
}
