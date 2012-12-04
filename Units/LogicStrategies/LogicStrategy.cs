using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stab_Face.Units.LogicStrategies
{
    public interface LogicStrategy
    {
        LogicRequest getRequest(Player p);
    }
}
